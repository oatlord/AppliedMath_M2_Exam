using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstabEnemy : MonoBehaviour
{
    [Header("Backstab Settings")]
    public Transform player;
    public float detectionRange = 2f;
    [Range(-1f, 1f)]
    public float backstabAngleThreshold = -0.7f;

    private bool isBackstabbed = false;
    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
    }

    void Update()
    {

    }

    public void OnBackstab()
    {
        if (isBackstabbed)
            return;

        isBackstabbed = true;
        Debug.Log($"{name} was backstabbed!");

        if (enemyRenderer != null)
            enemyRenderer.material.color = Color.red;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 forward = transform.forward * detectionRange;
        float angle = Mathf.Acos(backstabAngleThreshold) * Mathf.Rad2Deg;

        Vector3 rightBoundary = Quaternion.Euler(0, angle, 0) * -forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle, 0) * -forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position - forward);
    }
}

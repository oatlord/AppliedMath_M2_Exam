using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private YouDiedUI youDiedUI;
    [SerializeField] private PlayerBackstab backstabUI;

    [Header("RespawnPoint")]
    [SerializeField] private Transform respawnLocation;

    private Vector3 respawnPoint;
    private Quaternion respawnRotation;

    private void Start()
    {
        if (respawnLocation != null)
        {
            respawnPoint = respawnLocation.position;
            respawnRotation = respawnLocation.rotation;
        }
        else
        {
            Debug.LogWarning("No Respawn Location assigned in Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("VaporDeathFloor"))
        {
            Debug.Log("Player has died. Respawning...");
            StartCoroutine(HandleDeathSequence());
        }
    }
    private IEnumerator HandleDeathSequence()
    {
        // Disable Backstab UI during death fade
        if (backstabUI != null)
            backstabUI.SetPromptActive(false);

        // Play the fade + "You Died" sequence before respawning
        if (youDiedUI != null)
            yield return StartCoroutine(youDiedUI.PlayDeathSequence(Respawn));
        else
            Respawn();

        // Re-enable Backstab UI afterward
        if (backstabUI != null)
            backstabUI.SetPromptActive(true);
    }

    private void Respawn()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        transform.position = respawnPoint;
        transform.rotation = respawnRotation;

        if (rb != null)
        {
            rb.position = respawnPoint;
            rb.rotation = respawnRotation;
        }

        if (cc != null) cc.enabled = true;

        Debug.Log($"Respawned at {transform.position}");
    }

}
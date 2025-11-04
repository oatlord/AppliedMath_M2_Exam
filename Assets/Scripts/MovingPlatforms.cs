using System.Collections;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [Header("Platform Movement Settings")]
    public float moveDistance = 3f;   // How far it moves up/down
    public float moveSpeed = 2f;      // How fast it moves
    public float pauseDelay = 1f;     // Delay between each movement
    public float startDelay = 0f;     // Initial delay before starting movement

    private Vector3 startPosition;
    private bool movingUp = true;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        // 🕒 Wait before starting movement
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Determine the next target
            Vector3 targetPosition = movingUp ? startPosition + Vector3.up * moveDistance : startPosition;
            Vector3 initialPosition = transform.position;
            float elapsedTime = 0f;
            float journeyLength = Vector3.Distance(initialPosition, targetPosition);

            // Move platform smoothly
            while (elapsedTime < journeyLength / moveSpeed)
            {
                float t = elapsedTime * moveSpeed / journeyLength;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to target and wait before switching direction
            transform.position = targetPosition;
            yield return new WaitForSeconds(pauseDelay);

            // Flip direction
            movingUp = !movingUp;
        }
    }
}

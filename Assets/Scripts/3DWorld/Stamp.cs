using UnityEngine;

public class Stamp : MonoBehaviour
{
    private Vector3 originalPosition; // Stores the object's initial position
    private bool isMoving = false;   // Tracks if the object is currently moving

    void Start()
    {
        // Record the object's original position
        originalPosition = transform.position;
    }

    /// <summary>
    /// Instantly moves the object to the target's position.
    /// </summary>
    /// <param name="target">The target GameObject to move to.</param>
    public void MoveToPosition(GameObject target)
    {
        if (target != null)
        {
            transform.position = target.transform.position;
            Debug.Log($"Object moved to {target.name}'s position: {transform.position}");
        }
        else
        {
            Debug.LogError("Target is null. Cannot move.");
        }
    }

    /// <summary>
    /// Aggressively moves the object to the target's position and then back to the original position slower over time.
    /// </summary>
    /// <param name="target">The target GameObject to move to.</param>
    /// <param name="aggressiveSpeed">The speed for the aggressive movement.</param>
    /// <param name="returnSpeed">The speed for the slower return movement.</param>
    public void MoveAggressivelyToPositionAndReturn(GameObject target, float aggressiveSpeed, float returnSpeed)
    {
        if (target == null || isMoving)
        {
            Debug.LogError("Target is null or the object is already moving.");
            return;
        }

        // Start the aggressive move coroutine
        StartCoroutine(MoveAggressivelyRoutine(target.transform.position, aggressiveSpeed, returnSpeed));
    }

    /// <summary>
    /// Instantly moves the object back to its original position.
    /// </summary>
    public void MoveBackToOriginalPosition()
    {
        transform.position = originalPosition;
        Debug.Log($"Object returned to its original position: {originalPosition}");
    }

    // Coroutine to handle aggressive movement and return
    private System.Collections.IEnumerator MoveAggressivelyRoutine(Vector3 targetPosition, float aggressiveSpeed, float returnSpeed)
    {
        isMoving = true;

        // Aggressive move to the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, aggressiveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Reached target position aggressively.");

        // Slower move back to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Returned to the original position slower.");
        isMoving = false;
    }
}

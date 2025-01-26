using UnityEngine;

public class PaperScaler : MonoBehaviour
{
    [Tooltip("The GameObject whose scale will be modified.")]
    public GameObject targetObject;

    [Tooltip("The amount to adjust the Y scale.")]
    public float scaleStep = 0.5f;
    public float maxScaleY = 9f;

    public void DecreaseScaleY()
    {
        if (targetObject != null)
        {
            Vector3 currentScale = targetObject.transform.localScale;
            currentScale.y = Mathf.Max(0, currentScale.y - scaleStep); // Ensures Y scale does not go below 0
            targetObject.transform.localScale = currentScale;
        }
        else
        {
            Debug.LogError("No target object assigned.");
        }
    }

    public void IncreaseScaleY()
    {
        Vector3 currentScale = transform.localScale;

        // Increase Y scale while staying below the maximum limit
        currentScale.y = Mathf.Min(maxScaleY, currentScale.y + scaleStep);

        // Apply the new scale
        transform.localScale = currentScale;

        Debug.Log($"Increased scale: {transform.localScale}");
    }

}
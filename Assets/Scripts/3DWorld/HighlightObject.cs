using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    [Header("Sine Wave Settings")]
    [Tooltip("Minimum Y scale value.")]
    public float minYScale = 1f;

    [Tooltip("Maximum Y scale value.")]
    public float maxYScale = 3f;

    [Tooltip("Frequency of the sine wave (oscillations per second).")]
    public float frequency = 1f;

    [Tooltip("Initial state of scaling.")]
    public bool isScaling = false;

    // Internal variables
    private Vector3 initialScale;
    private float amplitude;
    private float midYScale;
    private float elapsedTime = 0f;

    void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;

        // Calculate amplitude and mid-point based on min and max Y scale
        amplitude = (maxYScale - minYScale) / 2f;
        midYScale = minYScale + amplitude;

        // Initialize the object's Y scale to midYScale for smooth oscillation
        Vector3 newScale = initialScale;
        newScale.y = midYScale;
        transform.localScale = newScale;
    }

    void Update()
    {
        if (isScaling)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the new Y scale using sine wave
            float sineValue = Mathf.Sin(2 * Mathf.PI * frequency * elapsedTime);
            float newYScale = midYScale + sineValue * amplitude;

            // Apply the new Y scale while keeping X and Z scales unchanged
            Vector3 newScale = transform.localScale;
            newScale.y = newYScale;
            transform.localScale = newScale;
        }
        else
        {
            transform.localScale = initialScale;
        }
    }

    /// <summary>
    /// Toggles the scaling state between active and inactive.
    /// </summary>
    public void ToggleScaling()
    {
        isScaling = !isScaling;

        // Reset elapsed time to ensure smooth scaling after toggling
        if (isScaling)
        {
            elapsedTime = 0f;

            // Optionally, reset the scale to midYScale to avoid abrupt changes
            Vector3 newScale = transform.localScale;
            newScale.y = midYScale;
            transform.localScale = newScale;
        }
    }

    /// <summary>
    /// Sets the scaling state explicitly.
    /// </summary>
    /// <param name="state">Desired scaling state.</param>
    public void SetScaling(bool state)
    {
        isScaling = state;

        if (isScaling)
        {
            elapsedTime = 0f;
            Vector3 newScale = transform.localScale;
            newScale.y = midYScale;
            transform.localScale = newScale;
        }
    }
}

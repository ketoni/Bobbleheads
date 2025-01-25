using UnityEngine;
using UnityEngine.InputSystem;

public class Player3D : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float minHorizontalAngle = -45f;
    public float maxHorizontalAngle = 45f;

    private Transform playerBody;
    private Camera mainCamera;
    private float xRotation = 0f;
    private float initialYRotation;

    private bool isTaskHovered = false;
    private bool taskControlsActive = false;

    void Start()
    {
        playerBody = this.transform;
        mainCamera = Camera.main;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Record the initial Y rotation
        initialYRotation = playerBody.eulerAngles.y;
    }

    void Update()
    {
        HandleMouseLook();
        HandleTaskControls();
        HandleCursorHover();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation with limits
        float newYRotation = playerBody.eulerAngles.y + mouseX;

        // Calculate relative rotation based on initial rotation
        float relativeYRotation = Mathf.DeltaAngle(initialYRotation, newYRotation);
        relativeYRotation = Mathf.Clamp(relativeYRotation, minHorizontalAngle, maxHorizontalAngle);

        playerBody.rotation = Quaternion.Euler(0f, initialYRotation + relativeYRotation, 0f);
    }

    private void HandleCursorHover()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Task task = hit.collider.GetComponent<Task>();
            if (task != null)
            {
                if (!isTaskHovered)
                {
                    isTaskHovered = true;
                    ActivateTaskControls(true);
                }
            }
            else
            {
                if (isTaskHovered)
                {
                    isTaskHovered = false;
                    ActivateTaskControls(false);
                }
            }
        }
        else
        {
            if (isTaskHovered)
            {
                isTaskHovered = false;
                ActivateTaskControls(false);
            }
        }
    }

    private void ActivateTaskControls(bool activate)
    {
        taskControlsActive = activate;
        // Enable or disable WASD controls here
        // This could involve enabling/disabling a component or setting a flag
        // For example:
        // movementController.enabled = activate;
    }

    private void HandleTaskControls()
    {
        if (taskControlsActive)
        {
            // Implement task-specific controls, e.g., WASD movement
            // You can enable a specific input map or handle inputs conditionally
        }
        else
        {
            // Disable task-specific controls
        }

        // Example: Decrease stress when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Assuming you have a reference to StressManager
            // StressManager.Instance.DecreaseStress(10f);
        }
    }
}

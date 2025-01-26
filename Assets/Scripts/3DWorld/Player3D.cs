using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player3D : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float minHorizontalAngle = -45f;
    public float maxHorizontalAngle = 45f;

    private Transform playerBody;
    private Camera mainCamera;
    private float xRotation = 0f;
    private float initialYRotation;
    private bool inGame = true;
    private Vector3 outOfGamePosition = new Vector3(-13.9f, 0.855f, 8.886f);
    private Vector3 inGamePosition = new Vector3(-13.427f, 0.62f, 8.506f);
    private float zoomSpeed = 1;

    private Task currentHoveredTask = null;

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
        if (!inGame)
        {
            HandleMouseLook();
            HandleCursorHover();
        }

        if (Input.GetKeyDown("escape"))
        {
            inGame = !inGame;
            zoom();
        }
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
            if (task != null && task.quantity > 0)
            {
                if (currentHoveredTask != task)
                {
                    if (currentHoveredTask != null)
                    {
                        currentHoveredTask.Deactivate();
                    }

                    currentHoveredTask = task;
                    currentHoveredTask.Activate();
                }
            }
            else
            {
                if (currentHoveredTask != null)
                {
                    currentHoveredTask.Deactivate();
                    currentHoveredTask = null;
                }
            }
        }
        else
        {
            if (currentHoveredTask != null)
            {
                currentHoveredTask.Deactivate();
                currentHoveredTask = null;
            }
        }
    }

    private void zoom()
    {
        if (inGame)
        {
            transform.DOMove(inGamePosition, 1);
            transform.DORotate(new Vector3(0f, 135f, 0f), 1);
            mainCamera.transform.DORotate(new Vector3(0f, 135f, 0f), 1);
        }
        else
        {
            transform.DOMove(outOfGamePosition, 1);
            xRotation = 0f;
        }
    }
}

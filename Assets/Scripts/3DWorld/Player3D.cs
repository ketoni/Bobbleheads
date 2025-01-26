using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.UIElements;

public class Player3D : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float minHorizontalAngle = -45f;
    public float maxHorizontalAngle = 45f;
    public bool dead = false;
    public bool victory = false;
    public GameObject gameOverScreen;
    public GameObject winningText;
    public GameObject winningPanel;
    private Transform playerBody;
    private Camera mainCamera;
    private float xRotation = 0f;
    private float initialYRotation;
    public bool inGame = false;
    private Vector3 outOfGamePosition = new Vector3(-13.9f, 0.855f, 8.886f);
    private Vector3 inGamePosition = new Vector3(-13.427f, 0.62f, 8.506f);
    private Vector3 gameOverPosition = new Vector3(-13.9f, 0f, 8.886f);
    private float zoomSpeed = 1;

    public TaskTarget currentHoveredTarget = null;

    void Start()
    {
        playerBody = this.transform;
        mainCamera = Camera.main;

        // Lock the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        // Record the initial Y rotation
        initialYRotation = playerBody.eulerAngles.y;
    }

    void Update()
    {
        if (dead) return;
        if (victory) return;
        if (!inGame)
        {
            HandleMouseLook();
            HandleCursorHover();
        }

        if (Input.GetKeyDown("space"))
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
            TaskTarget target = hit.collider.GetComponent<TaskTarget>();
            if (target != null)
            {
                // Found target
                if (target.task == null)
                {
                    throw new NullReferenceException("TaskTaget has no task!");
                }
                if (currentHoveredTarget != target)
                {
                    // Found other one
                    if (currentHoveredTarget != null)
                    {
                        currentHoveredTarget.task.Deactivate();
                    }

                    currentHoveredTarget = target;
                    currentHoveredTarget.task.Activate();
                }
                return;
            }
        }

        // Otherwise deactivate the current
        if (currentHoveredTarget != null)
        {
            currentHoveredTarget.task.Deactivate();
            currentHoveredTarget = null;
        }
    }

    private void zoom()
    {
        if (inGame)
        {
            transform.DOMove(inGamePosition, 1);
            transform.DORotate(new Vector3(0f, 135f, 0f), 1);
            mainCamera.transform.DORotate(new Vector3(0f, 135f, 0f), 1);
            FindFirstObjectByType<BobbleHeadManager>().EnterGame();
        }
        else
        {
            transform.DOMove(outOfGamePosition, 1);
            xRotation = 0f;
            FindFirstObjectByType<BobbleHeadManager>().Exit();
        }
    }


    public void GameOver()
    {
        if (victory || dead) return;
        dead = true;
        transform.DOMove(gameOverPosition, 2);
        transform.DORotate(new Vector3(0f, 140f, 0f), 1);
        mainCamera.transform.DORotate(new Vector3(40f, 135f, 28f), 1);
        gameOverScreen.SetActive(true);
        MusicManager.Instance.PlayDeathSound();
    }

    public void Winning()
    {
        if (dead || victory) return;
        victory = true;
        winningText.SetActive(true);
        winningPanel.SetActive(true);
        MusicManager.Instance.PlayVictorySound();
    }
}

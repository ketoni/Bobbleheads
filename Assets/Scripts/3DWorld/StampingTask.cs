using UnityEngine;

public class StampingTask : Minigame
{
    private bool isStamped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StampPaper();
        }

        if (isStamped)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MovePaper(-1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MovePaper(1);
            }
        }
    }

    public override void StartMinigame()
    {
        // Initialize minigame
        isStamped = false;
        Debug.Log("Stamping Minigame Started");
    }

    private void StampPaper()
    {
        isStamped = true;
        Debug.Log("Paper Stamped");
    }

    private void MovePaper(int direction)
    {
        // Implement paper movement logic
        Debug.Log("Paper Moved " + (direction == 1 ? "Right" : "Left"));
        CompleteMinigame();
    }

    public override void CompleteMinigame()
    {
        // Notify TaskManager that this task is completed
        Task task = GetComponent<Task>();
        if (task != null)
        {
            task.CompleteTask();
        }
        Destroy(gameObject); // Or deactivate
    }

    public override void FailMinigame()
    {
        // Implement failure logic
        Debug.Log("Stamping Minigame Failed");
    }
}

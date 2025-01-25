using UnityEngine;

public class Task : MonoBehaviour
{
    public enum TaskType
    {
        StampingPapers,
        Email,
        FaxMachine
    }

    public TaskType taskType;
    public int quantity = 1;

    // Reference to TaskManager
    private TaskManager taskManager;

    void Start()
    {
        taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            taskManager.RegisterTask(this);
        }
    }

    void OnDestroy()
    {
        if (taskManager != null)
        {
            taskManager.UnregisterTask(this);
        }
    }

    // Method to complete the task
    public void CompleteTask()
    {
        quantity--;
        if (quantity <= 0)
        {
            // Task completed, destroy or deactivate the task object
            Debug.Log("quantity " + quantity);
            Destroy(gameObject);
        }
    }
}

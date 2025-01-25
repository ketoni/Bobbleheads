using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    // List of all registered tasks
    private List<Task> allTasks = new List<Task>();

    // List of currently active tasks (hovered)
    private List<Task> activeTasks = new List<Task>();

    // Timer for adding random tasks
    public float randomTaskInterval = 10f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optionally, DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(AddRandomTaskRoutine());
    }

    public void RegisterTask(Task task)
    {
        if (!allTasks.Contains(task))
        {
            allTasks.Add(task);
            UpdateTaskQuantity(task.taskType, task.quantity);
        }
    }

    public void UnregisterTask(Task task)
    {
        if (allTasks.Contains(task))
        {
            allTasks.Remove(task);
            UpdateTaskQuantity(task.taskType, task.quantity);
        }

        // Also ensure it's removed from activeTasks if it's being destroyed
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
        }
    }

    // Methods to manage activeTasks list
    public void AddActiveTask(Task task)
    {
        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
            Debug.Log($"{task.taskType} added to active tasks.");
        }
    }

    public void RemoveActiveTask(Task task)
    {
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
            Debug.Log($"{task.taskType} removed from active tasks.");
        }
    }

    // Update the UI via UIManager
    public void UpdateTaskQuantity(Task.TaskType taskType, int quantity)
    {
        UIManager.Instance.UpdateTaskQuantity(taskType, quantity);
    }

    // Check if a task is active
    public bool IsTaskActive(Task task)
    {
        return task != null && activeTasks.Contains(task);
    }

    // Get total number of tasks yet to complete
    public int GetTotalPendingTasks()
    {
        int total = 0;
        foreach (var task in allTasks)
        {
            total += task.quantity;
        }
        return total;
    }

    // Coroutine to add a random task every 10 seconds
    private IEnumerator AddRandomTaskRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomTaskInterval);
            AddRandomTask();
        }
    }

    private void AddRandomTask()
    {
        if (allTasks.Count == 0)
            return;

        // Select a random task from all tasks, regardless of quantity
        Task randomTask = allTasks[Random.Range(0, allTasks.Count)];
        randomTask.quantity += 1;
        randomTask.UpdateTaskUI(); // Ensure the task updates its UI and collider
        Debug.Log($"Randomly increased {randomTask.taskType} quantity to {randomTask.quantity}");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private List<Task> activeTasks = new List<Task>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterTask(Task task)
    {
        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
        }
    }

    public void UnregisterTask(Task task)
    {
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
        }
    }

    // Get total number of tasks yet to complete
    public int GetTotalPendingTasks()
    {
        int total = 0;
        foreach (var task in activeTasks)
        {
            total += task.quantity;
        }
        return total;
    }
}

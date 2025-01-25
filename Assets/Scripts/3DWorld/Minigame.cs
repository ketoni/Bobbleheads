using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public abstract void StartMinigame();
    public abstract void PauseMinigame();
    public abstract void ResetMinigame();
    public abstract void CompleteMinigame();
    public abstract void FailMinigame();

    // Check if this minigame is active
    protected bool IsActive()
    {
        Task task = GetComponent<Task>();
        return TaskManager.Instance != null && TaskManager.Instance.IsTaskActive(task);
    }
}

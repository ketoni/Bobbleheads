using UnityEngine;

public class TaskInteraction : MonoBehaviour
{
    private Task task;

    void Start()
    {
        task = GetComponent<Task>();
    }

    void OnMouseDown()
    {
        if (task != null)
        {
            StartMinigame();
        }
    }

    private void StartMinigame()
    {
        switch (task.taskType)
        {
            case Task.TaskType.StampingPapers:
                StartMinigame<StampingTask>();
                break;
            case Task.TaskType.Email:
                StartMinigame<EmailTask>();
                break;
            case Task.TaskType.FaxMachine:
                StartMinigame<FaxTask>();
                break;
        }
    }

    private void StartMinigame<T>() where T : Minigame
    {
        // Assuming minigames are prefabs
        GameObject minigamePrefab = Resources.Load<GameObject>("Minigames/" + typeof(T).Name);
        if (minigamePrefab != null)
        {
            GameObject minigameInstance = Instantiate(minigamePrefab, Vector3.zero, Quaternion.identity);
            Minigame minigame = minigameInstance.GetComponent<Minigame>();
            if (minigame != null)
            {
                minigame.StartMinigame();
            }
        }
        else
        {
            Debug.LogError("Minigame prefab not found for: " + typeof(T).Name);
        }
    }
}

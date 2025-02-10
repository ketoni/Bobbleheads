using UnityEngine;
using TMPro;

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
    public int test = 1;

    // UI Text reference
    public TextMeshProUGUI taskText;
    public GameObject taskPopup;
    public AudioClip taskAddedAudio;

    private AudioSource audioSource;

    // Reference to TaskManager
    private TaskManager taskManager;

    // Reference to the Minigame
    private Minigame currentMinigame;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        taskManager = TaskManager.Instance;
        if (taskManager != null)
        {
            taskManager.RegisterTask(this);
        }

        UpdateTaskUI();
    }

    void OnDestroy()
    {
        if (taskManager != null)
        {
            taskManager.UnregisterTask(this);
        }
    }

    // Activate the task's minigame
    public bool Activate()
    {
        if (quantity <= 0)
        {
            Debug.Log($"Cannot activate {taskType}. Quantity is 0.");
            return false;
        }

        if (currentMinigame == null)
        {
            StartMinigame();
        }

        // Inform TaskManager that this task is active
        if (taskManager != null)
        {
            taskManager.AddActiveTask(this);
        }

        currentMinigame.gameObject.GetComponent<HighlightObject>().SetScaling(false);
        currentMinigame.gameObject.GetComponent<Outline>().enabled = false;

        // Show popup object
        //UIManager.Instance.SetInstruction(GetInstructionForTask());
        taskPopup.SetActive(true);

        Debug.Log($"{taskType} minigame activated.");
        return true;
    }

    // Deactivate the task's minigame
    public void Deactivate()
    {
        if (currentMinigame != null)
        {
            currentMinigame.gameObject.GetComponent<HighlightObject>().SetScaling(true);
            currentMinigame.gameObject.GetComponent<Outline>().enabled = true;
            PauseMinigame();
            Debug.Log($"{taskType} minigame deactivated.");
        }

        // Inform TaskManager that this task is no longer active
        if (taskManager != null)
        {
            taskManager.RemoveActiveTask(this);
        }

        if(quantity <= 0 && currentMinigame != null)
        {
            currentMinigame.gameObject.GetComponent<HighlightObject>().SetScaling(false);
            currentMinigame.gameObject.GetComponent<Outline>().enabled = false;
        }
        // Hide popup
        //UIManager.Instance.SetDefaultInstruction();
        taskPopup.SetActive(false);

    }

    // Start the minigame
    private void StartMinigame()
    {
        // Assuming minigames are components attached to the task GameObject
        switch (taskType)
        {
            case TaskType.StampingPapers:
                currentMinigame = GetComponent<StampingTask>();
                break;
            case TaskType.Email:
                currentMinigame = GetComponent<EmailTask>();
                break;
            case TaskType.FaxMachine:
                currentMinigame = GetComponent<FaxTask>();
                break;
        }

        if (currentMinigame != null)
        {
            currentMinigame.gameObject.GetComponent<HighlightObject>().SetScaling(false);
            currentMinigame.gameObject.GetComponent<Outline>().enabled = false;
            currentMinigame.StartMinigame();
        }
        else
        {
            Debug.LogError($"Minigame script for {taskType} not found on {gameObject.name}.");
        }
    }

    // Pause the minigame
    private void PauseMinigame()
    {
        if (currentMinigame != null)
        {
            currentMinigame.PauseMinigame();
        }
    }

    // Complete the task once a minigame is finished
    public void CompleteTask()
    {
        quantity--;

        if (quantity <= 0)
        {
            quantity = 0;

            currentMinigame.gameObject.GetComponent<HighlightObject>().SetScaling(false);
            currentMinigame.gameObject.GetComponent<Outline>().enabled = false;

            Debug.Log($"{taskType} task completed.");
            //Destroy(gameObject);
            currentMinigame.ResetMinigame();
        }

        UpdateTaskUI();

    }

    // Update the UI Text for this task
    public void UpdateTaskUI()
    {
        if (taskText != null)
        {
            taskText.text = $"{taskType.ToString()}: {quantity}";
        }

        // Update via TaskManager/UIManager
        if (taskManager != null)
        {
            taskManager.UpdateTaskQuantity(taskType, quantity);
        }

        // Disable collider if quantity is zero
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = quantity > 0;
        }
    }

    // Get instructions based on task type
    private string GetInstructionForTask()
    {
        switch (taskType)
        {
            case TaskType.FaxMachine:
                return "Press the highlighted buttons in order to send the fax. Pressing the wrong button will cause a delay.";
            case TaskType.Email:
                return "Mash any keys to fill the progress bar. Press ENTER to send the email once filled.";
            case TaskType.StampingPapers:
                return "Press S to stamp the paper and D to move it aside.";
            default:
                return "Unknown Task.";
        }
    }

    public void PlayTaskAddedAudio()
    {
        audioSource.PlayOneShot(taskAddedAudio);
    }
}

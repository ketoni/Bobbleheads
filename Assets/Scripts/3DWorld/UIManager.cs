using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Task Quantities")]
    public TextMeshProUGUI faxesText;
    public TextMeshProUGUI emailsText;
    public TextMeshProUGUI stampsText;

    [Header("Instruction Text")]
    public TextMeshProUGUI instructionText;
    public string defaultInstruction = "Hover over a task to see instructions.";

    [Header("Task UI Panels")]
    public GameObject faxTaskUI;
    public GameObject emailTaskUI;
    public GameObject stampTaskUI;

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
        SetDefaultInstruction();
    }

    public void UpdateTaskQuantity(Task.TaskType taskType, int quantity)
    {
        switch (taskType)
        {
            case Task.TaskType.FaxMachine:
                faxesText.text = $"Faxes: {quantity}";
                SetTaskUIInteractable(faxTaskUI, quantity > 0);
                break;
            case Task.TaskType.Email:
                emailsText.text = $"Emails: {quantity}";
                SetTaskUIInteractable(emailTaskUI, quantity > 0);
                break;
            case Task.TaskType.StampingPapers:
                stampsText.text = $"Stamps: {quantity}";
                SetTaskUIInteractable(stampTaskUI, quantity > 0);
                break;
        }
    }

    private void SetTaskUIInteractable(GameObject taskUI, bool interactable)
    {
        // Assuming taskUI has a CanvasGroup or some component to indicate interactivity
        // For example, disabling the collider or changing the UI color
        if (taskUI != null)
        {
            CanvasGroup canvasGroup = taskUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = interactable;
                canvasGroup.alpha = interactable ? 1f : 0.5f; // Dim if not interactable
            }

            // Optionally, disable collider if not interactable
            Collider collider = taskUI.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = interactable;
            }
        }
    }

    public void SetInstruction(string instruction)
    {
        if (instructionText != null)
        {
            instructionText.text = instruction;
        }
    }

    public void SetDefaultInstruction()
    {
        SetInstruction(defaultInstruction);
    }
}

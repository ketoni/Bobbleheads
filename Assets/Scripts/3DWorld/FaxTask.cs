using UnityEngine;
using UnityEngine.UI;

public class FaxTask : Minigame
{
    public string[] buttonSequence;
    public KeyCode[] validKeys; // Corresponding keys for the sequence
    public float delayOnWrong = 2f;

    private int currentStep = 0;
    private float delayTimer = 0f;
    private bool isDelayed = false;

    void Start()
    {
        // Initialize the button sequence
        // Example: ["A", "S", "D"]
        buttonSequence = new string[] { "A", "S", "D" };
        validKeys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D };
    }

    void Update()
    {
        if (isDelayed)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayOnWrong)
            {
                isDelayed = false;
                delayTimer = 0f;
                Debug.Log("Delay Over. Continue.");
            }
            return;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in validKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    CheckInput(key);
                    break;
                }
            }
        }
    }

    public override void StartMinigame()
    {
        // Initialize minigame
        currentStep = 0;
        isDelayed = false;
        delayTimer = 0f;
        Debug.Log("Fax Machine Minigame Started");
    }

    private void CheckInput(KeyCode inputKey)
    {
        if (inputKey == validKeys[currentStep])
        {
            Debug.Log("Correct Key Pressed: " + inputKey);
            currentStep++;
            if (currentStep >= validKeys.Length)
            {
                SendFax();
            }
        }
        else
        {
            Debug.Log("Wrong Key Pressed: " + inputKey);
            isDelayed = true;
            delayTimer = 0f;
        }
    }

    private void SendFax()
    {
        Debug.Log("Fax Sent");
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
        Debug.Log("Fax Machine Minigame Failed");
    }
}

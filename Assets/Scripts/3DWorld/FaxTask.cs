using UnityEngine;

public class FaxTask : Minigame
{
    public string[] buttonSequence = { "A", "S", "D" };
    private KeyCode[] validKeys = { KeyCode.A, KeyCode.S, KeyCode.D };
    public float delayOnWrong = 2f;

    private int currentStep = 0;
    private float delayTimer = 0f;
    private bool isDelayed = false;
    private int completions = 0;

    void Update()
    {
        if (!IsActive())
            return;

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
        completions = 0;
        Debug.Log("Fax Machine Minigame Started");
    }

    public override void PauseMinigame()
    {
        // Implement pause logic if needed
        Debug.Log("Fax Machine Minigame Paused");
    }

    public override void ResetMinigame()
    {
        currentStep = 0;
        isDelayed = false;
        delayTimer = 0f;
        completions = 0;
        Debug.Log("Fax Machine Minigame Reset");
    }

    private void CheckInput(KeyCode inputKey)
    {
        if (inputKey == validKeys[currentStep])
        {
            Debug.Log($"Correct Key Pressed: {inputKey}");
            currentStep++;
            if (currentStep >= validKeys.Length)
            {
                SendFax();
            }
        }
        else
        {
            Debug.Log($"Wrong Key Pressed: {inputKey}");
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
        completions++;
        Task task = GetComponent<Task>();
        if (task != null)
        {
            task.CompleteTask();
        }

        Debug.Log("Fax Machine Minigame Completed");

        if (task != null && task.quantity > 0)
        {
            ResetMinigame();
        }
        else
        {
            // Task completed entirely
            //Destroy(gameObject);
        }
    }

    public override void FailMinigame()
    {
        // Implement failure logic if needed
        Debug.Log("Fax Machine Minigame Failed");
    }
}

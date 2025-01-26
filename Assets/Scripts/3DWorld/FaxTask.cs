using UnityEngine;

public class FaxTask : Minigame
{
    // Removed the unused string array 'buttonSequence'

    // Define the list of valid keys
    private KeyCode[] validKeys = { KeyCode.A, KeyCode.S, KeyCode.D , KeyCode.W};

    // New array to hold the expected sequence
    private KeyCode[] expectedSequence;

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

        // Initialize and shuffle the expected sequence
        InitializeExpectedSequence();
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

        // Re-initialize and shuffle the expected sequence
        InitializeExpectedSequence();
    }

    /// <summary>
    /// Initializes and shuffles the expected key sequence.
    /// </summary>
    private void InitializeExpectedSequence()
    {
        // Clone the validKeys array to avoid modifying the original
        expectedSequence = (KeyCode[])validKeys.Clone();

        // Shuffle the expectedSequence array
        ShuffleArray(expectedSequence);

        // Log the shuffled sequence for debugging
        string sequenceLog = "Expected Sequence: ";
        foreach (KeyCode key in expectedSequence)
        {
            sequenceLog += key.ToString() + " ";
        }
        Debug.Log(sequenceLog);
    }

    /// <summary>
    /// Implements the Fisher-Yates shuffle algorithm to randomize the array.
    /// </summary>
    /// <param name="array">The array to shuffle.</param>
    private void ShuffleArray(KeyCode[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            // Swap elements at positions i and j
            KeyCode temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    private void CheckInput(KeyCode inputKey)
    {
        if (currentStep >= expectedSequence.Length)
        {
            Debug.LogError("Current step exceeds the expected sequence length.");
            return;
        }

        if (inputKey == expectedSequence[currentStep])
        {
            Debug.Log($"Correct Key Pressed: {inputKey}");
            currentStep++;
            if (currentStep >= expectedSequence.Length)
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


//using UnityEngine;

//public class FaxTask : Minigame
//{
//    public string[] buttonSequence = { "A", "S", "D" };
//    private KeyCode[] validKeys = { KeyCode.A, KeyCode.S, KeyCode.D };
//    public float delayOnWrong = 2f;

//    private int currentStep = 0;
//    private float delayTimer = 0f;
//    private bool isDelayed = false;
//    private int completions = 0;

//    void Update()
//    {
//        if (!IsActive())
//            return;

//        if (isDelayed)
//        {
//            delayTimer += Time.deltaTime;
//            if (delayTimer >= delayOnWrong)
//            {
//                isDelayed = false;
//                delayTimer = 0f;
//                Debug.Log("Delay Over. Continue.");
//            }
//            return;
//        }

//        if (Input.anyKeyDown)
//        {
//            foreach (KeyCode key in validKeys)
//            {
//                if (Input.GetKeyDown(key))
//                {
//                    CheckInput(key);
//                    break;
//                }
//            }
//        }
//    }

//    public override void StartMinigame()
//    {


//        // Initialize minigame
//        currentStep = 0;
//        isDelayed = false;
//        delayTimer = 0f;
//        completions = 0;
//        Debug.Log("Fax Machine Minigame Started");
//    }

//    public override void PauseMinigame()
//    {
//        // Implement pause logic if needed
//        Debug.Log("Fax Machine Minigame Paused");
//    }

//    public override void ResetMinigame()
//    {
//        currentStep = 0;
//        isDelayed = false;
//        delayTimer = 0f;
//        completions = 0;
//        Debug.Log("Fax Machine Minigame Reset");
//    }

//    private void CheckInput(KeyCode inputKey)
//    {
//        if (inputKey == validKeys[currentStep])
//        {
//            Debug.Log($"Correct Key Pressed: {inputKey}");
//            currentStep++;
//            if (currentStep >= validKeys.Length)
//            {
//                SendFax();
//            }
//        }
//        else
//        {
//            Debug.Log($"Wrong Key Pressed: {inputKey}");
//            isDelayed = true;
//            delayTimer = 0f;
//        }
//    }

//    private void SendFax()
//    {
//        Debug.Log("Fax Sent");
//        CompleteMinigame();
//    }

//    public override void CompleteMinigame()
//    {
//        completions++;
//        Task task = GetComponent<Task>();
//        if (task != null)
//        {
//            task.CompleteTask();
//        }

//        Debug.Log("Fax Machine Minigame Completed");

//        if (task != null && task.quantity > 0)
//        {
//            ResetMinigame();
//        }
//        else
//        {
//            // Task completed entirely
//            //Destroy(gameObject);
//        }
//    }

//    public override void FailMinigame()
//    {
//        // Implement failure logic if needed
//        Debug.Log("Fax Machine Minigame Failed");
//    }
//}

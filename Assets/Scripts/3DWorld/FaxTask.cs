using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI;

public class FaxTask : Minigame
{
    // Define the list of valid keys
    private KeyCode[] validKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W };

    // Array to hold the expected sequence
    private KeyCode[] expectedSequence;

    [Header("Audio Clips")]
    public AudioClip keyPressAudio;
    public AudioClip faxSendAudio;
    public AudioClip faxErrorAudio;

    [Header("UI Elements")]
    [Tooltip("Text element to display the next key to press.")]
    public TextMeshPro nextKeyText; // Reference to the UI Text element

    [Header("Materials")]
    public Material noTaskMaterial;
    public Material thereIsATaskMaterial;
    public Material wrongButtonMaterial;
    public GameObject screen;

    private int currentStep = 0;
    private float delayTimer = 0f;
    private bool isDelayed = false;
    private int completions = 0;
    private AudioSource audioSource;
    private float delayOnWrong = 1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Optional: Check if nextKeyText is assigned
        if (nextKeyText == null)
        {
            Debug.LogError("NextKeyText is not assigned in the Inspector.");
        }
    }

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
                // Switch back material
                SwitchMaterial(true);
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

        // Update the UI prompt with the first key
        UpdateNextKeyPrompt();
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

        // Update the UI prompt with the first key
        UpdateNextKeyPrompt();
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
            audioSource.PlayOneShot(keyPressAudio);

            if (currentStep < expectedSequence.Length)
            {
                // Update the UI prompt to the next key
                UpdateNextKeyPrompt();
            }

            if (currentStep >= expectedSequence.Length)
            {
                SendFax();
                audioSource.PlayOneShot(faxSendAudio);
            }
        }
        else
        {
            Debug.Log($"Wrong Key Pressed: {inputKey}");
            isDelayed = true;
            delayTimer = 0f;
            audioSource.PlayOneShot(faxErrorAudio);
            screen.GetComponent<Renderer>().material = wrongButtonMaterial;

            //// Optionally, provide feedback on wrong input
            //if (nextKeyText != null)
            //{
            //    nextKeyText.text = "X";
            //}
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
            // Put the light off in the machine
            SwitchMaterial(false);

            //Destroy(gameObject);
            if (nextKeyText != null)
            {
                //nextKeyText.text = "";

            }
        }
    }

    public override void FailMinigame()
    {
        // Implement failure logic if needed
        Debug.Log("Fax Machine Minigame Failed");

        //// Optionally, provide failure feedback
        //if (nextKeyText != null)
        //{
        //    nextKeyText.text = "";
        //}
    }

    /// <summary>
    /// Updates the UI text to display the next key the player should press.
    /// </summary>
    private void UpdateNextKeyPrompt()
    {
        if (nextKeyText == null)
        {
            Debug.LogWarning("NextKeyText is not assigned.");
            return;
        }

        if (currentStep < expectedSequence.Length)
        {
            nextKeyText.text = $"{expectedSequence[currentStep]}";
        }
        else
        {
            nextKeyText.text = "+";
        }
    }

    public void SwitchMaterial(bool thereIsTask)
    {
        if(thereIsTask)
        {
            screen.GetComponent<Renderer>().material = thereIsATaskMaterial;
            nextKeyText.enabled = true;
        }
        else
        {
            screen.GetComponent<Renderer>().material = noTaskMaterial;
            nextKeyText.enabled = false;
        }
    }
}


//using UnityEngine;

//public class FaxTask : Minigame
//{
//    // Removed the unused string array 'buttonSequence'

//    // Define the list of valid keys
//    private KeyCode[] validKeys = { KeyCode.A, KeyCode.S, KeyCode.D , KeyCode.W};

//    // New array to hold the expected sequence
//    private KeyCode[] expectedSequence;

//    public float delayOnWrong = 2f;
//    public AudioClip keyPressAudio;
//    public AudioClip faxSendAudio;
//    public AudioClip faxErrorAudio;

//    private int currentStep = 0;
//    private float delayTimer = 0f;
//    private bool isDelayed = false;
//    private int completions = 0;
//    private AudioSource audioSource;

//    private void Awake()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

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

//        // Initialize and shuffle the expected sequence
//        InitializeExpectedSequence();
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

//        // Re-initialize and shuffle the expected sequence
//        InitializeExpectedSequence();
//    }

//    /// <summary>
//    /// Initializes and shuffles the expected key sequence.
//    /// </summary>
//    private void InitializeExpectedSequence()
//    {
//        // Clone the validKeys array to avoid modifying the original
//        expectedSequence = (KeyCode[])validKeys.Clone();

//        // Shuffle the expectedSequence array
//        ShuffleArray(expectedSequence);

//        // Log the shuffled sequence for debugging
//        string sequenceLog = "Expected Sequence: ";
//        foreach (KeyCode key in expectedSequence)
//        {
//            sequenceLog += key.ToString() + " ";
//        }
//        Debug.Log(sequenceLog);
//    }

//    /// <summary>
//    /// Implements the Fisher-Yates shuffle algorithm to randomize the array.
//    /// </summary>
//    /// <param name="array">The array to shuffle.</param>
//    private void ShuffleArray(KeyCode[] array)
//    {
//        for (int i = array.Length - 1; i > 0; i--)
//        {
//            int j = Random.Range(0, i + 1);
//            // Swap elements at positions i and j
//            KeyCode temp = array[i];
//            array[i] = array[j];
//            array[j] = temp;
//        }
//    }

//    private void CheckInput(KeyCode inputKey)
//    {
//        if (currentStep >= expectedSequence.Length)
//        {
//            Debug.LogError("Current step exceeds the expected sequence length.");
//            return;
//        }

//        if (inputKey == expectedSequence[currentStep])
//        {
//            Debug.Log($"Correct Key Pressed: {inputKey}");
//            currentStep++;
//            audioSource.PlayOneShot(keyPressAudio);
//            if (currentStep >= expectedSequence.Length)
//            {
//                SendFax();
//                audioSource.PlayOneShot(faxSendAudio);
//            }
//        }
//        else
//        {
//            Debug.Log($"Wrong Key Pressed: {inputKey}");
//            isDelayed = true;
//            delayTimer = 0f;
//            audioSource.PlayOneShot(faxErrorAudio);
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
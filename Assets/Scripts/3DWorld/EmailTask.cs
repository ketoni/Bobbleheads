using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmailTask : Minigame
{
    public Slider progressBar;
    public float targetProgress = 1f;
    public float progressIncrement = 0.05f;

    private int charAmount = 20;
    public AudioClip keyPressAudio;
    public AudioClip emailSendAudio;
    private bool isBarFilled = false;
    private int completions = 0;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsActive())
            return;

        if(TaskManager.Instance.player.GetComponent<Player3D>().inGame == false)
        {
            if (!isBarFilled && Input.anyKeyDown && GetComponent<Task>().quantity > 0)
            {
                MashKeys();
            }

            if (isBarFilled && Input.GetKeyDown(KeyCode.Return))
            {
                SendEmail();
            }
        }
    }

    public override void StartMinigame()
    {
        // Initialize minigame
        isBarFilled = false;
        completions = 0;
        if (progressBar != null)
        {
            progressBar.value = 0f;
            charAmount = 20;
            FindFirstObjectByType<BobbleHeadManager>().UpdateEmailCharAmount(charAmount.ToString());
            Debug.Log("EmailTask Slider Initialized.");
        }
        Debug.Log("Email Minigame Started");
    }

    public override void PauseMinigame()
    {
        // Implement pause logic if needed
        Debug.Log("Email Minigame Paused");
    }

    public override void ResetMinigame()
    {
        isBarFilled = false;
        completions = 0;
        if (progressBar != null)
        {
            progressBar.value = 0f;
            charAmount = 20;
            FindFirstObjectByType<BobbleHeadManager>().UpdateEmailCharAmount(charAmount.ToString());
        }
        Debug.Log("Email Minigame Reset");
    }

    private void MashKeys()
    {
        bool foundKey = false;
         // Loop through all keys to check if any of them is pressed
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            // Ignore mouse buttons and joystick buttons
            if (Input.GetKeyDown(key) && !IsMouseButton(key) && !IsJoystickButton(key))
            {
                foundKey = true;
                break;
            }
        }

        // Player pressed something else than keyboard buttons
        if(!foundKey) return;

        if (progressBar != null)
        {
            progressBar.value += progressIncrement;
            Debug.Log($"Progress Bar Increased: {progressBar.value}");
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(keyPressAudio);
            charAmount--;
            FindFirstObjectByType<BobbleHeadManager>().UpdateEmailCharAmount(charAmount.ToString());

            if (charAmount <= 0)
            {
                charAmount = 0;
                isBarFilled = true;
                Debug.Log("Progress Bar Filled. Press ENTER to send.");
            }
        }
        else
        {
            Debug.LogError("ProgressBar is not assigned in EmailTask.");
        }
    }

    // Helper method to check if the key is a mouse button
    private bool IsMouseButton(KeyCode key)
    {
        return key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6;
    }

    // Helper method to check if the key is a joystick button
    private bool IsJoystickButton(KeyCode key)
    {
        return key >= KeyCode.JoystickButton0 && key <= KeyCode.JoystickButton19;
    }

    private void SendEmail()
    {
        if (isBarFilled)
        {
            Debug.Log("Email Sent");
            CompleteMinigame();
            audioSource.PlayOneShot(emailSendAudio);
        }
        else
        {
            Debug.Log("Progress Bar not filled yet. Cannot send email.");
        }
    }

    public override void CompleteMinigame()
    {
        completions++;
        Task task = GetComponent<Task>();
        if (task != null)
        {
            task.CompleteTask();
        }

        Debug.Log("Email Minigame Completed");

        if (task != null && task.quantity > 0)
        {
            ResetMinigame();
        }
        else
        {
            // Task completed entirely
            FindFirstObjectByType<BobbleHeadManager>().UpdateEmailCharAmount("?");
        }
    }

    public override void FailMinigame()
    {
        // Implement failure logic if needed
        Debug.Log("Email Minigame Failed");
    }
}

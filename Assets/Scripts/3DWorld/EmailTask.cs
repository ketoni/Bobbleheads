using UnityEngine;
using UnityEngine.UI;

public class EmailTask : Minigame
{
    public Slider progressBar;
    public float targetProgress = 1f;
    public float progressIncrement = 0.05f;

    private bool isBarFilled = false;
    private int completions = 0;

    void Update()
    {
        if (!IsActive())
            return;

        if (!isBarFilled && Input.anyKeyDown)
        {
            MashKeys();
        }

        if (isBarFilled && Input.GetKeyDown(KeyCode.Return))
        {
            SendEmail();
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
        }
        Debug.Log("Email Minigame Reset");
    }

    private void MashKeys()
    {
        if (progressBar != null)
        {
            progressBar.value += progressIncrement;
            Debug.Log($"Progress Bar Increased: {progressBar.value}");

            if (progressBar.value >= targetProgress)
            {
                isBarFilled = true;
                Debug.Log("Progress Bar Filled. Press ENTER to send.");
            }
        }
        else
        {
            Debug.LogError("ProgressBar is not assigned in EmailTask.");
        }
    }

    private void SendEmail()
    {
        if (isBarFilled)
        {
            Debug.Log("Email Sent");
            CompleteMinigame();
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
            //Destroy(gameObject);
        }
    }

    public override void FailMinigame()
    {
        // Implement failure logic if needed
        Debug.Log("Email Minigame Failed");
    }
}

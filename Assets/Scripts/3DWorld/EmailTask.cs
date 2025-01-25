using UnityEngine;
using UnityEngine.UI;

public class EmailTask : Minigame
{
    public Slider progressBar;
    public float targetProgress = 1f;
    public float progressIncrement = 0.05f;

    private bool isBarFilled = false;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
    }

    void Update()
    {
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
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
        Debug.Log("Email Minigame Started");
    }

    private void MashKeys()
    {
        if (progressBar != null)
        {
            progressBar.value += progressIncrement;
            if (progressBar.value >= targetProgress)
            {
                isBarFilled = true;
                Debug.Log("Progress Bar Filled. Press ENTER to send.");
            }
        }
    }

    private void SendEmail()
    {
        Debug.Log("Email Sent");
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
        Debug.Log("Email Minigame Failed");
    }
}

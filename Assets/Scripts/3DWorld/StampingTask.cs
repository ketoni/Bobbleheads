using UnityEngine;

public class StampingTask : Minigame
{
    private bool isStamped = false;
    private int completions = 0;
    public AudioClip stampAudio;
    public AudioClip movePaperAudio;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsActive())
            return;


        if (Input.GetKeyDown(KeyCode.S))
        {
            StampPaper();
        }

        if (isStamped && Input.GetKeyDown(KeyCode.D))
        {
            MovePaper();
        }
    }

    public override void StartMinigame()
    {
        // Initialize minigame
        isStamped = false;
        completions = 0;
        Debug.Log("Stamping Minigame Started");
    }

    public override void PauseMinigame()
    {
        // Implement pause logic if needed
        Debug.Log("Stamping Minigame Paused");
    }

    public override void ResetMinigame()
    {
        isStamped = false;
        completions = 0;
        Debug.Log("Stamping Minigame Reset");
    }

    private void StampPaper()
    {
        isStamped = true;
        Debug.Log("Paper Stamped");
        audioSource.PlayOneShot(stampAudio);
    }

    private void MovePaper()
    {
        if (isStamped)
        {
            Debug.Log("Paper Moved");
            CompleteMinigame();
            audioSource.PlayOneShot(movePaperAudio);
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

        Debug.Log("Stamping Minigame Completed");

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
        Debug.Log("Stamping Minigame Failed");
    }
}

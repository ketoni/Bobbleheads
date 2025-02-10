using UnityEngine;

public class StampingTask : Minigame
{
    private bool isStamped = false;
    private int completions = 0;
    public AudioClip stampAudio;
    public AudioClip movePaperAudio;
    private AudioSource audioSource;
    public PaperScaler paperScaler;
    public Stamp stamper;
    public GameObject originalPos;
    public GameObject activePos;
    public GameObject slamPos;
    public GameObject paperStack;

    private Task task;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        task = GetComponent<Task>();
    }

    void Update()
    {
        if(task.quantity > 0)
        {
            paperStack.SetActive(true);
        }
        else
        {
            paperStack.SetActive(false);
        }
        if (!IsActive())
            return;

        // Lol not good but its fine:d 1h left
        if(IsActive() && !isStamped)
        {
            stamper.MoveToPosition(activePos);
        }

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
        stamper.MoveToPosition(activePos);
    }

    public override void PauseMinigame()
    {
        // Implement pause logic if needed
        Debug.Log("Stamping Minigame Paused");
        stamper.MoveBackToOriginalPosition();
    }

    public override void ResetMinigame()
    {
        isStamped = false;
        completions = 0;
        Debug.Log("Stamping Minigame Reset");
        stamper.MoveToPosition(activePos);
    }

    private void StampPaper()
    {
        isStamped = true;
        Debug.Log("Paper Stamped");
        audioSource.PlayOneShot(stampAudio);
        stamper.MoveAggressivelyToPositionAndReturn(slamPos, 2f, 1f);
    }

    private void MovePaper()
    {
        if (isStamped)
        {
            Debug.Log("Paper Moved");
            paperScaler.DecreaseScaleY();
            CompleteMinigame();
            audioSource.PlayOneShot(movePaperAudio);
            stamper.MoveToPosition(activePos);
        }
    }

    public override void CompleteMinigame()
    {
        completions++;
        if (task != null)
        {
            task.CompleteTask();
            stamper.MoveToPosition(activePos);
        }

        Debug.Log("Stamping Minigame Completed");

        if (task != null && task.quantity > 0)
        {
            ResetMinigame();
        }
        else
        {
            // Task completed entirely
            stamper.MoveBackToOriginalPosition();
            //Destroy(gameObject);
        }
    }

    public override void FailMinigame()
    {
        // Implement failure logic if needed
        Debug.Log("Stamping Minigame Failed");
    }
}

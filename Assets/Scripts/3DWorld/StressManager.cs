using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }
    public GameObject player;
    public float stress = 0f;
    public float maxStress = 100f;
    public float decreaseAmount = 10f;
    public float increaseInterval = 1f;

    private float stressMultiplier = 0f;
    private float timer = 0f;

    // UI element to represent the stress bar
    public Slider stressBar;
    private bool gameOver = false;

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
        UpdateStressBar();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= increaseInterval)
        {
            timer = 0f;
            UpdateStressMultiplier();
            IncreaseStress(stressMultiplier);
        }

        // Check for game over
        if (stress >= maxStress)
        {
            GameOver();
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneController.Instance.RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneController.Instance.QuitGame();
            }
        }
    }

    private void UpdateStressMultiplier()
    {
        if (TaskManager.Instance != null)
        {
            int pendingTasks = TaskManager.Instance.GetTotalPendingTasks();
            stressMultiplier = pendingTasks;
        }
        else
        {
            stressMultiplier = 0f;
        }
    }

    public void IncreaseStress(float amount)
    {
        stress += amount;
        stress = Mathf.Clamp(stress, 0f, maxStress);
        UpdateStressBar();
    }

    public void DecreaseStress(float amount)
    {
        stress -= amount;
        stress = Mathf.Clamp(stress, 0f, maxStress);
        UpdateStressBar();
    }

    public void MultiplyStress(float amount)
    {
        stress *= amount;
        stress = Mathf.Clamp(stress, 0f, maxStress);
        UpdateStressBar();
    }

    private void UpdateStressBar()
    {
        if (stressBar != null)
        {
            stressBar.value = stress / maxStress;
        }
    }

    private void GameOver()
    {
        gameOver = true;
        player.GetComponent<Player3D>().GameOver();
    }
}

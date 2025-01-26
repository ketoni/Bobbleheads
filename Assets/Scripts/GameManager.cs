using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InputActionAsset inputActions;
    public GameObject enemyPrefab;
    public float spawnCooldown;

    public Transform[] spawnLocations;

    private float timer = 0;
    private InputAction pauseAction;
    public GameObject defaultScreen;
    public GameObject adBreak;
    public TextMeshPro adCounterText;
    public GameObject startGameScreen;
    private List<GameObject> characters;

    public bool paused = true;

    private bool gameStarted = false;
    private GameObject playerHead;

    private float punishTimer = 1000f;

    public float LosingPunishTime = 11f;

    private bool isAdBreak = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startGameScreen.SetActive(true);
        playerHead = GameObject.Find("PlayerHead");
        // Find the action map and action for pausing
        paused = true;
        var actionMap = inputActions.FindActionMap("Player");
        pauseAction = actionMap.FindAction("Quit");
        if (pauseAction != null)
        {
            pauseAction.performed += OnQuitPerformed;
            pauseAction.Enable();
        }
        pauseAction = actionMap.FindAction("EnterGame");
        if (pauseAction != null)
        {
            pauseAction.performed += OnGameEnterPerformed;
            pauseAction.Enable();
        }
        pauseAction = actionMap.FindAction("Throw");
        if (pauseAction != null)
        {
            pauseAction.performed += OnLeftClickPerformed;
            pauseAction.Enable();
        }
        characters = new List<GameObject>
        {
            GameObject.Find("Player")
        };
    }

    // Update is called once per frame
    void Update()
    {
        // StressManager.Instance.DecreaseStress(0.1f);
        if(!paused)
        {
            timer += Time.deltaTime;
            if(timer > spawnCooldown)
            {
                SpawnEnemy();
            }
        }
        else
        {
            punishTimer += Time.deltaTime;
            int timeLeft = (int)(LosingPunishTime-punishTimer);
            if(timeLeft > 0)
            {
                // Ads
                adCounterText.text = timeLeft + "s";
            }
            else
            {
                adBreak.SetActive(false);
                startGameScreen.SetActive(true);
                isAdBreak = false;
            }
        }
    }

    private void SpawnEnemy()
    {
        float minY = spawnLocations[1].position.y;
        float maxY = spawnLocations[0].position.y;
        float randomY = UnityEngine.Random.Range(minY, maxY);
        var enemy = Instantiate(enemyPrefab, new Vector3(spawnLocations[0].position.x, randomY, spawnLocations[0].position.z), quaternion.identity);
        characters.Add(enemy);
        timer = 0;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        characters.Remove(enemy);
    }

    private void OnQuitPerformed(InputAction.CallbackContext context)
    {
        paused = true;
        Physics2D.simulationMode = SimulationMode2D.Script;
        defaultScreen.SetActive(true);
    }

    private void OnGameEnterPerformed(InputAction.CallbackContext context)
    {
        EnterGame();
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        if((!gameStarted && !isAdBreak) || punishTimer > LosingPunishTime)
        {
            StartGame();
            gameStarted = true;
        }
    }

    public void StartGame()
    {
        
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        playerHead.SetActive(true);
        paused = false;
        for (int i = characters.Count - 1; i >= 1; i--)
        {
            Destroy(characters[i]);
            characters.RemoveAt(i); // Optionally, if you need to update the list
        }
        startGameScreen.SetActive(false);
    }

    public void Lose()
    {
        punishTimer = 0;
        paused = true;
        gameStarted = false;
        adBreak.SetActive(true);
        isAdBreak = true;
    }

    public void EnterGame()
    {
        defaultScreen.SetActive(false);
    }
}

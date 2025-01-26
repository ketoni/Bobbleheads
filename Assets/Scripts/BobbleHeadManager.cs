using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobbleHeadManager : MonoBehaviour
{
    public static BobbleHeadManager Instance { get; private set; }
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
    public AudioClip testAudio;

    private bool gameStarted = false;
    private GameObject playerHead;

    private float punishTimer = 1000f;

    public float LosingPunishTime = 11f;
    private MusicManager musicManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultScreen.SetActive(true);
        adBreak.SetActive(false);
        startGameScreen.SetActive(false);
        playerHead = GameObject.Find("PlayerHead");
        // Find the action map and action for pausing
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
        paused = defaultScreen.activeSelf || adBreak.activeSelf || startGameScreen.activeSelf;
        if(!paused)
        {
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
            timer += Time.deltaTime;
            if(timer > spawnCooldown)
            {
                SpawnEnemy();
            }
        }
        else
        {
            Physics2D.simulationMode = SimulationMode2D.Script;
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
        
        defaultScreen.SetActive(true);
    }

    private void OnGameEnterPerformed(InputAction.CallbackContext context)
    {
        EnterGame();
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        if(paused && !defaultScreen.activeSelf)
        {
            if(!gameStarted)
            {
                StartGame();
                gameStarted = true;
            }
            else
            {
                startGameScreen.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        playerHead.SetActive(true);
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
        gameStarted = false;
        adBreak.SetActive(true);
    }

    public void EnterGame()
    {
        defaultScreen.SetActive(false);
        MusicManager.Instance.Play2DGameMusic();
    }

    public void Activate()
    {
        EnterGame();
    }

    public void Exit()
    {
        defaultScreen.SetActive(true);
        MusicManager.Instance.Play3DWorldMusic();
    }
}

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
    public GameObject pauseText;
    private List<GameObject> characters;

    public bool paused = true;

    private bool gameStarted = false;
    private GameObject playerHead;

    public SpriteRenderer overlaySprite;

    private float punishTimer = 1000f;

    public float LosingPunishTime = 11f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHead = GameObject.Find("PlayerHead");
        // Find the action map and action for pausing
        paused = true;
        var actionMap = inputActions.FindActionMap("Player");
        pauseAction = actionMap.FindAction("Pause");
        if (pauseAction != null)
        {
            pauseAction.performed += OnPausePerformed;
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
        pauseText.GetComponent<TextMeshPro>().text = "Left-Click to Start";
        overlaySprite.color = new Color(overlaySprite.color.r, overlaySprite.color.g, overlaySprite.color.b, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused)
        {
            timer += Time.deltaTime;
            if(timer > spawnCooldown)
            {
                SpawnEnemy();
            }
        }
        else{
            punishTimer += Time.deltaTime;
            int timeLeft = (int)(LosingPunishTime-punishTimer);
            if(timeLeft > 0)
            {
                pauseText.GetComponent<TextMeshPro>().text = "Oh no you lost, restart in "+timeLeft+" seconds";
            }
            else
            {
                pauseText.GetComponent<TextMeshPro>().text = "Left-Click to start";
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

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        pauseText.SetActive(!pauseText.activeSelf);
        paused = pauseText.activeSelf;
        // Handle your pause logic here
        if(paused)
        {
            // disable game
            Physics2D.simulationMode = SimulationMode2D.Script;
            pauseText.GetComponent<TextMeshPro>().text = "Paused";
            overlaySprite.color = new Color(overlaySprite.color.r, overlaySprite.color.g, overlaySprite.color.b, 0.5f);
        }
        else
        {
            // active game again
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
            overlaySprite.color = new Color(overlaySprite.color.r, overlaySprite.color.g, overlaySprite.color.b, 0f);
        }
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        if(!gameStarted && punishTimer > LosingPunishTime)
        {
            StartGame();
        }
        gameStarted = true;
    }

    public void StartGame()
    {
        playerHead.SetActive(true);
        pauseText.SetActive(false);
        overlaySprite.color = new Color(overlaySprite.color.r, overlaySprite.color.g, overlaySprite.color.b, 0f);
        paused = false;
        for (int i = characters.Count - 1; i >= 1; i--)
        {
            Destroy(characters[i]);
            characters.RemoveAt(i); // Optionally, if you need to update the list
        }
    }

    public void Lose()
    {
        punishTimer = 0;
        paused = true;
        gameStarted = false;
        pauseText.SetActive(true);
        pauseText.GetComponent<TextMeshPro>().text = "Oh no you lost, restart in "+LosingPunishTime+" seconds";
        overlaySprite.color = new Color(overlaySprite.color.r, overlaySprite.color.g, overlaySprite.color.b, 1f);
    }
}

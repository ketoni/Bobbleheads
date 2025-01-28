using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
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

    public void StartGame()
    {
        // This can be done in many different ways but since I'm creating the Title screen as a separate scene, this is done this way!
        SceneManager.LoadScene("FinalScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("FinalScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

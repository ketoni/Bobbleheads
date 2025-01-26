using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource worldMusic3D;
    [SerializeField] private AudioSource gameMusic2D;

    private readonly float crossFadeTime = 0.5f;

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

    public void Play3DWorldMusic()
    {
        gameMusic2D.FadeOut(crossFadeTime);
        worldMusic3D.FadeIn(crossFadeTime);
    }

    public void Play2DGameMusic()
    {
        worldMusic3D.FadeOut(crossFadeTime);
        gameMusic2D.FadeIn(crossFadeTime);
    }
}
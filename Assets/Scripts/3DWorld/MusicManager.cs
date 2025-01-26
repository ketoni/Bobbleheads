using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource worldMusic3D;
    [SerializeField] private AudioSource gameMusic2D;
    [SerializeField] private AudioSource gameEndingAudio;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip deathSound;

    private readonly float crossFadeTime = 0.5f;
    private bool canFadeToBubbleheads = true;
    private Coroutine fadeIn;
    private Coroutine fadeOut;

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
        if (fadeIn != null)
        {
            StopCoroutine(fadeIn);
        }
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }
        fadeOut = StartCoroutine(FadeOut(gameMusic2D, crossFadeTime));
        fadeIn = StartCoroutine(FadeIn(worldMusic3D, crossFadeTime));
    }

    public void Play2DGameMusic()
    {
        if (fadeIn != null)
        {
            StopCoroutine(fadeIn);
        }
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }
        fadeOut = StartCoroutine(FadeOut(worldMusic3D, crossFadeTime));
        fadeIn = StartCoroutine(FadeIn(gameMusic2D, crossFadeTime));
    }

    private IEnumerator FadeOut(AudioSource a, float duration)
    {
        //float startVolume = a.volume;
        float startVolume = 1;

        while (a.volume > 0)
        {
            a.volume -= startVolume * Time.deltaTime / duration;
            yield return new WaitForEndOfFrame();
        }

        a.Stop();
        a.volume = startVolume;
    }

    private static IEnumerator FadeIn(AudioSource a, float duration)
    {
        float startVolume = 0.2f;

        a.volume = 0;
        a.Play();

        while (a.volume < 1.0f)
        {
            a.volume += startVolume * Time.deltaTime / duration;
            yield return null;
        }

        a.volume = 1f;
    }

    private void FadeOut3DAnd2D()
    {
        if (fadeIn != null)
        {
            StopCoroutine(fadeIn);
        }
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }
        if (worldMusic3D.isPlaying)
        {
            StartCoroutine(FadeOut(worldMusic3D, 0.2f));
        }
        if (gameMusic2D.isPlaying)
        {
            StartCoroutine(FadeOut(gameMusic2D, 0.2f));
        }
    }

    public void PlayVictorySound()
    {
        FadeOut3DAnd2D();
        worldMusic3D.PlayOneShot(victorySound);
    }

    public void PlayDeathSound()
    {
        FadeOut3DAnd2D();
        gameMusic2D.PlayOneShot(deathSound);
    }
}
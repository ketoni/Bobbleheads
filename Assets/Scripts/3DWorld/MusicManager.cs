using System;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource worldMusic3D;
    [SerializeField] private AudioSource gameMusic2D;

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
}
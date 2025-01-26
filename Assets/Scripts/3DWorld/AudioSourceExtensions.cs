using System.Collections;

namespace UnityEngine
{
    public static class AudioSourceExtensions
    {
        public static void FadeOut(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(a, duration));
        }

        private static IEnumerator FadeOutCore(AudioSource a, float duration)
        {
            float startVolume = a.volume;

            while (a.volume > 0)
            {
                a.volume -= startVolume * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

            a.Stop();
            a.volume = startVolume;
        }

        public static void FadeIn(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCore(a, duration));
        }

        private static IEnumerator FadeInCore(AudioSource a, float duration)
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

}

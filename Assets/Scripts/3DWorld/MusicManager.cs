using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource worldMusic3D;
    [SerializeField] private AudioSource gameMusic2D;

    public void Play2DGameMusic()
    {

    }

    public void Play3DWorldMusic()
    {

    }
}
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public abstract void StartMinigame();
    public abstract void CompleteMinigame();
    public abstract void FailMinigame();
}

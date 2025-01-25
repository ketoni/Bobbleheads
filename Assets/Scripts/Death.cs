using UnityEngine;

public interface IDiable
{
    void Die();
}

public abstract class DiableBehaviour : MonoBehaviour, IDiable
{
    public abstract void Die();
}
public class Death : DiableBehaviour
{
    public override void Die()
    {
        GameObject.Find("PlayerHead").SetActive(false);
        FindFirstObjectByType<GameManager>().Lose();
    }
}

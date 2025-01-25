using UnityEngine;

public class EnemyDeath : DiableBehaviour
{
    public override void Die()
    {
        FindFirstObjectByType<GameManager>().RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}

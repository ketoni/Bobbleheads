
public class EnemyDeath : DiableBehaviour
{
    public override void Die()
    {
        FindFirstObjectByType<BobbleHeadManager>().RemoveEnemy(gameObject);
        StressManager.Instance.DecreaseStress(5f);
        Destroy(gameObject);
    }
}

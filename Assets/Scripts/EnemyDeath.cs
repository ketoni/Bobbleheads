
using UnityEngine;

public class EnemyDeath : DiableBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public override void Die()
    {
        FindFirstObjectByType<BobbleHeadManager>().RemoveEnemy(gameObject);
        animator.SetBool("Popping", true);
    }

    void Update()
    {
        // Check if the animation is finished
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Pop") && stateInfo.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}

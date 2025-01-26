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
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void Die()
    {
        animator.SetBool("Popping", true);
    }

    void Update()
    {
        // Check if the animation is finished
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Pop") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("Popping", false);
            GameObject.Find("PlayerHead").SetActive(false);
            FindFirstObjectByType<BobbleHeadManager>().Lose();
        }
    }
}

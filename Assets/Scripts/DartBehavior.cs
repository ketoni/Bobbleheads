using Unity.VisualScripting;
using UnityEngine;

public class DartBehavior : MonoBehaviour
{
    void FixedUpdate()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().linearVelocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleBehavior bubble = collider.gameObject.GetComponent<BubbleBehavior>();
        if(bubble != null && collider.name != "PlayerHead")
        {
            bubble.Pop();
            Destroy(gameObject);
            // StressManager.Instance.DecreaseStress(10f);
            StressManager.Instance.MultiplyStress(0.80f);
        }
    }
}

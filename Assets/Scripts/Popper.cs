using UnityEngine;

public class Popper : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name != "PlayerHead") return;
        BubbleBehavior bubble = collider.gameObject.GetComponent<BubbleBehavior>();
        if(bubble != null)
        {
            bubble.Pop();
        }
    }
}

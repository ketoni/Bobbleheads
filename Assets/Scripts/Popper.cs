using UnityEngine;

public class Popper : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        BubbleBehavior bubble = collider.gameObject.GetComponent<BubbleBehavior>();
        if(bubble != null)
        {
            //animation?
            bubble.Pop();
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public DiableBehaviour diable;
    public void Pop()
    {
        diable.Die();
    }
}

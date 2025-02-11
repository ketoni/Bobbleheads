using UnityEngine;
using TMPro;

public class StressArrow : MonoBehaviour
{
    public void UpdateArrows(int taskQuantity)
    {
        GetComponent<TMP_Text>().text = new string('>', taskQuantity);
    }
}

using UnityEngine;

public class Ads : MonoBehaviour
{
    public void Open() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }

        transform.GetChild(Random.Range(0, 13)).gameObject.SetActive(true);
    }
}

using UnityEngine;

public class Wincon : MonoBehaviour
{
    public GameObject player;
    public void Winnered()
    {
        player.GetComponent<Player3D>().Winning();
    }
}

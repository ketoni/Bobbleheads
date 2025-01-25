using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float moveSpeed = 1f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gameManager.paused)
        {
            Vector3 moveVector = new Vector3(-moveSpeed*Time.fixedDeltaTime, 0, 0);
            transform.position += moveVector;
        }
    }
}

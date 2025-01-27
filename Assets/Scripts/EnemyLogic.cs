using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float moveSpeed = 1f;

    private BobbleHeadManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<BobbleHeadManager>();
        moveSpeed = moveSpeed * Random.Range(0.95f, 1.05f);
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

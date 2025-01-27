using UnityEngine;

public class AirEnemyLogic : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Transform target;
    private BobbleHeadManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<BobbleHeadManager>();
        target = GameObject.Find("PlayerHead").transform;
        moveSpeed = moveSpeed * Random.Range(0.95f, 1.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gameManager.paused)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            Vector3 moveVector = new Vector3(moveSpeed*Time.fixedDeltaTime*direction.x, moveSpeed*Time.fixedDeltaTime*direction.y, 0);
            transform.position += moveVector;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, angle+180);
        }
    }
}

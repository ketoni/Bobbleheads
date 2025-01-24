using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float maxMoveSpeed = 1f;
    public float forcePower = 10f;
    public Rigidbody2D rbBody;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Left-stick or WASD
    private void Move()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        rbBody.AddForce(new Vector2(moveInput.x * forcePower, 0), ForceMode2D.Force);
        if(rbBody.linearVelocity.x > maxMoveSpeed)
        {
            rbBody.linearVelocity = new Vector2(maxMoveSpeed, rbBody.linearVelocity.y);
        }
        else if(rbBody.linearVelocity.x < -maxMoveSpeed)
        {
            rbBody.linearVelocity = new Vector2(-maxMoveSpeed, rbBody.linearVelocity.y);
        }
    }
    // Right-stick or Mouse
    public void Aim(Vector2 direction)
    {
        // !!! should have a struct for input bindings, to allow players to change their controls
        // Should dynamically store somehow delegates or events to trigger depending on input bindings

    }
    // D-pad or ?? on keyboard
    public void Taunt(Vector2 direction)
    {
        
    }
    // L1 or Right-click
    public void Drink(Vector2 direction)
    {
        
    }
    // R1 or Left-click
    public void Throw(Vector2 direction)
    {
        
    }
    //etc
}

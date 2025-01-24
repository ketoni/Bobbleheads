using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmBehavior : MonoBehaviour
{
    private Transform upperArm;
    private Transform lowerArm;
    private PlayerInput playerInput;
    
    // player is on the left side
    private bool leftSide = true;
    private float armLength;

    public GameObject dartPrefab;
    private GameObject dart;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnDart();
        upperArm = transform;
        lowerArm = transform.GetChild(0).GetChild(0).GetChild(0);
        playerInput = transform.parent.parent.parent.GetComponent<PlayerInput>();
        // !!! if screen size changes, this will not apply anymore
        armLength = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y * Screen.height/1080;
    }

    private void FixedUpdate()
    {
        MoveArms();
        Throw();
    }

    private void SpawnDart()
    {
        var hand = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        dart = Instantiate(dartPrefab, hand);
        dart.transform.localPosition = Vector3.zero;
    }

    private void MoveArms()
    {
        Vector2 moveInput = playerInput.actions["Look"].ReadValue<Vector2>();
        
        // it's a mouse
        if(moveInput.magnitude > 1)
        {
            Vector2 camPos = Camera.main.WorldToScreenPoint(transform.position);
            Debug.Log("armLength: "+armLength);
            float reach = armLength * 100; // pixels *100
            Debug.Log("reach: "+reach);
            moveInput = moveInput - camPos;
            Debug.Log("moveInput: "+moveInput);
            moveInput /= reach;
             Debug.Log("moveInput/reach: "+moveInput);
            if(moveInput.magnitude > 1) moveInput.Normalize();
        }

        var angles = InverseKinematics(moveInput.x, moveInput.y, armLength, armLength);
        float theta1Degrees = angles.Item1 * Mathf.Rad2Deg;
        float theta2Degrees = angles.Item2 * Mathf.Rad2Deg;
        lowerArm.eulerAngles = new Vector3(0,0, theta1Degrees);
        upperArm.eulerAngles = new Vector3(0,0, theta2Degrees);
    }

    // Input x, y and lengths of joints
    private (float theta1, float theta2) InverseKinematics(float x, float y, float L1, float L2)
    {
        // return fixed angles if no input
        if(x == 0 && y == 0) return (Mathf.PI*0.5f, 0);

        // Scale target point with arm distance
        float maxReach = L1 + L2;
        x *= maxReach;
        y *= maxReach;

        float value = (x*x+y*y-L1*L1-L2*L2)/(2*L1*L2);
        value = Mathf.Clamp(value, -1, 1); // clamp so acos accepts
        float theta2 = Mathf.Acos(value);
        value = L2*math.sin(theta2)/(L1+L2*math.cos(theta2));
        value = Mathf.Clamp(value, -1, 1); // clamp so atan accepts
        float flipper = x < 0 ? Mathf.PI: 0; // add 180 degrees when x is negative
        float theta1 = Mathf.Atan(y/x) - math.atan(value) + Mathf.PI*0.5f + flipper;
        theta2 = theta1 + theta2;

        // Flip bone angles so that it looks nicer
        if(leftSide)
        {
            float temp = theta1;
            // Fixes weird orientation of the auto-generated bones
            theta1 = theta2 - math.PI*0.5f;
            theta2 = temp;
        }
        else
        {
            // Fixes weird orientation of the auto-generated bones
            theta1 -= math.PI*0.5f;
        }

        return (theta1, theta2);
    }

    private void Throw()
    {


    }
}

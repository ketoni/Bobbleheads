using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ArmBehavior : MonoBehaviour
{
    private BobbleHeadManager gameManager;
    private Transform upperArm;
    private Transform lowerArm;
    private PlayerInput playerInput;
    
    // player is on the left side
    private bool leftSide = true;
    private float armLength;

    public GameObject dartPrefab;
    private GameObject dart;

    private List<Vector2> handPositions;

    public int handPositionFrames = 2;
    private Transform hand;
    public int throwCoolDown = 10;
    private int coolDownCounter;
    public float throwForce = 10f;
    private Vector2 lastHandPos;
    private Vector2 lastMousePos;
    private Vector2 IKInput;
    public float mouseMoveSpeed = 0.0001f;

    private bool leftClick = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IKInput = Vector2.zero;
        gameManager = FindFirstObjectByType<BobbleHeadManager>();
        hand = transform.GetChild(0).GetChild(0);
        handPositions = new List<Vector2>(new Vector2[handPositionFrames]);
        SpawnDart();
        upperArm = transform;
        lowerArm = transform.GetChild(0);
        armLength = transform.parent.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            leftClick = true;
        }
    }

    private void FixedUpdate()
    {
        
        if(!gameManager.paused)
        {
            MoveArms();
            Throw();
        }
    }

    private void SpawnDart()
    {
        dart = Instantiate(dartPrefab, hand);
        dart.transform.localPosition = Vector3.zero;
        dart.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        dart.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void MoveArms()
    {
        Vector2 moveInput = Input.mousePositionDelta;
        IKInput += moveInput * mouseMoveSpeed;

        if(IKInput.magnitude > 1) IKInput.Normalize();

        var angles = InverseKinematics(IKInput.x, IKInput.y, armLength, armLength);
        float theta1Degrees = angles.Item1 * Mathf.Rad2Deg;
        float theta2Degrees = angles.Item2 * Mathf.Rad2Deg;

        lowerArm.eulerAngles = new Vector3(0,0, theta1Degrees);
        upperArm.eulerAngles = new Vector3(0,0, theta2Degrees-90);
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
        
        // Shift positions forward
        for(int i = handPositionFrames-1; i > 0; i--)
        {
            handPositions[i] = handPositions[i-1];
        }
        // Store current pos
        handPositions[0] = (Vector2) hand.position - lastHandPos;
        lastHandPos = hand.position;
        
        // throwing on cooldown
        if(coolDownCounter > 0)
        {
            coolDownCounter--;
            if(coolDownCounter == 0) SpawnDart();
            else return;
        }

        // throw dart
        if(leftClick)
        {
            Vector2 avgSpd = Vector2.zero;
            for(int i = 0; i < handPositionFrames; i++)
            {
                avgSpd += handPositions[i];
            }
            avgSpd = avgSpd / handPositionFrames;
            dart.transform.SetParent(null);
            dart.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            dart.GetComponent<Rigidbody2D>().linearVelocity = avgSpd*throwForce;
            dart.GetComponent<BoxCollider2D>().enabled = true;
            float angle = Mathf.Atan2(avgSpd.y, avgSpd.x) * Mathf.Rad2Deg;
            dart.transform.rotation = Quaternion.Euler(0, 0, angle);
            coolDownCounter = throwCoolDown;
            gameManager.PlayThrowDartAudio();
            leftClick = false;
        }
    }
}

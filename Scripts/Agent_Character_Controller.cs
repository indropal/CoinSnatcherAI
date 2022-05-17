using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Character_Controller : MonoBehaviour
{
    [Tooltip("Maximum slope the character can jump on")]
    [Range(5f, 60f)]
    public float slopeLimit = 45f;
    [Tooltip("Movement Speed of AI Agent")]
    public float moveSpeed = 500f;
    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]
    public float turnSpeed = 300;
    [Tooltip("Whether the character can jump")]
    public bool allowJump = false;
    [Tooltip("Upward speed to apply when jumping in meters/second")]
    public float jumpSpeed = 10f;

    public bool IsGrounded { get; private set; } 
    public float ForwardInput { get; set; } 
    public float TurnInput { get; set; }
    public bool JumpInput { get; set; } 

    private Rigidbody r_body;
    private CapsuleCollider capsuleCollider;

    //private void Awake()
    private void Start()
    {
        r_body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void CheckGrounded()
    {
        //  Checks whether the character is on the ground and updates "IsGrounded"
        IsGrounded = false;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }

    private void ProcessActions()
    {
        if (GameController.instance.gamePlaying)
        {
            // This checks if the initial count down for starting the game has concluded via the GameController instance
            if (TurnInput != 0f)
            {
                // Turning
                float angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
                transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
            }

            // Movement
            Vector3 move = transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed * Time.fixedDeltaTime;
            r_body.MovePosition(transform.position + move);

            // Jump
            if (JumpInput && allowJump && IsGrounded)
            {
                r_body.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
            }
        }  

    }


    private void Update()
    {
        if (GameController.instance.gamePlaying)
        {
            // This checks if the initial count down for starting the game has concluded via the GameController instance
            CheckGrounded();
            ProcessActions();
        }
        else
        {
            ForwardInput = 0.0f;
            TurnInput = 0.0f;
            JumpInput = false;
            transform.eulerAngles = new Vector3(4f, 0f, 0f); 

            float angleOption = Random.Range(0, 5);

            if (GameController.instance.gameEnd)
            {
                transform.eulerAngles = new Vector3(4f, 180f, 0f);
            }
            else
            {
                if (angleOption < 1)
                {
                    transform.eulerAngles = new Vector3(4f, 18f, 0f);
                }

                if ((1 <= angleOption) && (angleOption < 2))
                {
                    transform.eulerAngles = new Vector3(4f, 40f, 0f);
                }

                if ((2 <= angleOption) && (angleOption < 3))
                {
                    transform.eulerAngles = new Vector3(4f, 178f, 0f);
                }

                if ((3 <= angleOption) && (angleOption < 4))
                {
                    transform.eulerAngles = new Vector3(4f, 260f, 0f);
                }

                if ((4 <= angleOption) && (angleOption < 5))
                {
                    transform.eulerAngles = new Vector3(4f, 118f, 0f);
                }
            }
            

        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    
    bool isMovementPressed;
    float rotationFactorInFrame = 5.0f;
    float runFactor = 5.0f;
    bool isJump = false;
    float initialJumpVelo;
    float maxJumpHeight = 1.65f; 
    float maxJumpTime = 0.6f; 
    bool isCharJump = false;
    float gravity = -9.8f;
    float groundedGravity = -0.05f;
    bool jumpAnimating = false;
    int jumpCount = 0; // keep track of the number of times Jump key has been toggled
    Dictionary<int, float> initJumpVelo = new Dictionary<int, float>(); // key-value mapping for different Jump-type velocities ~ accordingly different animation
    Dictionary<int, float> jumpGravity = new Dictionary<int, float>(); // key=value mapping for different Jump gravity influence
    Coroutine currentJumpResetRoutine = null; // coroutine initialiser

    void Awake()
    {
        // 'Awake' method invoked earlier than the 'Start' method in the Update Lifecycle
        
        playerInput = new PlayerInput(); 
        characterController = GetComponent<CharacterController>(); 
        animator = GetComponent<Animator>(); 

        playerInput.CharacterControl.Movement.started += context => {
            
            currentMovementInput = context.ReadValue<Vector2>(); // Store WASD character movement keys ~ player inputs

            // Assign values to respective components => Player movement is in X-Z axes...
            currentMovement.x = currentMovementInput.x * runFactor;
            currentMovement.z = currentMovementInput.y * runFactor;

            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        }; 

        // This is a callback definition which is executed when the user lets go of the input key ~ we use 'canceled' attribute
        playerInput.CharacterControl.Movement.canceled += context => {
            
            currentMovementInput = context.ReadValue<Vector2>(); // Store WASD character movement keys ~ player inputs

            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;

            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        };

        playerInput.CharacterControl.Movement.performed += context => {
            currentMovementInput = context.ReadValue<Vector2>(); // Store WASD character movement keys ~ player inputs

            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;

            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        };

        playerInput.CharacterControl.Jump.started += context => {

            isJump = context.ReadValueAsButton(); //check if jump button os pressed or not
        };

        playerInput.CharacterControl.Jump.canceled += context => {

            isJump = context.ReadValueAsButton(); //check if jump button os pressed or not
        };

        initJumpVars(); // initialise Jump parameters to handle character jump mechanic.
        
    }


    void initJumpVars()
    {
        // initialise character JUMP variables ~ handle character jump
        float timeToApex = maxJumpTime / 2;

        // First-type Jump specifications
        gravity = (-2 * maxJumpHeight) / Mathf.Pow( timeToApex, 2 );
        initialJumpVelo = (2 * maxJumpHeight) / timeToApex;
        
        // Second-type Jump specifications
        float secondJumpGravity = (-2 * (maxJumpHeight + 0.5f)) / Mathf.Pow( (timeToApex * 1.20f), 2 );
        float secondJumpInitialVelo = (2 * (maxJumpHeight + 0.5f)) / (timeToApex * 1.20f);

        // Third-type Jump specifications
        float thirdJumpGravity = (-2 * (maxJumpHeight + 1.0f)) / Mathf.Pow((timeToApex * 1.35f), 2);
        float thirdJumpInitialVelo = (2 * (maxJumpHeight + 1.0f)) / (timeToApex * 1.35f);

        // assign respective jump-velocity definitions to the Dictionary
        initJumpVelo.Add(1, initialJumpVelo);
        initJumpVelo.Add(2, secondJumpInitialVelo);
        initJumpVelo.Add(3, thirdJumpInitialVelo);
        initJumpVelo.Add(4, thirdJumpInitialVelo); // just for safe keeping

        // assign respective jump-gravity definitions to the Dictionary
        jumpGravity.Add(0, gravity);
        jumpGravity.Add(1, gravity);
        jumpGravity.Add(2, secondJumpGravity);
        jumpGravity.Add(3, thirdJumpGravity);
        jumpGravity.Add(4, thirdJumpGravity); // just for safe keeping
    }

    // handle character jump mechanic
    void handleJump()
    {
        if( !isCharJump && characterController.isGrounded && isJump)
        {
            
            if (jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }

            isCharJump = true;

            animator.SetBool("isJumping", true);
            jumpAnimating = true;

            jumpCount += 1; // update the Jump type by incrementing with every Jump Key toogle
            currentMovement.y = initJumpVelo[jumpCount] * 0.5f;

        }
        else if( !isJump && isCharJump && characterController.isGrounded)
        {
            isCharJump = false;
        }
    }

    // Co-Routine function ~ Function pauses execution for sometime & then resumes execution again 
    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(0.15f); // Co-Routine pauses for 0.15 seconds before resuming execution 
        jumpCount = 0;
    }

    // handle movement orientation of character i.e. character should face the direction of movement ~this is done via Quaternion functionality (Unity) / Quaternion Nummber system
    void handleRotation()
    {
        
        Vector3 posLookAt; 
        posLookAt.x = currentMovement.x;
        posLookAt.y = 0.0f;
        posLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation( posLookAt );
            transform.rotation = Quaternion.Slerp( currentRotation, targetRotation, rotationFactorInFrame );
        }
    }

    // Take care of all the animation state changes..
    void handleAnimation()
    {
        // The boolean parameters in the Animator / Animation Controller which governs the animation state changes
        bool isRunning = animator.GetBool("isRunning");

        if ( isMovementPressed && !isRunning )
        {
            // if the player has pressed corresponding movement key & the character hasn't enacted the running animation.. => set corresponding animation Boolean value for animation transition
            animator.SetBool("isRunning", true);
        }
        else if( !isMovementPressed && isRunning )
        {
            // if the player has not pressed corresponding movement key & the character is still enacting the running animation.. => set corresponding animation Boolean value for animation transition
            animator.SetBool("isRunning", false);
        }
    }

    // Take care of the effect of Gravity...
    void handleGravity()
    {
        // check when character initiates fall ~ if the character is falling then we'd want to increase gravity, also to make height of jump adaptive to duration of key press
        bool isFalling = currentMovement.y <= 0.0f || !isJump; 
        float gravityMultiplier = 2.0f;

        if( characterController.isGrounded )
        {
            if (jumpAnimating)
            {
                // disable boolean parameter for character Jump animation
                animator.SetBool("isJumping", false);
                jumpAnimating = false;

                // call co-routine for jump-type re-check ~ prevent reset of jump count because the jump key was pressed before the countdown
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if (jumpCount > 3)
                {
                    jumpCount = 0;
                }
            }

            currentMovement.y = groundedGravity;
        }
        else if ( isFalling )
        {
            // increase steep of character fall while jumping to increase the Steep of fall
            float previousYVelocity = currentMovement.y;
            
            // New implementation ~ Jump gravity varies with the Jump type..
            float newYVelocity = currentMovement.y + (jumpGravity[jumpCount] * gravityMultiplier * Time.deltaTime);

            // apply upper limit of fall to limit insanely high velocity of fall
            float nextYVelocity = Mathf.Max( (previousYVelocity + newYVelocity) * 0.5f, -20.0f );
            
            currentMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            
            // New implementation ~ Jump gravity varies with the Jump type..
            float newYVelocity = currentMovement.y + (jumpGravity[jumpCount] * Time.deltaTime);

            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;

            currentMovement.y = nextYVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Implementing the CountDown Timer check Prior to starting the Game...
        if (GameController.instance.gamePlaying)
        {
            handleRotation(); // check for appropriate orientation of character based on player input
            handleAnimation(); // check for any animation state changes prior to frame update

            // Move the character based on player input
            characterController.Move(currentMovement * 5.15f * Time.deltaTime); 

            handleGravity(); // handle the game gravity mechanic
            handleJump(); // handle the character Jump mechanic
        }
        else
        {
            // if countdown is still yet to complete, then Please enure that character is still...
            currentMovement = Vector3.zero;
            animator.SetBool("isRunning", false);
        }
    }

    // Enable OR Disable Character Controls Action-Map Monobehavior
    void OnEnable()
    {
        // Enable Character Controls Action Map
        playerInput.CharacterControl.Enable();
    }

    void OnDisable()
    {
        // Disable Character Controls Action Map
        playerInput.CharacterControl.Disable();
    }
}

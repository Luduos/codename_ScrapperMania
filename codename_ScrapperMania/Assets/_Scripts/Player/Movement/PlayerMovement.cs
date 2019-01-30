using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool UseGravity { get; set; }

    [SerializeField]
    private Camera playerCam = null;

    [Header("Vertical Movement Variables")]
    [SerializeField]
    private float runSpeed = 0;
    [SerializeField]
    private float walkSpeed = 0;
    [SerializeField]
    private float airMovementMultiplier = 0.8f;

    [Header("Jump Feeling Adjustment")]
    [SerializeField]
    private float jumpStrength = 0;

    [SerializeField]
    [Tooltip("How \"difficult\" is it to get off the ground?")]
    private float stickToGroundForce = 1f;

    [SerializeField]
    [Tooltip("Multiplier while jumping up.")]
    private float jumpGravityMultiplier = 1.0f;

    [SerializeField]
    [Tooltip("Multiplier while falling down.")]
    private float fallGravityMultiplier = 0.8f;

    [SerializeField]
    [Tooltip("Seconds after falling during which we can still jump.")]
    private float timeForJump = 0.5f;

    [Header("Buttons")]
    [SerializeField]
    private PlayerButtons playerButtons = null;

    private CharacterController controller = null;
    private Rigidbody rigidBody = null;

    /// <summary>
    /// Whether or not the player performed a jump since he gleft the ground.
    /// </summary>
    private bool performedJump = false;
    /// <summary>
    /// How long has the player been in the air?
    /// </summary>
    private float inAirTimer = 0f;
 

    private float currentSpeed = 0.0f;
    private Vector3 velocity = Vector3.zero;


    private Vector2 input = Vector2.zero;
    private bool pressedJump = false;

    private void Awake()
    {
        UseGravity = true;

        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {
        GetInput();
        UpdateMovementSpeed();
        if(UseGravity)
            ApplyGravity();
        VerticalMovement();
        Jump();

        controller.Move(velocity * Time.deltaTime);
    }

    private void GetInput()
    {
        float horizontalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.horizontalAxisName);
        float verticalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.verticalAxisName);
        input.Set(horizontalAxis, verticalAxis);

        pressedJump = CrossPlatformInputManager.GetButton(playerButtons.jumpButtonName);
    }

    private void UpdateMovementSpeed()
    {
        if (CrossPlatformInputManager.GetButton(playerButtons.walkButtonName))
            currentSpeed = walkSpeed;
        else
            currentSpeed = runSpeed;

        if (!controller.isGrounded)
            currentSpeed *= airMovementMultiplier;
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            Vector3 gravityVector = Physics.gravity * Time.deltaTime;
            // we are currently going down = falling
            if (Vector3.Dot(rigidBody.velocity, Physics.gravity) > 0)
                velocity += gravityVector * fallGravityMultiplier;
            else
                velocity += gravityVector * jumpGravityMultiplier;
        }
        else
            velocity.y = -stickToGroundForce * Time.deltaTime;
    }

    private void VerticalMovement()
    {
        // projects camera forward vector onto x and z plane       
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerCam.transform.forward, Vector3.up);
        Vector3 verticalMovement = projectedForward * input.y + playerCam.transform.right * input.x;
        verticalMovement.Normalize();

        if (controller.isGrounded)
        {
            velocity.x = verticalMovement.x * currentSpeed;
            velocity.z = verticalMovement.z * currentSpeed;
        }
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            inAirTimer = 0f;
            performedJump = false;
        }
        else
        {
            inAirTimer += Time.fixedDeltaTime;
        }

        // Can't jump if we've been falling too long or if we allready pressed jump
        bool canJump = inAirTimer < timeForJump && !performedJump;
        if (pressedJump && canJump)
        {
            velocity += Vector3.up * jumpStrength;
            performedJump = true;
        }
    }
}

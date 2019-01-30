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
    /// <summary>
    /// How fast is the player when leaving the ground? 
    /// This is used to keep him flying into a certain direction, 
    /// </summary>
    private Vector2 airStartVelocity = Vector2.zero;

    private float currentSpeed = 0.0f;
    private Vector3 currentMovement = Vector3.zero;

    private void Awake()
    {
        UseGravity = true;

        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        UpdateMovementSpeed();
        if(UseGravity)
            ApplyGravity();
        VerticalMovement();
        Jump();

        rigidBody.MovePosition(transform.position + currentMovement * Time.fixedDeltaTime);
        controller.Move(currentMovement * Time.fixedDeltaTime);
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
            Vector3 gravityVector = Physics.gravity * Time.fixedDeltaTime;
            // we are currently going down = falling
            if (Vector3.Dot(rigidBody.velocity, Physics.gravity) > 0)
                currentMovement += gravityVector * fallGravityMultiplier;
            else
                currentMovement += gravityVector * jumpGravityMultiplier;
        }
        else
            currentMovement.y = -stickToGroundForce * Time.fixedDeltaTime;
    }

    private void VerticalMovement()
    {
        float horizontalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.horizontalAxisName);
        float verticalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.verticalAxisName);

        // projects camera forward vector onto x and z plane       
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerCam.transform.forward, Vector3.up);
        Vector3 verticalMovement = projectedForward * verticalAxis + playerCam.transform.right * horizontalAxis;
        verticalMovement.Normalize();

        

        // keep speed in air
        if (controller.isGrounded)
        {
            currentMovement.x = verticalMovement.x * currentSpeed;
            currentMovement.z = verticalMovement.z * currentSpeed;
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
        if (CrossPlatformInputManager.GetButton(playerButtons.jumpButtonName) && canJump)
        {
            currentMovement += Vector3.up * jumpStrength;
            airStartVelocity.Set(rigidBody.velocity.x, rigidBody.velocity.z);
            performedJump = true;
        }
    }
}

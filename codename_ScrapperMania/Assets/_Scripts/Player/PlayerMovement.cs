using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Camera playerCam = null;

    [Header("Ground Movement Variables")]
    [SerializeField]
    private float runSpeed = 0;
    [SerializeField]
    private float walkSpeed = 0;

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

    [SerializeField]
    private MovementButtonInfo movementInfo = new MovementButtonInfo();

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
    private Vector3 movementDirection = Vector3.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        UpdateMovementSpeed();
        ApplyGravity();
        GroundMovement();
        Jump();

        controller.Move(movementDirection * Time.fixedDeltaTime);
    }

    private void UpdateMovementSpeed()
    {
        if (CrossPlatformInputManager.GetButton("Walk"))
            currentSpeed = walkSpeed;
        else
            currentSpeed = runSpeed;
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            Vector3 gravityVector = Physics.gravity * Time.fixedDeltaTime;
            // we are currently going down = falling
            if (Vector3.Dot(controller.velocity, Physics.gravity) > 0)
                movementDirection += gravityVector * fallGravityMultiplier;
            else
                movementDirection += gravityVector * jumpGravityMultiplier;
        }
        else
            movementDirection.y = -stickToGroundForce * Time.fixedDeltaTime;
    }

    private void GroundMovement()
    {
        float horizontalAxis = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float verticalAxis = CrossPlatformInputManager.GetAxisRaw("Vertical");

        // projects camera forward vector onto x and z plane       
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerCam.transform.forward, Vector3.up);
        Vector3 groundMovement = projectedForward * verticalAxis + playerCam.transform.right * horizontalAxis;
        groundMovement.Normalize();
        movementDirection.x = groundMovement.x * currentSpeed;
        movementDirection.z = groundMovement.z * currentSpeed;
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
        if (CrossPlatformInputManager.GetButton("Jump") && canJump)
        {
            movementDirection += Vector3.up * jumpStrength;
            performedJump = true;
        }
    }
}

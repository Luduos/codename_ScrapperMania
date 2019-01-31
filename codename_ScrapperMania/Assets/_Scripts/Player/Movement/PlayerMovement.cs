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
    private float maxRunSpeed = 0f;
    [SerializeField]
    private float maxWalkSpeed = 0f;
    [SerializeField]
    private float moveAcceleration = 0f;
    [SerializeField]
    private float groundFriction = 1.0f;
    public float GroundFriction { get { return groundFriction; } set { groundFriction = value; } }

    [Header("Jump Feeling Adjustment")]
    [SerializeField]
    private float jumpStrength = 0;
    [SerializeField]
    [Range(0f,1f)]
    private float airMovementMultiplier = 0.5f;
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
 
    private float maxSpeed = 0.0f;

    private float currentAcceleration = 0f;
    private Vector3 acceleration = Vector3.zero;
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

    void FixedUpdate()
    {
        acceleration = Vector3.zero;
        GetInput();
        UpdateSpeed();
        if(UseGravity)
            ApplyGravity();
        VerticalMovement();
        Jump();

        if (input.sqrMagnitude < Mathf.Epsilon && controller.isGrounded)
        {
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(controller.velocity, Vector3.up);
            acceleration -= projectedVelocity.normalized * GroundFriction;
        }

        Vector3 oldVelocity = controller.velocity;
        if (controller.isGrounded)
            oldVelocity.y = 0f;

        velocity = oldVelocity + acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0f, velocity.z), maxSpeed) + velocity.y * Vector3.up;

        if (input.sqrMagnitude < Mathf.Epsilon)
        {
            Vector3 projectedVelocityController = Vector3.ProjectOnPlane(controller.velocity, Vector3.up);
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);

            if (Vector3.Dot(projectedVelocity, projectedVelocityController) < 0f || velocity.sqrMagnitude < 1f)
                velocity.Set(0f, velocity.y, 0f);
        }

        controller.Move(velocity * Time.fixedDeltaTime);
    }

    private void GetInput()
    {
        float horizontalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.horizontalAxisName);
        float verticalAxis = CrossPlatformInputManager.GetAxisRaw(playerButtons.verticalAxisName);
        input.Set(horizontalAxis, verticalAxis);

        pressedJump = CrossPlatformInputManager.GetButton(playerButtons.jumpButtonName);
    }

    private void UpdateSpeed()
    {
        if (CrossPlatformInputManager.GetButton(playerButtons.walkButtonName))
            maxSpeed = maxWalkSpeed;
        else
            maxSpeed = maxRunSpeed;

        currentAcceleration = moveAcceleration;
        if (!controller.isGrounded)
            currentAcceleration *= airMovementMultiplier;
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            Vector3 gravityVector = Physics.gravity;
            // we are currently going down = falling
            if (Vector3.Dot(rigidBody.velocity, Physics.gravity) > 0)
                acceleration += gravityVector * fallGravityMultiplier;
            else
                acceleration += gravityVector * jumpGravityMultiplier;
        }
        else
            acceleration.y += -stickToGroundForce;
    }

    private void VerticalMovement()
    {
        // projects camera forward vector onto x and z plane       
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerCam.transform.forward, Vector3.up);
        Vector3 verticalMovement = projectedForward * input.y + playerCam.transform.right * input.x;
        verticalMovement.Normalize();

        acceleration.x += verticalMovement.x * currentAcceleration;
        acceleration.z += verticalMovement.z * currentAcceleration;
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
            acceleration += Vector3.up * jumpStrength;

            pressedJump = false;
            performedJump = true;
        }
    }
}

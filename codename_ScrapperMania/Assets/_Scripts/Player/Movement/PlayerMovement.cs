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
    private Camera _playerCam = null;
    [SerializeField]
    private Hook _hook = null;

    [Header("Vertical Movement Variables")]
    [SerializeField]
    private float _maxRunSpeed = 15f;
    [SerializeField]
    private float _maxWalkSpeed = 10f;
    [SerializeField]
    private float _moveAcceleration = 125f;
    [SerializeField]
    private float _groundFriction = 250f;
    public float GroundFriction { get { return _groundFriction; } set { _groundFriction = value; } }

    [Header("Jump Feeling Adjustment")]
    [SerializeField]
    private float _jumpStrength = 1250;
    [SerializeField]
    [Range(0f,1f)]
    private float _airMovementMultiplier = 0.2f;
    [SerializeField]
    [Tooltip("How \"difficult\" is it to get off the ground?")]
    private float _stickToGroundForce = 50f;
    [SerializeField]
    [Tooltip("Multiplier while jumping up.")]
    private float _jumpGravityMultiplier = 5.0f;
    [SerializeField]
    [Tooltip("Multiplier while falling down.")]
    private float _fallGravityMultiplier = 3.0f;
    [SerializeField]
    [Tooltip("Seconds after falling during which we can still jump.")]
    private float _timeForJump = 0.25f;

    [Header("Buttons")]
    [SerializeField]
    private PlayerButtons _playerButtons = null;

    private CharacterController _controller = null;
    private Rigidbody _rigidBody = null;

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

        _controller = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        acceleration = Vector3.zero;
        GetInput();
        UpdateSpeed();

        //if (UseGravity && !_hook.Info.IsHooking)
        if(UseGravity)
            ApplyGravity();

        VerticalMovement();
        Jump();

        if(!_hook.Info.IsHooking)
            ApplyFriction();

        CalculateNormalVelocity();
        if (_hook.Info.IsHooking)
            velocity += _hook.Info.HookAcceleration * Time.fixedDeltaTime;

        _controller.Move(velocity * Time.fixedDeltaTime);
    }

    

    private void GetInput()
    {
        float horizontalAxis = CrossPlatformInputManager.GetAxisRaw(_playerButtons.horizontalAxisName);
        float verticalAxis = CrossPlatformInputManager.GetAxisRaw(_playerButtons.verticalAxisName);
        input.Set(horizontalAxis, verticalAxis);

        pressedJump = CrossPlatformInputManager.GetButton(_playerButtons.jumpButtonName);
    }

    private void UpdateSpeed()
    {
        if (CrossPlatformInputManager.GetButton(_playerButtons.walkButtonName))
            maxSpeed = _maxWalkSpeed;
        else
            maxSpeed = _maxRunSpeed;

        currentAcceleration = _moveAcceleration;
        if (!_controller.isGrounded)
            currentAcceleration *= _airMovementMultiplier;
    }

    private void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            Vector3 gravityVector = Physics.gravity;
            // we are currently going down = falling
            if (Vector3.Dot(_rigidBody.velocity, Physics.gravity) > 0)
                acceleration += gravityVector * _fallGravityMultiplier;
            else
                acceleration += gravityVector * _jumpGravityMultiplier;
        }
        else
            acceleration.y += -_stickToGroundForce;
    }

    private void VerticalMovement()
    {
        // projects camera forward vector onto x and z plane       
        Vector3 projectedForward = Vector3.ProjectOnPlane(_playerCam.transform.forward, Vector3.up);
        Vector3 verticalMovement = projectedForward * input.y + _playerCam.transform.right * input.x;
        verticalMovement.Normalize();

        acceleration.x += verticalMovement.x * currentAcceleration;
        acceleration.z += verticalMovement.z * currentAcceleration;
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {
            inAirTimer = 0f;
            performedJump = false;
        }
        else
        {
            inAirTimer += Time.fixedDeltaTime;
        }

        // Can't jump if we've been falling too long or if we allready pressed jump
        bool canJump = inAirTimer < _timeForJump && !performedJump;
        if (pressedJump && canJump)
        {
            acceleration += Vector3.up * _jumpStrength;

            pressedJump = false;
            performedJump = true;
        }
    }

    private void ApplyFriction()
    {
        if (input.sqrMagnitude < Mathf.Epsilon && _controller.isGrounded)
        {
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_controller.velocity, Vector3.up);
            acceleration -= projectedVelocity.normalized * GroundFriction;
        }
    }

    /// <summary>
    /// Calculates and adjusts velocity, like clamping on the movement plane or settings up/down velocity to zero, if grounded.
    /// </summary>
    private void CalculateNormalVelocity()
    {
        // if we are grounded, set the y velocity to 0, makes every jump jump to the same height
        // otherwise there will be negative velocity that's left from earlier frames and quick jumps
        // one after another will result in random jump heights
        Vector3 oldVelocity = _controller.velocity;
        if (_controller.isGrounded)
            oldVelocity.y = 0f;

        velocity = oldVelocity + acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0f, velocity.z), maxSpeed) + velocity.y * Vector3.up;

        if (input.sqrMagnitude < Mathf.Epsilon)
        {
            Vector3 projectedVelocityController = Vector3.ProjectOnPlane(_controller.velocity, Vector3.up);
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);

            if (Vector3.Dot(projectedVelocity, projectedVelocityController) < 0f || velocity.sqrMagnitude < 1f)
                velocity.Set(0f, velocity.y, 0f);
        }

    }
}

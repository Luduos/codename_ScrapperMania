using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player/MovementInfo")]
public class PlayerMovementInfo : ScriptableObject
{
    [Header("Vertical Movement Variables")]
    [SerializeField]
    public float maxRunSpeed = 15f;
    [SerializeField]
    public float maxWalkSpeed = 10f;
    [SerializeField]
    public float moveAcceleration = 125f;
    [SerializeField]
    public float groundFriction = 250f;


    [Header("Jump Feeling Adjustment")]
    [SerializeField]
    public float jumpStrength = 1250;
    [SerializeField]
    [Range(0f, 1f)]
    public float airMovementMultiplier = 0.2f;
    [SerializeField]
    [Tooltip("How \"difficult\" is it to get off the ground?")]
    public float stickToGroundForce = 50f;
    [SerializeField]
    [Tooltip("Multiplier while jumping up.")]
    public float jumpGravityMultiplier = 5.0f;
    [SerializeField]
    [Tooltip("Multiplier while falling down.")]
    public float fallGravityMultiplier = 3.0f;
    [SerializeField]
    [Tooltip("Seconds after falling during which we can still jump.")]
    public float timeForJump = 0.25f;
}

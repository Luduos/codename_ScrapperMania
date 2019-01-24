using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicMovement : MonoBehaviour
{
    [SerializeField]
    Camera playerCam = null;

    [SerializeField]
    private float runSpeed = 0;
    [SerializeField]
    private float walkSpeed = 0;
    [SerializeField]
    private float jumpStrength = 0;
    [SerializeField]
    private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

    private CharacterController controller = null;

    private float currentSpeed = 0.0f;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        UpdateMovementSpeed();
        // ApplyGravity();
        GroundMovement();
        CheckJump();
    }

    private void UpdateMovementSpeed()
    {
        if (Input.GetButton("Walk"))
            currentSpeed = walkSpeed;
        else
            currentSpeed = runSpeed;
    }

    private void ApplyGravity()
    {
        if(!controller.isGrounded)
            controller.Move(gravity * Time.deltaTime);
    }

    private void GroundMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        // projects camera forward vector onto x and z plane
        Vector3 projectedForward = new Vector3(playerCam.transform.forward.x, 0f, playerCam.transform.forward.z); 
        Vector3 groundMovement = projectedForward * verticalAxis + playerCam.transform.right * horizontalAxis;
        groundMovement.Normalize();
        controller.Move(groundMovement * currentSpeed * Time.deltaTime);
    } 

    private void CheckJump()
    {
        if(Input.GetButton("Jump") && controller.isGrounded)
        {
            controller.Move(Vector3.up * jumpStrength * Time.deltaTime);
        }
    }
}

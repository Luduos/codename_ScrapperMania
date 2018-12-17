using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 0;

    [SerializeField]
    private float walkSpeed = 0;

    [SerializeField]
    private float jumpStrength = 0;

    private Rigidbody player = null;
    private CapsuleCollider playerCollider = null;
    private Vector3 playerSize = Vector3.zero;
    private float movementSpeed = 0.0f;
    private bool isWalking = false;
    private bool pressedJump = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = this.GetComponent<Rigidbody>();
        playerCollider = this.GetComponent<CapsuleCollider>();
        playerSize = playerCollider.bounds.size;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movementHandler();
        jumpHandler();
        if (Input.GetButton("Walk"))
            isWalking = true;
        else
            isWalking = false;
    }

    private void movementHandler()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if(hAxis != 0 || vAxis != 0)
        {
            checkMovementMode();
            Vector3 movement = new Vector3(hAxis * movementSpeed * Time.deltaTime, 0.0f, vAxis * movementSpeed * Time.deltaTime);
            Vector3 newPos = transform.position + movement;
            player.MovePosition(newPos);

            Vector3 moveDirection = new Vector3(hAxis, 0, vAxis);
            player.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void checkMovementMode()
    {
        if (isWalking)
            movementSpeed = walkSpeed;
        else
            movementSpeed = runSpeed;
    }

    private void jumpHandler()
    {
        float jAxis = Input.GetAxis("Jump");

        if (jAxis > 0)
        {
            bool isGrounded = checkGrounded();
            if(!pressedJump && isGrounded)
            {
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0.0f, jAxis * jumpStrength, 0.0f);
                player.AddForce(jumpVector, ForceMode.VelocityChange);
            }
        }
        else
            pressedJump = false;
    }

    private bool checkGrounded()
    {
        Vector3 corner1 = transform.position + new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        Vector3 corner2 = transform.position + new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, playerSize.z / 2);
        Vector3 corner3 = transform.position + new Vector3(playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);
        Vector3 corner4 = transform.position + new Vector3(-playerSize.x / 2, -playerSize.y / 2 + 0.01f, -playerSize.z / 2);
        Vector3 middle = transform.position;

        bool grounded1 = Physics.Raycast(corner1, -Vector3.up, 0.02f);
        bool grounded2 = Physics.Raycast(corner2, -Vector3.up, 0.02f);
        bool grounded3 = Physics.Raycast(corner3, -Vector3.up, 0.02f);
        bool grounded4 = Physics.Raycast(corner4, -Vector3.up, 0.02f);
        bool grounded5 = Physics.Raycast(middle, -Vector3.up, 0.02f);

        return (grounded1 || grounded2 || grounded3 || grounded4 || grounded5);
    }
}

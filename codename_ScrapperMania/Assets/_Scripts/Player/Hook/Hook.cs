using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    

    [Header("Hook feeling")]
    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxRange = 50.0f;
    [SerializeField]
    private float hookStrength = 20.0f;

    [Header("Script Access")]
    [SerializeField]
    private Camera playerCamera = null;
    [SerializeField]
    private PlayerMovement playerMovement = null;
    [SerializeField]
    private CharacterController controller = null;
    [SerializeField]
    private HookVisualizer visualizer = null;

    [Header("Buttons")]
    [SerializeField]
    private PlayerButtons playerButtons = null;

    private bool isHooking = false;
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(playerButtons.hookButton))
            StartHook();

        if (isHooking && Input.GetButton(playerButtons.hookButton))
            UpdateHook();
        if (Input.GetButtonUp(playerButtons.hookButton))
            StopHook();
        
    }

    private void StartHook()
    {
        //playerMovement.UseGravity = false;

        RaycastHit hit;
        isHooking = Physics.Raycast(this.transform.position, playerCamera.transform.forward, out hit, maxRange);
        if (isHooking)
        {
            this.hit = hit;
            visualizer.ShowHookStart();
        }
        else
        {
            visualizer.ShowHookFail();
        }

    }

    private void UpdateHook()
    {
        Vector3 hookToHit = hit.point - this.transform.position;
        if(hookToHit.magnitude < minDistance )
        {
            StopHook();
        }
        else
        {
            Vector3 hookMovement = hookToHit * hookStrength;
            controller.Move(hookMovement * Time.fixedDeltaTime);

            visualizer.ShowHookUpdate(this.transform.position, hit.point);
        }
    }

    private void StopHook()
    {
        isHooking = false;
        //playerMovement.UseGravity = true;
        visualizer.ShowHookEnd();
    }
}

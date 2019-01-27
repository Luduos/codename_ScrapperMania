using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    private HookButtonInfo hookButtons = new HookButtonInfo();

    [Header("Hook feeling")]
    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxRange = 50.0f;
    [SerializeField]
    private float hookStrength = 20.0f;

    [Header("Script Access")]
    
   
    [SerializeField]
    [Tooltip("The object from which the hook originates (comes out from)")]
    private Transform hookOrigin = null;

    [SerializeField]
    private CharacterController controller = null;

    [SerializeField]
    private PlayerMovement playerMovement = null; 

    [SerializeField]
    private Camera playerCamera = null;

    [SerializeField]
    private HookVisualizer visualizer = null;

    private bool isHooking = false;
    private RaycastHit hit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHooking)
        {
            if (Input.GetButtonDown(hookButtons.HookButton))
                StartHook();
        }
        else
        {
            if (Input.GetButton(hookButtons.HookButton))
                UpdateHook();
            if (Input.GetButtonUp(hookButtons.HookButton))
                StopHook();
        }
    }

    private void StartHook()
    {
        playerMovement.UseGravity = false;

        RaycastHit hit;
        bool registeredHit = Physics.Raycast(hookOrigin.transform.position, playerCamera.transform.forward, out hit, maxRange);
        if (registeredHit)
        {
            isHooking = true;
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
        Vector3 hookToHit = hit.point - hookOrigin.transform.position;
        if(hookToHit.magnitude < minDistance )
        {
            StopHook();
        }
        else
        {
            Vector3 hookMovement = hookToHit.normalized * hookStrength;
            controller.Move(hookMovement * Time.fixedDeltaTime);

            visualizer.ShowHookUpdate();
        }
    }

    private void StopHook()
    {
        isHooking = false;
        playerMovement.UseGravity = true;

        visualizer.ShowHookEnd();
    }
}

[System.Serializable]
public class HookButtonInfo
{
    public string HookButton = "Fire1";
}

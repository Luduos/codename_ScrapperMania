using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
    }

    private void FixedUpdate()
    {
        MouseLookRotation();
    }

    public void MouseLookRotation()
    {
        float mouseX = CrossPlatformInputManager.GetAxis("Mouse X");
        float mouseY = -CrossPlatformInputManager.GetAxis("Mouse Y");

        rotX += mouseX * mouseSensitivity * Time.fixedDeltaTime;
        rotY += mouseY * mouseSensitivity * Time.fixedDeltaTime;

        rotY = Mathf.Clamp(rotY, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0.0f);
        transform.rotation = localRotation;
    }
}
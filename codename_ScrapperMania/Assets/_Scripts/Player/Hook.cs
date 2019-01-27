using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    private HookButtonInfo hookButtons = new HookButtonInfo();

    [SerializeField]
    private LineRenderer debugLine = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(hookButtons.HookButton))
            StartHook();
        else if (Input.GetButtonUp(hookButtons.HookButton))
            StopHook();
    }

    private void StartHook()
    {

    }

    private void StopHook()
    {

    }
}

[System.Serializable]
public class HookButtonInfo
{
    public string HookButton = "Fire1";
}

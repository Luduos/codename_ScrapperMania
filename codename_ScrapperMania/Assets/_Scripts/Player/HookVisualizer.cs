using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookVisualizer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer = null;

    public void ShowHookFail()
    {
        Debug.Log("Hook Fail.");
    }

    public void ShowHookStart()
    {
        Debug.Log("Start Hook.");
    }

    public void ShowHookUpdate()
    {

    }

    public void ShowHookEnd()
    {
        Debug.Log("End Hook.");

    }
}

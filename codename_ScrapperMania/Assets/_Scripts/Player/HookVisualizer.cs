using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookVisualizer : MonoBehaviour
{
    [SerializeField]
    private float lineWidth = .25f;
    [SerializeField]
    private LineRenderer lineRenderer = null;

    private void Start()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    public void ShowHookFail()
    {
        Debug.Log("Hook Fail.");
    }

    public void ShowHookStart()
    {
        lineRenderer.enabled = true;
    }

    public void ShowHookUpdate(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPositions(new [] { start, end });
    }

    public void ShowHookEnd()
    {
        Debug.Log("End Hook.");

        lineRenderer.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookVisualizer : MonoBehaviour
{
    [SerializeField]
    private float _lineWidth = .25f;
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    private void Start()
    {
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
    }

    public void ShowHookFail()
    {
        Debug.Log("Hook Fail.");
    }

    public void ShowHookStart()
    {
        _lineRenderer.enabled = true;
    }

    public void ShowHookUpdate(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPositions(new [] { start, end });
    }

    public void ShowHookEnd()
    {
        Debug.Log("End Hook.");

        _lineRenderer.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    private HookInfo _info = null;
   

    [Header("Script Access")]
    [SerializeField]
    private Camera _playerCamera = null;
    [SerializeField]
    private HookVisualizer _visualizer = null;

    [Header("Buttons")]
    [SerializeField]
    private PlayerButtons _playerButtons = null;

    private HookHit _hit = new HookHit();
    public HookHit Hit { get { return _hit; } private set { _hit = value; } }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hit.IsHooking && Input.GetButtonDown(_playerButtons.hookButton))
            StartHook();
        if (_hit.IsHooking && Input.GetButton(_playerButtons.hookButton))
            UpdateHook();
        if (_hit.IsHooking && Input.GetButtonUp(_playerButtons.hookButton))
            StopHook();       
    }

    private void StartHook()
    {
        RaycastHit hit;
        _hit.IsHooking = Physics.Raycast(this.transform.position, _playerCamera.transform.forward, out hit, _info.maxRange);
        if (_hit.IsHooking)
        {
            _hit.Hit = hit;
            _visualizer.ShowHookStart();
        }
        else
        {
            _visualizer.ShowHookFail();
        }

    }

    private void UpdateHook()
    {
        _hit.UseGravity = _info.useGravity;

        Vector3 hookToHit = _hit.Hit.point - this.transform.position;
        _hit.Length = hookToHit.magnitude;

        float hookStrengthMultiplier = Mathf.Clamp(_hit.Length, 0f, _info.maxHookStrengthDistance);
        if(_hit.Length < _info.minDistance )
        {
            StopHook();
        }
        else
        {
            _hit.HookNormal = hookToHit.normalized;
            _hit.HookAcceleration = _hit.HookNormal * hookStrengthMultiplier * _info.hookStrength;

            _visualizer.ShowHookUpdate(this.transform.position, _hit.Hit.point);
        }
    }

    private void StopHook()
    {
        _hit.IsHooking = false;
        _visualizer.ShowHookEnd();
    }
}

/// <summary>
/// Information about the current hook hit
/// </summary>
public class HookHit
{
    public bool IsHooking { get; set; }
    public bool UseGravity { get; set; }
    public float Length { get; set; }
    public Vector3 HookNormal { get; set; }
    public Vector3 HookAcceleration { get; set; }
    public RaycastHit Hit { get; set; }
}
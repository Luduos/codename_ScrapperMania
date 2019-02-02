using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [Header("Hook feeling")]
    [SerializeField]
    [Tooltip("If the hook length is below this threshold, the hook gets canceled")]
    private float _minDistance = 1.0f;
    /// <summary>
    /// For distances below this the hook strength is also dependent on the distance.
    /// Otherwise the hook strength is clamped to this value.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance from which on the distance dependent strength is clamped.")]
    private float _maxHookStrengthDistance = 50;
    [SerializeField]
    [Tooltip("If a hook is shot and goes beyond this distance, the hook will fail to start.")]
    private float _maxRange = 100f;

    [SerializeField]
    private float _hookStrength = 20.0f;
   

    [Header("Script Access")]
    [SerializeField]
    private Camera _playerCamera = null;
    [SerializeField]
    private HookVisualizer _visualizer = null;

    [Header("Buttons")]
    [SerializeField]
    private PlayerButtons _playerButtons = null;

    private HookInfo _info = new HookInfo();
    public HookInfo Info { get { return _info; } private set { _info = value; } }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_info.IsHooking && Input.GetButtonDown(_playerButtons.hookButton))
            StartHook();
        if (_info.IsHooking && Input.GetButton(_playerButtons.hookButton))
            UpdateHook();
        if (_info.IsHooking && Input.GetButtonUp(_playerButtons.hookButton))
            StopHook();       
    }

    private void StartHook()
    {
        RaycastHit hit;
        _info.IsHooking = Physics.Raycast(this.transform.position, _playerCamera.transform.forward, out hit, _maxRange);
        if (_info.IsHooking)
        {
            _info.Hit = hit;
            _visualizer.ShowHookStart();
        }
        else
        {
            _visualizer.ShowHookFail();
        }

    }

    private void UpdateHook()
    {
        Vector3 hookToHit = _info.Hit.point - this.transform.position;
        _info.Length = hookToHit.magnitude;

        float hookStrengthMultiplier = Mathf.Clamp(_info.Length, 0f, _maxHookStrengthDistance);
        if(_info.Length < _minDistance )
        {
            StopHook();
        }
        else
        {
            _info.HookNormal = hookToHit.normalized;
            _info.HookAcceleration = _info.HookNormal * hookStrengthMultiplier * _hookStrength;

            _visualizer.ShowHookUpdate(this.transform.position, _info.Hit.point);
        }
    }

    private void StopHook()
    {
        _info.IsHooking = false;
        _visualizer.ShowHookEnd();
    }
}

public class HookInfo
{
    public bool IsHooking { get; set; }
    public float Length { get; set; }
    public Vector3 HookNormal { get; set; }
    public Vector3 HookAcceleration { get; set; }
    public RaycastHit Hit { get; set; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Hook Info")]
public class HookInfo : ScriptableObject
{
    [Header("Hook feeling")]
    [SerializeField]
    [Tooltip("If the hook length is below this threshold, the hook gets canceled")]
    public float minDistance = 1.0f;
    /// <summary>
    /// For distances below this the hook strength is also dependent on the distance.
    /// Otherwise the hook strength is clamped to this value.
    /// </summary>
    [SerializeField]
    [Tooltip("Distance from which on the distance dependent strength is clamped.")]
    public float maxHookStrengthDistance = 50;
    [SerializeField]
    [Tooltip("If a hook is shot and goes beyond this distance, the hook will fail to start.")]
    public float maxRange = 100f;

    [SerializeField]
    public float hookStrength = 20.0f;
    [SerializeField]
    [Tooltip("Use gravity during the hook.")]
    public bool useGravity = false;
}

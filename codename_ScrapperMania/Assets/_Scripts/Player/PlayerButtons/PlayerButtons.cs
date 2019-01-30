using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerButtons : ScriptableObject
{
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";

    public string jumpButtonName = "Jump";
    public string walkButtonName = "Walk";
    public string hookButton = "Fire1";

    public string debugRespawn = "Debug_Respawn";
}

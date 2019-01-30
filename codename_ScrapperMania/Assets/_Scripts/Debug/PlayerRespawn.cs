using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    CharacterController player = null;

    [SerializeField]
    PlayerButtons playerButtons = null;

    // Update is called once per frame
    void Update()
    {
        // Debug_Respawn with F1
        if (Input.GetButtonDown(playerButtons.debugRespawn))
        {
            player.enabled = false;
            player.transform.position = this.transform.position + Vector3.up * player.height;
            player.enabled = true; 
        }
    }
}

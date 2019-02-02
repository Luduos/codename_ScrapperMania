using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    CharacterController _player = null;

    [SerializeField]
    PlayerButtons _playerButtons = null;

    // Update is called once per frame
    void Update()
    {
        // Debug_Respawn with F1
        if (Input.GetButtonDown(_playerButtons.debugRespawn))
        {
            _player.enabled = false;
            _player.transform.position = this.transform.position + Vector3.up * _player.height;
            _player.enabled = true; 
        }
    }
}

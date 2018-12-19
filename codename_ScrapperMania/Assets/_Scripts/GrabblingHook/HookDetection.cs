using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetection : MonoBehaviour
{
    GameObject player;

   void OnTriggerEnter(Collider other)
    {
        //Checks if we hit a hookableObj
        if (other.tag.Equals("hookable"))
        {  
            //sets isHooked to true
            player.GetComponent<Hook>().setIsHooked(true);

        }
    }
}

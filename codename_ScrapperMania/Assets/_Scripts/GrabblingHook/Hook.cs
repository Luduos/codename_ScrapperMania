using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    
    public GameObject hook;
    public GameObject hookHolder;

    public float hookTravelSpeed;
    public float playerTravelSpeed;
    public float maxDistance;
    public float currentDistance;


    private static bool ishooked;
    public static bool fired;

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !fired)
        {

            fired = true;
        }
        if (fired)
        {
            //Move the Hook torwads pointed direction
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);

            // calculates the current traveldistance of the hook relative to the position of the Player
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
            {   
                //called when the maxDistance of our Hook is reached
                ReturnHook();
            }
        }
        
    }



    void ReturnHook()
    {
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        ishooked = false;

    }

    public bool getHooked()
    {

        return ishooked;
    }

    public void setHooked( bool updatedIsHooked)
    {
        ishooked = updatedIsHooked;
    }
}

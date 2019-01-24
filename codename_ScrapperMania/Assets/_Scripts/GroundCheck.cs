using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded
    {
        get { return numGroundTouches > 0; }
    }

    private int numGroundTouches = 0;
    private void Start()
    {
    }

    private void LateUpdate()
    {
        numGroundTouches = 0;

    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 toNearest = transform.position - other.ClosestPointOnBounds(transform.position);
        if (Vector3.Dot(toNearest, Vector3.up) > 0)
            numGroundTouches++;
    }

  
}

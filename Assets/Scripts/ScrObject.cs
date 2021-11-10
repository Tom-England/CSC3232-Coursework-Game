using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrObject : MonoBehaviour
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    /// <summary> method OnTriggerEnter
    /// Detects if the object has been hit and applys a force in the direction away from the player to its rigid body.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Vector3 dir = helper.CalculateDirection(transform.position, other.transform.position);
            GetComponent<Rigidbody>().AddForce(dir * -500, ForceMode.Force);
        }
    }
}

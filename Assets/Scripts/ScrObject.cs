using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrObject : MonoBehaviour
{
    Vector3 CalculateDirection(Vector3 other)
    {
        Vector3 dir = other - transform.position;
        Vector3 norm_dir = dir.normalized;
        return norm_dir;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Vector3 dir = CalculateDirection(other.transform.position);
            GetComponent<Rigidbody>().AddForce(dir * -500, ForceMode.Force);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrThrowable : MonoBehaviour
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    public Rigidbody rb;
    public GameObject player;
    public int throw_force = 1000;

    /// <summary> method OnCollisionEnter
    /// Removes the damage tag from the thrown object once it hits the floor
    /// This is done so you do not get damaged walking over old thrown objects
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            this.tag = "Untagged";
        }
    }

    /// <summary> method Start
    /// Locates the player in the scene then applys a force towards them.
    /// This was done by the enemy when creating the object but this failed
    /// as the rigidbody was not fully active when the force was being added
    /// </summary>
    void Start()
    {
        player = GameObject.Find("Player");
        Vector3 direction = helper.CalculateDirection(transform.position, player.transform.position);
        rb.AddForce(direction * throw_force, ForceMode.Force);
    }
}

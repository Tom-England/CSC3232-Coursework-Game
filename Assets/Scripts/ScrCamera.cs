using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrCamera : MonoBehaviour
{

    public GameObject player;
    Vector3 new_pos;
    /// <summary> method Update
    /// Method causes the camera to follow the player by translating along the x axis and panning on the y and z axis
    /// </summary>
    void Update()
    {

        new_pos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = new_pos;
        transform.LookAt(player.transform.position);
    }
}

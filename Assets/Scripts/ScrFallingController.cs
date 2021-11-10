using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ScrFallingController : MonoBehaviour
{

    public Vector3 area = new Vector3(10, 1, 10);
    public float height;
    public Double spawn_timer = 5f;
    Double elapsed_time = 0;

    public GameObject[] hazards;
    System.Random rnd = new System.Random();

    /// <summary> method Update
    /// Attempts to generate a new falling object hazard within the provided range.
    /// New object is set to destroy itself after 10 seconds of existing to reduce clutter
    /// </summary>
    void Update()
    {
        if (elapsed_time > spawn_timer)
        {
            // This generates a position to spawn the object in by creating an offset
            // based on the defined area of the fall zone
            // It then adds it onto the lower bound of the zone
            // Then creates the new object  in the air above the zone

            elapsed_time = 0;
            double offset_x = rnd.Next((int)area.x) + rnd.NextDouble();
            double offset_z = rnd.Next((int)area.z) + rnd.NextDouble();

            Vector3 new_pos = transform.position;
            new_pos.x -= area.x / 2;
            new_pos.z -= area.z / 2;

            new_pos.x += (float)offset_x;
            new_pos.z += (float)offset_z;

            new_pos.y += height;

            int hazard_index = rnd.Next(hazards.Length);
            GameObject obj;
            obj = Instantiate(hazards[hazard_index], new_pos, Quaternion.identity);
            obj.transform.Rotate(20 * Time.deltaTime, 10 * Time.deltaTime, 50 * Time.deltaTime);
            Destroy(obj, 10);
        }
        else
        {
            elapsed_time += Time.deltaTime;
        }
    }

    public bool showInspectorBox = true;

    /// <summary> method OnDrawGizmos
    /// This is helper code that draws a box in the Unity inspector to highlight the area the falling zone exists in.
    /// Not visible in game.
    /// </summary>
    void OnDrawGizmos()
    {
        if (showInspectorBox)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, area);
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + height, transform.position.z), 1);
        }
    }
}

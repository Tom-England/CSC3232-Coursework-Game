using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDamageZone : MonoBehaviour
{

    public BoxCollider col;

    // Fills the box collider in with red in the inspector.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, col.size);
    }
}

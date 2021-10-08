using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDamageZone : MonoBehaviour
{

    public BoxCollider col;
    public bool showInspectorBox = true;

    // Fills the box collider in with red in the inspector.
    void OnDrawGizmos()
    {
        if (showInspectorBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, col.size);
        }
    }
}

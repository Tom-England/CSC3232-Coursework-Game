using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDamageZone : MonoBehaviour
{

    public BoxCollider col;
    public bool showInspectorBox = true;

    /// <summary> method OnDrawGizmos
    /// This is helper code that draws a box in the Unity inspector to highlight the area the damagezone exists in.
    /// Not visible in game.
    /// </summary>
    void OnDrawGizmos()
    {
        if (showInspectorBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, col.size);
        }
    }
}

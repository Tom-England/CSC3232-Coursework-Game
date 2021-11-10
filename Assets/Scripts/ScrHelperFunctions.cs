using System;
using UnityEngine;
public class ScrHelperFunctions
{
    /// <summary> method CalculateDistance
    /// Helper Function for calculating the distance between two points in 3d space
    /// <returns>Float value representing the distance between the two points</returns>
    /// </summary>
    public float CalculateDistance(Vector3 a, Vector3 b)
    {
        double d = Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        return (float)d;
    }
    /// <summary> method CalculateDirection
    /// Helper Function for calculating the direction vector between two points in 3d space
    /// <returns>Vector3 representing the normalized direction vector between the two points</returns>
    /// </summary>
    public Vector3 CalculateDirection(Vector3 a, Vector3 b)
    {
        Vector3 dir = new Vector3(b.x - a.x, b.y - a.y, b.z - a.z).normalized;
        return dir;
    }
}

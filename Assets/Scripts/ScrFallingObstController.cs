using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrFallingObstController : MonoBehaviour
{

    public GameObject falling;
    float remaining = 0;
    public float cooldown = 10;

    public Vector2 x_range;
    public Vector2 z_range;

    /// <summary> method create_pos
    /// Creates a position in the air between a set of x and z positions
    /// <returns> Vector3 position.</returns>
    /// </summary>

    Vector3 create_pos(Vector2 x, Vector2 z)
    {
        Vector3 to_return = new Vector3(Random.Range(x.x, x.y), 10, Random.Range(z.x, z.y));
        return to_return;
    }

    /// <summary> method Update
    /// After a set cooldown, create a falling nav mesh obsticle that will despawn as the next one falls
    /// <returns> Vector3 position.</returns>
    /// </summary>
    void Update()
    {
        if (remaining <= 0)
        {
            remaining = cooldown;
            GameObject instanced = Instantiate(falling, create_pos(x_range, z_range), Quaternion.identity);
            Destroy(instanced, cooldown);
        }
        else
        {
            remaining -= Time.deltaTime;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrSpawner : MonoBehaviour
{
    public int to_spawn;
    public int spawned;
    GameObject[] enemies;
    public int max_spawned;
    public float interval;
    float time_remaining;
    public GameObject enemy;

    /// <summary> method CheckAlive
    /// Calculates how many enemies out of those spawned by this spawner are currently alive
    /// <returns>int value representing the quantity of alive enemies created by this spawner</returns>
    /// </summary>
    int CheckAlive()
    {
        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<ScrEnemyController>().lives > 0)
                {
                    count++;
                }
            }

        }
        return count;
    }

    /// <summary> method Start
    /// Initialises the array used to store spawned enemies
    /// </summary>
    void Start()
    {
        enemies = new GameObject[to_spawn];
    }

    /// <summary> method Update
    /// Attempts to spawn a new enemy based off the remaining cooldown and the current status of the spawned enemies
    /// </summary>
    void Update()
    {
        int alive = CheckAlive();
        if (time_remaining > 0)
        {
            time_remaining -= Time.deltaTime;
            if (time_remaining < 0)
            {
                time_remaining = 0;
            }
        }
        else
        {
            if (alive < max_spawned && spawned < to_spawn)
            {
                enemies[spawned] = Instantiate(enemy, transform.position, Quaternion.identity);
                spawned += 1;
                time_remaining = interval;
            }
        }
    }
}

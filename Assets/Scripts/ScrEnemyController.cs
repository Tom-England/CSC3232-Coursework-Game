using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class ScrEnemyController : MonoBehaviour
{
    [SerializeField]
    float drop_chance = 0.5f;
    public GameObject[] drops;

    [SerializeField]
    int max_lives = 2;
    public int lives;

    [SerializeField]
    float attack_range;

    float max_attack_delay = 3f;
    float attack_delay = 0;

    [SerializeField]
    Transform destination;
    NavMeshAgent nav_mesh_agent;

    // Start is called before the first frame update
    void Start()
    {
        nav_mesh_agent = this.GetComponent<NavMeshAgent>();
        lives = max_lives;
        if (nav_mesh_agent == null)
        {
            Debug.LogError("No navmesh agent found on " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        if (destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            nav_mesh_agent.SetDestination(targetVector);
        }
    }

    float CalculateDistance(Vector3 a, Vector3 b)
    {
        double d = Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        return (float)d;
    }

    void Attack()
    {
        Debug.Log("Attacking");
        attack_delay = max_attack_delay;
    }

    void StateMachine()
    {
        // Expected order:
        // 1. Move close to player
        // 2. Once in range, attempt to attack
        // 3. Wait until attack delay is over or player moved away
        // 4. Repeat from step 2 or 1.

        if (CalculateDistance(transform.position, destination.transform.position) < attack_range)
        {
            // Player is within range, check attack
            nav_mesh_agent.isStopped = true;
            if (attack_delay <= 0)
            {
                // Attack is ready
                Attack();
            }
        }
        else
        {
            // Player is out of range, move
            nav_mesh_agent.isStopped = false;
            SetDestination();
        }
    }

    bool GenerateDrop()
    {
        bool drop_created = false;
        float chance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (chance < drop_chance)
        {
            drop_created = true;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(drops[UnityEngine.Random.Range(0, drops.Length - 1)], pos, Quaternion.identity);
        }
        return drop_created;
    }

    void Die()
    {
        // Controls destroying the enemy object and generating the drop
        Debug.Log(GenerateDrop());
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Debug.Log(gameObject.name + "Hit");
            lives -= 1;
            if (lives <= 0)
            {
                Die();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        if (attack_delay > 0)
        {
            attack_delay -= Time.deltaTime;
            if (attack_delay < 0) { attack_delay = 0; }
        }
    }
}

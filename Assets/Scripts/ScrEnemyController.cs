using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class ScrEnemyController : MonoBehaviour
{

    ScrHelperFunctions helper = new ScrHelperFunctions();
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
    Transform backup_destination;
    NavMeshAgent nav_mesh_agent;

    [SerializeField]
    GameObject throwable;
    bool has_item = true;

    [SerializeField]
    string ID;


    // Start is called before the first frame update
    void Start()
    {
        // Creates an ID for the enemy, used for tracking thrown items
        Guid guid = Guid.NewGuid();
        ID = guid.ToString();

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

    void Attack()
    {
        Debug.Log("Attacking");
        attack_delay = max_attack_delay;
        has_item = false;
        GameObject thrown;
        thrown = Instantiate(throwable, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        thrown.name = ID;
        backup_destination = destination;
        destination = thrown.transform;
    }

    void StateMachine()
    {
        // Expected order:
        // 0. Pick up item if thrown
        // 1. Move close to player
        // 2. Once in range, attempt to attack
        // 3. Wait until attack delay is over or player moved away
        // 4. Repeat from step 2 or 1.

        if (helper.CalculateDistance(transform.position, destination.transform.position) < attack_range && has_item)
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == ID)
        {
            has_item = true;
            Destroy(col.gameObject);
            destination = backup_destination;
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

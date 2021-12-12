using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TMPro;
public class ScrEnemyController : MonoBehaviour
{
    // Variables for speaking system
    public TMP_Text text;
    public string[] messages;
    float speech_cooldown;

    ScrHelperFunctions helper = new ScrHelperFunctions();
    [SerializeField]
    float drop_chance = 0.5f;
    public GameObject[] drops;

    [SerializeField]
    int max_lives = 2;
    public int lives;

    [SerializeField]
    float attack_range;

    public float max_attack_delay = 3f;
    public float attack_delay = 0;

    [SerializeField]
    public Transform destination;
    public Transform backup_destination;
    NavMeshAgent nav_mesh_agent;

    [SerializeField]
    public GameObject throwable;
    public bool has_item = true;

    [SerializeField]
    string ID;

    public string GetID()
    {
        return ID;
    }

    /// <summary> method Start
    /// Start method is used to generate a new ID for the enemy as well as setting up the navmesh and giving life to enemy.
    /// </summary>
    void Start()
    {

        destination = GameObject.Find("Player").transform;

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

    /// <summary> method SetDestination
    /// Method verifies that the destination is set to a transform before beginning the navmesh pathfinding.
    /// </summary>
    private void SetDestination()
    {
        if (destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            nav_mesh_agent.SetDestination(targetVector);
        }
    }

    /// <summary> method SetHasItem
    /// Setter for has_item
    /// </summary>
    public void SetHasItem(bool b)
    {
        has_item = b;
    }

    public void SetTarget(string target)
    {
        if (target == "player")
        {
            destination = GameObject.Find("Player").transform;
        }
        else
        {
            if (target == "item")
            {
                destination = GameObject.Find(ID).transform;
            }
        }
    }

    /// <summary> method Attack
    /// Generates the enemys throwable and modifies their destination so they go and pick it up.
    /// </summary>
    public void Attack()
    {
        attack_delay = max_attack_delay;
        has_item = false;
        GameObject thrown;
        thrown = Instantiate(throwable, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        thrown.name = ID;
        backup_destination = destination;
        destination = thrown.transform;
    }

    /// <summary> method StateMachine
    /// Controls the state of the enemy based off their location relative to the player and the current state of their attack.
    /// </summary>
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

            if (attack_delay <= 0)
            {
                // Attack is ready
                Attack();
            }
        }
        else
        {
            // Player is out of range, move
            Move();
        }
    }

    public void Move()
    {
        nav_mesh_agent.isStopped = false;
        SetDestination();
    }
    public void Stop()
    {
        nav_mesh_agent.isStopped = true;
    }

    public float GetRange()
    {
        return attack_range;
    }

    // GOAP
    // Actions:
    //  Move  -  no requirements
    //  Change Target  -  has item or not
    //  Attack  -  needs item
    //  Retreat  -  too close & target is player
    //  Pickup item  -  target is item and near item

    /// <summary> method GenerateDrop
    /// Controls if a powerup is created on the enemys death.
    /// Powerup type is also random (although there are only 2 for now)
    /// This introduces a degree of randomness into the game and prevents the player
    /// From blasting through the game too easily as surviving long enough to get the correct
    /// powerup can be fairly difficult
    /// <returns>Boolean value, true if a drop is created and false otherwise</returns>
    /// </summary>
    bool GenerateDrop()
    {
        bool drop_created = false;
        float chance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (chance < drop_chance)
        {
            drop_created = true;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(drops[UnityEngine.Random.Range(0, drops.Length)], pos, Quaternion.identity);
        }
        return drop_created;
    }

    /// <summary> method Die
    /// Destroys the object and calls Generate Drop
    /// </summary>
    void Die()
    {
        // Controls destroying the enemy object and generating the drop
        GenerateDrop();
        Destroy(gameObject);
    }

    /// <summary> method OnTriggerEnter
    /// Handles interaction when the enemy enters a trigger zone.
    /// Current interactions:
    ///     * Enemy has been hit
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Speak();
            lives -= 1;
            if (lives <= 0)
            {
                Die();
            }
        }
    }

    /// <summary> method Speak
    /// Displays a message from the messages array and sets its cooldown
    /// </summary>
    void Speak()
    {
        text.text = messages[UnityEngine.Random.Range(0, messages.Length)];
        speech_cooldown = 1;
    }

    /// <summary> method OnCollisionEnter
    /// Handles interaction when the enemy enters a collision.
    /// Current interactions:
    ///     * Enemy is attempting to pickup its thrown item
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        // This function does work however it only works if unity can be bothered to call it
        // For some strange reason checking if two boxes overlap consistently was too hard for unitys
        // developers so the enemy regularly fails to pick up their object because this is not called
        //Debug.Log("Attempting Pickup");
        if (col.gameObject.name == ID)
        {
            //Debug.Log("Success");
            has_item = true;
            Destroy(col.gameObject);
            destination = backup_destination;
        }
    }

    /// <summary> method IfNearThrown
    // This function is the backup for when OnCollisionEnter does not run despite the collision occuring.
    // This shouldn't be needed but unfortunatly it is.
    /// </summary>
    void IfNearThrown()
    {
        if (!has_item)
        {
            if (helper.CalculateDistance(transform.position, destination.position) <= 1 && attack_delay < max_attack_delay / 2)
            {
                has_item = true;
                Destroy(destination.gameObject);
                destination = backup_destination;
            }
        }
    }

    /// <summary> method Update
    /// In charge of calling the state machine and checking if the enemy is near their thrown object
    /// Also updates the various cooldowns for states.
    /// </summary>
    void Update()
    {
        //StateMachine();
        //IfNearThrown();
        if (attack_delay > 0)
        {
            attack_delay -= Time.deltaTime;
            if (attack_delay < 0) { attack_delay = 0; }
        }
        if (speech_cooldown > 0)
        {
            speech_cooldown -= Time.deltaTime;
        }
        else if (text.text != "")
        {
            text.text = "";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ScrPlayerController : MonoBehaviour
{

    public float max_speed = 10f;
    public float speed = 500f;
    public float jump_force = 100f;
    bool in_air = false;
    bool in_air_bob = false;
    public Rigidbody playerRB;
    public GameObject mesh;

    // Player Attack Variables
    public GameObject attackbox;
    bool attack_flag = false;
    float max_attack_time_on = 1f;
    float attack_time_on = 0f;
    public GameObject tr;

    // Player Lives Variables
    static int max_lives = 3;
    int lives = 3;
    static float max_inv_time = 1f;
    float current_inv_time = 0f;
    public Image[] live_graphics;

    // Fixing Hazards Variables
    string current_power = "";


    /// <summary> method OnCollisionEnter
    /// Handles interactions when the player enters a collision. 
    /// Only used to detect if the player is on the floor plane
    /// so that the bobbing animation and jumping function correctly.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            in_air = false;
            in_air_bob = false;
        }
    }

    /// <summary> method OnTriggerEnter
    /// Handles interactions when the player enters a trigger zone. 
    /// Current interactions:
    ///     * Player has been hit
    ///     * Player has recieved a powerup/buff
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ScrHazard hazardScr = null;
        //if (other.gameObject.name.Contains("Hazard"))
        //{
        if (other.gameObject.name.Contains("DamageZone"))
        {
            hazardScr = other.gameObject.transform.parent.GetComponent<ScrHazard>();
            Debug.Log(other.gameObject.name + ": " + hazardScr);
        }

        //}
        if (other.gameObject.tag == "Damage")
        {
            if (hazardScr == null || !hazardScr.IsSafe())
            {
                if (current_inv_time == 0)
                {
                    lives -= 1;
                    current_inv_time = max_inv_time;
                    UpdateLives();
                }
            }
        }
        if (other.gameObject.tag == "Buff")
        {
            if (other.gameObject.name == "ElectricBuff(Clone)")
            {

            }
            switch (other.gameObject.name)
            {
                case "ElectricBuff(Clone)":
                    current_power = "wires";
                    Destroy(other.gameObject);
                    break;
                case "CementBuff(Clone)":
                    current_power = "cement";
                    Destroy(other.gameObject);
                    break;
            }
        }
    }

    /// <summary> method UpdateLives
    /// Updates the life indicator in the UI
    /// </summary>
    void UpdateLives()
    {
        for (int i = 0; i < max_lives; i++)
        {
            if (i < lives)
            {
                live_graphics[i].enabled = true;
            }
            else
            {
                live_graphics[i].enabled = false;
            }

        }
    }

    public int GetLives()
    {
        return lives;
    }

    Vector3 GetPos()
    {
        return playerRB.transform.position;
    }

    /// <summary> method RotatePlayer
    /// Takes in the x and y values from player input and rotates the player to face in the direction of movement.
    /// </summary>
    void RotatePlayer(float x, float y)
    {
        Vector3 direction = new Vector3(x, 0, y);
        Vector3 direction_normal = direction.normalized;
        //Debug.Log(direction_normal);
        var headingChange = Quaternion.FromToRotation(Vector3.left, direction_normal);
        transform.localRotation = headingChange;
    }

    /// <summary> method HandleMovement
    /// Handles player movement each frame.
    /// The current speed of the player is obtained from its rigidbody and compared
    /// to the specified maximum speed to prevent the player obtaining unlimited speed
    /// from the force being constantly applied.
    /// <returns> Boolean value, true if movement occured, false if not.</returns>
    /// </summary>
    bool HandleMovement()
    {
        float current_speed = playerRB.velocity.magnitude;
        if (current_speed < max_speed)
        {
            float y_axis = Input.GetAxis("Vertical");
            float x_axis = Input.GetAxis("Horizontal");
            bool jump_flag = Input.GetButtonDown("Jump");
            if (in_air) { jump_flag = false; }
            if (y_axis == 0 && x_axis == 0 && !jump_flag)
            {
                // No Movement
                return false;
            }
            else
            {
                // Handle Jump
                if (jump_flag)
                {
                    playerRB.AddForce(Vector3.up * jump_force, ForceMode.Force);
                    in_air = true;
                }
                else
                {
                    // Movement
                    playerRB.AddForce(Vector3.forward * y_axis * speed * Time.deltaTime, ForceMode.Force);
                    playerRB.AddForce(Vector3.right * x_axis * speed * Time.deltaTime, ForceMode.Force);
                    RotatePlayer(x_axis, y_axis);
                    // Bobbing animation
                    if (!in_air_bob)
                    {
                        playerRB.AddForce(Vector3.up * 100, ForceMode.Force);
                        in_air_bob = !in_air_bob;
                    };
                }
                return true;
            }
        }
        return false;

    }

    /// <summary> method HandleAttack
    /// Handles player attack by enabling the attackbox collider and setting the timer for keeping it on.
    /// The attackbox has a habit of not being detected every time (See comments on ScrEnemyController.OnCollisionEnter for more)
    /// To remedy this, the attack box has a fairly long cooldown so there is time for the detection however this can make it feel unresponsive
    /// <returns> Boolean value, true if attack occured, false if not.</returns>
    /// </summary>
    bool HandleAttack()
    {
        attack_flag = Input.GetButtonDown("Fire1");
        if (attack_flag)
        {
            attackbox.SetActive(true);
            attack_time_on = max_attack_time_on;
            Vector3 new_pos = new Vector3(tr.transform.position.x, tr.transform.position.y, -1);
            tr.transform.position = new_pos;
            //Debug.Log("pow");

        }
        else
        {
            if (attack_time_on == 0)
            {
                attackbox.SetActive(false);
            }
            else
            {
                attack_time_on -= Time.deltaTime;
                if (attack_time_on < 0)
                {
                    attack_time_on = 0;
                }
            }
        }
        return attack_flag;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        attackbox.SetActive(false);
    }

    /// <summary> method CanFix
    /// Public function for checking if the player can fix a provided hazard type.
    /// <returns> Boolean value, true if player can fix provided hazard, false if not.</returns>
    /// </summary>
    public bool CanFix(string type)
    {
        if (current_power == type)
        {
            return true;
        }
        return false;
    }

    public string GetBuff()
    {
        return current_power;
    }
    public void RemoveBuff()
    {
        current_power = "";
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
        if (current_inv_time > 0)
        {
            current_inv_time -= Time.deltaTime;
            if (current_inv_time < 0)
            {
                current_inv_time = 0;
            }
        }
    }
}

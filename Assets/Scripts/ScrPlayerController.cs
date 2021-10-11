using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ScrPlayerController : MonoBehaviour
{

    float speed = 4f;
    public float jump_force = 100f;
    bool in_air = false;
    bool in_air_bob = false;
    public Rigidbody playerRB;
    public GameObject mesh;
    public GameObject attackbox;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            in_air = false;
            in_air_bob = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Damage")
        {
            Debug.Log("Ow");
        }
    }

    Vector3 GetPos()
    {
        return playerRB.transform.position;
    }

    // Takes in the x and y values from player input and rotates the player to face in the direction of movement
    void RotatePlayer(float x, float y)
    {
        Vector3 direction = new Vector3(x, 0, y);
        Vector3 direction_normal = direction.normalized;
        //Debug.Log(direction_normal);
        var headingChange = Quaternion.FromToRotation(Vector3.left, direction_normal);
        transform.localRotation = headingChange;
    }

    // Handles player movement each frame, returns true if the player moved and false if they did not.
    bool HandleMovement()
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
                playerRB.AddForce(Vector3.forward * y_axis * speed, ForceMode.Force);
                playerRB.AddForce(Vector3.right * x_axis * speed, ForceMode.Force);
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

    bool HandleAttack()
    {

        bool attack_flag = Input.GetButtonDown("Fire1");
        if (attack_flag)
        {
            attackbox.SetActive(true);
            Debug.Log("pow");

        }
        else
        {
            attackbox.SetActive(false);
        }
        return attack_flag;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        attackbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
    }
}

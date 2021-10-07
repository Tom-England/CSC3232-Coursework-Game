using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayerController : MonoBehaviour
{

    float speed = 10f;
    public float jump_force = 100f;
    bool in_air = false;
    public Rigidbody playerRB;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Floor")
        {
            in_air = false;
        }
    }

    Vector3 GetPos()
    {
        return playerRB.transform.position;
    }

    // Handles player movement each frame, returns true if the player moved and false if they did not.
    bool HandleMovement()
    {
        float y_axis = Input.GetAxis("Vertical");
        float x_axis = Input.GetAxis("Horizontal");
        bool jump_flag = Input.GetButtonDown("Jump");

        if (y_axis == 0 && x_axis == 0 && !jump_flag)
        {
            // No Movement
            return false;
        }
        else
        {
            // Movement
            playerRB.AddForce(Vector3.forward * y_axis * speed, ForceMode.Force);
            playerRB.AddForce(Vector3.right * x_axis * speed, ForceMode.Force);

            // Handle Jump
            if (jump_flag && !in_air)
            {
                playerRB.AddForce(Vector3.up * jump_force, ForceMode.Force);
                in_air = true;
            }
            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
}

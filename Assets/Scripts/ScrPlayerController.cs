using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayerController : MonoBehaviour
{

    float speed = 10f;
    public Rigidbody playerRB;

    Vector3 GetPos()
    {
        return playerRB.transform.position;
    }

    // Handles player movement each frame, returns true if the player moved and false if they did not.
    bool HandleMovement()
    {
        float y_axis = Input.GetAxis("Vertical");
        float x_axis = Input.GetAxis("Horizontal");
        playerRB.AddForce(Vector3.forward * y_axis * speed, ForceMode.Force);
        playerRB.AddForce(Vector3.right * x_axis * speed, ForceMode.Force);
        if (y_axis == 0 && x_axis == 0)
        {
            return false;
        }
        else
        {
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

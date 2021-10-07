using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPlayerController : MonoBehaviour
{

    float speed = 10f;
    public Rigidbody playerRB;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerRB.AddForce(Vector3.forward * Input.GetAxis("Vertical") * speed, ForceMode.Force);
        playerRB.AddForce(Vector3.right * Input.GetAxis("Horizontal") * speed, ForceMode.Force);
    }
}

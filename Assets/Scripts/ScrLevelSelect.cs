using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScrLevelSelect : MonoBehaviour
{

    public Canvas canvas;

    void Start()
    {
        canvas.enabled = false;
    }

    bool can_interact = false;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            can_interact = true;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            can_interact = false;
        }
    }

    void HandleUIInputs()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            canvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (can_interact && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Open Level Select");
            canvas.enabled = true;
        }
        if (canvas.enabled)
        {
            HandleUIInputs();
        }
    }

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScrLevelSelect : MonoBehaviour
{

    public Canvas canvas;

    /// <summary> method Start
    /// Hides the UI in the scene until it is needed.
    /// </summary>
    void Start()
    {
        canvas.enabled = false;
    }

    bool can_interact = false;
    /// <summary> method OnTriggerEnter
    /// Sets the flag indicating the player is within interaction range.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            can_interact = true;
        }
    }
    /// <summary> method OnTriggerExit
    /// Sets the flag indicating the player is out of interaction range.
    /// </summary>
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            can_interact = false;
        }
    }

    /// <summary> method HandleUIInputs
    /// Checks for the esc key being pressed and closes the UI if it is
    /// </summary>
    void HandleUIInputs()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            canvas.enabled = false;
        }
    }

    /// <summary> method Update
    /// Checks for a fire1 (mouse1 down) event and enables the level selection UI.
    /// </summary>
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

    /// <summary> method LoadLevel
    /// Method is called when the buttons on the UI are pressed, loads the specified scene
    /// </summary>
    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

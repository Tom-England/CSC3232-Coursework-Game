using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ScrDeathScreen : MonoBehaviour
{
    public Text safe_text;
    public Text time_text;
    /// <summary> method Start
    /// This gets the players total time survived and how many hazards they fixed and displays them on the screen
    /// </summary>
    void Start()
    {
        safe_text.text = "TOTAL HAZARDS FIXED: " + ScrGameController.hazards_safe + "/" + ScrGameController.hazards_total;
        time_text.text = "TOTAL TIME SURVIVED: " + ScrGameController.game_time.ToString("0.00");
    }

    /// <summary> method Update
    /// Monitors for the fire1 (mouse1 down) event and loads the overworld when pressed.
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Down");
            SceneManager.LoadScene("Overworld");
        }
    }
}

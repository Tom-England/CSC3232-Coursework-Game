using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ScrGameController : MonoBehaviour
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    public bool game_running;
    public bool game_finished = false;

    public Transform hazard_master;
    public List<Transform> hazards = new List<Transform>();
    public static int hazards_safe;
    public static int hazards_total;

    // Variables for UI
    public Text paused_text;
    public Text buff_text;
    public Text time_text;
    public static float game_time = 0f;
    public ScrPlayerController pc;

    /// <summary> method PauseGame
    /// Modifies the timescale to 0 to disable all physics and time based interactions
    /// Effectivly pausing the game as movement can only occur between two points in time
    /// and by setting the time scale to 0, the next moment will never occur.
    /// Also sets the paused text to display
    /// </summary>
    void PauseGame()
    {
        Time.timeScale = 0;
        paused_text.text = "Paused";
    }
    /// <summary> method Resume
    /// Modifies the timescale to 1 to enable all physics and time based interactions
    /// Effectivly un-pausing the game.
    /// Also sets the paused text to hide
    /// </summary>
    void ResumeGame()
    {
        Time.timeScale = 1;
        paused_text.text = "";
    }
    public int[] game_area;
    public bool showInspectorBox = true;
    /// <summary> method OnDrawGizmos
    /// This is helper code that draws a box in the Unity inspector to highlight the area the game_area exists in.
    /// Not visible in game.
    /// </summary>
    void OnDrawGizmos()
    {
        if (showInspectorBox)
        {
            Gizmos.color = Color.blue;
            Vector3 box_start = new Vector3(game_area[0], 0, game_area[1]);
            Vector3 box_end = new Vector3(game_area[2], 1, game_area[3]);
            Gizmos.DrawCube(box_start, box_end);
        }
    }

    /// <summary> method Start
    /// Finds all the hazards in the scene and counts them
    /// Hazards are also stored in a List for easy access later
    /// </summary>
    private void Start()
    {
        game_running = true;
        foreach (Transform child in hazard_master)
        {
            hazards.Add(child);
            hazards_total += 1;
        }
    }
    /// <summary> method CheckFinished
    /// Iterates through all of the hazards and works out if they have all been rendered safe
    /// <returns> Boolean value, true if all hazards are safe and the level is complete </returns>
    /// </summary>
    bool CheckFinished()
    {
        foreach (Transform hazard in hazards)
        {
            if (!hazard.GetComponent<ScrHazard>().IsSafe())
            {
                return false;
            }
        }
        return true;
    }
    /// <summary> method UpdateUI
    /// Updates the time and buff UI elements
    /// </summary>
    void UpdateUI()
    {
        game_time += Time.deltaTime;
        time_text.text = "Time: " + game_time.ToString("0.00");
        buff_text.text = "Buff: " + pc.GetBuff();
    }

    /// <summary> method Update
    /// Handles the input for toggling the pause status of the game
    /// Also handles the overall game state (dead or won)
    /// </summary>
    private void Update()
    {
        bool pause_flag = Input.GetButtonDown("Cancel");
        if (pause_flag)
        {
            game_running = !game_running;
            if (game_running)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (pc.GetLives() <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
        }
        game_finished = CheckFinished();
        if (game_finished)
        {
            SceneManager.LoadScene("Overworld");
        }
        UpdateUI();
    }
}

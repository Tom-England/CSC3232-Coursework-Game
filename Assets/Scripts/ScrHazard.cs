using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScrHazard : MonoBehaviour
{

    ScrHelperFunctions helper = new ScrHelperFunctions();
    public bool safe = false;
    public float fixing_range = 3f;
    public GameObject player;
    ScrPlayerController playerController;
    public TMP_Text text;
    public string type;

    public GameObject particles;

    public bool IsSafe()
    {
        return safe;
    }
    /// <summary> method Start
    /// Finds the player and gets their script component
    /// </summary>
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<ScrPlayerController>();
    }

    /// <summary> method Update
    /// Checks if the player is both within fixing range and has the appropriate buff to fix them
    /// If so, a prompt is displayed and the player can render it safe
    /// </summary>
    void Update()
    {
        if (!IsSafe())
        {
            if (helper.CalculateDistance(transform.position, player.transform.position) < fixing_range && playerController.CanFix(type))
            {
                text.text = "E";
                if (Input.GetButtonDown("Submit"))
                {
                    safe = true;
                    particles.SetActive(false);
                    playerController.RemoveBuff();
                    ScrGameController.hazards_safe += 1;
                    text.text = "";
                }
            }
            else
            {
                text.text = "";
            }
        }
    }
}

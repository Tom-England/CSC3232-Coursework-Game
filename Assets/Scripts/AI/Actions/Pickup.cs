using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : GoapActivity
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    float delay = 3f;
    public Pickup()
    {
        AddRequirement("hasItem", false);
        AddRequirement("playerTarget", false);
        AddEffect("hasItem", true);
        SetWeight(1);
    }

    public void SetDelay(float d)
    {
        delay = d;
    }

    /// <summary> method Reset
    /// Resets the done flag to false as usual but also resets the delay to 3 seconds
    /// </summary>
    public override void Reset()
    {
        done = false;
        SetDelay(3);
    }

    /// <summary> method DoActivity
    /// Moves the agent to its destination then attempts pickup
    /// if the delay is up.
    /// delay prevents the object being picked up immediatly after being thrown
    /// </summary>
    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        string ID = control.GetID();
        GameObject thrown = GameObject.Find(ID);

        if (helper.CalculateDistance(agent.transform.position, thrown.transform.position) < 2)
        {
            control.Stop();
            if (delay < control.max_attack_delay / 2)
            {
                thrown.GetComponent<ScrThrowable>().Pickup();
                control.SetHasItem(true);
                done = true;
            }
            else
            {
                //Debug.Log(Time.deltaTime);
                delay = delay - Time.deltaTime;
                //Debug.Log(delay);
            }

        }
        else
        {
            control.Move();
        }
    }
}

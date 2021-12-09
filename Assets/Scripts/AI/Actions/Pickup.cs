using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : GoapActivity
{
    public Pickup()
    {
        AddRequirement("hasItem", false);
        AddRequirement("playerTarget", false);
        AddRequirement("nearTarget", true);
        AddEffect("hasItem", true);
        SetWeight(1);
    }

    public override void DoActivity(GameObject agent)
    {
        string ID = agent.GetComponent<ScrEnemyController>().GetID();
        GameObject thrown = GameObject.Find(ID);
        thrown.GetComponent<ScrThrowable>().Pickup();
        agent.GetComponent<ScrEnemyController>().SetHasItem(true);
        done = true;
    }
}

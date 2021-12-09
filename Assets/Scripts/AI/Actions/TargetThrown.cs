using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetThrown : GoapActivity
{
    public TargetThrown()
    {
        AddRequirement("hasItem", false);
        AddRequirement("playerTarget", true);
        AddEffect("playerTarget", false);
        SetWeight(1);
    }

    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        //control.SetTarget("item");
        done = true;
    }
}

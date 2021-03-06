using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : GoapActivity
{
    public TargetPlayer()
    {
        AddRequirement("hasItem", true);
        AddRequirement("playerTarget", false);
        AddEffect("playerTarget", true);
        SetWeight(1);
    }

    /// <summary> method DoActivity
    /// Sets the agents target to the player
    /// </summary>
    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        control.SetTarget("player");
        done = true;
    }
}

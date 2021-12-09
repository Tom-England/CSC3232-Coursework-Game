using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : GoapActivity
{
    public Attack()
    {
        AddRequirement("hasItem", true);
        AddRequirement("nearTarget", true);
        AddRequirement("playerTarget", true);
        AddEffect("hasItem", false);
        SetWeight(1);
    }

    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        control.Attack();
        done = true;
    }
}

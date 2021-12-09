using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GoapActivity
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    public Move()
    {
        AddEffect("nearTarget", true);
        SetWeight(0);
    }

    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        control.Move();
        if (helper.CalculateDistance(agent.transform.position, control.destination.position) < control.GetRange())
        {
            done = true;
        }
    }
}

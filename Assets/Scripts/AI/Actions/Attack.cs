using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : GoapActivity
{
    ScrHelperFunctions helper = new ScrHelperFunctions();
    public Attack()
    {
        AddRequirement("hasItem", true);
        AddRequirement("playerTarget", true);
        AddEffect("hasItem", false);
        SetWeight(1);
    }

    public override void DoActivity(GameObject agent)
    {
        ScrEnemyController control = agent.GetComponent<ScrEnemyController>();
        if (helper.CalculateDistance(agent.transform.position, control.destination.position) < control.GetRange())
        {
            control.Stop();
            if (control.attack_delay <= 0)
            {
                Debug.Log("Pow");
                control.Attack();
                done = true;
            }

        }
        else
        {
            control.Move();
        }

    }
}

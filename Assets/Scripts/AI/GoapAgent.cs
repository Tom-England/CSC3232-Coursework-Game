using UnityEngine;
using System.Collections.Generic;

class GoapAgent : MonoBehaviour
{
    public KeyValuePair<string, bool> goal;
    public List<KeyValuePair<string, bool>> has;
    public List<GoapActivity> activities = new List<GoapActivity>();
    public List<GoapActivity> path = new List<GoapActivity>();

    public GoapAgent()
    {
        has = new List<KeyValuePair<string, bool>>();
        activities.Add(new Attack());
        activities.Add(new Pickup());
        activities.Add(new Move());
        activities.Add(new TargetPlayer());
        activities.Add(new TargetThrown());
    }

    /// <summary> method GenerateValidActivities
    /// Generates the valid activites from the total activities
    /// <returns>List of valid activities</returns>
    /// </summary>
    List<GoapActivity> GenerateValidActivities(List<KeyValuePair<string, bool>> has)
    {
        List<GoapActivity> valid = new List<GoapActivity>();
        foreach (GoapActivity activity in activities)
        {
            if (activity.CanDo(has))
            {
                valid.Add(activity);
            }
        }
        return valid;
    }

    /// <summary> method FindHelpfulActivities
    /// Generates the valid activites that can lead to a goal activity
    /// <returns>List of valid activities</returns>
    /// </summary>
    List<GoapActivity> FindHelpfulActivities(List<KeyValuePair<string, bool>> has, GoapActivity target)
    {
        List<GoapActivity> valid = new List<GoapActivity>();
        foreach (GoapActivity activity in activities)
        {
            List<KeyValuePair<string, bool>> requirements = UpdateHas(has, activity.GetEffects());
            if (requirements != has && target.CanDo(requirements))
            {
                valid.Add(activity);
            }
        }
        return valid;
    }

    /// <summary> method SelectBest
    /// Selects the activity with the lowest weight from a list of activites
    /// <returns>best activity</returns>
    /// </summary>
    public GoapActivity SelectBest(List<GoapActivity> activities)
    {
        GoapActivity best = activities[0];
        foreach (GoapActivity activity in activities)
        {
            if (activity.GetWeight() < best.GetWeight())
            {
                best = activity;
            }
        }
        return best;
    }

    void OutputList(List<GoapActivity> list)
    {
        foreach (GoapActivity item in list)
        {
            Debug.Log(item);
        }
    }
    void OutputList(List<KeyValuePair<string, bool>> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }

    /// <summary> method UpdateHas
    /// Creates a new list of has tags by taking the old list and applying changes
    /// <returns>Updated list of has tags</returns>
    /// </summary>
    List<KeyValuePair<string, bool>> UpdateHas(List<KeyValuePair<string, bool>> has, List<KeyValuePair<string, bool>> changes)
    {
        List<KeyValuePair<string, bool>> new_has = new List<KeyValuePair<string, bool>>();
        for (int i = 0; i < has.Count; i++)
        {
            new_has.Add(new KeyValuePair<string, bool>(has[i].Key, has[i].Value));
        }
        foreach (KeyValuePair<string, bool> change in changes)
        {
            for (int i = 0; i < has.Count; i++)
            {
                if (new_has[i].Key == change.Key)
                {
                    new_has[i] = change;
                    break;
                }
            }
        }
        return new_has;
    }

    /// <summary> method CalculatePathToGoal
    /// Calculates a path to the current goal status
    /// <returns>Path of activities that result in the goal condition</returns>
    /// </summary>
    public List<GoapActivity> CalculatePathToGoal()
    {
        List<GoapActivity> path = new List<GoapActivity>();
        // 1. Search for all activities that has goal as an effect
        // 2. Select one with lowest weight
        // 3. Check if activity can be done
        // 4. If not, find all activities that enable this
        // 5. Select activity with lowest weight
        // 6. Repeat 2-4 until task can be done
        List<KeyValuePair<string, bool>> fake_has = has;
        // Step 1
        List<GoapActivity> valid = new List<GoapActivity>();
        foreach (GoapActivity activity in activities)
        {
            if (activity.GetEffects().Contains(goal))
            {
                valid.Add(activity);
            }
        }
        //Debug.Log("Valid End Goals");
        //OutputList(valid);
        // Step 2
        GoapActivity best = SelectBest(valid);
        path.Add(best);

        // Step 3
        if (best.CanDo(has))
        {
            return path;
        }

        bool found = false;
        int pointer = 0;
        while (!found)
        {
            // Step 4
            List<KeyValuePair<string, bool>> requirements = path[pointer].GetRequirements();
            valid = FindHelpfulActivities(fake_has, path[pointer]);
            //Debug.Log("Valid steps");
            //OutputList(valid);
            best = SelectBest(valid);
            //Debug.Log("Adding: " + best);
            path.Add(best);
            if (best.CanDo(has))
            {
                found = true;
            }
            pointer += 1;

        }
        return path;

    }

    void Start()
    {
        // Sets up has with values for attacking the player
        has.Add(new KeyValuePair<string, bool>("hasItem", true));
        has.Add(new KeyValuePair<string, bool>("nearTarget", false));
        has.Add(new KeyValuePair<string, bool>("playerTarget", true));
        goal = new KeyValuePair<string, bool>("hasItem", false);
        path = CalculatePathToGoal();
        Debug.Log("Starting Path Output");
        for (int i = path.Count - 1; i >= 0; i--)
        {
            Debug.Log(path[i]);
        }
        Debug.Log("Ending path output");
    }
}
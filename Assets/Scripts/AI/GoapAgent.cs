using UnityEngine;
using System.Collections.Generic;

class GoapAgent : MonoBehaviour
{
    public KeyValuePair<string, bool> goal;
    public List<KeyValuePair<string, bool>> has;
    public List<GoapActivity> activities = new List<GoapActivity>();
    public List<GoapActivity> path = new List<GoapActivity>();

    int current_task_index = 0;

    public GoapAgent()
    {
        has = new List<KeyValuePair<string, bool>>();
        activities.Add(new Attack());
        activities.Add(new Pickup());
        //activities.Add(new Move());
        activities.Add(new TargetPlayer());
        activities.Add(new TargetThrown());
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
            //Debug.Log("Trying:" + activity);
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
            //Debug.Log(item);
        }
    }
    void OutputList(List<KeyValuePair<string, bool>> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
        }
    }

    void OutputState(List<KeyValuePair<string, bool>> h)
    {
        Debug.Log("State: " + h[0] + h[1]);
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

    GoapActivity FindTargetActivity(KeyValuePair<string, bool> target)
    {
        GoapActivity temp = activities[0];
        bool found = false;
        foreach (GoapActivity activity in activities)
        {
            if (activity.GetEffects().Contains(target))
            {
                if (activity.GetWeight() < temp.GetWeight() || found == false)
                {
                    temp = activity;
                    found = true;
                }
            }
        }
        return temp;
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
        GoapActivity best = FindTargetActivity(goal);
        Debug.Log("Valid End Goal: " + best);
        // Step 2
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
        path.Reverse();
        return path;

    }

    void RunAgent()
    {
        if (path[current_task_index].Finished())
        {
            Debug.Log("Finished Doing: " + path[current_task_index]);
            path[current_task_index].Reset();
            has = UpdateHas(has, path[current_task_index].GetEffects());
            current_task_index += 1;

            if (current_task_index == path.Count)
            {
                // Final task is complete
                current_task_index = 0;
                goal = new KeyValuePair<string, bool>(goal.Key, !goal.Value);
                OutputState(has);
                Debug.Log("New Goal:" + goal);
                path = CalculatePathToGoal();
                Debug.Log("New Path of length: " + path.Count);
                for (int i = 0; i < path.Count; i++)
                {
                    Debug.Log(path[i]);
                }
            }
            else { Debug.Log("Starting doing: " + path[current_task_index]); }
        }
        else
        {
            path[current_task_index].DoActivity(transform.gameObject);
        }
    }

    void Start()
    {
        OutputList(activities[2].GetRequirements());
        // Sets up has with values for attacking the player
        has.Add(new KeyValuePair<string, bool>("hasItem", true));
        has.Add(new KeyValuePair<string, bool>("playerTarget", true));
        goal = new KeyValuePair<string, bool>("hasItem", false);
        path = CalculatePathToGoal();
        Debug.Log("Starting Path Output");
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log(path[i]);
        }
        Debug.Log("Ending path output");
    }
    void Update()
    {
        RunAgent();
    }
}

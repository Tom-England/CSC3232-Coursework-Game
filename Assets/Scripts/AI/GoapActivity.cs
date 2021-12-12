using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapActivity
{
    public bool done = false;
    List<KeyValuePair<string, bool>> requires;
    List<KeyValuePair<string, bool>> effects;

    int weight;
    /// <summary> Constructor GoapActivity
    /// Constructs the activity object by creating a list for its requirements
    /// and effects and defaults its weight to 0
    /// </summary>
    public GoapActivity()
    {
        requires = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();
        weight = 0;
    }

    /// <summary> method CanDo
    /// Calculates if the activity can be done from the provided list of 
    /// fufilled requirements
    /// <returns>Boolean value, true if all requirements are met</returns>
    /// </summary>
    public bool CanDo(List<KeyValuePair<string, bool>> has)
    {
        if (requires == null || requires.Count == 0) { Debug.Log("NoRequirements"); return true; }
        foreach (KeyValuePair<string, bool> item in requires)
        {
            //Debug.Log(item);
            if (!has.Contains(item))
            {
                //Debug.Log("Failed due to:" + item);
                return false;
            }
        }
        return true;
    }
    /// <summary> method Reset
    /// Sets done to false so the activity can be repeated
    /// </summary>
    public virtual void Reset()
    {
        done = false;
    }
    /// <summary> method Set Weight
    /// Setter for weight
    /// </summary>
    public void SetWeight(int w)
    {
        weight = w;
    }
    /// <summary> method GetWeight
    /// Getter for weight
    /// <returns>Returns the weight</returns>
    /// </summary>
    public int GetWeight()
    {
        return weight;
    }
    /// <summary> method AddRequirement
    /// Creates and adds a requirement to the list
    /// </summary>
    public void AddRequirement(string key, bool value)
    {
        requires.Add(new KeyValuePair<string, bool>(key, value));
    }
    /// <summary> method NoRequirements
    /// Sets the requirement list to empty, just in case
    /// </summary>
    public void NoRequirements()
    {
        requires = new List<KeyValuePair<string, bool>>();
    }
    /// <summary> method AddEffect
    /// Creates and adds an effect to the list
    /// </summary>
    public void AddEffect(string key, bool value)
    {
        effects.Add(new KeyValuePair<string, bool>(key, value));
    }
    /// <summary> method GetEffects
    /// Getter for the effects
    /// <returns>Returns the list of effects on the activity</returns>
    /// </summary>
    public List<KeyValuePair<string, bool>> GetEffects()
    {
        return effects;
    }
    /// <summary> method GetRequirements
    /// Getter for the requirements
    /// <returns>Returns the list of requirements on the activity</returns>
    /// </summary>
    public List<KeyValuePair<string, bool>> GetRequirements()
    {
        return requires;
    }
    /// <summary> method Finished
    /// Getter for done
    /// <returns>Returns if the activity has finished</returns>
    /// </summary>
    public bool Finished()
    {
        return done;
    }
    /// <summary> method DoActivity
    /// Virtual method for running the activities logic
    /// Does nothing here as it is overwritten by each individual 
    /// activity in their own classes
    /// </summary>
    public virtual void DoActivity(GameObject agent)
    {
        Debug.Log("Not Declared");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapActivity
{
    public bool done = false;
    List<KeyValuePair<string, bool>> requires;
    List<KeyValuePair<string, bool>> effects;

    int weight;
    public GoapActivity()
    {
        requires = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();
        weight = 0;
    }

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
    public virtual void Reset()
    {
        done = false;
    }
    public void SetWeight(int w)
    {
        weight = w;
    }
    public int GetWeight()
    {
        return weight;
    }
    public void AddRequirement(string key, bool value)
    {
        requires.Add(new KeyValuePair<string, bool>(key, value));
    }
    public void NoRequirements()
    {
        requires = new List<KeyValuePair<string, bool>>();
    }
    public void AddEffect(string key, bool value)
    {
        effects.Add(new KeyValuePair<string, bool>(key, value));
    }
    public List<KeyValuePair<string, bool>> GetEffects()
    {
        return effects;
    }
    public List<KeyValuePair<string, bool>> GetRequirements()
    {
        return requires;
    }
    public bool Finished()
    {
        return done;
    }
    public virtual void DoActivity(GameObject agent)
    {
        Debug.Log("Not Declared");
    }
}
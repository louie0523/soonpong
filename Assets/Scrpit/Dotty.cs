using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DottyEvents
{
    public int Range;
    public List<DottyEvent> Events = new List<DottyEvent>();
}

public class Dotty : MonoBehaviour
{
    public GameObject DottyObj;
    [SerializeField]
    public List<DottyEvents> Events = new List<DottyEvents>();

    public DottyEvent GetEvent()
    {
        if (Events == null || Events.Count == 0)
            return null;


        int totalRange = 0;
        foreach (var e in Events)
        {
            totalRange += e.Range;
        }

        if (totalRange != 100)
            return null;

        int rand = Random.Range(0, 100);

        foreach (var e in Events)
        {
            if (rand < e.Range)
            {
                if (e.Events != null && e.Events.Count > 0)
                {
                    int idx = Random.Range(0, e.Events.Count);
                    return e.Events[idx];
                }
                else
                {
                    return null;
                }
            }
            rand -= e.Range;
        }

        return null;
    }
}
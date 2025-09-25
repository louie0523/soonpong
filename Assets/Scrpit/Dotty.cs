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

        // 전체 범위 합계 계산
        int totalRange = 0;
        foreach (var e in Events)
        {
            totalRange += e.Range;
        }

        // 합계가 100이 아니면 null 반환
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
                    // 빈 범위가 선택됨 (확률적으로 아무것도 얻지 못함)
                    return null;
                }
            }
            rand -= e.Range;
        }

        return null;
    }
}
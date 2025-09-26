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

        // BadRandMinus 적용
        int badRandMinus = GameManager.instance.BadRandMinus;

        // 임시 복사본으로 조정
        List<int> adjustedRanges = new List<int>();
        foreach (var e in Events)
            adjustedRanges.Add(e.Range);

        if (badRandMinus > 0 && Events.Count > 1)
        {
            int firstRange = adjustedRanges[0] + badRandMinus; // 첫 번째 이벤트 증가
            int remainingDecrease = badRandMinus;
            int otherCount = Events.Count - 1;

            for (int i = 1; i < Events.Count; i++)
            {
                int decrease = remainingDecrease / otherCount; // 균등 감소
                adjustedRanges[i] = Mathf.Max(0, adjustedRanges[i] - decrease);
            }

            // 합을 다시 100으로 맞추기 위해 조정 (혹시 오차 발생하면 첫 번째 이벤트로 보정)
            int newTotal = 0;
            foreach (var r in adjustedRanges) newTotal += r;
            int diff = 100 - newTotal;
            adjustedRanges[0] += diff;
        }

        int rand = Random.Range(0, 100);

        for (int i = 0; i < Events.Count; i++)
        {
            if (rand < adjustedRanges[i])
            {
                if (Events[i].Events != null && Events[i].Events.Count > 0)
                {
                    int idx = Random.Range(0, Events[i].Events.Count);
                    return Events[i].Events[idx];
                }
                else
                {
                    return null;
                }
            }
            rand -= adjustedRanges[i];
        }

        return null;
    }

}
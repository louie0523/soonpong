using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public List<Dotty> currentDotty = new List<Dotty>();
    public Dictionary<string, DottyEvent> eventStr = new Dictionary<string, DottyEvent>(); 
    public float Money;

    public bool Checking = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Setting();
        } else
        {
            Destroy(gameObject);
        }
    }

    void Setting()
    {
        DottyEvent[] events = Resources.LoadAll<DottyEvent>("이벤트/");

        foreach(DottyEvent e in events)
        {
            eventStr.Add(e.name, e);
        }
    }


    public void GetOutDottys()
    {
        if(currentDotty.Count == 0) { return; }

        SfxManager.instance.PlaySfx("호잇짜잇호잇호");

        Checking = true;
        // 이벤트별로 참여자 수와 합산 금액을 저장
        Dictionary<string, List<string>> namesByEvent = new Dictionary<string, List<string>>();
        Dictionary<string, float> moneyByEvent = new Dictionary<string, float>();


        float EndMoney = 0;
        for (int i = 0; i < currentDotty.Count; i++)
        {
            DottyEvent e = currentDotty[i].GetEvent();
            if (e == null) continue;

            // 이벤트별 참가자 관리
            if (!namesByEvent.ContainsKey(e.name))
            {
                namesByEvent[e.name] = new List<string>();
                moneyByEvent[e.name] = 0;
            }

            namesByEvent[e.name].Add(currentDotty[i].name);
            moneyByEvent[e.name] += e.EventMoney;

            EndMoney += e.EventMoney;
            Money += e.EventMoney; // 총합 갱신
        }

        // 메시지 생성
        string str = "";
        foreach (var kvp in namesByEvent)
        {
            string eventName = kvp.Key;
            List<string> participants = kvp.Value;
            float totalMoney = moneyByEvent[eventName];

            str += $"도티 {participants.Count}명은 {eventStr[eventName].EventStr} {totalMoney}원\n";
        }

        str += $"총합: {EndMoney}원";
        UIManager.Instance.DottyResult(str);
        Debug.Log(str);

        currentDotty.Clear();

        foreach(Transform tr in SoonPung.instance.gameObject.transform)
        {
            Destroy(tr.gameObject);
        }

        StartCoroutine(CheckNo(0.25f));
    }

    public IEnumerator CheckNo(float time)
    {
        yield return new WaitForSeconds(time);
        Checking = false;
    }



}

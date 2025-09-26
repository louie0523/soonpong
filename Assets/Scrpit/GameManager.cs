using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;


public enum AddType
{
    Plus,
    Mult
}

[System.Serializable]
public class PriceUp
{
    public AddType type;
    public float value;

    public PriceUp(AddType addType , float value)
    {
        this.type = addType;
        this.value = value;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public List<Dotty> currentDotty = new List<Dotty>();
    public Dictionary<string, DottyEvent> eventStr = new Dictionary<string, DottyEvent>(); 
    public float Money;
    public float AutoTimer;
    public float AutoTime = 10f;
    public bool Checking = false;
    public int SDottyCount = 0;
    public int MaxSDottyCount = 25;
    public List<bool> DottyUnLock = new List<bool> { true, false, false, false, false, false, false, false, false };
    public bool SpecialDotty = false;

    [Header("업그레이드")]
    public int AutoSoonPungUp = 0;
    public float SoonPungMoney = 0;
    public float MoneyValue = 1f;
    public int DoubleRand = 0;
    public int BadRandMinus = 0;
    public List<int> SoonPungLv = new List<int>();
    public List<PriceUp> UpGradeMoneyUpper = new List<PriceUp>();
    public List<float> SoonPungPrice = new List<float>();
    public List<float> UpgradeValue = new List<float>();

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Setting();
            AutoTimer = AutoTime;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        AutoSoonPung();
    }

    void AutoSoonPung()
    {
        AutoTimer -= Time.deltaTime;
        if (AutoTimer <= 0)
        {
            SoonPung.instance.DottySoonPung();
            AutoTimer = AutoTime;
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

    public void SoonPungUpgrade(int num)
    {
        if (SoonPungLv[num] >= 10)
        {
            UIManager.Instance.Message("이미 최대 레벨입니다.");
            SfxManager.instance.PlaySfx("취소");
            return;
        }

        if (SoonPungPrice[num] > Money)
        {
            UIManager.Instance.Message("돈이 부족합니다.");
            SfxManager.instance.PlaySfx("취소");
            return;
        }

        AddMoeny(-SoonPungPrice[num]);
        SoonPungLv[num]++;
        switch(num)
        {
            case 0:
                AutoTime -= Mathf.Round(UpgradeValue[num] * 100f) / 100f;
                break;
            case 1:
                SoonPungMoney += Mathf.Round(UpgradeValue[num] * 10f) / 10f;
                break;
            case 2:
                MoneyValue += Mathf.Round(UpgradeValue[num] * 10f) / 10f;
                break;
            case 3:
                DoubleRand += (int)UpgradeValue[num];
                break;
            case 4:
                BadRandMinus += (int)UpgradeValue[num];
                break;

        }
        SfxManager.instance.PlaySfx("구매");
        SoonPungPrice[num] = PriceUp(UpGradeMoneyUpper[num], SoonPungPrice[num]);



    }

    public float PriceUp(PriceUp up, float lastMoney)
    {
        float result = 0;
        if(up.type == AddType.Plus)
        {
            result = lastMoney + up.value;
        } else
        {
            result = lastMoney * up.value;
        }

        return result;
    }

    public void AddMoeny(float value )
    {

        if (value > 0)
            Money += value * MoneyValue;
        else
            Money += value;

    }


    public void GetOutDottys()
    {
        if(currentDotty.Count == 0) { return; }

        SfxManager.instance.PlaySfx("호잇짜잇호잇호");

        Checking = true;
        Dictionary<string, List<string>> namesByEvent = new Dictionary<string, List<string>>();
        Dictionary<string, float> moneyByEvent = new Dictionary<string, float>();


        float EndMoney = 0;
        for (int i = 0; i < currentDotty.Count; i++)
        {
            DottyEvent e = currentDotty[i].GetEvent();
            if (e == null) continue;

            if (!namesByEvent.ContainsKey(e.name))
            {
                namesByEvent[e.name] = new List<string>();
                moneyByEvent[e.name] = 0;
            }

            namesByEvent[e.name].Add(currentDotty[i].name);
            moneyByEvent[e.name] += e.EventMoney;

            EndMoney += (int)(e.EventMoney * MoneyValue);
            AddMoeny(e.EventMoney);
        }
        

        string str = "";
        foreach (var kvp in namesByEvent)
        {
            string eventName = kvp.Key;
            List<string> participants = kvp.Value;
            float totalMoney = moneyByEvent[eventName];

            str += $"도티 {participants.Count}명은 {eventStr[eventName].EventStr} {totalMoney}원\n";
        }

        str += $"합계: {EndMoney}원";
        SfxManager.instance.PlaySfx("구매");
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

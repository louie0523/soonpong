using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using System.Net.NetworkInformation;
using UnityEditor.PackageManager;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;


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




    [Header("시작 bgm")]
    public string bgmName;
    [Header("인게임 정보")]
    public List<Dotty> currentDotty = new List<Dotty>();
    public Dictionary<string, DottyEvent> eventStr = new Dictionary<string, DottyEvent>(); 
    public float Money;
    public float AutoTimer;
    public float AutoTime = 10f;
    public bool Checking = false;
    public List<bool> DottyUnLock = new List<bool> { true, false, false, false, false, false, false, false, false };
    public bool SpecialDotty = false;
    public int currentGetDotty = 0;
    public List<int> NeedLvDotty = new List<int>();
    public int SoonPungRealLv = 0;
    public int currentMap = 0;
    public List<string> LevelBuff = new List<string>();
    public List<int> LvUpGtick = new List<int>();
    [Header("갸차")]
    public int GTicket;
    public int Gpoint = 0;
    public GameObject GCamara;
    public GameObject GotChaCanvas;
    public int gotChaDangay = 0;
    public float heartSfxTimer;
    public Volume volume;
    public GameObject Tea;
    public GameObject DottyGotcahParent;
    public TextMeshProUGUI DottyName;
    public GameObject Backbtn;
    public GameObject GotcahBannerParent;

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

    private void Start()
    {
        SfxManager.instance.PlayBgm(bgmName);
    }

    private void Update()
    {
        AutoSoonPung();

        if(gotChaDangay == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            SfxManager.instance.PlaySfx("글리치");
            GatcahCanAct();
        }

        if(gotChaDangay == 3 )
        {
            heartSfxTimer += Time.deltaTime;
            if(heartSfxTimer >= 1)
            {
                SfxManager.instance.PlaySfx("갸차2");
                heartSfxTimer = 0;
            }
        }
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

    public void LvCheck()
    {
        Debug.Log("레벨 체킹");
        if(SoonPungRealLv < 8)
        {
            if(currentGetDotty >= NeedLvDotty[SoonPungRealLv])
            {
                SoonPungRealLv++;
                SfxManager.instance.PlaySfx("레벨업");
                UIManager.Instance.Message("레벨이 증가하였습니다!");
                Debug.Log("레벨 업!");
                UIManager.Instance.LevelUP();
                currentGetDotty = 0;
                GTicket += LvUpGtick[SoonPungRealLv - 1];
            }
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

    public void GatchaGo()
    {
        if(GTicket <= 0)
        {
            UIManager.Instance.Message("뽑기 티겟이 부족합니다.");
            SfxManager.instance.PlaySfx("취소");
            return;
        }

        GTicket--;

        SfxManager.instance.PlayBgm("갸차1");

        StartCoroutine(gotcahYeon());
    }

    public void GatcahCanAct()
    {
        gotChaDangay = 3;
        GotChaCanvas.SetActive(true);
        RectTransform rect = Tea.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }
    
    public IEnumerator gotcahYeon()
    {
        ObjectRotator objectRotator = Tea.GetComponent<ObjectRotator>();
        objectRotator.isRotating = false;
        ChromaticAberration ch;
        volume.profile.TryGet<ChromaticAberration>(out ch);
        ch.active = true;
        LiftGammaGain LGG;
        volume.profile.TryGet<LiftGammaGain>(out LGG);
        LGG.active = true;
        DottyName.text = "";
        Tea.SetActive(true);
        Backbtn.SetActive(false);
        HeartbeatEffect heartbeatEffect = Tea.GetComponent<HeartbeatEffect>();
        heartbeatEffect.EnableHeartbeat();
        gotChaDangay = 1;
        GCamara.SetActive(true);
        Animator ani = GCamara.GetComponent<Animator>();
        ani.SetTrigger("Move");
        yield return new WaitForSeconds(2.5f);
        RectTransform rect = Tea.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
        rect.localRotation = Quaternion.identity;
        gotChaDangay = 2;
    }


    public void GetDotty()
    {
        if(gotChaDangay == 3)
        {
            gotChaDangay = 4;
            HeartbeatEffect heartbeatEffect = Tea.GetComponent<HeartbeatEffect>();
            heartbeatEffect.DisableHeartbeat();
            SfxManager.instance.PlaySfx("갸차3");
            ObjectRotator objectRotator = Tea.GetComponent<ObjectRotator>();
            objectRotator.isRotating = true;
            StartCoroutine(RotateGO());
        }
    }

    IEnumerator RotateGO()
    {
        yield return new WaitForSeconds(1f);
        ObjectRotator objectRotator = Tea.GetComponent<ObjectRotator>();
        objectRotator.isRotating = false;
        Tea.SetActive(false);
        for(int i = 0; i < DottyGotcahParent.transform.childCount; i++)
        {
            if(i == Gpoint)
            {
                DottyGotcahParent.transform.GetChild(i).gameObject.SetActive(true);
                DottyName.text = DottyGotcahParent.transform.GetChild(i).gameObject.name;
                DottyUnLock[i] = true;
                Image img = GotcahBannerParent.transform.GetChild(i).GetComponent<Image>();
                img.color = Color.white;
            } else
            {
                DottyGotcahParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        Gpoint++;
        Backbtn.SetActive(true);

    }

    public void BackToTheGame()
    {
        GCamara.SetActive(false);
        GotChaCanvas.SetActive(false);
        gotChaDangay = 0;
        ChromaticAberration ch;
        volume.profile.TryGet<ChromaticAberration>(out ch);
        ch.active = false;
        LiftGammaGain LGG;
        volume.profile.TryGet<LiftGammaGain>(out LGG);
        LGG.active = false;
        for (int i = 0; i < DottyGotcahParent.transform.childCount; i++)
        {
            DottyGotcahParent.transform.GetChild(i).gameObject.SetActive(false);
            
        }
        SfxManager.instance.PlayBgm(bgmName);
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

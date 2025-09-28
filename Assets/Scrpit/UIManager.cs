using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UI;




public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public TextMeshProUGUI MessageText;
    public TextMeshProUGUI MessageText2;
    private Coroutine MessageCoroutine;
    public TextMeshProUGUI DottyCountText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI MoneyText2;
    public GameObject DottyResultObj;
    public GameObject DottyResultObj2;
    public TextMeshProUGUI DottyResultTxt;
    public TextMeshProUGUI DottyResultTxt2;
    public TextMeshProUGUI SoonPungLvTxt;
    public Slider SoonPungLvSlider;
    public GameObject LevelObj;
    public TextMeshProUGUI GaCahTicket;
    public TextMeshProUGUI GaCahTicketPrcie;
    [Header("출산 지원")]
    public GameObject SoonPungParent;
    [Header("병원")]
    public TextMeshProUGUI hospitalTxt;
    public TextMeshProUGUI currentDottyinHos;
    public TextMeshProUGUI HosExplain;
    public TextMeshProUGUI HosUpgradePrice;
    public GameObject HosUpgradeParent;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        DottyCountText.text = $"현재 까지 낳은 도티 : {GameManager.instance.currentDotty.Count}명";

        SetMoneyText();

        SoonPungUpgrade();

        GaCahTicketPrcie.text = "티켓 가격  : " + GameManager.instance.TichketPrice.ToString();

        if (GameManager.instance.SoonPungRealLv < 9)
            SoonPungLvTxt.text = $"출산 레벨 : {GameManager.instance.SoonPungRealLv + 1} ( 남은 도티 수 : {GameManager.instance.NeedLvDotty[GameManager.instance.SoonPungRealLv] - GameManager.instance.currentGetDotty} )";
        else
            SoonPungLvTxt.text = $"출산 레벨 : MAX";

        SoonPungLvSlider.value = GameManager.instance.currentGetDotty / (float)GameManager.instance.NeedLvDotty[GameManager.instance.SoonPungRealLv];

        GaCahTicket.text = "뽑기 티켓 : " + GameManager.instance.GTicket.ToString();

        HospitalText();
    }

    void HospitalText()
    {
        hospitalTxt.text = $"병원 레벨 : {GameManager.instance.hospitalLv}";
        currentDottyinHos.text = $"현재 도티 수 : {GameManager.instance.CurrentHosDottyCount} / {GameManager.instance.MaxCurrentDootyCoun}";
        string str = $"{GameManager.instance.hospitalTime}초마다 도티 {GameManager.instance.HosPeople * GameManager.instance.hospitalLv}명 출산";
        if (GameManager.instance.hospitalLv >= 5)
            str += "\n병원에서 출산하는 도티는 쌍둥이 확률 10% 증가";
        HosExplain.text = str ;
        if (GameManager.instance.SoonPungRealLv < 10)
            HosUpgradePrice.text = $"가격 : {FormatKoreanCurrency((long)GameManager.instance.HospUpgradePrice[GameManager.instance.hospitalLv])}";
        else
            HosUpgradePrice.text = $"";

        for (int i = 0; i < HosUpgradeParent.transform.childCount; i ++)
        {
            GameObject obj = HosUpgradeParent.transform.GetChild(i).gameObject;

            TextMeshProUGUI NameTxt = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI EffectTxt = obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI UpGradePriceTxt = obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

            string name = "";
            string Explain = "";
            switch (i)
            {
                case 0:
                    name = $"병원 인구 Lv.{GameManager.instance.HosAddToolLv[i]}";
                    Explain = $"병원의 인구 수가 증가합니다.(현재 {GameManager.instance.HosPeople}명)";
                    break;
                case 1:
                    name = $"신생아실 공간 확장 Lv.{GameManager.instance.HosAddToolLv[i]}";
                    Explain = $"병원에 최대로 있을 수 있는 도티의 수 입니다.";
                    break;
                case 2:
                    name = $"출산 속도 Lv.{GameManager.instance.HosAddToolLv[i]}";
                    Explain = $"출산 속도가 증가합니다. (현재 {GameManager.instance.hospitalTime.ToString("F2")}초)";
                    break;
                case 3:
                    name = $"인원 보충 Lv.{GameManager.instance.HosAddToolLv[i]}";
                    Explain = $"{GameManager.instance.GetHosPeopleTime}초마다 도시에 있던 시민 {GameManager.instance.GetHosPeopleCoun}명을 병원에 데려옵니다.)";
                    break;
            }
            NameTxt.text = name ;
            EffectTxt.text = Explain ;


            if (GameManager.instance.HosAddToolLv[i] < 10)
                UpGradePriceTxt.text = $"가격 : {GameManager.instance.HospAddTooUpgradePrice[i]}원";
            else
                UpGradePriceTxt.text = "MAX";
        }

    }



    public void SoonPungUpgrade()
    {
        for(int i = 0; i < SoonPungParent.transform.childCount; i++)
        {
            GameObject obj = SoonPungParent.transform.GetChild(i).gameObject;

            TextMeshProUGUI NameTxt = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI EffectTxt = obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI UpGradePriceTxt = obj.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();



            switch (i)
            {
                case 0:
                    NameTxt.text = $"자동 출산 LV.{GameManager.instance.SoonPungLv[i]}";
                    EffectTxt.text = $"{GameManager.instance.AutoTime.ToString("F2")}초마다 1명의 도티를 자동 출산합니다.";
                    break;
                case 1:
                    NameTxt.text = $"출산 장려금 LV.{GameManager.instance.SoonPungLv[i]}";
                    EffectTxt.text = $"출산시 추가로 {GameManager.instance.SoonPungMoney}원을 획득합니다.";
                    break;
                case 2:
                    NameTxt.text = $"자산 관리 LV.{GameManager.instance.SoonPungLv[i]}";
                    EffectTxt.text = $"돈 획득시 추가로 {((GameManager.instance.MoneyValue - 1) * 100).ToString("f1")}% 획득";
                    break;
                case 3:
                    NameTxt.text = $"쌍둥이 출산 LV.{GameManager.instance.SoonPungLv[i]}";
                    EffectTxt.text = $"{GameManager.instance.DoubleRand}% 확률로 쌍둥이 출산";
                    break;
                case 4:
                    NameTxt.text = $"순산 도구 LV.{GameManager.instance.SoonPungLv[i]}";
                    EffectTxt.text = $"도티가 사회에서 나쁜 일을 당할 확률이 {GameManager.instance.BadRandMinus}% 감소합니다.";
                    break;
            }

            if (GameManager.instance.SoonPungLv[i] < 10)
                UpGradePriceTxt.text = "업그레이드 : " + GameManager.instance.SoonPungPrice[i].ToString() + "원";
            else
                UpGradePriceTxt.text = "MAX";
        }   
    }

    public void LevelUP()
    {
        LevelObj.SetActive(true);
        TextMeshProUGUI LevelText = LevelObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI LevelEx = LevelObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        LevelText.text = $"{GameManager.instance.SoonPungRealLv } -> {GameManager.instance.SoonPungRealLv + 1}";
        LevelEx.text = $"뽑기 티켓 + {GameManager.instance.LvUpGtick[GameManager.instance.SoonPungRealLv-1]}\n {GameManager.instance.LevelBuff[GameManager.instance.SoonPungRealLv]}";

    }


    void SetMoneyText()
    {
        long money = (long)GameManager.instance.Money;

        string result = FormatKoreanCurrency(money);
        MoneyText.text = "현재 돈 : " + result;
        MoneyText2.text = "현재 돈 : " + result;
    }

    public string FormatKoreanCurrency(long money)
    {
        if (money == 0) return "원";

        long eok = money / 100000000;     // ��
        money %= 100000000;

        long man = money / 10000;         // ��
        money %= 10000;

        long won = money;                 // ��

        string result = "";

        if (eok > 0) result += $"{eok}억 ";
        if (man > 0) result += $"{man}만 ";
        if (won > 0) result += $"{won}";

        result = result.Trim();
        result += "원";

        return result;
    }


    public void DottyResult(string str)
    {

        if(GameManager.instance.currentMap == 0)
        {
            DottyResultObj.SetActive(true);


            DottyResultTxt.text = str;
        } else if(GameManager.instance.currentMap == 1)
        {
            DottyResultObj2.SetActive(true);


            DottyResultTxt2.text = str;
        }



    }

    public void CloseDottyResult()
    {
        TextMeshProUGUI text = DottyResultObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        text.text = "";
        DottyResultObj.SetActive(false);
    }

    public void Message(string message)
    {
        if(MessageCoroutine != null)
            StopCoroutine(MessageCoroutine);

        MessageText.text = message;
        MessageText.color = new Color(MessageText.color.r, MessageText.color.g, MessageText.color.b, 1f);

        MessageCoroutine = StartCoroutine(MessageFade(2.5f));
    }

    IEnumerator MessageFade(float time)
    {
        yield return new WaitForSeconds(time);
        float fade = 1;
        while(fade > 0)
        {
            fade -= Time.deltaTime;

            MessageText.color = new Color(MessageText.color.r, MessageText.color.g, MessageText.color.b, fade);

            yield return null;
        }

        MessageText.text = "";
        MessageText.color = new Color(MessageText.color.r, MessageText.color.g, MessageText.color.b, 0f);
    }

    public void Message2(string message)
    {
        if (MessageCoroutine != null)
            StopCoroutine(MessageCoroutine);

        MessageText2.text = message;
        MessageText2.color = new Color(MessageText2.color.r, MessageText2.color.g, MessageText2.color.b, 1f);

        MessageCoroutine = StartCoroutine(MessageFade2(2.5f));
    }

    IEnumerator MessageFade2(float time)
    {
        yield return new WaitForSeconds(time);
        float fade = 1;
        while (fade > 0)
        {
            fade -= Time.deltaTime;

            MessageText2.color = new Color(MessageText2.color.r, MessageText2.color.g, MessageText2.color.b, fade);

            yield return null;
        }

        MessageText2.text = "";
        MessageText2.color = new Color(MessageText2.color.r, MessageText2.color.g, MessageText2.color.b, 0f);
    }
}

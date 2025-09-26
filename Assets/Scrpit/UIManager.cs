using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;




public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public TextMeshProUGUI MessageText;
    private Coroutine MessageCoroutine;
    public TextMeshProUGUI DottyCountText;
    public TextMeshProUGUI MoneyText;
    public GameObject DottyResultObj;
    [Header("출산 지원")]
    public GameObject SoonPungParent;

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
                    EffectTxt.text = $"돈 획득시 추가로 {(int)((GameManager.instance.MoneyValue - 1) * 100)}% 획득";
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

            UpGradePriceTxt.text = "업그레이드 : " + GameManager.instance.SoonPungPrice[i].ToString() + "원";
        }
    }



    void SetMoneyText()
    {
        long money = (long)GameManager.instance.Money;

        string result = FormatKoreanCurrency(money);
        MoneyText.text = "현재 돈 : " + result;
    }

    string FormatKoreanCurrency(long money)
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
        DottyResultObj.SetActive(true);

        TextMeshProUGUI text = DottyResultObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        text.text = str;


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
}

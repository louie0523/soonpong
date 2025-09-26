using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


[System.Serializable]
public class SoonPungUI
{
    public Sprite icon;
    public string Name;
    public string Explain;
    public List<float> values = new List<float>();
}


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public TextMeshProUGUI DottyCountText;
    public TextMeshProUGUI MoneyText;
    public GameObject DottyResultObj;
    [Header("출산 지원")]
    public List<SoonPungUI> soonPungUIs = new List<SoonPungUI>();
    public GameObject SoonpungBtnPrefab;
    public GameObject SoonpungBtnParent;

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
}

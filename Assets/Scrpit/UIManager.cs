using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    public TextMeshProUGUI DottyCountText;
    public TextMeshProUGUI MoneyText;
    public GameObject DottyResultObj;


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
        DottyCountText.text = $"���� ���� ��Ƽ : {GameManager.instance.currentDotty.Count}��";

        SetMoneyText();
    }

    void SetMoneyText()
    {
        long money = (long)GameManager.instance.Money;

        string result = FormatKoreanCurrency(money);
        MoneyText.text = "���� �� : " + result;
    }

    string FormatKoreanCurrency(long money)
    {
        if (money == 0) return "0��";

        long eok = money / 100000000;     // ��
        money %= 100000000;

        long man = money / 10000;         // ��
        money %= 10000;

        long won = money;                 // ��

        string result = "";

        if (eok > 0) result += $"{eok}�� ";
        if (man > 0) result += $"{man}�� ";
        if (won > 0) result += $"{won}";

        result = result.Trim();
        result += "��";

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

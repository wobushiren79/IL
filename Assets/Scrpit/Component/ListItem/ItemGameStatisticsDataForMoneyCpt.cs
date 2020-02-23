using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameStatisticsDataForMoneyCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    public void SetData(Sprite spIcon, Color colorIcon, string name, Color colorName, long moneyL, long moneyM, long moneyS)
    {
        SetIcon(spIcon, colorIcon);
        SetName(name, colorName);
        SetMoney(moneyL, moneyM, moneyS);
    }

    public void SetData(Sprite spIcon, string name, long moneyL, long moneyM, long moneyS)
    {
        SetData(spIcon, Color.white, name, Color.black, moneyL, moneyM, moneyS);
    }

    public void SetIcon(Sprite spIcon, Color spColor)
    {
        if (ivIcon != null)
        {
            ivIcon.sprite = spIcon;
            ivIcon.color = spColor;
        }
    }

    public void SetName(string name, Color colorName)
    {
        if (tvName != null)
        {
            tvName.text = name;
            tvName.color = colorName;
        }     
    }

    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (tvMoneyL)
        {
            tvMoneyL.text = moneyL + "";
        }
        if (tvMoneyM)
        {
            tvMoneyM.text = moneyM + "";
        }
        if (tvMoneyS)
        {
            tvMoneyS.text = moneyS + "";
        }
    }
}
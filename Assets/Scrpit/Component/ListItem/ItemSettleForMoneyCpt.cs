using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class ItemSettleForMoneyCpt : ItemGameBaseCpt
{
    public Image ivIcon;
    public Text tvContent;
    public Text tvStatus;

    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public void SetData(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
        if (tvContent != null)
            tvContent.text = name;
        SetPrice(moneyL, moneyM, moneyS);
        if (tvStatus != null)
        {
            //收入
            if (status == 1)
            {
                tvStatus.text = GameCommonInfo.GetUITextById(181);
                tvStatus.color = Color.green;
            }
            //支出
            else if (status == 0)
            {
                tvStatus.text = GameCommonInfo.GetUITextById(182);
                tvStatus.color = Color.red;
            }
        }
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetPrice(long moneyL, long moneyM, long moneyS)
    {
        if (moneyL <= 0)
        {
            objPriceL.SetActive(false);
        }
        if (moneyM <= 0)
        {
            objPriceM.SetActive(false);
        }

        if (tvPriceL != null)
            tvPriceL.text = moneyL + "";
        if (tvPriceM != null)
            tvPriceM.text = moneyM + "";
        if (tvPriceS != null)
            tvPriceS.text = moneyS + "";
    }
}
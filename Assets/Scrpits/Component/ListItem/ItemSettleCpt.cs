using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class ItemSettleCpt : ItemGameBaseCpt
{
    public Image ivIcon;

    public Text tvName;
    public Text tvStatus;

    public GameObject objConsume;
    public Text tvConsume;
    public GameObject objPrice;
    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public void SetData(Sprite spIcon, string name, int status, long moneyL, long moneyM, long moneyS)
    {
        objPrice.SetActive(true);
        objConsume.SetActive(false);
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
        if (tvName != null)
            tvName.text = name;
        SetPrice(moneyL, moneyM, moneyS);
        if (tvStatus != null)
        {
            //收入
            if (status == 1)
            {
                tvStatus.text = TextHandler.Instance.manager.GetTextById(181);
                tvStatus.color = Color.green;
            }
            //支出
            else if (status == 0)
            {
                tvStatus.text = TextHandler.Instance.manager.GetTextById(182);
                tvStatus.color = Color.red;
            }
        }
    }


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="number"></param>
    /// <param name="icon"></param>
    /// <param name="name"></param>
    public void SetData(string number, Sprite icon, string name, Color numerColor)
    {
        objPrice.SetActive(false);
        objConsume.SetActive(true);
        if (ivIcon != null)
            ivIcon.sprite = icon;
        if (tvName != null)
            tvName.text = name;
        if (tvConsume != null)
        {
            tvConsume.text = number;
            tvConsume.color = numerColor;
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
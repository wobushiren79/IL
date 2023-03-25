using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemPopupRecordCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvNumber;

    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="name"></param>
    /// <param name="number"></param>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetData(Sprite spIcon, string name, long number, long moneyL, long moneyM, long moneyS)
    {
        SetIcon(spIcon);
        SetName(name);
        SetNumber(number);
        SetSellMoney(moneyL, moneyM, moneyS);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        if (tvNumber != null)
            if (number == 0)
            {
                tvNumber.text ="";
            }
            else
            {
                tvNumber.text = number + TextHandler.Instance.manager.GetTextById(81);
            }

    }

    /// <summary>
    /// 设置销售金额
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetSellMoney(long moneyL, long moneyM, long moneyS)
    {
        if (tvMoneyL != null)
            tvMoneyL.text = moneyL + "";
        if (tvMoneyM != null)
            tvMoneyM.text = moneyM + "";
        if (tvMoneyS != null)
            tvMoneyS.text = moneyS + "";
    }
}
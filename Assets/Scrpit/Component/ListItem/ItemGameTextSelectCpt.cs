using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ItemGameTextSelectCpt : ItemGameBaseCpt
{
    [Header("控件")]
    public Text tvContent;
    public Button btSubmit;

    [Header("数据")]
    public TextInfoBean textData;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(Submit);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemData"></param>
    public void SetData(TextInfoBean itemData)
    {
        this.textData = itemData;
        SetText(itemData.content);
    }

    /// <summary>
    ///  设置文本数据
    /// </summary>
    /// <param name="content"></param>
    public void SetText(string content)
    {
        UIGameText uiGameText = (UIGameText)uiComponent;
        if (tvContent != null && uiGameText != null)
        {
            string contentDetails = uiGameText.SetContentDetails(content);
            tvContent.text = contentDetails;
        }
        UIGameManager uiGameManager = uiGameText.GetUIMananger<UIGameManager>();
        //检测是否有钱
        textData.GetAddMoney(out long moneyL, out long moneyM, out long moneyS);
        if (moneyL > 0 || moneyM > 0 || moneyS > 0)
        {
            if (uiGameManager.gameDataManager.gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
            {

            }
            else
            {
                tvContent.text += "("+ GameCommonInfo.GetUITextById(1005) + ")";
                tvContent.color = Color.red;
            }
        }
    }

    public void Submit()
    {
        UIGameText uiGameText = (UIGameText)uiComponent;
        UIGameManager uiGameManager = uiGameText.GetUIMananger<UIGameManager>();
        textData.GetAddMoney(out long moneyL, out long moneyM, out long moneyS);
        if (moneyL > 0 || moneyM > 0 || moneyS > 0)
        {
            if (uiGameManager.gameDataManager.gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
            {
                uiGameManager.gameDataManager.gameData.PayMoney(moneyL, moneyM, moneyS);
            }
            else
            {
                //uiGameManager.toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
                return;
            }
        }
        uiGameText.SelectText(textData);
    }
}
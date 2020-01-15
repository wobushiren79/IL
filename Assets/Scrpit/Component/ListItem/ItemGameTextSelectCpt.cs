using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ItemGameTextSelectCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
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
        UIGameManager uiGameManager = uiGameText.GetUIManager<UIGameManager>();
        //检测是否有钱
        RewardTypeEnumTools.GetRewardForAddMoney(textData.reward_data, out long moneyL, out long moneyM, out long moneyS);
        if (moneyL > 0 || moneyM > 0 || moneyS > 0)
        {
            if (uiGameManager.gameDataManager.gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
            {

            }
            else
            {
                tvContent.text += "(" + GameCommonInfo.GetUITextById(1005) + ")";
                tvContent.color = Color.red;
            }
        }
    }

    public void Submit()
    {
        UIGameText uiGameText = (UIGameText)uiComponent;
        UIGameManager uiGameManager = uiGameText.GetUIManager<UIGameManager>();
        if (CheckUtil.StringIsNull(textData.pre_data_minigame))
        {
            //如果没有前置条件 则直接进行下一步
            RewardTypeEnumTools.GetRewardForAddMoney(textData.reward_data, out long moneyL, out long moneyM, out long moneyS);
            if (moneyL > 0 || moneyM > 0 || moneyS > 0)
            {
                if (uiGameManager.gameDataManager.gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
                {
                    uiGameManager.gameDataManager.gameData.PayMoney(moneyL, moneyM, moneyS);
                }
                else
                {
                    //uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
                    return;
                }
            }
            uiGameText.SelectText(textData);
        }
        else
        {
            DialogBean dialogBean = new DialogBean();
            PickForCharacterDialogView dialogView = (PickForCharacterDialogView)uiGameManager.dialogManager.CreateDialog(DialogEnum.PickForCharacter, this, dialogBean);
            PreTypeForMiniGameEnumTools.GetPlayerNumber(textData.pre_data_minigame, out int playerNumber);
            dialogView.SetPickCharacterMax(playerNumber);
        }

    }

    #region dilaog回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (CheckUtil.StringIsNull(textData.pre_data_minigame))
            return;
        UIGameText uiGameText = (UIGameText)uiComponent;
        PickForCharacterDialogView pickDialog = (PickForCharacterDialogView)dialogView;
        uiGameText.callBack.UITextSelectResult(textData, pickDialog.GetPickCharacter());
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
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
            btSubmit.onClick.AddListener(OnClickSubmit);
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
    }

    /// <summary>
    /// 提交按钮
    /// </summary>
    public void OnClickSubmit()
    {
        UIGameText uiGameText = (UIGameText)uiComponent;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //检测是否启用小游戏
        if (textData.pre_data_minigame.IsNull())
        {
            List<PreTypeBean> listPre = PreTypeEnumTools.GetListPreData(textData.pre_data);
            foreach (PreTypeBean itemPreData in listPre)
            {
                PreTypeEnumTools.GetPreDetails(itemPreData, gameData);
                if (!itemPreData.isPre)
                {
                    UIHandler.Instance.ToastHint<ToastView>(itemPreData.spPreIcon, itemPreData.preFailStr);
                    return;
                }
            }
            //完成前置条件
            PreTypeEnumTools.CompletePre(listPre, gameData);
            //完成所有奖励
            RewardTypeEnumTools.CompleteReward(null, textData.reward_data);
            uiGameText.SelectText(textData);
        }
        else
        {
            DialogBean dialogData = new DialogBean();
            dialogData.dialogType = DialogEnum.PickForCharacter;
            dialogData.callBack = this;
            PickForCharacterDialogView dialogView = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
            PreTypeForMiniGameEnumTools.GetPlayerNumber(textData.pre_data_minigame, out int playerNumber);
            dialogView.SetPickCharacterMax(playerNumber);
        }
    }

    #region dilaog回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (textData.pre_data_minigame.IsNull())
            return;
        UIGameText uiGameText = (UIGameText)uiComponent;
        PickForCharacterDialogView pickDialog = dialogView as PickForCharacterDialogView;
        if (uiGameText.callBack != null)
            uiGameText.callBack.UITextSelectResult(textData, pickDialog.GetPickCharacter());
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
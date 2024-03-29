﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemMountainInfiniteTowersCpt : ItemGameBaseCpt,DialogView.IDialogCallBack
{
    public Text tvLayer;
    public Text tvIsSend;
    public ProgressView pvForSend;
    public Button btCancel;
    public Button btContinue;

    public GameObject objMembersContianer;
    public GameObject objMembersModel;

    protected UserInfiniteTowersBean infiniteTowersData;

    private void Awake()
    {
        if (btContinue)
            btContinue.onClick.AddListener(OnClickForContinue);

        if (btCancel)
            btCancel.onClick.AddListener(OnClickForCancel);
    }

    public void SetData(UserInfiniteTowersBean infiniteTowersData)
    {
        this.infiniteTowersData = infiniteTowersData;
        SetLayer(infiniteTowersData.layer);
        SetIsSend(infiniteTowersData.isSend, infiniteTowersData.proForSend);
        SetMembers(infiniteTowersData.listMembers);
    }

    public void SetLayer(long layer)
    {
        tvLayer.text = layer + "";
    }

    public void SetIsSend(bool isSend, float sendPro)
    {
        if (isSend)
        {
            tvIsSend.gameObject.SetActive(true);
            btContinue.gameObject.SetActive(false);
            pvForSend.gameObject.SetActive(true);
            pvForSend.SetData(sendPro);
        }
        else
        {
            tvIsSend.gameObject.SetActive(false);
            btContinue.gameObject.SetActive(true);
            pvForSend.gameObject.SetActive(false);
            pvForSend.SetData(0);
        }
    }

    public void SetMembers(List<string> listMembers)
    {
        CptUtil.RemoveChildsByActive(objMembersContianer);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        for (int i = 0; i < listMembers.Count; i++)
        {
            string memberId = listMembers[i];
            GameObject objItem = Instantiate(objMembersContianer, objMembersModel);

            CharacterUICpt characterUI = objItem.GetComponentInChildren<CharacterUICpt>();
            Text tvName = objItem.GetComponentInChildren<Text>();

            //设置数据
            CharacterBean characterData = gameData.GetCharacterDataById(memberId);
            if (characterData == null)
            {
                //如果没有这个角色。该角色已经被辞退
                continue;
            }
            characterUI.SetCharacterData(characterData.body, characterData.equips);
            tvName.text = characterData.baseInfo.name;
        }
    }

    /// <summary>
    /// 点击 取消
    /// </summary>
    public void OnClickForCancel()
    {       
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.manager.GetTextById(3111);
        dialogData.dialogPosition = 0;
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    /// <summary>
    /// 点击继续
    /// </summary>
    public void OnClickForContinue()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        foreach (string memberId in infiniteTowersData.listMembers)
        {
            CharacterBean characterData = gameData.GetCharacterDataById(memberId);
            if (characterData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest
                && characterData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work)
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1141));
                return;
            }
        }

        //跳转场景
        GameCommonInfo.SetInfiniteTowersPrepareData(infiniteTowersData);
        GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameInfiniteTowersScene);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogBean.dialogPosition == 0)
        {
            //删除确认
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData(); 
            if (infiniteTowersData.isSend)
            {
                foreach (string memberId in infiniteTowersData.listMembers)
                {
                    CharacterBean characterData = gameData.GetCharacterDataById(memberId);
                    characterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                }
            }
            infiniteTowersData.proForSend = -1;
            gameData.RemoveInfiniteTowersData(infiniteTowersData);
            uiComponent.RefreshUI();
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
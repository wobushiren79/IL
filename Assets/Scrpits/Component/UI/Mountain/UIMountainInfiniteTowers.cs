﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIMountainInfiniteTowers : BaseUIComponent, DialogView.IDialogCallBack
{
    //返回按钮
    public Button btBack;
    public Button btStart;
    public Button btSend;
    public Button btClean;

    public Text tvMaxLayer;
    public ScrollGridVertical gridVertical;
    public GameObject objNull;

    protected List<UserInfiniteTowersBean> listData = new List<UserInfiniteTowersBean>();

    public override void Awake()
    {
        base.Awake();
        if (btBack)
            btBack.onClick.AddListener(OnClickForBack);
        if (btStart)
            btStart.onClick.AddListener(OnClickForStart);
        if (btSend)
            btSend.onClick.AddListener(OnClickForSend);
        if (btClean)
            btClean.onClick.AddListener(OnClickForClean);
        if (gridVertical)
            gridVertical.AddCellListener(OnCellForInfiniteTowers);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameControlHandler.Instance.StopControl();

        GameTimeHandler.Instance.SetTimeStatus(true);

        //大于10层才显示派遣
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (userAchievement.maxInfiniteTowersLayer > 10)
        {
            btSend.gameObject.SetActive(true);
        }
        else
        {
            btSend.gameObject.SetActive(false);
        }
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        GameTimeHandler.Instance.SetTimeRestore();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listData = gameData.listInfinteTowers;
        //纠错处理 如果员工被辞退了 则不显示该条数据
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData =  listData[i];
            bool isAllAt = true;
            foreach (var memberId in itemData.listMembers)
            {
                CharacterBean characterData = gameData.GetCharacterDataById(memberId);
                if (characterData == null)
                {
                    isAllAt = false;
                    break;
                }              
            }
            if (!isAllAt)
            {
                listData.Remove(itemData);
                i--;
            }
        }

        if (listData.Count <= 0)
        {
            objNull.SetActive(true);
        }
        else
        {
            objNull.SetActive(false);
        }
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        SetMaxLayer(userAchievement.maxInfiniteTowersLayer);
        gridVertical.SetCellCount(listData.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 设置最大层数
    /// </summary>
    /// <param name="maxLayer"></param>
    public void SetMaxLayer(long maxLayer)
    {
        if (tvMaxLayer)
            tvMaxLayer.text = maxLayer + "";
    }

    /// <summary>
    /// 数据显示
    /// </summary>
    /// <param name="scrollGridCell"></param>
    public void OnCellForInfiniteTowers(ScrollGridCell scrollGridCell)
    {
        ItemMountainInfiniteTowersCpt itemCpt = scrollGridCell.GetComponent<ItemMountainInfiniteTowersCpt>();
        UserInfiniteTowersBean userInfiniteTowers = listData[scrollGridCell.index];
        itemCpt.SetData(userInfiniteTowers);
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public virtual void OnClickForBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }

    /// <summary>
    /// 点击开始
    /// </summary>
    public void OnClickForStart()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 0;
        dialogData.dialogType = DialogEnum.PickForCharacter;
        dialogData.callBack = this;
        PickForCharacterDialogView pickForCharacterDialog = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
        //排除不能参加的人
        List<string> listExpel = new List<string>();
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            CharacterBean itemCharacter = listCharacter[i];
            if ((itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work))
            {
                listExpel.Add(itemCharacter.baseInfo.characterId);
            }
        }

        pickForCharacterDialog.SetExpelCharacter(listExpel);
        pickForCharacterDialog.SetPickCharacterMax(3);
    }

    /// <summary>
    /// 点击派遣
    /// </summary>
    public void OnClickForSend()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 1;
        dialogData.dialogType = DialogEnum.PickForCharacter;
        dialogData.callBack = this;
        PickForCharacterDialogView pickForCharacterDialog = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
        //排除主角和不能参加的人
        List<string> listExpel = new List<string>();
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            CharacterBean itemCharacter = listCharacter[i];
            if (itemCharacter == gameData.userCharacter
                || (itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work))
            {
                listExpel.Add(itemCharacter.baseInfo.characterId);
            }
        }
        pickForCharacterDialog.SetExpelCharacter(listExpel);
        pickForCharacterDialog.SetPickCharacterMax(3);
    }


    /// <summary>
    /// 重置所有
    /// </summary>
    public void OnClickForClean()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 1;
        dialogData.callBack = this;
        dialogData.content = TextHandler.Instance.manager.GetTextById(3112);
        dialogData.dialogType = DialogEnum.Normal;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as PickForCharacterDialogView)
        {
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            UserInfiniteTowersBean infiniteTowersData = new UserInfiniteTowersBean();
            List<CharacterBean> listMembers = pickForCharacterDialog.GetPickCharacter();
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (dialogBean.dialogPosition == 0)
            {
                //亲自
                infiniteTowersData.isSend = false;
                foreach (CharacterBean itemCharacter in listMembers)
                {
                    infiniteTowersData.listMembers.Add(itemCharacter.baseInfo.characterId);
                }
                gameData.AddInfinteTowersData(infiniteTowersData);
                //跳转场景
                GameCommonInfo.SetInfiniteTowersPrepareData(infiniteTowersData);
                GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameInfiniteTowersScene);
            }
            else if (dialogBean.dialogPosition == 1)
            {
                //派遣
                infiniteTowersData.isSend = true;
                //检测
                foreach (CharacterBean itemCharacter in listMembers)
                {
                    WorkerStatusEnum workerStatusEnum = itemCharacter.baseInfo.GetWorkerStatus();
                    if (workerStatusEnum != WorkerStatusEnum.Rest && workerStatusEnum != WorkerStatusEnum.Work)
                    {
                        UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1142));
                        return;
                    }
                }
                //派遣
                foreach (CharacterBean itemCharacter in listMembers)
                {
                    infiniteTowersData.listMembers.Add(itemCharacter.baseInfo.characterId);
                    itemCharacter.baseInfo.SetWorkerStatus(WorkerStatusEnum.InfiniteTowers);
                }
                //计算每层攀登几率
                infiniteTowersData.InitSuccessRate(GameItemsHandler.Instance.manager, listMembers);
                gameData.AddInfinteTowersData(infiniteTowersData);
                RefreshUI();
            }
        }
        else
        {
            if (dialogBean.dialogPosition == 1)
            {
                //重置所有爬塔记录
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                gameData.CleanInfinteTowers();
                RefreshUI();
            }
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
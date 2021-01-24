using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIMountainInfiniteTowers : BaseUIComponent, DialogView.IDialogCallBack
{
    //返回按钮
    public Button btBack;
    public Button btStart;
    public Button btSend;

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
        UserAchievementBean userAchievement = uiGameManager.gameData.GetAchievementData();
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

    public override void RefreshUI()
    {
        base.RefreshUI();
        listData = uiGameManager.gameData.listInfinteTowers;
        gridVertical.SetCellCount(listData.Count);
        gridVertical.RefreshAllCells();
        if (listData.Count <= 0)
        {
            objNull.SetActive(true);
        }
        else
        {
            objNull.SetActive(false);
        }
        UserAchievementBean userAchievement = uiGameManager.gameData.GetAchievementData();
        SetMaxLayer(userAchievement.maxInfiniteTowersLayer);
    }

    /// <summary>
    /// 设置最大层数
    /// </summary>
    /// <param name="maxLayer"></param>
    public void SetMaxLayer(long maxLayer)
    {
        if (tvMaxLayer)
            tvMaxLayer.text = maxLayer+"";
    }

    /// <summary>
    /// 数据显示
    /// </summary>
    /// <param name="scrollGridCell"></param>
    public void OnCellForInfiniteTowers(ScrollGridCell scrollGridCell)
    {
        ItemMountainInfiniteTowersCpt itemCpt = scrollGridCell.GetComponent<ItemMountainInfiniteTowersCpt>();
        itemCpt.SetData(listData[scrollGridCell.index]);
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public virtual void OnClickForBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

    /// <summary>
    /// 点击开始
    /// </summary>
    public void OnClickForStart()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 0;
        PickForCharacterDialogView pickForCharacterDialog = DialogHandler.Instance.CreateDialog<PickForCharacterDialogView>(DialogEnum.PickForCharacter, this, dialogData);
        //排除不能参加的人
        List<string> listExpel = new List<string>();
        List<CharacterBean> listCharacter = uiGameManager.gameData.GetAllCharacterData();
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 1;
        PickForCharacterDialogView pickForCharacterDialog = DialogHandler.Instance.CreateDialog<PickForCharacterDialogView>(DialogEnum.PickForCharacter, this, dialogData);
        //排除主角和不能参加的人
        List<string> listExpel = new List<string>();
        List<CharacterBean> listCharacter = uiGameManager.gameData.GetAllCharacterData();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            CharacterBean itemCharacter = listCharacter[i];
            if (itemCharacter == uiGameManager.gameData.userCharacter
                || (itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work))
            {
                listExpel.Add(itemCharacter.baseInfo.characterId);
            }
        }
        pickForCharacterDialog.SetExpelCharacter(listExpel);
        pickForCharacterDialog.SetPickCharacterMax(3);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as PickForCharacterDialogView)
        {
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            UserInfiniteTowersBean infiniteTowersData = new UserInfiniteTowersBean();
            List<CharacterBean> listMembers = pickForCharacterDialog.GetPickCharacter();
            if (dialogBean.dialogPosition == 0)
            {
                //亲自
                infiniteTowersData.isSend = false;
                foreach (CharacterBean itemCharacter in listMembers)
                {
                    infiniteTowersData.listMembers.Add(itemCharacter.baseInfo.characterId);
                }
                uiGameManager.gameData.AddInfinteTowersData(infiniteTowersData);
                //跳转场景
                GameCommonInfo.SetInfiniteTowersPrepareData(infiniteTowersData);
                SceneUtil.SceneChange(ScenesEnum.GameInfiniteTowersScene);
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
                        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1142));
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
                uiGameManager.gameData.AddInfinteTowersData(infiniteTowersData);
                RefreshUI();
            }
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
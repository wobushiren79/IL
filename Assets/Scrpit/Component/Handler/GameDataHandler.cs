using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameDataHandler : BaseHandler<GameDataHandler,GameDataManager>, DialogView.IDialogCallBack
{
    public enum NotifyTypeEnum
    {
        AddMoney = 1,
        PayMoney = 2,
        MenuResearchChange = 3,
        BedResearchChange = 4,
        InfiniteTowerProChange = 5,
    }

    //系统清理倒计时
    protected int timeForSystemClear = 0;
    //10分钟自动清理一次
    protected int maxTimeForSystemClear = 600;

    protected Action<NotifyTypeEnum, object[]> notifyForData;


    private void Start()
    {
        StartCoroutine(CoroutineForRealtime());
        StartCoroutine(CoroutineForTime());
    }

    public void RegisterNotifyForData(Action<NotifyTypeEnum, object[]> notifyForData)
    {
        this.notifyForData += notifyForData;
    }

    public void UnRegisterNotifyForData(Action<NotifyTypeEnum, object[]> notifyForData)
    {
        this.notifyForData -= notifyForData;
    }

    /// <summary>
    /// 协程-真实时间处理
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForRealtime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            HandleForPlayTime();
            HandleForSystemClear();
        }
    }

    /// <summary>
    /// 协程-游戏时间处理-受到时间缩放影响
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!GameTimeHandler.Instance.isStopTime)
            {
                HandleForTimeProcess();
            }
        }
    }

    /// <summary>
    /// 游玩时间处理
    /// </summary>
    public void HandleForPlayTime()
    {
        GameDataBean gameData = manager.GetGameData();
        gameData.playTime.AddTimeForHMS(0, 0, 1);
    }


    /// <summary>
    /// 系统清理
    /// </summary>
    public void HandleForSystemClear()
    {
        timeForSystemClear++;
        if (timeForSystemClear >= maxTimeForSystemClear)
        {
            SystemUtil.GCCollect();
            timeForSystemClear = 0;
        }
    }

    /// <summary>
    /// 菜单研究处理
    /// </summary>
    public void HandleForTimeProcess()
    {
        if (GameTimeHandler.Instance.isStopTime)
            return;
        AddTimeProcess(1);
    }

    /// <summary>
    /// 增加所有研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddTimeProcess(int time)
    {
        AddMenuResearch(time);
        AddBedResearch(time);
        AddInfiniteTowers(time);
    }

    /// <summary>
    /// 增加爬塔进度
    /// </summary>
    /// <param name="time"></param>
    public void AddInfiniteTowers(int time)
    {
        if (GameTimeHandler.Instance.isStopTime)
            return; 
        GameDataBean gameData = manager.GetGameData();
        List<UserInfiniteTowersBean> listInfiniteTowersData = gameData.listInfinteTowers;
        if (CheckUtil.ListIsNull(listInfiniteTowersData))
            return;
        float addTime = 0.01f * time;
        List<UserInfiniteTowersBean> listSendData = new List<UserInfiniteTowersBean>();
        for (int i = 0; i < listInfiniteTowersData.Count; i++)
        {
            UserInfiniteTowersBean itemInfiniteTowerData = listInfiniteTowersData[i];
            if (itemInfiniteTowerData.isSend == false)
            {
                //如果不是派遣数据则不处理
                continue;
            }
            listSendData.Add(itemInfiniteTowerData);
            itemInfiniteTowerData.proForSend += addTime;
            if (itemInfiniteTowerData.proForSend >= 1)
            {
                //计算总计攀登层数
                int addLayer = (int)Mathf.Floor(itemInfiniteTowerData.proForSend);
                List<CharacterBean> listCharacterData = gameData.GetCharacterDataByIds(itemInfiniteTowerData.listMembers);
                for (int f = 0; f < addLayer; f++)
                {
                    itemInfiniteTowerData.proForSend = 0;
                    bool isSuccessNextLayer = itemInfiniteTowerData.CheckIsSccessNextLayer();
                    if (isSuccessNextLayer)
                    {
                        //如果是成功攻略
                        //弹出提示
                        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
                        ToastHandler.Instance.ToastHint(string.Format(GameCommonInfo.GetUITextById(1331), itemInfiniteTowerData.layer+""));
                        //添加奖励物品
                        int totalLucky = 0;
                        for (int c = 0; c < listCharacterData.Count; c++)
                        {
                            CharacterBean itemCharacterData = listCharacterData[c];
                            itemCharacterData.GetAttributes(out CharacterAttributesBean characterAttributes);
                            totalLucky += characterAttributes.lucky;
                        }
                        List<RewardTypeBean> listRewardItems = RewardTypeEnumTools.GetRewardItemsForInfiniteTowers(null, itemInfiniteTowerData.layer, totalLucky,true);
                        if (!CheckUtil.ListIsNull(listRewardItems))
                            RewardTypeEnumTools.CompleteReward(listCharacterData, listRewardItems);
                        //增加层数
                        itemInfiniteTowerData.layer++;
                        //达到最大层数
                        UserAchievementBean userAchievement = gameData.GetAchievementData();
                        if (itemInfiniteTowerData.layer > userAchievement.maxInfiniteTowersLayer - 1)
                        {
                            //弹出提示
                            AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
                            ToastHandler.Instance.ToastHint(string.Format(GameCommonInfo.GetUITextById(1332), (itemInfiniteTowerData.layer-1) + ""));
                            itemInfiniteTowerData.proForSend = -1;
                            //还原员工状态
                            for (int c = 0; c < listCharacterData.Count; c++)
                            {
                                CharacterBean itemCharacterData = listCharacterData[c];
                                itemCharacterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                            }
                            //移除数据
                            listInfiniteTowersData.Remove(itemInfiniteTowerData);
                            i--;
                            break;
                        }
                    }
                    else
                    {
                        //弹出提示
                        AudioHandler.Instance.PlaySound(AudioSoundEnum.Passive);
                        ToastHandler.Instance.ToastHint(string.Format(GameCommonInfo.GetUITextById(1333), itemInfiniteTowerData.layer + ""));
                        //如果是失败攻略
                        itemInfiniteTowerData.proForSend = -1;
                        //还原员工状态
                        for (int c = 0; c < listCharacterData.Count; c++)
                        {
                            CharacterBean itemCharacterData = listCharacterData[c];
                            itemCharacterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                        }
                        //移除数据
                        listInfiniteTowersData.Remove(itemInfiniteTowerData);
                        i--;
                        break;
                    }
                }
            }
        }
        notifyForData?.Invoke(NotifyTypeEnum.InfiniteTowerProChange,new object[] { listSendData });
    }


    /// <summary>
    /// 增加菜品研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddBedResearch(int time)
    {
        if (GameTimeHandler.Instance.isStopTime)
            return;
        GameDataBean gameData = manager.GetGameData();
        List<BuildBedBean> listBed = gameData.GetBedListForResearching();
        if (CheckUtil.ListIsNull(listBed))
            return;
        for (int i=0;i< listBed.Count;i++)
        {
            BuildBedBean itemBed = listBed[i];
            //获取研究人员
            CharacterBean researcher = itemBed.GetResearchCharacter(gameData);
            //如果没有研究人员则停止研究
            if (researcher == null)
            {
                itemBed.CancelResearch(gameData);
                continue;
            }
            long addExp = researcher.CalculationBedResearchAddExp();
            bool isCompleteResearch = itemBed.AddResearchExp((int)addExp * time);
            //完成研究
            if (isCompleteResearch)
            {
                itemBed.CompleteResearch(gameData);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1071), itemBed.bedName);
                AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
                ToastHandler.Instance.ToastHint(InnFoodHandler.Instance.manager.GetFoodSpriteByName("ui_features_bed"), toastStr, 5);

                DialogBean dialogData = new DialogBean
                {
                    title = GameCommonInfo.GetUITextById(1048),
                    content = toastStr
                };
                AchievementDialogView achievementDialog = DialogHandler.Instance.CreateDialog<AchievementDialogView>(DialogEnum.Achievement, this, dialogData);
                achievementDialog.SetData(2, "ui_features_bed");
            }
        }
        notifyForData?.Invoke(NotifyTypeEnum.BedResearchChange, new object[] { listBed });
    }

    /// <summary>
    /// 增加菜品研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddMenuResearch(int time)
    {
        if (GameTimeHandler.Instance.isStopTime)
            return; 
        GameDataBean gameData = manager.GetGameData();
        List<MenuOwnBean> listMenu = gameData.GetMenuListForResearching();
        if (CheckUtil.ListIsNull(listMenu))
            return;
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemMenu = listMenu[i];
            //获取研究人员
            CharacterBean researcher = itemMenu.GetResearchCharacter(gameData);
            //如果没有研究人员则停止研究
            if (researcher == null)
            {
                itemMenu.CancelResearch(gameData);
                continue;
            }
            MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(itemMenu.menuId);
            if (menuInfo == null)
                continue;
            long addExp = researcher.CalculationMenuResearchAddExp();
            bool isCompleteResearch = itemMenu.AddResearchExp((int)addExp * time);
            //完成研究
            if (isCompleteResearch)
            {
                itemMenu.CompleteResearch(gameData);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1071), menuInfo.name);
                AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
                ToastHandler.Instance.ToastHint(InnFoodHandler.Instance.manager.GetFoodSpriteByName(menuInfo.icon_key), toastStr, 5);

                DialogBean dialogData = new DialogBean
                {
                    title = GameCommonInfo.GetUITextById(1048),
                    content = toastStr
                };
                AchievementDialogView achievementDialog = DialogHandler.Instance.CreateDialog<AchievementDialogView>(DialogEnum.Achievement, this, dialogData);
                achievementDialog.SetData(1, menuInfo.icon_key);
            }
        }
        notifyForData?.Invoke(NotifyTypeEnum.MenuResearchChange, new object[] { listMenu });
    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void AddMoney(long priceL, long priceM, long priceS)
    {
        GameDataBean gameData = manager.GetGameData();
        gameData.AddMoney(priceL, priceM, priceS); 
        notifyForData?.Invoke(NotifyTypeEnum.AddMoney, new object[] { priceL, priceM, priceS });
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as AchievementDialogView)
        {

        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion

}
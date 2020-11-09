using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameDataHandler : BaseHandler, DialogView.IDialogCallBack, IBaseObserver
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

    protected GameDataManager gameDataManager;
    protected GameTimeHandler gameTimeHandler;
    protected GameItemsManager gameItemsManager;
    protected ToastManager toastManager;
    protected InnFoodManager innFoodManager;
    protected AudioHandler audioHandler;
    protected DialogManager dialogManager;
    protected NpcInfoManager npcInfoManager;
    protected IconDataManager iconDataManager;
    protected InnBuildManager innBuildManager;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    private void Start()
    {
        StartCoroutine(CoroutineForRealtime());
        StartCoroutine(CoroutineForTime());
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
            if (gameTimeHandler != null && !gameTimeHandler.isStopTime)
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
        if (gameDataManager != null)
            gameDataManager.gameData.playTime.AddTimeForHMS(0, 0, 1);
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
        if (gameTimeHandler == null)
            return;
        if (gameTimeHandler.isStopTime)
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
        if (gameTimeHandler == null || gameItemsManager == null || gameDataManager == null)
            return;
        List<UserInfiniteTowersBean> listInfiniteTowersData = gameDataManager.gameData.listInfinteTowers;
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
                List<CharacterBean> listCharacterData = gameDataManager.gameData.GetCharacterDataByIds(itemInfiniteTowerData.listMembers);
                for (int f = 0; f < addLayer; f++)
                {
                    itemInfiniteTowerData.proForSend = 0;
                    bool isSuccessNextLayer = itemInfiniteTowerData.CheckIsSccessNextLayer();
                    if (isSuccessNextLayer)
                    {
                        //如果是成功攻略
                        //弹出提示
                        audioHandler.PlaySound(AudioSoundEnum.Reward);
                        toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1331), itemInfiniteTowerData.layer+""));
                        //增加层数
                        itemInfiniteTowerData.layer++;
                        //添加奖励物品
                        int totalLucky = 0;
                        for (int c = 0; c < listCharacterData.Count; c++)
                        {
                            CharacterBean itemCharacterData = listCharacterData[c];
                            itemCharacterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
                            totalLucky += characterAttributes.lucky;
                        }
                        List<RewardTypeBean> listRewardItems = RewardTypeEnumTools.GetRewardItemsForInfiniteTowers(null, itemInfiniteTowerData.layer, totalLucky);
                        if (!CheckUtil.ListIsNull(listRewardItems))
                            RewardTypeEnumTools.CompleteReward(toastManager, npcInfoManager, iconDataManager, gameItemsManager, innBuildManager, gameDataManager, listCharacterData, listRewardItems);
                        //达到最大层数
                        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
                        if (itemInfiniteTowerData.layer >= userAchievement.maxInfiniteTowersLayer - 1)
                        {
                            //弹出提示
                            audioHandler.PlaySound(AudioSoundEnum.Reward);
                            toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1332), itemInfiniteTowerData.layer + ""));
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
                        audioHandler.PlaySound(AudioSoundEnum.Passive);
                        toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1333), itemInfiniteTowerData.layer + ""));
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
        NotifyAllObserver((int)NotifyTypeEnum.InfiniteTowerProChange, listSendData);
    }


    /// <summary>
    /// 增加菜品研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddBedResearch(int time)
    {
        if (gameTimeHandler == null || gameItemsManager == null || gameDataManager == null)
            return;
        List<BuildBedBean> listBed = gameDataManager.gameData.GetBedListForResearching();
        if (CheckUtil.ListIsNull(listBed))
            return;
        for (int i=0;i< listBed.Count;i++)
        {
            BuildBedBean itemBed = listBed[i];
            //获取研究人员
            CharacterBean researcher = itemBed.GetResearchCharacter(gameDataManager.gameData);
            //如果没有研究人员则停止研究
            if (researcher == null)
            {
                itemBed.CancelResearch(gameDataManager.gameData);
                continue;
            }
            long addExp = researcher.CalculationBedResearchAddExp(gameItemsManager);
            bool isCompleteResearch = itemBed.AddResearchExp((int)addExp * time);
            //完成研究
            if (isCompleteResearch)
            {
                itemBed.CompleteResearch(gameDataManager.gameData);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1071), itemBed.bedName);
                audioHandler.PlaySound(AudioSoundEnum.Reward);
                toastManager.ToastHint(innFoodManager.GetFoodSpriteByName("ui_features_bed"), toastStr, 5);

                DialogBean dialogData = new DialogBean
                {
                    title = GameCommonInfo.GetUITextById(1048),
                    content = toastStr
                };
                AchievementDialogView achievementDialog = (AchievementDialogView)dialogManager.CreateDialog(DialogEnum.Achievement, this, dialogData);
                achievementDialog.SetData(2, "ui_features_bed");
            }
        }
        NotifyAllObserver((int)NotifyTypeEnum.BedResearchChange, listBed);
    }

    /// <summary>
    /// 增加菜品研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddMenuResearch(int time)
    {
        if (gameTimeHandler == null || gameItemsManager == null || gameDataManager == null)
            return;
        List<MenuOwnBean> listMenu = gameDataManager.gameData.GetMenuListForResearching();
        if (CheckUtil.ListIsNull(listMenu))
            return;
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemMenu = listMenu[i];
            //获取研究人员
            CharacterBean researcher = itemMenu.GetResearchCharacter(gameDataManager.gameData);
            //如果没有研究人员则停止研究
            if (researcher == null)
            {
                itemMenu.CancelResearch(gameDataManager.gameData);
                continue;
            }
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(itemMenu.menuId);
            if (menuInfo == null)
                continue;
            long addExp = researcher.CalculationMenuResearchAddExp(gameItemsManager);
            bool isCompleteResearch = itemMenu.AddResearchExp((int)addExp * time);
            //完成研究
            if (isCompleteResearch)
            {
                itemMenu.CompleteResearch(gameDataManager.gameData);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1071), menuInfo.name);
                audioHandler.PlaySound(AudioSoundEnum.Reward);
                toastManager.ToastHint(innFoodManager.GetFoodSpriteByName(menuInfo.icon_key), toastStr, 5);

                DialogBean dialogData = new DialogBean
                {
                    title = GameCommonInfo.GetUITextById(1048),
                    content = toastStr
                };
                AchievementDialogView achievementDialog = (AchievementDialogView)dialogManager.CreateDialog(DialogEnum.Achievement, this, dialogData);
                achievementDialog.SetData(1, menuInfo.icon_key);
            }
        }

        NotifyAllObserver((int)NotifyTypeEnum.MenuResearchChange, listMenu);
    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void AddMoney(long priceL, long priceM, long priceS)
    {
        gameDataManager.gameData.AddMoney(priceL, priceM, priceS);
        NotifyAllObserver((int)NotifyTypeEnum.AddMoney, priceL, priceM, priceS);
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


    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {

            }
            else if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {

            }
        }
    }
    #endregion
}
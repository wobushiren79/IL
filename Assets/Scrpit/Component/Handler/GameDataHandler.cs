using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameDataHandler : BaseHandler, DialogView.IDialogCallBack,IBaseObserver
{

    public enum NotifyTypeEnum
    {
        AddMoney = 1,
        PayMoney = 2,
        MenuResearchChange = 3,
        BedResearchChange = 4,
    }

    protected GameDataManager gameDataManager;
    protected GameTimeHandler gameTimeHandler;
    protected GameItemsManager gameItemsManager;
    protected ToastManager toastManager;
    protected InnFoodManager innFoodManager;
    protected AudioHandler audioHandler;
    protected DialogManager dialogManager;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        
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
            HandleForResearch();
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
    /// 菜单研究处理
    /// </summary>
    public void HandleForResearch()
    {
        if (gameTimeHandler == null)
            return;
        if (gameTimeHandler.isStopTime)
            return;
        AddResearch(1);
    }

    /// <summary>
    /// 增加所有研究点数
    /// </summary>
    /// <param name="time"></param>
    public void AddResearch(int time)
    {
        AddMenuResearch(time);
        AddBedResearch(time);
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
        if (listBed == null)
            return;
        foreach (BuildBedBean itemBed in listBed)
        {
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
        if (listMenu == null)
            return;
        foreach (MenuOwnBean itemMenu in listMenu)
        {
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
        if(observable == gameTimeHandler)
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
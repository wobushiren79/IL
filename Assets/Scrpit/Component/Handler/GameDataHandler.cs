using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameDataHandler : BaseHandler
{

    public enum NotifyTypeEnum
    {
        AddMoney = 1,
        PayMoney = 2,
    }

    protected GameDataManager gameDataManager;
    protected GameTimeHandler gameTimeHandler;
    protected GameItemsManager gameItemsManager;
    protected ToastManager toastManager;
    protected InnFoodManager innFoodManager;
    protected AudioHandler audioHandler;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

    private void Start()
    {
        StartCoroutine(CoroutineForPlayTime());
    }

    /// <summary>
    /// 协程-游玩时间记录
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForPlayTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            HandleForPlayTime();
            HandleForMenuResearch();
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
    public void HandleForMenuResearch()
    {
        if (gameTimeHandler == null || gameItemsManager == null || gameDataManager == null)
            return;
        if (gameTimeHandler.isStopTime)
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
            bool isCompleteResearch = itemMenu.AddResearchExp((int)addExp);
            //完成研究
            if (isCompleteResearch)
            {
                itemMenu.CompleteResearch(gameDataManager.gameData);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1071), menuInfo.name);
                audioHandler.PlaySound(AudioSoundEnum.Reward);
                toastManager.ToastHint(innFoodManager.GetFoodSpriteByName(menuInfo.icon_key), toastStr,5);
            }
        }
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
}
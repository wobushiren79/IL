using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using Steamworks;

public class UITownGuildRank : UIBaseOne, IRadioGroupCallBack
    , ISteamLeaderboardFindCallBack, ISteamLeaderboardUpdateCallBack, ISteamLeaderboardEntriesCallBack
{
    public Button btUpdate;
    public Button btRefresh;
    public RadioGroupView rgRankType;

    public GameObject objRankContainer;
    public GameObject objRankModel;

    public ItemTownGuildRankCpt itemRankForLocal;
    public ItemTownGuildRankCpt itemRankForUser;

    public ulong rankTypeId;

    public RankTypeEnum rankType = RankTypeEnum.GetMoneyS;

    protected SteamHandler steamHandler;
    protected ToastManager toastManager;

    public override void Awake()
    {
        base.Awake();
        steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        if (rgRankType != null) rgRankType.SetCallBack(this);
    }

    public override void Start()
    {
        base.Start();
        if (btUpdate != null) btUpdate.onClick.AddListener(OnClickForUpdate);
        if (btRefresh != null) btRefresh.onClick.AddListener(OnClickForRefresh);
    }


    public override void OpenUI()
    {
        base.OpenUI();
        rgRankType.SetPosition(0, true);
    }

    public void SetLocalData()
    {
        int score = 0;
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                score = (int)userAchievement.ownMoneyS;
                break;
            case RankTypeEnum.NumberOrder:
                score = (int)userAchievement.GetNumberForAllCustomer();
                break;
            case RankTypeEnum.NumberPraiseAnger:
                score = (int)userAchievement.GetPraiseNumber(PraiseTypeEnum.Anger);
                break;
            case RankTypeEnum.NumberPraiseExcited:
                score = (int)userAchievement.GetPraiseNumber(PraiseTypeEnum.Excited);
                break;
            case RankTypeEnum.TimePlay:
                score = gameData.playTime.GetTimeForTotalS();
                break;
        }
        string innName = gameData.GetInnAttributesData().innName;
        string playerName = gameData.userCharacter.baseInfo.name;
        SteamLeaderboardEntryBean steamLeaderboardEntry = new SteamLeaderboardEntryBean();
        steamLeaderboardEntry.rank = 0;
        steamLeaderboardEntry.score = score;
        steamLeaderboardEntry.steamID = SteamUser.GetSteamID();
        steamLeaderboardEntry.details = TypeConversionUtil.StringToInt32(innName + "-" + playerName);
        itemRankForLocal.SetData(rankType, steamLeaderboardEntry);
    }

    /// <summary>
    /// 按钮-更新数据
    /// </summary>
    public void OnClickForUpdate()
    {
        if (rankTypeId == 0)
            return;
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        int score = 0;
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                score = (int)userAchievement.ownMoneyS;
                break;
            case RankTypeEnum.NumberOrder:
                score = (int)userAchievement.GetNumberForAllCustomer();
                break;
            case RankTypeEnum.NumberPraiseAnger:
                score = (int)userAchievement.GetPraiseNumber(PraiseTypeEnum.Anger);
                break;
            case RankTypeEnum.NumberPraiseExcited:
                score = (int)userAchievement.GetPraiseNumber(PraiseTypeEnum.Excited);
                break;
            case RankTypeEnum.TimePlay:
                score = gameData.playTime.GetTimeForTotalS();
                break;
        }
        string innName = gameData.GetInnAttributesData().innName;
        string playerName = gameData.userCharacter.baseInfo.name;
        steamHandler.SetGetLeaderboardData(rankTypeId, score, innName + "-" + playerName , this);
    }

    /// <summary>
    /// 按钮-刷新数据
    /// </summary>
    public void OnClickForRefresh()
    {
        if (rankTypeId == 0)
            return;
        CptUtil.RemoveChildsByActive(objRankContainer);
        steamHandler.GetLeaderboardDataForUser(rankTypeId, this);
        steamHandler.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }

    /// <summary>
    /// 创建排名Item
    /// </summary>
    /// <param name="itemData"></param>
    protected void CreateRankItem(SteamLeaderboardEntryBean itemData)
    {
        GameObject objItem = Instantiate(objRankContainer, objRankModel);
        ItemTownGuildRankCpt rankItem = objItem.GetComponent<ItemTownGuildRankCpt>();
        rankItem.SetData(rankType, itemData);
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearData()
    {
        CptUtil.RemoveChildsByActive(objRankContainer);
        rankTypeId = 0;
        itemRankForUser.gameObject.SetActive(false);
    }

    #region 排行榜类型回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        ClearData();
        rankType = EnumUtil.GetEnum<RankTypeEnum>(rbview.name);
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        //获取排行榜ID
        steamHandler.GetLeaderboardId(rankName, this);
        //设置本地数据
        SetLocalData();
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion


    #region 获取排行榜ID回调
    public void FindLeaderboardSuccess(ulong leaderboard)
    {
        this.rankTypeId = leaderboard;
        steamHandler.GetLeaderboardDataForUser(rankTypeId, this);
        steamHandler.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }

    public void FindLeaderboardFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        toastManager.ToastHint(GameCommonInfo.GetUITextById(7004));
    }
    #endregion


    #region 排行榜更新回调
    public void UpdateLeaderboardSucess()
    {
        toastManager.ToastHint(GameCommonInfo.GetUITextById(7001));
        OnClickForRefresh();
    }

    public void UpdateLeaderboardFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        toastManager.ToastHint(GameCommonInfo.GetUITextById(7002));
    }
    #endregion


    #region 排行榜数据获取回调
    public void GetEntriesSuccess(ulong leaderboardID,List<SteamLeaderboardEntryBean> listData)
    {
        if (leaderboardID != rankTypeId)
            return;
        CptUtil.RemoveChildsByActive(objRankContainer);
        foreach (SteamLeaderboardEntryBean itemData in listData)
        {
            CreateRankItem(itemData);
        }
    }

    public void GetEntriesForUserListSuccess(ulong leaderboardID, List<SteamLeaderboardEntryBean> listData)
    {
        if (leaderboardID != rankTypeId)
            return;
        if (!CheckUtil.ListIsNull(listData))
        {
            itemRankForUser.gameObject.SetActive(true);
            itemRankForUser.SetData(rankType, listData[0]);
        }
        else
        {
            itemRankForUser.gameObject.SetActive(false);
        }
    }

    public void GetEntriesFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        toastManager.ToastHint(GameCommonInfo.GetUITextById(7004));
    }

    public void GetEntriesForUserListFail( SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        toastManager.ToastHint(GameCommonInfo.GetUITextById(7004));
    }
    #endregion
}
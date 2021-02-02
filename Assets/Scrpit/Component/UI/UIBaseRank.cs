using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class UIBaseRank : UIBaseOne, IRadioGroupCallBack
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


    public override void Awake()
    {
        base.Awake();
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

    public virtual void OnClickForUpdate()
    {
        if (rankTypeId == 0)
            return;
    }

    public virtual void OnClickForRefresh()
    {
        if (rankTypeId == 0)
            return;
        CptUtil.RemoveChildsByActive(objRankContainer);
    }

    public virtual void SetLocalData()
    {

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
    /// 创建本地Item
    /// </summary>
    /// <param name="intScore"></param>
    public void CreateLocalItem(long score)
    {
        int intScore = 0;
        if (score > int.MaxValue)
        {
            intScore = int.MaxValue;
        }
        else
        {
            intScore = (int)score;
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        string innName = gameData.GetInnAttributesData().innName;
        string playerName = gameData.userCharacter.baseInfo.name;
        SteamLeaderboardEntryBean steamLeaderboardEntry = new SteamLeaderboardEntryBean();
        steamLeaderboardEntry.rank = 0;
        steamLeaderboardEntry.score = intScore;
        try
        {
            //如果steam没有初始化
            steamLeaderboardEntry.steamID = SteamUser.GetSteamID();
        }
        catch
        {

        }

        steamLeaderboardEntry.details = TypeConversionUtil.StringToInt32(innName + "-" + playerName);
        itemRankForLocal.SetData(rankType, steamLeaderboardEntry);
    }

    #region 排行榜类型回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        ClearData();
        rankType = EnumUtil.GetEnum<RankTypeEnum>(rbview.name);
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        //设置本地数据
        SetLocalData();
        //获取排行榜ID
        SteamHandler.Instance.GetLeaderboardId(rankName, this);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion


    #region 获取排行榜ID回调
    public void FindLeaderboardSuccess(ulong leaderboard)
    {
        this.rankTypeId = leaderboard;
        SteamHandler.Instance.GetLeaderboardDataForUser(rankTypeId, this);
        SteamHandler.Instance.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }

    public void FindLeaderboardFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7004));
    }
    #endregion


    #region 排行榜更新回调
    public void UpdateLeaderboardSucess()
    {
        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7001));
        OnClickForRefresh();
    }

    public void UpdateLeaderboardFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7002));
    }
    #endregion


    #region 排行榜数据获取回调
    public void GetEntriesSuccess(ulong leaderboardID, List<SteamLeaderboardEntryBean> listData)
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
        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7004));
    }

    public void GetEntriesForUserListFail(SteamLeaderboardImpl.SteamLeaderboardFailEnum msg)
    {
        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7004));
    }
    #endregion
}
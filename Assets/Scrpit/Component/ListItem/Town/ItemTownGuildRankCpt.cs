using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Steamworks;

public class ItemTownGuildRankCpt : ItemGameBaseCpt, IWebRequestCallBack<SteamWebPlaySummariesBean>, IWebRequestForSpriteCallBack
{
    public Image ivIcon;
    public Text tvUserName;
    public Text tvName;
    public Text tvData;
    public Image ivData;
    public Text tvRank;

    public SteamLeaderboardEntryBean rankData;
    public RankTypeEnum rankType;

    protected SteamHandler steamHandler;

    public Sprite spGetMoneyS;
    public Sprite spNumberOrder;
    public Sprite spNumberPraiseExcited;
    public Sprite spNumberPraiseAnger;
    public Sprite spTimePlay;
    public Sprite spMaxDayGetMoneyS;
    public Sprite spMaxDayCompleteOrder;

    protected IconDataManager iconDataManager;
    public void Awake()
    {
        steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

    public void SetData(RankTypeEnum rankType, SteamLeaderboardEntryBean rankData)
    {
        this.rankType = rankType;
        this.rankData = rankData;

        SetScore(rankType, rankData.score);
        SetRank(rankData.rank);
        string name = TypeConversionUtil.Int32ToString(rankData.details);
        SetName(name);

        //开始获取头像和名字
        if (rankData.steamID.m_SteamID != 0)
        {
            StartCoroutine(steamHandler.GetUserInfo(rankData.steamID.m_SteamID + "", this));
        }
    }

    public void SetScore(RankTypeEnum rankType, int score)
    {
        Sprite spData = null;
        string dataStr = "";
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                dataStr = score + GameCommonInfo.GetUITextById(18);
                spData = iconDataManager.GetIconSpriteByName("ach_money_s_2");
                break;
            case RankTypeEnum.NumberOrderForFood:
                dataStr = score + GameCommonInfo.GetUITextById(82);
                spData = iconDataManager.GetIconSpriteByName("ach_ordernumber_1");
                break;
            case RankTypeEnum.NumberOrderForHotel:
                dataStr = score + GameCommonInfo.GetUITextById(82);
                spData = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
                break;

            case RankTypeEnum.NumberPraiseExcited:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("ach_accost_1");
                break;
            case RankTypeEnum.NumberPraiseAnger:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("ach_accost_2");
                break;
            case RankTypeEnum.TimePlay:
                TimeBean timeData = new TimeBean();
                timeData.AddTimeForHMS(0, 0, score);
                dataStr = timeData.hour + ":" + timeData.minute + ":" + timeData.second;
                spData = iconDataManager.GetIconSpriteByName("time_wait_1_0");
                break;

            case RankTypeEnum.MaxDayGetMoneyForFoodS:
                dataStr = score + GameCommonInfo.GetUITextById(18);
                spData = iconDataManager.GetIconSpriteByName("ach_money_s_2");
                break;
            case RankTypeEnum.MaxDayGetMoneyForHotelS:
                dataStr = score + GameCommonInfo.GetUITextById(18);
                spData = iconDataManager.GetIconSpriteByName("ach_money_s_2");
                break;

            case RankTypeEnum.MaxDayCompleteOrderForFood:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("ach_ordernumber_1");
                break;
            case RankTypeEnum.MaxDayCompleteOrderForHotel:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
                break;

            case RankTypeEnum.NumberForGetElementary:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("trophy_1_0");
                break;
            case RankTypeEnum.NumberForGetIntermediate:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("trophy_1_1");
                break;
            case RankTypeEnum.NumberForGetAdvanced:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("trophy_1_2");
                break;
            case RankTypeEnum.NumberForGetLegendary:
                dataStr = score + "";
                spData = iconDataManager.GetIconSpriteByName("trophy_1_3");
                break;
        }
        if (tvData != null)
        {
            tvData.text = dataStr;
        }
        if (ivData != null)
        {
            ivData.sprite = spData;
        }
    }

    public void SetRank(int rank)
    {
        if (tvRank != null)
        {
            if (rank == 0)
            {

            }
            else
            {
                tvRank.text = rank + "";
            }

        }
    }

    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    public void SetUserName(string userName)
    {
        if (tvUserName != null)
        {
            tvUserName.text = userName;
        }
    }

    public void SetIcon(Sprite iconSprite)
    {

        if (ivIcon != null)
        {
            ivIcon.sprite = iconSprite;
        }
    }

    #region 网络回调
    public void WebRequestGetSuccess(string url, SteamWebPlaySummariesBean data)
    {
        if (data == null || data.response == null || CheckUtil.ListIsNull(data.response.players))
            return;
        SetUserName(data.response.players[0].personaname);
        //获取头像
        WebRequest webRequest = new WebRequest();
        StartCoroutine(webRequest.GetSprice(data.response.players[0].avatar, this));
    }

    public void WebRequestGetFail(string url, string fail)
    {

    }

    public void WebRequestForSpriteSuccess(string url, Sprite sprite)
    {
        SetIcon(sprite);
    }

    public void WebRequestForSpriteFail(string url, string fail)
    {

    }
    #endregion
}
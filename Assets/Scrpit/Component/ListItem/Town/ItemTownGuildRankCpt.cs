using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Steamworks;

public class ItemTownGuildRankCpt : ItemGameBaseCpt,IWebRequestCallBack<SteamWebPlaySummariesBean>,IWebRequestForSpriteCallBack
{
    public Image ivIcon;
    public Text tvUserName;
    public Text tvName;
    public Text tvData;
    public Text tvRank;

    public SteamLeaderboardEntryBean rankData;
    public RankTypeEnum rankType;

    protected SteamHandler steamHandler;

    public void Awake()
    {
        steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
    }

    public void SetData(RankTypeEnum rankType, SteamLeaderboardEntryBean rankData)
    {
        this.rankType = rankType;
        this.rankData = rankData;

        SetScore(rankData.score);
        SetRank(rankData.rank);
        string name = TypeConversionUtil.Int32ToString(rankData.details);
        SetName(name);

        //开始获取头像和名字
        StartCoroutine(steamHandler.GetUserInfo(rankData.steamID.m_SteamID + "", this));
    }

    public void SetScore(int score)
    {
        if (tvData!=null)
        {
            tvData.text = score + "";
        }
    }

    public void SetRank(int rank)
    {
        if (tvRank != null)
        {
            tvRank.text = rank + "";
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
        if (tvUserName!=null)
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
        if (data == null|| data.response == null||CheckUtil.ListIsNull(data.response.players))
            return;
        SetUserName(data.response.players[0].personaname);
        //获取头像
        WebRequest webRequest = new WebRequest();
        StartCoroutine(webRequest.GetSprice(data.response.players[0].avatar,this));
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
        LogUtil.Log("获取头像失败");
    }
    #endregion
}
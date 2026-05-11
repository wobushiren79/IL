using UnityEngine;
using UnityEditor;

public class UITownArenaRank : UIBaseRank
{
    public override void SetLocalData()
    {
        base.SetLocalData();
        long score = 0;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.NumberForGetElementary:
                score = userAchievement.ownTrophyElementary;
                break;
            case RankTypeEnum.NumberForGetIntermediate:
                score = userAchievement.ownTrophyIntermediate;
                break;
            case RankTypeEnum.NumberForGetAdvanced:
                score = userAchievement.ownTrophyAdvanced;
                break;
            case RankTypeEnum.NumberForGetLegendary:
                score = userAchievement.ownTrophyLegendary;
                break;

        }
        CreateLocalItem(score);
    }

    /// <summary>
    /// 按钮-更新数据
    /// </summary>
    public override void OnClickForUpdate()
    {
        base.OnClickForUpdate();
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        long score = 0;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.NumberForGetElementary:
                score = userAchievement.ownTrophyElementary;
                break;
            case RankTypeEnum.NumberForGetIntermediate:
                score = userAchievement.ownTrophyIntermediate;
                break;
            case RankTypeEnum.NumberForGetAdvanced:
                score = userAchievement.ownTrophyAdvanced;
                break;
            case RankTypeEnum.NumberForGetLegendary:
                score = userAchievement.ownTrophyLegendary;
                break;
        }
        int intScore = 0;
        if (score > int.MaxValue)
        {
            intScore = int.MaxValue;
        }
        else
        {
            intScore = (int)score;
        }
        string innName = gameData.GetInnAttributesData().innName;
        string playerName = gameData.userCharacter.baseInfo.name;
        SteamHandler.Instance.SetGetLeaderboardData(rankTypeId, intScore, innName + "-" + playerName, this);
    }

    /// <summary>
    /// 按钮-刷新数据
    /// </summary>
    public override void OnClickForRefresh()
    {
        base.OnClickForRefresh();

        SteamHandler.Instance.GetLeaderboardDataForUser(rankTypeId, this);
        SteamHandler.Instance.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }
}
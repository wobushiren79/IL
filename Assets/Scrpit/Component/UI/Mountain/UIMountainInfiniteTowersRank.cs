using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMountainInfiniteTowersRank : UIBaseRank
{
    public override void SetLocalData()
    {
        base.SetLocalData();
        long score = GetScoreByType(rankType);
        CreateLocalItem(score);
    }

    /// <summary>
    /// 按钮-更新数据
    /// </summary>
    public override void OnClickForUpdate()
    {
        base.OnClickForUpdate();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        string rankName = RankTypeEnumTool.GetRankTypeName(rankType);
        long score = GetScoreByType(rankType);
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
        steamHandler.SetGetLeaderboardData(rankTypeId, intScore, innName + "-" + playerName, this);
    }

    public long GetScoreByType(RankTypeEnum rankType)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        long score = 0;
        switch (rankType)
        {
            case RankTypeEnum.MaxLayer:
                score = userAchievement.maxInfiniteTowersLayer;
                break;
        }
        return score;
    }

    /// <summary>
    /// 按钮-刷新数据
    /// </summary>
    public override void OnClickForRefresh()
    {
        base.OnClickForRefresh();

        steamHandler.GetLeaderboardDataForUser(rankTypeId, this);
        steamHandler.GetLeaderboardDataForGlobal(rankTypeId, 1, 30, this);
    }

}

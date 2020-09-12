using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using Steamworks;

public class UITownGuildRank : UIBaseRank
{
    public override void SetLocalData()
    {
        base.SetLocalData();
        long score = 0;
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                score = userAchievement.ownMoneyS;
                break;
            case RankTypeEnum.NumberOrder:
                score = userAchievement.GetNumberForAllCustomerFood();
                break;
            case RankTypeEnum.NumberPraiseAnger:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Anger);
                break;
            case RankTypeEnum.NumberPraiseExcited:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Excited);
                break;
            case RankTypeEnum.TimePlay:
                score = gameData.playTime.GetTimeForTotalS();
                break;
            case RankTypeEnum.MaxDayGetMoneyS:
                score = userAchievement.maxDayGetMoneyS;
                break;
            case RankTypeEnum.MaxDayCompleteOrder:
                score = userAchievement.maxDayCompleteOrder;
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
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                score = userAchievement.ownMoneyS;
                break;
            case RankTypeEnum.NumberOrder:
                score = userAchievement.GetNumberForAllCustomerFood();
                break;
            case RankTypeEnum.NumberPraiseAnger:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Anger);
                break;
            case RankTypeEnum.NumberPraiseExcited:
                score = userAchievement.GetPraiseNumber(PraiseTypeEnum.Excited);
                break;
            case RankTypeEnum.TimePlay:
                score = gameData.playTime.GetTimeForTotalS();
                break;
            case RankTypeEnum.MaxDayGetMoneyS:
                score = userAchievement.maxDayGetMoneyS;
                break;
            case RankTypeEnum.MaxDayCompleteOrder:
                score = userAchievement.maxDayCompleteOrder;
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
        steamHandler.SetGetLeaderboardData(rankTypeId, intScore, innName + "-" + playerName , this);
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
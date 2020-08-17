using UnityEngine;
using UnityEditor;

public enum RankTypeEnum
{
    GetMoneyS = 1,//获取金钱
    NumberOrder=11,//接客数量
    NumberPraiseExcited = 21,//心情
    NumberPraiseHappy = 22,
    NumberPraiseOkay = 23,
    NumberPraiseOrdinary = 24,
    NumberPraiseDisappointed = 25,
    NumberPraiseAnger = 26,
    TimePlay=31,//游玩时间
    MaxDayGetMoneyL=41,//单日最高获取金额
    MaxDayGetMoneyM=42,
    MaxDayGetMoneyS=43,
    MaxDayCompleteOrder = 51,//单日最高完成订单
}

public class RankTypeEnumTool
{
    public static string GetRankTypeName(RankTypeEnum rankType)
    {
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                return "Get_Money_S";
            case RankTypeEnum.NumberOrder:
                return "Number_Order";
            case RankTypeEnum.NumberPraiseExcited:
                return "Number_Praise_Excited";
            case RankTypeEnum.NumberPraiseAnger:
                return "Number_Praise_Anger";
            case RankTypeEnum.TimePlay:
                return "Time_Play";
            case RankTypeEnum.MaxDayGetMoneyL:
                return "MaxDay_GetMoneyL";
            case RankTypeEnum.MaxDayGetMoneyM:
                return "MaxDay_GetMoneyM";
            case RankTypeEnum.MaxDayGetMoneyS:
                return "MaxDay_GetMoneyS";
            case RankTypeEnum.MaxDayCompleteOrder:
                return "MaxDay_CompleteOrder";
        }
        return "";
    }
}
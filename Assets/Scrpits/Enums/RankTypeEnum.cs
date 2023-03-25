using UnityEngine;
using UnityEditor;

public enum RankTypeEnum
{
    GetMoneyS = 1,//获取金钱
    NumberOrderForFood=11,//接客数量
    NumberOrderForHotel = 12,
    NumberPraiseExcited = 21,//心情
    NumberPraiseHappy = 22,
    NumberPraiseOkay = 23,
    NumberPraiseOrdinary = 24,
    NumberPraiseDisappointed = 25,
    NumberPraiseAnger = 26,
    TimePlay=31,//游玩时间
    MaxDayGetMoneyForFoodL=41,//单日最高获取金额
    MaxDayGetMoneyForFoodM = 42,
    MaxDayGetMoneyForFoodS = 43,
    MaxDayGetMoneyForHotelL = 44,//单日最高获取金额
    MaxDayGetMoneyForHotelM = 45,
    MaxDayGetMoneyForHotelS = 46,
    MaxDayCompleteOrderForFood = 51,//单日最高完成订单
    MaxDayCompleteOrderForHotel= 52,//单日最高完成订单
    NumberForGetElementary = 61,//初级
    NumberForGetIntermediate = 62,//中级
    NumberForGetAdvanced = 63,//高级
    NumberForGetLegendary = 64,//传奇

    MaxLayer = 101,//最高层数
}

public class RankTypeEnumTool
{
    public static string GetRankTypeName(RankTypeEnum rankType)
    {
        switch (rankType)
        {
            case RankTypeEnum.GetMoneyS:
                return "Get_Money_S";
            case RankTypeEnum.NumberOrderForFood:
                return "Number_Order";
            case RankTypeEnum.NumberOrderForHotel:
                return "Number_Order_Hotel";
            case RankTypeEnum.NumberPraiseExcited:
                return "Number_Praise_Excited";
            case RankTypeEnum.NumberPraiseAnger:
                return "Number_Praise_Anger";
            case RankTypeEnum.TimePlay:
                return "Time_Play";

            case RankTypeEnum.MaxDayGetMoneyForFoodL:
                return "MaxDay_GetMoneyL";
            case RankTypeEnum.MaxDayGetMoneyForFoodM:
                return "MaxDay_GetMoneyM";
            case RankTypeEnum.MaxDayGetMoneyForFoodS:
                return "MaxDay_GetMoneyS";
            case RankTypeEnum.MaxDayGetMoneyForHotelL:
                return "MaxDay_GetMoneyL_Hotel";
            case RankTypeEnum.MaxDayGetMoneyForHotelM:
                return "MaxDay_GetMoneyM_Hotel";
            case RankTypeEnum.MaxDayGetMoneyForHotelS:
                return "MaxDay_GetMoneyS_Hotel";

            case RankTypeEnum.MaxDayCompleteOrderForFood:
                return "MaxDay_CompleteOrder";
            case RankTypeEnum.MaxDayCompleteOrderForHotel:
                return "MaxDay_CompleteOrder_Hotel";

            case RankTypeEnum.NumberForGetElementary:
                return "Number_GetElementary";
            case RankTypeEnum.NumberForGetIntermediate:
                return "Number_GetIntermediate";
            case RankTypeEnum.NumberForGetAdvanced:
                return "Number_GetAdvanced";
            case RankTypeEnum.NumberForGetLegendary:
                return "Number_GetLegendary";

            case RankTypeEnum.MaxLayer:
                return "Max_Layer";
        }
        return "";
    }
}
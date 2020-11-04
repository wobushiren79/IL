using UnityEngine;
using UnityEditor;

public enum NpcTalkTypeEnum
{
    Talk = 1,    //对话
    OneTalk = 2,    //一次性对话
    Recruit = 3, //招募
    Gift = 4, //送礼
    GuildCoinExchange = 101,//公会勋章交换
    TrophyExchange = 102,//奖杯交换

    InfiniteTowerEquipExchange = 201,//无尽之塔装备交换
    InfiniteTowerItemsExchange = 202,//无尽之塔物品交换
}
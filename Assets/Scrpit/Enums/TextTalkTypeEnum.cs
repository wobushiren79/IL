using UnityEngine;
using UnityEditor;

public enum TextTalkTypeEnum
{

    Normal = 0,//普通对话
    Gift = 1,//送礼对话
    Recruit = 2,//招募对话
    Subsequent = 3,//后续 用于时间结束后的对话
    First = 4,//第一次对话
    Rascal = 5,// 捣乱对话
    Sundry = 6,//杂项对话
    Shout = 7,//喊话
    Exchange = 8,//交换
    None = 99,//无
}
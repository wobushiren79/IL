using UnityEngine;
using UnityEditor;

public enum AudioSoundEnum
{
    ButtonForNormal = 1,//普通的按键音效
    ButtonForBack = 2,//退出音效
    ButtonForHighLight = 3,//高亮音效

    PayMoney = 10,//支付音效
    Reward = 20,//奖励音效
    HitWall = 30,//撞墙
    HitCoin = 40,//撞金
    Shot = 50,//发射

    Thunderstorm = 101,//打雷音效

    Damage = 201,//伤害声

    CountDownStart = 211,//倒计时开始
    CountDownEnd = 221,//倒计时结束
    GetCard = 301,//抽卡
    SetCard = 311,//设置点击卡片
    CardDraw = 321,//设置点击卡片
    CardWin = 331,//设置点击卡片
    CardLose = 341,//设置点击卡片
}
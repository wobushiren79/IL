using UnityEngine;
using UnityEditor;

public enum AudioSoundEnum
{
    ButtonForNormal = 1,//普通的按键音效
    ButtonForBack = 2,//退出音效
    ButtonForHighLight = 3,//高亮音效
    ButtonForShow=4,//出现音效
    PayMoney = 10,//支付音效
    Reward = 20,//奖励音效
    HitWall = 30,//撞墙
    HitCoin = 40,//撞金
    Shot = 50,//发射
    ChangeSelect=60,//改变选择
    Show = 70,//登场展示
    Correct=80,//正确
    Error=90,//错误
    Set=100,//设置
    Thunderstorm = 101,//打雷音效
    Cook=110,//做菜声
    Eat=120,//吃东西
    Lock = 130,//锁
    Clean=140,//清理
    Damage = 201,//伤害声

    CountDownStart = 211,//倒计时开始
    CountDownEnd = 221,//倒计时结束
    GetCard = 301,//抽卡
    SetCard = 311,//设置点击卡片
    CardDraw = 321,//设置点击卡片
    CardWin = 331,//设置点击卡片
    CardLose = 341,//设置点击卡片
    Fight = 401,//打击
    FightForKnife=411,//刀

    UseMedicine =501,//使用药品
    Passive=601,//消极失败
    Dice=701,//骰子声音
    Thunder=801,//雷电声音
}
using UnityEngine;
using UnityEditor;

public enum AudioSoundEnum
{
    None = 1,
    ButtonForNormal = 100001,//普通的按键音效
    ButtonForBack = 100002,//退出音效
    ButtonForHighLight = 100003,//高亮音效
    ButtonForShow = 100004,//出现音效
    PayMoney = 100005,//支付音效
    Reward = 100006,//奖励音效
    Thunderstorm = 100007,//打雷音效
    Damage = 100008,//伤害声
    CountDownStart = 100009,//倒计时开始
    CountDownEnd = 100010,//倒计时结束
    HitWall = 100011,//撞墙
    HitCoin = 100012,//撞金
    Shot = 100014,//发射
    ChangeSelect = 100013,//改变选择
    GetCard = 100015,//抽卡
    SetCard = 100016,//设置点击卡片
    CardDraw = 100017,//设置点击卡片
    CardWin = 100018,//设置点击卡片
    CardLose = 100019,//设置点击卡片
    Fight = 100020,//打击
    FightForKnife = 100022,//刀
    UseMedicine = 100023,//使用药品
    Show = 100024,//登场展示
    Correct = 100025,//正确
    Error = 100026,//错误
    Set = 100027,//设置
    Cook = 100028,//做菜声
    Eat = 100030,//吃东西
    Lock = 100032,//锁
    Clean = 100029,//清理
    Passive = 100033,//消极失败
    Dice = 100034,//骰子声音
    Thunder = 100035,//雷电声音
    Door = 100036,//开门
    Firecrackers= 100037,//鞭炮
}
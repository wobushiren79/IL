using UnityEngine;
using UnityEditor;

public partial class GameConfigBean 
{

    //按键提示状态 1显示 0隐藏
    public int statusForKeyTip = 1;

    //鼠标镜头移动 1开启 0关闭
    public int statusForMouseMove = 1;

    //事件镜头移动 1开启 0关闭
    public int statusForEventCameraMove = 1;
    //事件停止加速
    public int statusForEventStopTimeScale = 1;

    //随机事件开关
    public int statusForEvent = 1;

    //顾客结账方式  0最近  1随机
    public int statusForCheckOut = 0;

    //员工当前工作状态数量
    public int statusForWorkerNumber = 0;

    //是否开启力度测试
    public int statusForCombatForPowerTest = 1;
    
    //是否固定战斗视角
    public int statusForFightCamera = 0;

    //是否展示详细的潘龙信息
    public bool isShowDetailsForTower = true;
}
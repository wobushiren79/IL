using UnityEngine;
using UnityEditor;

public enum GambleStatusType 
{
    Prepare,//准备中
    Changing,//游戏改变中
    Gambling,//游戏进行中
    Settlementing,//结算中
    End,//结束
}
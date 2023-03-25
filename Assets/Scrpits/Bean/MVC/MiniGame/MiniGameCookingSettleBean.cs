using UnityEngine;
using UnityEditor;

public class MiniGameCookingSettleBean
{

    public float maxTime;//游戏时间
    public float residueTime;//剩余时间
    public int correctNumber;//完成数
    public int errorNumber;//错误数
    public int unfinishNumber;//未完成数

    public int GetScore()
    {
        float scoreRate = (float)correctNumber / (float)(correctNumber + errorNumber + unfinishNumber);
        return (int)(scoreRate * 100);
    }
}
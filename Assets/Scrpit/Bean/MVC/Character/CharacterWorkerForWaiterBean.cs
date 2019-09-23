using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForWaiterBean : CharacterWorkerBaseBean
{
    //清理次数
    public long cleanTotalNumber;
    //送餐次数
    public long sendTotalNumber;

    //总计清理时间
    public float cleanTotalTime;
    /// <summary>
    /// 增加清理次数
    /// </summary>
    /// <param name="number"></param>
    public void AddCleanNumber(int number)
    {
        cleanTotalNumber += number;
    }

    /// <summary>
    /// 增加送餐次数
    /// </summary>
    /// <param name="number"></param>
    public void AddSendNumber(int number)
    {
        sendTotalNumber += number;
    }

    /// <summary>
    /// 增加清理时间
    /// </summary>
    /// <param name="time"></param>
    public void AddCleanTime(float time)
    {
        cleanTotalTime += time;
    }
}
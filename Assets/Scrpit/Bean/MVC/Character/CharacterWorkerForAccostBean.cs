using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForAccostBean : CharacterWorkerBaseBean
{
    //总计吆喝次数
    public long accostTotalNumber;
    //吆喝成功次数
    public long accostSuccessNumber;
    //吆喝失败次数
    public long accostFailNumber;
    //总计招揽时间
    public float accostTotalTime;

    public CharacterWorkerForAccostBean()
    {
        workerType = WorkerEnum.Accost;
    }

    /// <summary>
    /// 添加吆喝成功次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostSuccessNumber(int number)
    {
        accostSuccessNumber += number;
        accostTotalNumber += number;
    }

    /// <summary>
    /// 添加吆喝失败次数
    /// </summary>
    /// <param name="number"></param>
    public void AddAccostFailNumber(int number)
    {
        accostFailNumber += number;
        accostTotalNumber += number;
    }

    /// <summary>
    /// 添加交流时间
    /// </summary>
    /// <param name="time"></param>
    public void AddAccostTime(float time)
    {
        accostTotalTime += time;
    }
}
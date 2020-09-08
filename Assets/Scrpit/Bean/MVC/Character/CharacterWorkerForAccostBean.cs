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


    //是否开启
    public bool isWorkingForSolicit = true;
    public int priorityForSolicit = 0;
    //是否开启
    public bool isWorkingForGuide = false;
    public int priorityForGuide = 0;

    public CharacterWorkerForAccostBean()
    {
        workerType = WorkerEnum.Accost;
    }
    public void SetPriorityForSolicit(int priority)
    {
        this.priorityForSolicit = priority;
    }
    public void SetPriorityForGuide(int priority)
    {
        this.priorityForGuide = priority;
    }


    public void SetWorkStatusForSolicit(bool isWorking)
    {
        this.isWorkingForSolicit = isWorking;
    }
    public void SetWorkStatusForGuide(bool isWorking)
    {
        this.isWorkingForGuide = isWorking;
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
}
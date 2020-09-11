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
    //清理床单总次数
    public long cleanBedTotalNumber;

    //是否开启
    public bool isWorkingForSend = true;
    public int priorityForSend = 0;
    //是否开启
    public bool isWorkingForCleanTable = true;
    public int priorityForCleanTable = 0;
    //是否开启
    public bool isWorkingCleanBed = false;
    public int priorityForCleanBed = 0;

    public CharacterWorkerForWaiterBean()
    {
        workerType = WorkerEnum.Waiter;
    }

    public void SetPriorityForSend(int priority)
    {
        this.priorityForSend = priority;
    }
    public void SetPriorityForCleanTable(int priority)
    {
        this.priorityForCleanTable = priority;
    }
    public void SetPriorityForCleanBed(int priority)
    {
        this.priorityForCleanBed = priority;
    }

    public void SetWorkStatusForSend(bool isWorking)
    {
        this.isWorkingForSend = isWorking;
    }
    public void SetWorkStatusForCleanTable(bool isWorking)
    {
        this.isWorkingForCleanTable = isWorking;
    }
    public void SetWorkStatusForCleanBed(bool isWorking)
    {
        this.isWorkingCleanBed = isWorking;
    }

    /// <summary>
    /// 增加清理次数
    /// </summary>
    /// <param name="number"></param>
    public void AddCleanTableNumber(int number)
    {
        cleanTotalNumber += number;
    }

    /// <summary>
    /// 增加理床次数
    /// </summary>
    /// <param name="number"></param>
    public void AddCleanBedNumber(int number)
    {
        cleanBedTotalNumber += number;
    }

    /// <summary>
    /// 增加送餐次数
    /// </summary>
    /// <param name="number"></param>
    public void AddSendNumber(int number)
    {
        sendTotalNumber += number;
    }

}
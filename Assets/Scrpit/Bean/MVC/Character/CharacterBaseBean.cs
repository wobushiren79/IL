using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CharacterBaseBean
{
    public string characterId;
    //类型
    public int characterType;
    //称号
    public string titleName;
    //名字
    public string name;
    //工作天数
    public long workDay;
    //出生
    //public TimeBean born;
    //每天工资    
    public long priceS;
    public long priceM;
    public long priceL;

    public CharacterWorkerForChefBean chefInfo = new CharacterWorkerForChefBean();
    public CharacterWorkerForWaiterBean waiterInfo = new CharacterWorkerForWaiterBean();
    public CharacterWorkerForAccountantBean accountantInfo = new CharacterWorkerForAccountantBean();
    public CharacterWorkerForAccostBean accostInfo = new CharacterWorkerForAccostBean();
    public CharacterWorkerForBeaterBean beaterInfo = new CharacterWorkerForBeaterBean();

    public bool isChef = true;//是否开启厨师
    public bool isWaiter = true;//是否开启服务生
    public bool isAccountant = true;//是否开启记帐
    public bool isAccost = false;//是否开启招呼
    public bool isBeater = false;//是否开启打手

    //优先级
    public int priorityChef;
    public int priorityWaiter;
    public int priorityAccountant;
    public int priorityAccost;
    public int priorityBeater;

    //工作状态-关联 WorkerStatus
    public int workerStatus = 0;

    public List<long> listLoveItems = new List<long>();//喜欢的物品

    /// <summary>
    /// 获取所有职业的工作数据
    /// </summary>
    /// <returns></returns>
    public List<WorkerInfo> GetAllWorkerInfo()
    {
        List<WorkerInfo> workerInfos = new List<WorkerInfo>();
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Chef));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Waiter));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Accountant));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Accost));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Beater));
        //按照优先度排序，数值越高越靠前
        workerInfos = workerInfos.OrderByDescending(i => i.priority).ToList();
        return workerInfos;
    }

    /// <summary>
    /// 根据类型获取工作数据
    /// </summary>
    /// <param name="worker"></param>
    /// <returns></returns>
    public WorkerInfo GetWorkerInfoByType(WorkerEnum worker)
    {
        bool isWork = true;
        int priority = 1;
        switch (worker)
        {
            case WorkerEnum.Chef:
                isWork = isChef;
                priority = priorityChef;
                break;
            case WorkerEnum.Waiter:
                isWork = isWaiter;
                priority = priorityWaiter;
                break;
            case WorkerEnum.Accountant:
                isWork = isAccountant;
                priority = priorityAccountant;
                break;
            case WorkerEnum.Accost:
                isWork = isAccost;
                priority = priorityAccost;
                break;
            case WorkerEnum.Beater:
                isWork = isBeater;
                priority = priorityBeater;
                break;
        }
        WorkerInfo workerInfo = new WorkerInfo(worker, priority, isWork);
        return workerInfo;
    }

    /// <summary>
    /// 获取员工工作状态
    /// </summary>
    /// <returns></returns>
    public WorkerStatusEnum GetWorkerStatus(out string workerStatusStr)
    {
        workerStatusStr = "";
        WorkerStatusEnum workerStatus = (WorkerStatusEnum)this.workerStatus;
        switch (workerStatus)
        {
            case WorkerStatusEnum.Rest:
                workerStatusStr= GameCommonInfo.GetUITextById(282);
                break;
            case WorkerStatusEnum.Work:
                workerStatusStr = GameCommonInfo.GetUITextById(281);
                break;
            case WorkerStatusEnum.Vacation:
                workerStatusStr = GameCommonInfo.GetUITextById(283);
                break;
        }
        return workerStatus;
    }
    public WorkerStatusEnum GetWorkerStatus()
    {
        WorkerStatusEnum workerStatus = (WorkerStatusEnum)this.workerStatus;
        return workerStatus;
    }

    /// <summary>
    /// 设置枚举
    /// </summary>
    /// <param name="workerStatus"></param>
    public void SetWorkerStatus(WorkerStatusEnum workerStatus)
    {
        this.workerStatus = (int)workerStatus;
    }


    /// <summary>
    /// 检测是否是喜欢的物品
    /// </summary>
    /// <param name="itemsId"></param>
    /// <returns></returns>
    public bool CheckIsLoveItems(long itemsId)
    {
        if (listLoveItems.Contains(itemsId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
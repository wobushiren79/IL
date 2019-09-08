using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CharacterBaseBean
{
    public string characterId;
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

    public bool isChef;//是否开启厨师
    public bool isWaiter;//是否开启服务生
    public bool isAccounting;//是否开启记帐
    public bool isBeater;//是否开启打手
    public bool isAccost;//是否开启招呼

    //优先级
    public int priorityChef;
    public int priorityWaiter;
    public int priorityAccounting;
    public int priorityBeater;
    public int priorityAccost;

    public bool isAttendance;//是否出勤

    /// <summary>
    /// 根据能力生成工资
    /// </summary>
    /// <param name="attributesBean"></param>
    public void CreatePriceByAttributes(CharacterAttributesBean attributesBean)
    {
        int totalAttribute = attributesBean.cook + attributesBean.speed + attributesBean.charm + attributesBean.force + attributesBean.lucky + attributesBean.account;
        priceS = 100;
        if (totalAttribute > 6)
        {
            priceS += (totalAttribute - 6) * 50;
        }
    }

    /// <summary>
    /// 获取所有职业的工作数据
    /// </summary>
    /// <returns></returns>
    public List<WorkerInfo> GetAllWorkerInfo()
    {
        List<WorkerInfo> workerInfos = new List<WorkerInfo>();
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Chef));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Waiter));
        workerInfos.Add(GetWorkerInfoByType(WorkerEnum.Accounting));
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
            case WorkerEnum.Accounting:
                isWork = isAccounting;
                priority = priorityAccounting;
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
}
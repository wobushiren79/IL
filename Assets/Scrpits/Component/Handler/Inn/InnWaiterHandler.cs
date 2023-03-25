using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnWaiterHandler : BaseMonoBehaviour
{

    protected GameObject _waiterContainer;
    public GameObject waiterContainer
    {
        get
        {
            if (_waiterContainer == null)
            {
                _waiterContainer = GameObject.FindGameObjectWithTag(TagInfo.Tag_NpcWorkerContainer);
            }
            return _waiterContainer;
        }
    }

    /// <summary>
    /// 设置运送食物
    /// </summary>
    /// <returns></returns>
    public bool SetSendFood(OrderForCustomer orderForCustomer, NpcAIWorkerCpt waiterCpt)
    {
        if (waiterCpt != null)
        {
            waiterCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.WaiterSend, orderForCustomer);
            return true;
        }
        return false;
    }


    /// <summary>
    /// 设置清理食物
    /// </summary>
    /// <returns></returns>
    public bool SetCleanFood(OrderForCustomer orderForCustomer, NpcAIWorkerCpt waiterCpt)
    {
        if (waiterCpt != null)
        {
            waiterCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.WaiterClean, orderForCustomer);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置清理床单
    /// </summary>
    /// <param name="orderForHotel"></param>
    /// <param name="waiterCpt"></param>
    public bool SetCleanBed(OrderForHotel orderForHotel, NpcAIWorkerCpt waiterCpt)
    {
        if (waiterCpt != null)
        {
            waiterCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.WaiterBed, orderForHotel);
            return true;
        }
        return false;
    }
}
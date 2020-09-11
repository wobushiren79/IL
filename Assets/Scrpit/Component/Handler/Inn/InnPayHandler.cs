using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class InnPayHandler : BaseMonoBehaviour
{
    // 柜台列表
    public List<BuildCounterCpt> listCounterCpt = new List<BuildCounterCpt>();
    // 算账人列表
    public List<NpcAIWorkerCpt> listAccountingCpt = new List<NpcAIWorkerCpt>();

    //柜台容器
    public GameObject counterContainer;
    //算账人容器
    public GameObject accountingContainer;
    //支付特效
    public GameObject objPayEffects;

    /// <summary>
    /// 找到所有柜台
    /// </summary>
    /// <returns></returns>
    public List<BuildCounterCpt> InitCounterList()
    {
        if (counterContainer == null)
            return listCounterCpt;
        BuildCounterCpt[] tableArray = counterContainer.GetComponentsInChildren<BuildCounterCpt>();
        listCounterCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listCounterCpt;
    }

    /// <summary>
    /// 获取最近的柜台
    /// </summary>
    /// <param name="npcAICustomer"></param>
    /// <returns></returns>
    public BuildCounterCpt GetCloseCounter(Vector3 position)
    {
        BuildCounterCpt closeCounter = null;
        float minDistance = 0;
        foreach (BuildCounterCpt itemCounter in listCounterCpt)
        {
            float distance = Vector3.Distance(position, itemCounter.transform.position);
            if (minDistance == 0)
            {
                minDistance = distance;
                closeCounter = itemCounter;
            }
            else if (distance < minDistance)
            {
                minDistance = distance;
                closeCounter = itemCounter;
            }
        }
        return closeCounter;
    }

    /// <summary>
    /// 找到所有算账人
    /// </summary>
    /// <returns></returns>
    public List<NpcAIWorkerCpt> InitAccountingCpt()
    {
        listAccountingCpt.Clear();
        if (accountingContainer == null)
            return listAccountingCpt;
        NpcAIWorkerCpt[] chefArray = accountingContainer.GetComponentsInChildren<NpcAIWorkerCpt>();
        if (chefArray == null)
            return listAccountingCpt;
        for (int i = 0; i < chefArray.Length; i++)
        {
            NpcAIWorkerCpt npcAI = chefArray[i];
            if (npcAI.characterData.baseInfo.accountantInfo.isWorking)
            {
                listAccountingCpt.Add(npcAI);
            }
        }
        return listAccountingCpt;
    }

    /// <summary>
    /// 设置算账数据
    /// </summary>
    public bool SetPay(OrderForBase order, NpcAIWorkerCpt workNpc)
    {
        if (workNpc != null)
        {
            if (order.counter.counterStatus == BuildCounterCpt.CounterStatusEnum.Idle)
            {
                order.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Ready);
                if (order as OrderForCustomer != null)
                {
                    workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Accounting, (OrderForCustomer)order);
                }
                else if (order as OrderForHotel != null)
                {
                    workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Accounting, (OrderForHotel)order);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 设置算账数据  
    /// </summary>
    public bool SetPay(OrderForCustomer orderForCustomer)
    {
        NpcAIWorkerCpt accountingCpt = null;
        float distance = 0;
        //选取距离最近的NPC
        for (int i = 0; i < listAccountingCpt.Count; i++)
        {
            NpcAIWorkerCpt npcAI = listAccountingCpt[i];
            if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
            {
                float tempDistance = Vector2.Distance(orderForCustomer.counter.GetAccountingPosition(), npcAI.transform.position);
                if (distance == 0 || tempDistance < distance)
                {
                    distance = tempDistance;
                    accountingCpt = npcAI;
                }
            }
        }
        if (accountingCpt != null && orderForCustomer.counter.counterStatus == BuildCounterCpt.CounterStatusEnum.Idle)
        {
            orderForCustomer.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Ready);
            accountingCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Accounting, orderForCustomer);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 展示支付特效
    /// </summary>
    public void ShowPayEffects(Vector3 position, long priceL, long priceM, long priceS)
    {
        GameObject payEffects = Instantiate(objPayEffects, position, new Quaternion());
        PayMoneyCpt payMoneyCpt = payEffects.GetComponent<PayMoneyCpt>();
        payMoneyCpt.SetData(position, priceL, priceM, priceS);
    }

    /// <summary>
    /// 清理所有柜台
    /// </summary>
    public void CleanAllCounter()
    {
        if (listCounterCpt==null)
            return;
        for (int i = 0; i < listCounterCpt.Count; i++)
        {
            BuildCounterCpt buildCounterCpt = listCounterCpt[i];
            buildCounterCpt.ClearCounter();
        };
    }
}
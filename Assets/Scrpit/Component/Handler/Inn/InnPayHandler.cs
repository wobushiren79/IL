using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class InnPayHandler : BaseMonoBehaviour
{
    // 柜台列表
    public List<BuildCounterCpt> listCounterCpt = new List<BuildCounterCpt>();

    //柜台容器
    protected GameObject _counterContainer;
    public GameObject counterContainer
    {
        get
        {
            if (_counterContainer == null)
            {
                _counterContainer = GameObject.FindGameObjectWithTag(TagInfo.Tag_FurnitureContainer);
            }
            return _counterContainer;
        }
    }
    //算账人容器
    protected GameObject _accountingContainer;
    public GameObject accountingContainer
    {
        get
        {
            if (_accountingContainer == null)
            {
                _accountingContainer = GameObject.FindGameObjectWithTag(TagInfo.Tag_NpcWorkerContainer);
            }
            return _accountingContainer;
        }
    }

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
        for (int i = 0; i < listCounterCpt.Count; i++)
        {
            BuildCounterCpt itemCounter = listCounterCpt[i];
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
    /// 获取人少的柜台
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BuildCounterCpt GetLessCounter()
    {
        BuildCounterCpt lessCounter = null;
        int number = int.MaxValue;
        for (int i = 0; i < listCounterCpt.Count; i++)
        {
            BuildCounterCpt itemCounter = listCounterCpt[i];
            if (number > itemCounter.payQueue.Count)
            {
                lessCounter = itemCounter;
                number = itemCounter.payQueue.Count;
            }
            else if (number == itemCounter.payQueue.Count)
            {
                int randomTemp= Random.Range(0, 2);
                if (randomTemp == 0)
                {
                    lessCounter = itemCounter;
                    number = itemCounter.payQueue.Count;
                }
            }
        }
        return lessCounter;
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
    /// 展示支付特效
    /// </summary>
    public void ShowPayEffects(Vector3 position, long priceL, long priceM, long priceS)
    {
        GameObject payEffects = EffectHandler.Instance.manager.CreateOtherEffect(gameObject, "PayMoney");
        payEffects.transform.position = position;
        payEffects.transform.rotation = new Quaternion();
        PayMoneyCpt payMoneyCpt = payEffects.GetComponent<PayMoneyCpt>();
        payMoneyCpt.SetData(position, priceL, priceM, priceS);
    }

    /// <summary>
    /// 清理所有柜台
    /// </summary>
    public void CleanAllCounter()
    {
        if (listCounterCpt == null)
            return;
        for (int i = 0; i < listCounterCpt.Count; i++)
        {
            BuildCounterCpt buildCounterCpt = listCounterCpt[i];
            buildCounterCpt.ClearCounter();
        };
    }
}
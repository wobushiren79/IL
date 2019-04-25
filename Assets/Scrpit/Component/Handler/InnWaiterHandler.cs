using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnWaiterHandler : BaseMonoBehaviour
{

    public GameObject waiterContainer;
    //服务员列表
    public List<NpcAIWorkerCpt> listWaiterCpt = new List<NpcAIWorkerCpt>();

    //锁
    private static Object SetWaiterLock = new Object();

    /// <summary>
    /// 找到所有服务员
    /// </summary>
    /// <returns></returns>
    public List<NpcAIWorkerCpt> InitWaiterCpt()
    {
        listWaiterCpt.Clear();
        if (waiterContainer == null)
            return listWaiterCpt;
        NpcAIWorkerCpt[] chefArray = waiterContainer.GetComponentsInChildren<NpcAIWorkerCpt>();
        if (chefArray == null)
            return listWaiterCpt;

        for (int i = 0; i < chefArray.Length; i++)
        {
            NpcAIWorkerCpt itemCpt = chefArray[i];
            if (itemCpt.isWaiter)
            {
                listWaiterCpt.Add(itemCpt);
            }
        }
        return listWaiterCpt;
    }

    /// <summary>
    /// 设置运送食物
    /// </summary>
    /// <returns></returns>
    public bool SetSendFood(FoodForCustomerCpt foodCpt)
    {
        lock (SetWaiterLock)
        {
            NpcAIWorkerCpt waiterCpt = null;
            float distance = 0;
            for (int i = 0; i < listWaiterCpt.Count; i++)
            {
                NpcAIWorkerCpt npcAI = listWaiterCpt[i];
                //服务员空闲 并且能到达指定地点
                if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
                {
                    float  tempDistance = Vector2.Distance(foodCpt.transform.position, npcAI.transform.position);
                    if(distance==0 || tempDistance < distance)
                    {
                        distance = tempDistance;
                        waiterCpt = npcAI;
                    }
                }
            }
            if (waiterCpt != null)
            {
                waiterCpt.SetIntentForWaiterSend(foodCpt);
                return true;
            }
            return false;
        }
    }


    /// <summary>
    /// 设置清理食物
    /// </summary>
    /// <returns></returns>
    public bool SetClearFood(FoodForCustomerCpt foodCpt)
    {
        lock (SetWaiterLock)
        {
            NpcAIWorkerCpt waiterCpt = null;
            for (int i = 0; i < listWaiterCpt.Count; i++)
            {
                NpcAIWorkerCpt npcAI = listWaiterCpt[i];
                if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
                {
                    waiterCpt = npcAI;
                    break;
                }
            }
            if (waiterCpt != null)
            {
                waiterCpt.SetIntentForWaiterClear(foodCpt);
                return true;
            }
            return false;
        }
    }
}
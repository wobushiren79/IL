using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnPayHandler : BaseMonoBehaviour
{
    // 柜台列表
    public List<BuildCounterCpt> listCounterCpt;
    // 算账人列表
    public List<NpcAIWorkerCpt> listAccountingCpt = new List<NpcAIWorkerCpt>();
    //柜台容器
    public GameObject counterContainer;
    //算账人容器
    public GameObject accountingContainer;

    //锁
    private static Object SetPayLock = new Object();
    private void Start()
    {
        InitAccountingCpt();
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
            NpcAIWorkerCpt itemCpt = chefArray[i];
            if (itemCpt.isAccounting)
            {
                listAccountingCpt.Add(itemCpt);
            }
        }
        return listAccountingCpt;
    }

    /// <summary>
    /// 设置算账数据
    /// </summary>
    public bool SetPay()
    {
        lock (SetPayLock)
        {
            NpcAIWorkerCpt accountingCpt = null;
            for (int i = 0; i < listAccountingCpt.Count; i++)
            {
                NpcAIWorkerCpt npcAI = listAccountingCpt[i];
                if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
                {
                    accountingCpt = npcAI;
                    break;
                }
            }
            for (int i = 0; i < listCounterCpt.Count; i++)
            {
    
            }
            if (accountingCpt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
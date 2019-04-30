using UnityEngine;
using UnityEditor;
using DG.Tweening;
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
    //锁
    private static Object SetPayLock = new Object();

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
    public bool SetPay(NpcAICustomerCpt customer)
    {
        lock (SetPayLock)
        {
            NpcAIWorkerCpt accountingCpt = null;
            float distance = 0;
            //选取距离最近的NPC
            for (int i = 0; i < listAccountingCpt.Count; i++)
            {
                NpcAIWorkerCpt npcAI = listAccountingCpt[i];
                if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
                {
                    float tempDistance = Vector2.Distance(customer.counterCpt.GetAccountingPosition(), npcAI.transform.position);
                    if (distance == 0 || tempDistance < distance)
                    {
                        distance = tempDistance;
                        accountingCpt = npcAI;
                    }
                }
            }
            if (accountingCpt != null && customer.counterCpt.workerCpt == null)
            {
                customer.counterCpt.workerCpt = accountingCpt;
                accountingCpt.SetIntentForAccounting(customer);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 展示支付特效
    /// </summary>
    public void ShowPayEffects(Vector3 position, long priceL, long priceM, long priceS)
    {
        GameObject payEffects = Instantiate(objPayEffects, position, new Quaternion());
        SpriteRenderer pay = payEffects.GetComponent<SpriteRenderer>();
        pay.DOFade(0, 1).SetDelay(1); ;
        payEffects.transform.DOMoveY(position.y + 0.5f, 2).OnComplete(delegate ()
          {
              Destroy(payEffects);
          });
    }
}
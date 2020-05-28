using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnCookHandler : BaseMonoBehaviour
{
    //灶台列表
    public List<BuildStoveCpt> listStoveCpt = new List<BuildStoveCpt>();
    //厨师列表
    public List<NpcAIWorkerCpt> listChefCpt = new List<NpcAIWorkerCpt>();

    //灶台容器
    public GameObject stoveContainer;
    //厨师容器
    public GameObject ChefContainer;

    /// <summary>
    /// 找到所有灶台
    /// </summary>
    /// <returns></returns>
    public List<BuildStoveCpt> InitStoveList()
    {
        if (stoveContainer == null)
            return listStoveCpt;
        BuildStoveCpt[] tableArray = stoveContainer.GetComponentsInChildren<BuildStoveCpt>();
        listStoveCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listStoveCpt;
    }

    /// <summary>
    /// 找到所有厨师
    /// </summary>
    /// <returns></returns>
    public List<NpcAIWorkerCpt> InitChefCpt()
    {
        listChefCpt.Clear();
        if (ChefContainer == null)
            return listChefCpt;
        NpcAIWorkerCpt[] chefArray = ChefContainer.GetComponentsInChildren<NpcAIWorkerCpt>();
        if (chefArray == null)
            return listChefCpt;

        for (int i = 0; i < chefArray.Length; i++)
        {
            NpcAIWorkerCpt itemCpt = chefArray[i];
            if (itemCpt.characterData.baseInfo.chefInfo.isWorking)
            {
                listChefCpt.Add(itemCpt);
            }
        }
        return listChefCpt;
    }

    /// <summary>
    ///  分配厨师做饭
    /// </summary>
    public bool SetChefForCook(OrderForCustomer orderForCustomer, NpcAIWorkerCpt chefCpt)
    {
        BuildStoveCpt stoveCpt = null;
        if (chefCpt == null)
        {
            return false;
        }
        for (int i = 0; i < listStoveCpt.Count; i++)
        {
            BuildStoveCpt itemStove = listStoveCpt[i];
            //检测是否能到达烹饪点
            if (itemStove.stoveStatus == BuildStoveCpt.StoveStatusEnum.Idle && CheckUtil.CheckPath(chefCpt.transform.position, itemStove.GetCookPosition()))
            {
                stoveCpt = itemStove;
                break;
            }
        }
        if (chefCpt != null && stoveCpt != null)
        {
            orderForCustomer.stove = stoveCpt;
            orderForCustomer.stove.SetStoveStatus(BuildStoveCpt.StoveStatusEnum.Ready);
            orderForCustomer.chef = chefCpt;
            chefCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Cook, orderForCustomer);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  分配厨师做饭
    /// </summary>
    public bool SetChefForCook(OrderForCustomer orderForCustomer)
    {
        NpcAIWorkerCpt chefCpt = null;
        BuildStoveCpt stoveCpt = null;
        for (int i = 0; i < listChefCpt.Count; i++)
        {
            NpcAIWorkerCpt npcAI = listChefCpt[i];
            if (npcAI.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
            {
                chefCpt = npcAI;
                break;
            }
        }
        if (chefCpt == null)
        {
            return false;
        }
        for (int i = 0; i < listStoveCpt.Count; i++)
        {
            BuildStoveCpt itemStove = listStoveCpt[i];
            //检测是否能到达烹饪点
            if (itemStove.stoveStatus == BuildStoveCpt.StoveStatusEnum.Idle && CheckUtil.CheckPath(chefCpt.transform.position, itemStove.GetCookPosition()))
            {
                stoveCpt = itemStove;
                break;
            }
        }
        if (chefCpt != null && stoveCpt != null)
        {
            orderForCustomer.stove = stoveCpt;
            orderForCustomer.stove.SetStoveStatus(BuildStoveCpt.StoveStatusEnum.Ready);
            orderForCustomer.chef = chefCpt;
            chefCpt.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Cook, orderForCustomer);
            return true;
        }
        else
        {
            return false;
        }
    }
}
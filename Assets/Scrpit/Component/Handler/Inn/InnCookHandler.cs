using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnCookHandler : InnBaseHandler
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
    /// 清理所有灶台
    /// </summary>
    public void CleanAllStove()
    {
        if (listStoveCpt==null)
            return;
        for (int i = 0; i < listStoveCpt.Count; i++)
        {
            BuildStoveCpt buildStoveCpt = listStoveCpt[i];
            buildStoveCpt.ClearStove();
        };
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnCookHandler : BaseMonoBehaviour
{
    //灶台列表
    public List<BuildStoveCpt> listStoveCpt;
    //厨师列表
    public List<NpcAIWorkerCpt> listChefCpt = new List<NpcAIWorkerCpt>();

    //灶台容器
    public GameObject stoveContainer;
    //厨师容器
    public GameObject ChefContainer;


    //锁
    private static Object SetChefLock = new Object();

    private void Start()
    {
        InitChefCpt();
    }

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
            if (itemCpt.isChef)
            {
                listChefCpt.Add(itemCpt);
            }
        }
        return listChefCpt;
    }

    /// <summary>
    ///  分配厨师做饭
    /// </summary>
    public bool SetChefForCook(MenuForCustomer foodData)
    {
        lock (SetChefLock)
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
                if (itemStove.chefCpt == null && CheckUtil.CheckPath(chefCpt.transform.position, itemStove.GetCookPosition()[0]) )
                {
                    stoveCpt = itemStove;
                    break;
                }
            }
            if (chefCpt != null && stoveCpt != null)
            {
                stoveCpt.SetChef(chefCpt);
                foodData.stove = stoveCpt;
                chefCpt.SetIntentForCook(stoveCpt, foodData);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
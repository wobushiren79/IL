using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnTableHandler : BaseMonoBehaviour
{
    //桌子列表
    public List<BuildTableCpt> listTableCpt;
    //桌子容器
    public GameObject tableContainer;

    /// <summary>
    /// 找到所有桌子
    /// </summary>
    /// <returns></returns>
    public List<BuildTableCpt> InitTableList()
    {
        if (tableContainer == null)
            return listTableCpt;
        BuildTableCpt[] tableArray = tableContainer.GetComponentsInChildren<BuildTableCpt>();
        listTableCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listTableCpt;
    }

    //锁
   private static Object GetIdleTableLock = new Object();

    /// <summary>
    /// 获取随机空闲的座位
    /// </summary>
    /// <returns></returns>
    public BuildTableCpt GetIdleTable()
    {
        //加锁 防止出现1桌2人
        lock (GetIdleTableLock)
        {
            if (listTableCpt == null)
                return null;
            List<BuildTableCpt> idleTableList = new List<BuildTableCpt>();
            for (int i = 0; i < listTableCpt.Count; i++)
            {
                BuildTableCpt itemTable = listTableCpt[i];
                if (itemTable.tableState == BuildTableCpt.TableStateEnum.Idle)
                {
                    idleTableList.Add(itemTable);
                }
            }
            if (idleTableList.Count == 0)
                return null;
            BuildTableCpt buildTable = RandomUtil.GetRandomDataByList(idleTableList);
            buildTable.tableState = BuildTableCpt.TableStateEnum.Ready;
            return buildTable;
        }
    }

}
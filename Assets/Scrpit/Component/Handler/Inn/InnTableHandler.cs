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

    /// <summary>
    /// 获取随机空闲的座位
    /// </summary>
    /// <returns></returns>
    public BuildTableCpt GetIdleTable()
    {
        if (listTableCpt == null)
            return null;
        List<BuildTableCpt> idleTableList = new List<BuildTableCpt>();
        for (int i = 0; i < listTableCpt.Count; i++)
        {
            BuildTableCpt itemTable = listTableCpt[i];
            if (itemTable.tableStatus == BuildTableCpt.TableStatusEnum.Idle)
            {
                idleTableList.Add(itemTable);
            }
        }
        if (idleTableList.Count == 0)
            return null;
        BuildTableCpt buildTable = RandomUtil.GetRandomDataByList(idleTableList);
        buildTable.SetTableStatus(BuildTableCpt.TableStatusEnum.Ready);
        return buildTable;
    }

    /// <summary>
    /// 清理所有桌子
    /// </summary>
    public void CleanAllTable()
    {
        if (listTableCpt == null)
            return;
        for (int i = 0; i < listTableCpt.Count; i++)
        {
            BuildTableCpt buildTableCpt = listTableCpt[i];
            buildTableCpt.CleanTable();
        };
    }
}
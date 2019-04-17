using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnCookHandler : BaseMonoBehaviour
{
    //桌子列表
    public List<BuildStoveCpt> listTableCpt;
    //桌子容器
    public GameObject tableContainer;

    /// <summary>
    /// 找到所有灶台
    /// </summary>
    /// <returns></returns>
    public List<BuildStoveCpt> InitStoveList()
    {
        if (tableContainer == null)
            return listTableCpt;
        BuildStoveCpt[] tableArray = tableContainer.GetComponentsInChildren<BuildStoveCpt>();
        listTableCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listTableCpt;
    }
}
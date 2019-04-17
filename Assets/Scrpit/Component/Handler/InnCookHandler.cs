using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnCookHandler : BaseMonoBehaviour
{
    //灶台列表
    public List<BuildStoveCpt> listStoveCpt;
    //灶台容器
    public GameObject stoveContainer;

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
}
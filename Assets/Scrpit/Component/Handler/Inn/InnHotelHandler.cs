using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnHotelHandler : InnBaseHandler
{
    //桌子列表
    public List<BuildBedCpt> listBedCpt;
    //桌子容器
    public GameObject bedContainer;

    /// <summary>
    /// 找到所有桌子
    /// </summary>
    /// <returns></returns>
    public List<BuildBedCpt> InitBedList()
    {
        if (bedContainer == null)
            return listBedCpt;
        BuildBedCpt[] tableArray = bedContainer.GetComponentsInChildren<BuildBedCpt>();
        listBedCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listBedCpt;
    }
}
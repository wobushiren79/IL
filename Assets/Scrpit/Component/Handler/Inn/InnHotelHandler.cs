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

    /// <summary>
    /// 获取随机空闲的座位
    /// </summary>
    /// <returns></returns>
    public BuildBedCpt GetIdleBed()
    {
        if (listBedCpt == null)
            return null;
        List<BuildBedCpt> idleBedList = new List<BuildBedCpt>();
        for (int i = 0; i < listBedCpt.Count; i++)
        {
            BuildBedCpt itemBed = listBedCpt[i];
            if (itemBed.GetBedStatus() == BuildBedCpt.BedStatusEnum.Idle)
            {
                idleBedList.Add(itemBed);
            }
        }
        if (idleBedList.Count == 0)
            return null;
        BuildBedCpt buildBed = RandomUtil.GetRandomDataByList(idleBedList);
        buildBed.SetBedStatus(BuildBedCpt.BedStatusEnum.Ready);
        return buildBed;
    }


    /// <summary>
    /// 清理所有床
    /// </summary>
    public void CleanAllBed()
    {
        if (listBedCpt == null)
            return;
        for (int i=0;i< listBedCpt.Count;i++)
        {
            BuildBedCpt buildBed = listBedCpt[i];
            buildBed.CleanBed();
        }
    }
}
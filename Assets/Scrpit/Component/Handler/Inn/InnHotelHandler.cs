using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnHotelHandler : InnBaseHandler
{
    //桌子列表
    public List<BuildBedCpt> listBedCpt;
    //桌子容器
    protected GameObject _bedContainer;
    public GameObject bedContainer
    {
        get
        {
            if (_bedContainer == null)
            {
                _bedContainer = GameObject.FindGameObjectWithTag(TagInfo.Tag_FurnitureContainer);
            }
            return _bedContainer;
        }
    }
    /// <summary>
    /// 找到所有桌子
    /// </summary>
    /// <returns></returns>
    public List<BuildBedCpt> InitBedList(InnBuildBean innBuildData)
    {
        if (bedContainer == null)
            return listBedCpt;
        BuildBedCpt[] tableArray = bedContainer.GetComponentsInChildren<BuildBedCpt>();
        listBedCpt = TypeConversionUtil.ArrayToList(tableArray);
        //设置每张床的美观值
        try
        {
            List<InnResBean> listFurnitureData = innBuildData.GetFurnitureList(2);
            List<InnResBean> listFloorData = innBuildData.GetFloorList(2);
            List<InnResBean> listWallData = innBuildData.GetWallList(2);

            Dictionary<Vector3, InnResBean> mapFurnitureData = new Dictionary<Vector3, InnResBean>();
            Dictionary<Vector3, InnResBean> mapFloorData = new Dictionary<Vector3, InnResBean>();
            Dictionary<Vector3, InnResBean> mapWallData = new Dictionary<Vector3, InnResBean>();

            foreach (InnResBean innResData in listFurnitureData)
            {
                mapFurnitureData.Add(TypeConversionUtil.Vector3BeanToVector3(innResData.startPosition), innResData);
            }
            foreach (InnResBean innResData in listFloorData)
            {
                mapFloorData.Add(TypeConversionUtil.Vector3BeanToVector3(innResData.startPosition), innResData);
            }
            foreach (InnResBean innResData in listWallData)
            {
                mapWallData.Add(TypeConversionUtil.Vector3BeanToVector3(innResData.startPosition), innResData);
            }

            if (listBedCpt != null)
                for (int i = 0; i < listBedCpt.Count; i++)
                {
                    BuildBedCpt buildBedCpt = listBedCpt[i];
                    float totalAddAesthetics = 0;
                    float totalSubAesthetics = 0;
                    GetAroundAesthetics(mapFurnitureData, buildBedCpt.transform.position, 3,out float addFurnitureAesthetics,out float subFurnitureAesthetics);
                    totalAddAesthetics += addFurnitureAesthetics;
                    totalSubAesthetics += subFurnitureAesthetics;
                    GetAroundAesthetics(mapFloorData, buildBedCpt.transform.position - new Vector3(0.5f, 0.5f), 3, out float addFloorAesthetics, out float subFloorAesthetics);
                    totalAddAesthetics += addFloorAesthetics ;
                    totalSubAesthetics += subFloorAesthetics;
                    GetAroundAesthetics(mapWallData, buildBedCpt.transform.position - new Vector3(0.5f, 0.5f), 3, out float addWallAesthetics, out float subWallAesthetics);
                    totalAddAesthetics += addWallAesthetics ;
                    totalSubAesthetics += subWallAesthetics;
                    buildBedCpt.SetAddAesthetics((float)decimal.Round(decimal.Parse(totalAddAesthetics + ""), 1), (float)decimal.Round(decimal.Parse(totalSubAesthetics + ""), 1));
                }
        }
        catch
        {

        }
        return listBedCpt;
    }

    /// <summary>
    /// 获取坐标周围范围的美观加成
    /// </summary>
    /// <param name="mapBuildData"></param>
    /// <param name="position"></param>
    /// <param name="aroundRange"></param>
    /// <returns></returns>
    public void GetAroundAesthetics(Dictionary<Vector3, InnResBean> mapBuildData, Vector3 position, int aroundRange,out float addAesthetics,out float subAesthetics)
    {
        addAesthetics = 0;
        subAesthetics = 0;
        Vector3 selfPosition = new Vector3(position.x, position.y, position.z);
        position -= new Vector3(aroundRange, aroundRange);
        for (int i = 0; i <= aroundRange * 2; i++)
        {
            for (int f = 0; f <= aroundRange * 2; f++)
            {
                Vector3 currentPosition = position + new Vector3(i, f);
                mapBuildData.TryGetValue(currentPosition, out InnResBean buildData);
                if (buildData != null)
                {
                    BuildItemBean buildItem = InnBuildHandler.Instance.manager.GetBuildDataById(buildData.id);
                    addAesthetics += buildItem.aesthetics;
                    if(selfPosition!= currentPosition && buildItem.GetBuildType()== BuildItemTypeEnum.Bed)
                    {
                        subAesthetics -= 10;
                    }
                }
            }
        }
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
        for (int i = 0; i < listBedCpt.Count; i++)
        {
            BuildBedCpt buildBed = listBedCpt[i];
            buildBed.CleanBed();
        }
    }
}
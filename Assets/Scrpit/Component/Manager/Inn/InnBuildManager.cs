using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnBuildManager : BaseManager, IBuildDataView
{
    public List<BuildItemBean> listBuildFloorData;
    public List<BuildItemBean> listBuildWallData;
    public List<BuildItemBean> listBuildTableData;

    public BuildDataController buildDataController;

    //家具列表
    public List<BaseBuildItemCpt> listFurnitureCpt;

    private void Awake()
    {
        buildDataController = new BuildDataController(this, this);
    }

    /// <summary>
    /// 通过ID获取家具Obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetFurnitureObjById(long id)
    {
        GameObject furnitureObj=null;
        if (listFurnitureCpt == null)
            return furnitureObj;
        for (int i = 0; i < listFurnitureCpt.Count; i++)
        {
            BaseBuildItemCpt buildItemCpt = listFurnitureCpt[i];
            if(buildItemCpt.buildId== id)
            {
                furnitureObj = Instantiate(buildItemCpt.gameObject, buildItemCpt.transform);
                furnitureObj.SetActive(true);
            }
        }
        return furnitureObj; 
    }

    /// <summary>
    /// 根据地板ID获取地板数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildItemBean GetFloorDataById(long id)
    {
        return GetBuildDataById(id, listBuildFloorData);
    }

    /// <summary>
    /// 根据墙ID获取墙数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildItemBean GetWallDataById(long id)
    {
        return GetBuildDataById(id, listBuildWallData);
    }

    /// <summary>
    /// 根据桌子ID获取墙数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildItemBean GetTableDataById(long id)
    {
        return GetBuildDataById(id, listBuildTableData);
    }

    /// <summary>
    /// 根据建筑ID获取建筑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildItemBean GetBuildDataById(long id, List<BuildItemBean> listData)
    {
        if (listData == null)
            return null;
        for (int i = 0; i < listData.Count; i++)
        {
            BuildItemBean itemData = listData[i];
            if (itemData.id == id)
            {
                return itemData;
            }
        }
        return null;
    }


    #region 建筑数据回调
    public void GetAllBuildItemsSuccess(List<BuildItemBean> listData)
    {
        if (CheckUtil.ListIsNull(listData))
        {
            return;
        }
        listBuildFloorData.Clear();
        listBuildWallData.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BuildItemBean itemData = listData[i];
            if (itemData.build_type == (int)BuildItemBean.BuildType.Floor)
            {
                listBuildFloorData.Add(itemData);
            }
            else if (itemData.build_type == (int)BuildItemBean.BuildType.Wall)
            {
                listBuildWallData.Add(itemData);
            }
            else if (itemData.build_type == (int)BuildItemBean.BuildType.Table)
            {
                listBuildTableData.Add(itemData);
            }
        }
    }

    public void GetAllBuildItemsFail()
    {
    }

    public void GetBuildItemsByTypeSuccess(BuildItemBean.BuildType type, List<BuildItemBean> listData)
    {
        switch (type)
        {
            case BuildItemBean.BuildType.Floor:
                listBuildFloorData = listData;
                break;
            case BuildItemBean.BuildType.Wall:
                listBuildWallData = listData;
                break;
            case BuildItemBean.BuildType.Table:
                listBuildTableData = listData;
                break;
        }
    }

    public void GetBuildItemsByTypeFail()
    {
    }
    #endregion
}
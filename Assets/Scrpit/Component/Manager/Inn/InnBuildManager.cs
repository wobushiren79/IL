using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnBuildManager : BaseManager, IBuildDataView
{
    public Dictionary<long,BuildItemBean> listBuildData;

    public BuildDataController buildDataController;

    //家具列表
    public List<BaseBuildItemCpt> listFurnitureCpt;
    //家具图标
    public IconBeanDictionary listFurnitureIcon;

    private void Awake()
    {
        buildDataController = new BuildDataController(this, this);
    }

    /// <summary>
    /// 通过名字获取家具图标
    /// </summary>
    /// <returns></returns>
    public Sprite GetFurnitureSpriteByName(string name)
    {
       return GetSpriteByName(name, listFurnitureIcon);
    }

    /// <summary>
    /// 通过ID获取家具Obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetFurnitureObjById(long id, Transform tfFather)
    {
        GameObject furnitureObj=null;
        if (listFurnitureCpt == null)
            return furnitureObj;
        for (int i = 0; i < listFurnitureCpt.Count; i++)
        {
            BaseBuildItemCpt buildItemCpt = listFurnitureCpt[i];
            if (buildItemCpt == null)
                continue;
            if(buildItemCpt.buildId== id)
            {
                furnitureObj = Instantiate(buildItemCpt.gameObject, tfFather);
                furnitureObj.SetActive(true);
            }
        }
        return furnitureObj; 
    }


    /// <summary>
    /// 根据建筑ID获取建筑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildItemBean GetBuildDataById(long id)
    {
        if (listBuildData == null)
            return null;
        if (listBuildData.TryGetValue(id, out BuildItemBean itemData))
            return itemData;
        else
            return null;
    }


    #region 建筑数据回调
    public void GetAllBuildItemsSuccess(List<BuildItemBean> listData)
    {
        listBuildData = new Dictionary<long, BuildItemBean>();
        if (CheckUtil.ListIsNull(listData))
        {
            return;
        }
        foreach (BuildItemBean itemData in listData)
        {
            listBuildData.Add(itemData.id, itemData);
        }
    }

    public void GetAllBuildItemsFail()
    {
    }

    public void GetBuildItemsByTypeSuccess(BuildItemBean.BuildType type, List<BuildItemBean> listData)
    {
        listBuildData = new Dictionary<long, BuildItemBean>();
        foreach (BuildItemBean itemData in listData)
        {
            listBuildData.Add(itemData.id, itemData);
        }
    }

    public void GetBuildItemsByTypeFail()
    {
    }
    #endregion
}
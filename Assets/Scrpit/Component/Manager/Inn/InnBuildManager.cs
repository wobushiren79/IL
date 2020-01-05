using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnBuildManager : BaseManager, IBuildDataView
{
    public Dictionary<long, BuildItemBean> listBuildData;

    public BuildDataController buildDataController;

    //家具列表
    public GameObjectDictionary listFurnitureCpt = new GameObjectDictionary();
    //家具图标
    public IconBeanDictionary listFurnitureIcon = new IconBeanDictionary();

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
        GameObject furnitureObj = null;
        if (listFurnitureCpt == null)
            return furnitureObj;
        BuildItemBean buildItemData = GetBuildDataById(id);
        if (listFurnitureCpt.TryGetValue(buildItemData.model_id, out GameObject objItem))
        {
            furnitureObj = Instantiate(tfFather.gameObject, objItem);
            BaseBuildItemCpt buildItemCpt = furnitureObj.GetComponent<BaseBuildItemCpt>();
            buildItemCpt.SetData(buildItemData);
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
        return GetDataById(id, listBuildData);
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

    public void GetBuildItemsByTypeSuccess(BuildItemTypeEnum type, List<BuildItemBean> listData)
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
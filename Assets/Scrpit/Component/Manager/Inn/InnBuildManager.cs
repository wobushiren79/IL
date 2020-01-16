﻿using UnityEngine;
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

    public void Awake()
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
            List<string> listIcon = buildItemData.GetIconList();
            switch ((BuildItemTypeEnum)buildItemData.build_type)
            {
                case BuildItemTypeEnum.Counter:
                    BuildCounterCpt buildCounter = (BuildCounterCpt)buildItemCpt;
                    Sprite spLeftCounter = GetFurnitureSpriteByName(listIcon[0] + "_0");
                    Sprite spRightCounter = GetFurnitureSpriteByName(listIcon[0] + "_1");
                    Sprite spDownCounter = GetFurnitureSpriteByName(listIcon[0] + "_2");
                    Sprite spUpCounter = GetFurnitureSpriteByName(listIcon[0] + "_3");
                    buildCounter.SetData(buildItemData, spLeftCounter, spRightCounter, spDownCounter, spUpCounter);
                    break;
                case BuildItemTypeEnum.Stove:
                    BuildStoveCpt buildStove = (BuildStoveCpt)buildItemCpt;
                    Sprite spLeftStove = GetFurnitureSpriteByName(listIcon[0] + "_0");
                    Sprite spRightStove = GetFurnitureSpriteByName(listIcon[0] + "_1");
                    Sprite spDownStove = GetFurnitureSpriteByName(listIcon[0] + "_2");
                    Sprite spUpStove = GetFurnitureSpriteByName(listIcon[0] + "_3");
                    buildStove.SetData(buildItemData, spLeftStove, spRightStove, spDownStove, spUpStove);
                    break;
                case BuildItemTypeEnum.Table:
                    BuildTableCpt buildTable = (BuildTableCpt)buildItemCpt;
                    Sprite spTable = GetFurnitureSpriteByName(listIcon[0]);
                    Sprite spLeftChair = GetFurnitureSpriteByName(listIcon[1] + "_0");
                    Sprite spRightChair = GetFurnitureSpriteByName(listIcon[1] + "_1");
                    Sprite spDownChair = GetFurnitureSpriteByName(listIcon[1] + "_2");
                    Sprite spUpChair = GetFurnitureSpriteByName(listIcon[1] + "_3");
                    buildTable.SetData(buildItemData, spTable, spLeftChair, spRightChair, spDownChair, spUpChair);
                    break;
                case BuildItemTypeEnum.Door:
                    BuildDoorCpt buildDoor = (BuildDoorCpt)buildItemCpt;
                    Sprite spDoor = GetFurnitureSpriteByName(listIcon[0]);
                    buildDoor.SetData(buildItemData, spDoor, spDoor, spDoor, spDoor);
                    break;
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
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class InnBuildManager : BaseManager, IBuildDataView
{
    //建造物品数据
    public Dictionary<long, BuildItemBean> listBuildData;

    public BuildDataController buildDataController;

    //墙体tile列表
    public TileBeanDictionary listWallTile = new TileBeanDictionary();
    //地板tile列表
    public TileBeanDictionary listFloorTile = new TileBeanDictionary();
    //家具模型列表
    public GameObjectDictionary listFurnitureCpt = new GameObjectDictionary();
    //家具图标
    public SpriteAtlas  atlasForFunrniture;
    //地板图标
    public IconBeanDictionary listFloorIcon = new IconBeanDictionary();
    //墙体图标
    public IconBeanDictionary listWallIcon = new IconBeanDictionary();

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
        return GetSpriteByName(name, atlasForFunrniture);
    }

    /// <summary>
    /// 通过名字获取地板图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFloorSpriteByName(string name)
    {
        return GetSpriteByName(name, listFloorIcon);
    }
    /// <summary>
    /// 通过名字获取墙体图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetWallSpriteByName(string name)
    {
        return GetSpriteByName(name, listWallIcon);
    }

    /// <summary>
    /// 通过ID获取家具Obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetFurnitureObjById(InnResBean furnitureData, Transform tfFather, BuildBedBean buildBedData)
    {
        GameObject furnitureObj = null;
        if (listFurnitureCpt == null)
            return furnitureObj;
        BuildItemBean buildItemData = GetBuildDataById(furnitureData.id);
        if (listFurnitureCpt.TryGetValue(buildItemData.model_name, out GameObject objItem))
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

                    Sprite spLeftChair = GetFurnitureSpriteByName(listIcon[1] + "_0");
                    Sprite spRightChair = GetFurnitureSpriteByName(listIcon[1] + "_1");
                    Sprite spDownChair = GetFurnitureSpriteByName(listIcon[1] + "_2");
                    Sprite spUpChair = GetFurnitureSpriteByName(listIcon[1] + "_3");

                    if (buildItemData.model_name.Equals("Table_1"))
                    {
                        Sprite spTable = GetFurnitureSpriteByName(listIcon[0]);
                        buildTable.SetData(buildItemData, spTable, spLeftChair, spRightChair, spDownChair, spUpChair);
                    }
                    else if (buildItemData.model_name.Equals("Table_2"))
                    {
                        Sprite spLeftTable = GetFurnitureSpriteByName(listIcon[0] + "_0");
                        Sprite spRightTable = GetFurnitureSpriteByName(listIcon[0] + "_1");
                        Sprite spDownTable = GetFurnitureSpriteByName(listIcon[0] + "_2");
                        Sprite spUpTable = GetFurnitureSpriteByName(listIcon[0] + "_3");
                        buildTable.SetData(buildItemData, spLeftTable, spRightTable, spDownTable, spUpTable, spLeftChair, spRightChair, spDownChair, spUpChair);
                    }

                    break;
                case BuildItemTypeEnum.Decoration:
                    BuildDecorationCpt buildDecoration = (BuildDecorationCpt)buildItemCpt;
                    Sprite spDecoration = GetFurnitureSpriteByName(buildItemData.icon_key);
                    buildDecoration.SetData(buildItemData, spDecoration);
                    break;
                case BuildItemTypeEnum.Door:
                    BuildDoorCpt buildDoor = (BuildDoorCpt)buildItemCpt;
                    Sprite spDoor = GetFurnitureSpriteByName(listIcon[0]);
                    buildDoor.SetData(buildItemData, spDoor);
                    break;
                case BuildItemTypeEnum.Floor:
                    BuildFloorCpt buildFloor = (BuildFloorCpt)buildItemCpt;
                    Sprite spFloor = GetFloorSpriteByName(buildItemData.icon_key);
                    buildFloor.SetData(buildItemData, spFloor);
                    break;
                case BuildItemTypeEnum.Wall:
                    BuildWallCpt buildWall = (BuildWallCpt)buildItemCpt;
                    Sprite spWall = GetWallSpriteByName(buildItemData.icon_key);
                    buildWall.SetData(buildItemData, spWall);
                    break;
                case BuildItemTypeEnum.Bed:
                    BuildBedCpt buildBed = (BuildBedCpt)buildItemCpt;
                    buildBed.SetData(buildItemData, buildBedData);
                    break;
                case BuildItemTypeEnum.Stairs:
                    BuildStairsCpt buildStairs = (BuildStairsCpt)buildItemCpt;
                    buildStairs.SetData(buildItemData);
                    buildStairs.SetRemarkId(furnitureData.remarkId);
                    if (furnitureData.remark != null && furnitureData.remark.Equals("1"))
                    {
                        buildStairs.SetLayer(1);
                    }
                    else
                    {
                        buildStairs.SetLayer(2);
                    }
                    break;
                default:
                    buildItemCpt.SetData(buildItemData);
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

    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetFloorTileByName(string name)
    {
        return GetTileBaseByName(name, listFloorTile);
    }

    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetWallTileByName(string name)
    {
        return GetTileBaseByName(name, listWallTile);
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
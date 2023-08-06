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
    public TileBeanDictionary dicWallTile = new TileBeanDictionary();
    //地板tile列表
    public TileBeanDictionary dicFloorTile = new TileBeanDictionary();
    //地面tile列表
    public TileBeanDictionary dicGroundTile = new TileBeanDictionary();
    //其他tile列表
    public TileBeanDictionary dicOtherTile = new TileBeanDictionary();

    //家具模型列表
    public GameObjectDictionary dicFurnitureCpt = new GameObjectDictionary();

    public void Awake()
    {
        buildDataController = new BuildDataController(this, this);
        buildDataController.GetAllBuildItemsData();
    }

    /// <summary>
    /// 通过ID获取家具Obj
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetFurnitureObjById(InnResBean furnitureData, Transform tfFather, BuildBedBean buildBedData, ItemBean itemData = null)
    {
        BuildItemBean buildItemData = GetBuildDataById(furnitureData.id);
        GameObject furnitureObjModel = LoadAddressablesUtil.LoadAssetSync<GameObject>($"Assets/Prefabs/BuildItem/Base/{buildItemData.model_name}.prefab");
        GameObject furnitureObj = Instantiate(tfFather.gameObject, furnitureObjModel);
        BaseBuildItemCpt buildItemCpt = furnitureObj.GetComponent<BaseBuildItemCpt>();
        List<string> listIcon = buildItemData.GetIconList();
        switch ((BuildItemTypeEnum)buildItemData.build_type)
        {
            case BuildItemTypeEnum.Counter:
                BuildCounterCpt buildCounter = (BuildCounterCpt)buildItemCpt;
                Sprite spLeftCounter = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_0");
                Sprite spRightCounter = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_1");
                Sprite spDownCounter = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_2");
                Sprite spUpCounter = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_3");
                buildCounter.SetData(buildItemData, spLeftCounter, spRightCounter, spDownCounter, spUpCounter);
                break;
            case BuildItemTypeEnum.Stove:
                BuildStoveCpt buildStove = (BuildStoveCpt)buildItemCpt;
                Sprite spLeftStove = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_0");
                Sprite spRightStove = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_1");
                Sprite spDownStove = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_2");
                Sprite spUpStove = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_3");
                buildStove.SetData(buildItemData, spLeftStove, spRightStove, spDownStove, spUpStove);
                break;
            case BuildItemTypeEnum.Table:
                BuildTableCpt buildTable = (BuildTableCpt)buildItemCpt;

                Sprite spLeftChair = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[1] + "_0");
                Sprite spRightChair = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[1] + "_1");
                Sprite spDownChair = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[1] + "_2");
                Sprite spUpChair = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[1] + "_3");

                if (buildItemData.model_name.Equals("Table_1"))
                {
                    Sprite spTable = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0]);
                    buildTable.SetData(buildItemData, spTable, spLeftChair, spRightChair, spDownChair, spUpChair);
                }
                else if (buildItemData.model_name.Equals("Table_2"))
                {
                    Sprite spLeftTable = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_0");
                    Sprite spRightTable = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_1");
                    Sprite spDownTable = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_2");
                    Sprite spUpTable = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0] + "_3");
                    buildTable.SetData(buildItemData, spLeftTable, spRightTable, spDownTable, spUpTable, spLeftChair, spRightChair, spDownChair, spUpChair);
                }

                break;
            case BuildItemTypeEnum.Decoration:
                BuildDecorationCpt buildDecoration = (BuildDecorationCpt)buildItemCpt;
                Sprite spDecoration = IconHandler.Instance.GetFurnitureSpriteByName(buildItemData.icon_key);
                buildDecoration.SetData(buildItemData, spDecoration);
                break;
            case BuildItemTypeEnum.Door:
                BuildDoorCpt buildDoor = (BuildDoorCpt)buildItemCpt;
                Sprite spDoor = IconHandler.Instance.GetFurnitureSpriteByName(listIcon[0]);
                buildDoor.SetData(buildItemData, spDoor);
                break;
            case BuildItemTypeEnum.Floor:
                BuildFloorCpt buildFloor = (BuildFloorCpt)buildItemCpt;
                Sprite spFloor = IconHandler.Instance.GetFloorSpriteByName(buildItemData.icon_key);
                buildFloor.SetData(buildItemData, spFloor);
                break;
            case BuildItemTypeEnum.Wall:
                BuildWallCpt buildWall = (BuildWallCpt)buildItemCpt;
                Sprite spWall = IconHandler.Instance.GetWallSpriteByName(buildItemData.icon_key);
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
            case BuildItemTypeEnum.Seed:
                BuildSeedCpt buildSeed = (BuildSeedCpt)buildItemCpt;
                var itemSeedInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);
                Sprite spSeed = IconHandler.Instance.GetItemsSpriteByName(itemSeedInfo.icon_key);
                buildSeed.SetData(buildItemData, spSeed, itemData);
                break;
            default:
                buildItemCpt.SetData(buildItemData);
                break;


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
        return LoadAddressablesUtil.LoadAssetSync<TileBase>($"Assets/Tile/Tiles/Floor/{name}.asset");
    }

    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetWallTileByName(string name)
    {
        return LoadAddressablesUtil.LoadAssetSync<TileBase>($"Assets/Tile/Tiles/Wall/{name}.asset");
    }

    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetGroundTileByName(string name)
    {
        return LoadAddressablesUtil.LoadAssetSync<TileBase>($"Assets/Tile/Tiles/Ground/{name}.asset");
    }

    /// <summary>
    /// 获取后庭
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetCourtyardTileByName(string name)
    {
        return LoadAddressablesUtil.LoadAssetSync<TileBase>($"Assets/Tile/Tiles/Courtyard/{name}.asset");
    }

    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TileBase GetOtherTileByName(string name)
    {
        return LoadAddressablesUtil.LoadAssetSync<TileBase>($"Assets/Tile/Tiles/Other/{name}.asset");
    }

    #region 建筑数据回调
    public void GetAllBuildItemsSuccess(List<BuildItemBean> listData)
    {
        listBuildData = new Dictionary<long, BuildItemBean>();
        if (listData.IsNull())
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
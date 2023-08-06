using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InnCourtyardBuilder : BaseTilemapBuilder
{
    protected Tilemap _buildTilemapForGround;

    public Tilemap buildTilemapForGround
    {
        get
        {
            if (_buildTilemapForGround == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("GroundElement");
                if (obj != null)
                    _buildTilemapForGround = obj.GetComponent<Tilemap>();
            }
            return _buildTilemapForGround;
        }
    }
    protected Tilemap _buildTilemapForPlant;

    public Tilemap buildTilemapForPlant
    {
        get
        {
            if (_buildTilemapForPlant == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("Plant");
                if (obj != null)
                    _buildTilemapForPlant = obj.GetComponent<Tilemap>();
            }
            return _buildTilemapForPlant;
        }
    }
    /// <summary>
    /// 开始建筑
    /// </summary>
    public void StartBuild()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
        if (innCourtyardData.courtyardLevel == 0)
            return;
        StartBuildGroundElement(innCourtyardData);
        StartBuildPlant(innCourtyardData);
    }

    /// <summary>
    /// 修建植物
    /// </summary>
    public void StartBuildPlant(InnCourtyardBean innCourtyardData)
    {
        var listSeedData = innCourtyardData.listSeedData;

        for (int i = 0; i < listSeedData.Count; i++)
        {
            InnResBean itemData = listSeedData[i];
            SeedInfoBean seedInfo = SeedInfoCfg.GetItemDataByItemId(itemData.id);
            InnCourtyardSeedBean seedData = JsonUtil.FromJson<InnCourtyardSeedBean>(itemData.remark);
            //根据成长天天数 算出处在哪个周期内
            int growLoop = Mathf.FloorToInt(seedData.growDay / (float)seedInfo.growup_oneloopday);
            //获取指定周期的tile
            string tileName = seedInfo.GetSeedTile(growLoop);
            TileBase plantTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(tileName);

            Vector3 position = itemData.startPosition.GetVector3();
            Build(buildTilemapForPlant, plantTile, new Vector3Int((int)position.x, (int)position.y, 0));

            //湿润的土地
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(999998);
            TileBase groundTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(buildItemData.tile_name);
            Build(buildTilemapForGround, groundTile, new Vector3Int((int)position.x, (int)position.y, 0));
        }
    }

    /// <summary>
    /// 改变地板
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="changeTileName"></param>
    public void ChangeSeed(Vector3Int changePosition, InnResBean itemData)
    {
        if (itemData != null)
        {
            SeedInfoBean seedInfo = SeedInfoCfg.GetItemDataByItemId(itemData.id);
            InnCourtyardSeedBean seedData = JsonUtil.FromJson<InnCourtyardSeedBean>(itemData.remark);
            //根据成长天天数 算出处在哪个周期内
            int growLoop = Mathf.FloorToInt(seedData.growDay / (float)seedInfo.growup_oneloopday);
            //获取指定周期的tile
            string tileName = seedInfo.GetSeedTile(growLoop);

            TileBase plantTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(tileName);
            Build(buildTilemapForPlant, plantTile, changePosition);

            //湿润的土地
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(999998);
            TileBase groundTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(buildItemData.tile_name);
            Build(buildTilemapForGround, groundTile, changePosition);
        }
        else
        {
            ClearTile(buildTilemapForPlant, changePosition);

            //干燥的土地
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(999999);
            TileBase groundTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(buildItemData.tile_name);
            Build(buildTilemapForGround, groundTile, changePosition);
        }
    }


    /// <summary>
    /// 修建地面
    /// </summary>
    public void StartBuildGroundElement(InnCourtyardBean innCourtyardData)
    {
        int level = innCourtyardData.courtyardLevel + 1;
        //获取耕地的数据
        int size = level;
        int startOffsetZ = -size - 3;
        BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(999999);
        TileBase groundTile = InnBuildHandler.Instance.manager.GetCourtyardTileByName(buildItemData.tile_name);
        for (int x = -size; x <= size; x++)
        {
            for (int z = -size; z <= size; z++)
            {
                Build(buildTilemapForGround, groundTile, new Vector3Int(x, z + startOffsetZ, 0));
            }
        }
    }
}
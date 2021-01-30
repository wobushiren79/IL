using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SceneBuilder : BaseTilemapBuilder
{
    protected Tilemap _tileMapForGround;
    protected Tilemap _tileMapForGroundElement;

    public List<TileBean> listTileGroundForSeasons;
    public List<TileBean> listTileGressForSeasons;

    public Tilemap tileMapForGround
    {
        get
        {
            if (_tileMapForGround == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("Ground");
                if (obj != null)
                    _tileMapForGround = obj.GetComponent<Tilemap>();
            }
            return _tileMapForGround;
        }
    }
    public Tilemap tileMapForGroundElement
    {
        get
        {
            if (_tileMapForGroundElement == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("GroundElement");
                if (obj != null)
                    _tileMapForGroundElement = obj.GetComponent<Tilemap>();
            }
            return _tileMapForGroundElement;
        }
    }
    /// <summary>
    /// 构建场景
    /// </summary>
    /// <param name="seasons"></param>
    public void BuildScene(SeasonsEnum seasons)
    {
        string tileGroundName = "";
        string tileGressName = "";
        switch (seasons)
        {
            case SeasonsEnum.Spring:
                tileGroundName = "tile_gress_1";
                tileGressName = "Gress_1";
                break;
            case SeasonsEnum.Summer:
                tileGroundName = "tile_gress_2";
                tileGressName = "Gress_2";
                break;
            case SeasonsEnum.Autumn:
                tileGroundName = "tile_gress_3";
                tileGressName = "Gress_3";
                break;
            case SeasonsEnum.Winter:
                tileGroundName = "tile_gress_4";
                tileGressName = "Gress_4";
                break;
        }       
        //替换地面
        TileBase newTileDataForGround = InnBuildHandler.Instance.manager.GetGroundTileByName(tileGroundName);
        TileBase defGroundTile = tileMapForGround.GetTile(Vector3Int.zero);
        if (!newTileDataForGround.name.Equals(defGroundTile.name))
        {
            tileMapForGround.SwapTile(defGroundTile, newTileDataForGround);
        }

        //替换草地
        TileBase newTileDataForGress = InnBuildHandler.Instance.manager.GetOtherTileByName(tileGressName);
        TileBase defGressTile = tileMapForGroundElement.GetTile(Vector3Int.zero);
        if (!newTileDataForGress.name.Equals(defGressTile.name))
        {
            tileMapForGroundElement.SwapTile(defGressTile, newTileDataForGress);
        }
    }
}
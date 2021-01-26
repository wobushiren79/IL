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

    public TileBean tileDataForGround;
    public TileBean tileDataForGress;

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
        //替换地面
        TileBean newTileDataForGround = GetTileByName(EnumUtil.GetEnumName(seasons), listTileGroundForSeasons);
        if (!newTileDataForGround.key.Equals(tileDataForGround.key))
        {
            tileMapForGround.SwapTile(tileDataForGround.value, newTileDataForGround.value);
            tileDataForGround = newTileDataForGround;
        }

        //替换草地

        TileBean newTileDataForGress = GetTileByName(EnumUtil.GetEnumName(seasons), listTileGressForSeasons);
        if (!newTileDataForGress.key.Equals(tileDataForGress.key))
        {
            tileMapForGroundElement.SwapTile(tileDataForGress.value, newTileDataForGress.value);
            tileDataForGress = newTileDataForGress;
        }


        switch (seasons)
        {
            case SeasonsEnum.Spring:
                break;
            case SeasonsEnum.Summer:
                break;
            case SeasonsEnum.Autumn:
                break;
            case SeasonsEnum.Winter:
                break;
        }
    }

    /// <summary>
    /// 通过名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    private TileBean GetTileByName(string name, List<TileBean> listData)
    {
        foreach (TileBean itemTile in listData)
        {
            if (itemTile.key.Equals(name))
            {
                return itemTile;
            }
        }
        return null;
    }
}
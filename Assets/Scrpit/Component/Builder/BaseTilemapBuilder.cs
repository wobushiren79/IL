using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BaseTilemapBuilder : BaseMonoBehaviour
{
    // 瓦片地图
    public Tilemap buildTilemap;
    //瓦片笔
    public List<TileBean> buildTileList;

    public void Build(string tileName, int startX, int startY, int endX, int endY)
    {
        if (buildTilemap == null|| buildTileList==null)
            return;
        TileBase tile = null;

        for(int i=0;i< buildTileList.Count; i++)
        {
            TileBean tileBean = buildTileList[i];
            if (tileBean.key.Equals(tileName))
            {
                tile = tileBean.value;
            }
        }
        if (tile == null)
            return;
        buildTilemap.BoxFill(Vector3Int.zero, tile, startX, startY, endX, endY);
    }

    public void Build(string tileName,Vector3Int position)
    {
        if (buildTilemap == null || buildTileList == null)
            return;
        TileBase tile = null;
        for (int i = 0; i < buildTileList.Count; i++)
        {
            TileBean tileBean = buildTileList[i];
            if (tileBean.key.Equals(tileName))
            {
                tile = tileBean.value;
            }
        }
        if (tile == null)
            return;
        buildTilemap.SetTile(position, tile);
    }

    public void Build(string tileName,int x, int y)
    {
        Build(tileName,new Vector3Int(x,y,0));
    }

    /// <summary>
    /// 替换tile
    /// </summary>
    /// <param name="changeBase"></param>
    /// <param name="newBase"></param>
    public void SwapTile(TileBase changeBase, TileBase newBase)
    {
        buildTilemap.SwapTile(changeBase, newBase);
    }
    public void SwapTile(Tilemap tilemap, TileBase changeBase, TileBase newBase)
    {
        tilemap.SwapTile(changeBase, newBase);
    }

    /// <summary>
    /// 清空所有tiles
    /// </summary>
    public void ClearAllTiles()
    {
        buildTilemap.ClearAllTiles();
    }
}
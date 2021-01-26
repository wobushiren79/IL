using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BaseTilemapBuilder : BaseBuilder
{
    public void Build(Tilemap buildTilemap, TileBase tile, int startX, int startY, int endX, int endY)
    {
        if (buildTilemap == null || tile == null)
            return;
        buildTilemap.BoxFill(Vector3Int.zero, tile, startX, startY, endX, endY);
    }

    public void Build(Tilemap buildTilemap, TileBase tile, Vector3Int position)
    {
        if (buildTilemap == null || tile == null)
            return;
        buildTilemap.SetTile(position, tile);
    }

    public void Build(Tilemap buildTilemap, TileBase tile, int x, int y)
    {
        Build(buildTilemap, tile, new Vector3Int(x, y, 0));
    }

    /// <summary>
    /// 替换tile
    /// </summary>
    /// <param name="changeBase"></param>
    /// <param name="newBase"></param>
    public void SwapTile(Tilemap buildTilemap, TileBase changeBase, TileBase newBase)
    {
        buildTilemap.SwapTile(changeBase, newBase);
    }
    public void SwapTile(Tilemap buildTilemap, Tilemap tilemap, TileBase changeBase, TileBase newBase)
    {
        tilemap.SwapTile(changeBase, newBase);
    }

    /// <summary>
    /// 清空所有tiles
    /// </summary>
    public void ClearAllTiles(Tilemap buildTilemap)
    {
        buildTilemap.ClearAllTiles();
    }
    /// <summary>
    /// 清理一个tile
    /// </summary>
    /// <param name="position"></param>
    public void ClearTile(Tilemap buildTilemap, Vector3Int position)
    {
        buildTilemap.SetTile(position, null);
    }
    /// <summary>
    /// 获取tilemap容器
    /// </summary>
    /// <returns></returns>
    public virtual GameObject GetTilemapContainer(Tilemap buildTilemap)
    {
        return buildTilemap.gameObject;
    }
}
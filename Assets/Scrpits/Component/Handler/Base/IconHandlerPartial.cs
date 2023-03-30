using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public partial class IconHandler 
{
    //�Ҿ�ͼ��
    public SpriteAtlas atlasForFunrniture;
    //ǽ��ͼ��
    public SpriteAtlas atlasForWall;
    //�ذ�ͼ��
    public SpriteAtlas atlasForFloor;
    //�ذ�ͼ��
    public Dictionary<string,Sprite> dicFunrnitureIcon = new Dictionary<string, Sprite>();
    //�ذ�ͼ��
    public Dictionary<string, Sprite> dicFloorIcon = new Dictionary<string, Sprite>();
    //ǽ��ͼ��
    public Dictionary<string, Sprite> dicWallIcon = new Dictionary<string, Sprite>();

    public static string PathSpriteAtlas = "Assets/Texture/SpriteAtlas";

    public string PathAtlasForFurniture = $"{PathSpriteAtlas}/AtlasForFurniture.spriteatlas";
    public string PathAtlasForFloor = $"{PathSpriteAtlas}/AtlasForFloor.spriteatlas";
    public string PathAtlasForWall = $"{PathSpriteAtlas}/AtlasForWall.spriteatlas";
    /// <summary>
    /// ͨ�����ֻ�ȡ�Ҿ�ͼ��
    /// </summary>
    /// <returns></returns>
    public Sprite GetFurnitureSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFunrnitureIcon, ref atlasForFunrniture, PathAtlasForFurniture, name);
    }

    /// <summary>
    /// ͨ�����ֻ�ȡ�ذ�ͼ��
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFloorSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFloorIcon, ref atlasForFloor, PathAtlasForFloor, name);
    }

    /// <summary>
    /// ͨ�����ֻ�ȡǽ��ͼ��
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetWallSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicWallIcon, ref atlasForWall, PathAtlasForWall, name);
    }
}

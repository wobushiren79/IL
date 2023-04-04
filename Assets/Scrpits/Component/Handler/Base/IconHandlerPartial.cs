using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public partial class IconHandler
{
    //UI图标
    public SpriteAtlas iconAtlas;

    public SpriteAtlas backgroundAtlas;
    //家具图标
    public SpriteAtlas atlasForFunrniture;
    //墙壁图标
    public SpriteAtlas atlasForWall;
    //地板图标
    public SpriteAtlas atlasForFloor;
    //地板图标
    public Dictionary<string, Sprite> dicFunrnitureIcon = new Dictionary<string, Sprite>();
    //地板图标
    public Dictionary<string, Sprite> dicFloorIcon = new Dictionary<string, Sprite>();
    //墙体图标
    public Dictionary<string, Sprite> dicWallIcon = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> dicIcon = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> dicBackground = new Dictionary<string, Sprite>();

    public static string PathSpriteAtlas = "Assets/Texture/SpriteAtlas";

    public string PathAtlasForFurniture = $"{PathSpriteAtlas}/AtlasForFurniture.spriteatlas";
    public string PathAtlasForFloor = $"{PathSpriteAtlas}/AtlasForFloor.spriteatlas";
    public string PathAtlasForWall = $"{PathSpriteAtlas}/AtlasForWall.spriteatlas";
    public string PathAtlasForIcon = $"{PathSpriteAtlas}/AtlasForIcon.spriteatlas";
    public string PathAtlasForBackground = $"{PathSpriteAtlas}/AtlasForBackground.spriteatlas";

    public Dictionary<string, Texture2D> dicTextureUI = new Dictionary<string, Texture2D>();

    /// <summary>
    /// 根据名字获取UI图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetIconSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicIcon, ref iconAtlas, PathAtlasForIcon, name);
    }

    public Sprite GetBackgroundSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicBackground, ref backgroundAtlas, PathAtlasForBackground, name);
    }

    /// <summary>
    /// 通过名字获取家具图标
    /// </summary>
    /// <returns></returns>
    public Sprite GetFurnitureSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFunrnitureIcon, ref atlasForFunrniture, PathAtlasForFurniture, name);
    }

    /// <summary>
    /// 通过名字获取地板图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFloorSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFloorIcon, ref atlasForFloor, PathAtlasForFloor, name);
    }

    /// <summary>
    /// 通过名字获取墙体图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetWallSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicWallIcon, ref atlasForWall, PathAtlasForWall, name);
    }

    public Texture2D GetTextureUIByName(string name)
    {
        return manager.GetModelForAddressablesSync(dicTextureUI, name);
    }
}

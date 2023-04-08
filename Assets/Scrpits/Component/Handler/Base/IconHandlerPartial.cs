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
    //食物图标
    public SpriteAtlas foodAtlas;
    //物品图标
    public SpriteAtlas itemsAtlas;
    //物品图标
    public Dictionary<string, Sprite> dicItemsIcon = new Dictionary<string, Sprite>();
    //地板图标
    public Dictionary<string, Sprite> dicFunrnitureIcon = new Dictionary<string, Sprite>();
    //地板图标
    public Dictionary<string, Sprite> dicFloorIcon = new Dictionary<string, Sprite>();
    //墙体图标
    public Dictionary<string, Sprite> dicWallIcon = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> dicIcon = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> dicBackground = new Dictionary<string, Sprite>();
    //食物图标
    public Dictionary<string, Sprite> dicFoodIcon = new Dictionary<string, Sprite>();

    public static string PathSpriteAtlas = "Assets/Texture/SpriteAtlas";

    public string PathAtlasForFurniture = $"{PathSpriteAtlas}/AtlasForFurniture.spriteatlas";
    public string PathAtlasForFloor = $"{PathSpriteAtlas}/AtlasForFloor.spriteatlas";
    public string PathAtlasForWall = $"{PathSpriteAtlas}/AtlasForWall.spriteatlas";
    public string PathAtlasForIcon = $"{PathSpriteAtlas}/AtlasForIcon.spriteatlas";
    public string PathAtlasForBackground = $"{PathSpriteAtlas}/AtlasForBackground.spriteatlas";
    public string PathAtlasForFood = $"{PathSpriteAtlas}/AtlasForFood.spriteatlas";
    public string PathAtlasForItems = $"{PathSpriteAtlas}/AtlasForItems.spriteatlas";

    public Dictionary<string, Texture2D> dicTextureUI = new Dictionary<string, Texture2D>();

    #region 身体相关
    //身体列表
    public SpriteAtlas trunkAtlas;
    public Dictionary<string, Sprite> dicIconBodyTrunk = new Dictionary<string, Sprite>();
    //头发列表
    public SpriteAtlas hairAtlas;
    public Dictionary<string, Sprite> dicIconBodyHair = new Dictionary<string, Sprite>();
    //眼睛列表
    public SpriteAtlas eyeAtlas;
    public Dictionary<string, Sprite> dicIconBodyEye = new Dictionary<string, Sprite>();
    //嘴巴列表
    public SpriteAtlas mouthAtlas;
    public Dictionary<string, Sprite> dicIconBodyMouth = new Dictionary<string, Sprite>();

    public Dictionary<string, Texture2D> dicBodyTex = new Dictionary<string, Texture2D>();
    #endregion

    #region 服装相关
    public SpriteAtlas maskAtlas;
    //面具列表
    public Dictionary<string, Sprite> dicIconMask = new Dictionary<string, Sprite>();

    public SpriteAtlas hatAtlas;
    //帽子列表
    public Dictionary<string, Sprite> dicIconHat = new Dictionary<string, Sprite>();

    public SpriteAtlas clothesAtlas;
    //衣服列表
    public Dictionary<string, Sprite> dicIconClothes = new Dictionary<string, Sprite>();

    public SpriteAtlas shoesAtlas;
    //鞋子列表
    public Dictionary<string, Sprite> dicIconShoes = new Dictionary<string, Sprite>();

    public Dictionary<string, Texture2DArray> dicDressTex = new Dictionary<string, Texture2DArray>();
    #endregion

    /// <summary>
    /// 通过名字获取Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemsSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicItemsIcon, ref itemsAtlas, PathAtlasForItems, name);
    }

    /// <summary>
    /// 获取装备贴图
    /// </summary>
    /// <param name="name"></param>
    /// <param name="animLength"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Texture2DArray GetDressTexture(string name, int animLength, int type)
    {
        if (name == null)
            return null;
        if (dicDressTex.TryGetValue(name, out Texture2DArray texture2DArray))
        {
            return texture2DArray;
        }
        for (int i = 0; i < animLength; i++)
        {
            Sprite spData = null;
            string spName;
            if (animLength == 1)
            {
                spName = name;
            }
            else
            {
                spName = $"{name}_{i}";
            }
            spData = GetDressSpriteDataByName(type, name);
            if (spData == null)
                return null;
            Texture2D itemTex = TextureUtil.SpriteToTexture2D(spData);

            if (texture2DArray == null)
            {
                texture2DArray = new Texture2DArray(itemTex.width, itemTex.height, animLength, itemTex.format, true, false);
                texture2DArray.filterMode = FilterMode.Point;
                texture2DArray.wrapMode = TextureWrapMode.Clamp;
            }
            texture2DArray.SetPixels(itemTex.GetPixels(), i);
        }
        texture2DArray.Apply();
        dicDressTex.Add(name, texture2DArray);
        return texture2DArray;
    }

    /// <summary>
    /// 获取身体图片
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetDressSpriteDataByName(int type, string name)
    {
        if (name == null)
            return null;

        Dictionary<string, Sprite> dicData = null;
        SpriteAtlas spriteData = null;
        string atlasName = "";
        switch (type)
        {
            case 1:
                atlasName = "AtlasForMask";
                dicData = dicIconMask;
                spriteData = maskAtlas;
                break;
            case 2:
                atlasName = "AtlasForHat";
                dicData = dicIconHat;
                spriteData = hatAtlas;
                break;
            case 3:
                atlasName = "AtlasForClothes";
                dicData = dicIconClothes;
                spriteData = clothesAtlas;
                break;
            case 4:
                atlasName = "AtlasForShoes";
                dicData = dicIconShoes;
                spriteData = shoesAtlas;
                break;
        }
        Sprite itemSprite = manager.GetSpriteByNameSync(dicData, ref spriteData, $"{PathSpriteAtlas}/{atlasName}.spriteatlas",name);
        switch (type)
        {
            case 1:
                maskAtlas = spriteData;
                break;
            case 2:
                hatAtlas = spriteData;
                break;
            case 3:
                clothesAtlas = spriteData;
                break;
            case 4:
                shoesAtlas = spriteData;
                break;
        }
        return itemSprite;
    }


    /// <summary>
    /// 获取躯体的贴图
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Texture2D GetBodyTexture(int type, string name)
    {
        if (name.IsNull())
            return null;
        if (dicBodyTex.TryGetValue(name, out Texture2D value))
        {
            return value;
        }
        Sprite spData = GetBodySpriteByName(type, name);
        if (spData == null)
            return null;
        value = TextureUtil.SpriteToTexture2D(spData);
        dicBodyTex.Add(name, value);
        return value;
    }

    /// <summary>
    /// 获取躯体的图片
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetBodySpriteByName(int type, string name)
    {
        if (name.IsNull())
            return null;
        Dictionary<string, Sprite> dicData = null;
        SpriteAtlas spriteData = null;
        string atlasName = "";
        switch (type)
        {
            case 1:
                atlasName = "AtlasForTrunk";
                dicData = dicIconBodyTrunk;
                spriteData = trunkAtlas;
                break;
            case 2:
                atlasName = "AtlasForHair";
                dicData = dicIconBodyHair;
                spriteData = hairAtlas;
                break;
            case 3:
                atlasName = "AtlasForEye";
                dicData = dicIconBodyEye;
                spriteData = eyeAtlas;
                break;
            case 4:
                atlasName = "AtlasForMouth";
                dicData = dicIconBodyMouth;
                spriteData = mouthAtlas;
                break;
        }
        Sprite spData = manager.GetSpriteByNameSync(dicData, ref spriteData, $"{PathSpriteAtlas}/{atlasName}.spriteatlas", name);
        switch (type)
        {
            case 1:
                trunkAtlas = spriteData;
                break;
            case 2:
                hairAtlas = spriteData;
                break;
            case 3:
                eyeAtlas = spriteData;
                break;
            case 4:
                mouthAtlas = spriteData;
                break;
        }
        return spData;
    }

    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFoodIcon, ref foodAtlas, PathAtlasForFood, name + "_0");
    }
    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodLastSpriteByName(string name)
    {
        return manager.GetSpriteByNameSync(dicFoodIcon, ref foodAtlas, PathAtlasForFood, name + "_1");
    }

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

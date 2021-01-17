using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;

public class CharacterDressManager : BaseManager
{
    public SpriteAtlas maskAtlas;
    //面具列表
    public IconBeanDictionary listIconMask = new IconBeanDictionary();
    //面具动画列表
    public AnimBeanDictionary listMaskAnim = new AnimBeanDictionary();

    public SpriteAtlas hatAtlas;
    //帽子列表
    public IconBeanDictionary listIconHat = new IconBeanDictionary();
    //帽子动画列表
    public AnimBeanDictionary listHatAnim = new AnimBeanDictionary();

    public SpriteAtlas clothesAtlas;
    //衣服列表
    public IconBeanDictionary listIconClothes = new IconBeanDictionary();
    //衣服动画列表
    public AnimBeanDictionary listClothesAnim = new AnimBeanDictionary();

    public SpriteAtlas shoesAtlas;
    //鞋子列表
    public IconBeanDictionary listIconShoes = new IconBeanDictionary();
    //鞋子动画列表
    public AnimBeanDictionary listShoesAnim = new AnimBeanDictionary();



    public AnimationClip GetAnimByName(GeneralEnum generalEnum, string name)
    {
        switch (generalEnum)
        {
            case GeneralEnum.Mask:
                return GetMaskAnimClipByName(name);
            case GeneralEnum.Hat:
                return GetHatAnimClipByName(name);
            case GeneralEnum.Clothes:
                return GetClothesAnimClipByName(name);
            case GeneralEnum.Shoes:
                return GetShoesAnimClipByName(name);
        }
        return null;
    }

    public Sprite GetSpriteByName(GeneralEnum generalEnum, string name)
    {
        switch (generalEnum)
        {
            case GeneralEnum.Mask:
                return GetMaskSpriteByName(name);
            case GeneralEnum.Hat:
                return GetHatSpriteByName(name);
            case GeneralEnum.Clothes:
                return GetClothesSpriteByName(name);
            case GeneralEnum.Shoes:
                return GetShoesSpriteByName(name);
        }
        return null;
    }

    /// <summary>
    /// 根据名字获取面具
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMaskSpriteByName(string name)
    {
        return GetSpriteDataByName(1, name);
    }

    /// <summary>
    /// 根据名字获取面具动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetMaskAnimClipByName(string name)
    {
        return GetAnimClipByName(1,  name);
    }

    /// <summary>
    /// 根据名字获取鞋子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteByName(string name)
    {
        return GetSpriteDataByName(4, name);
    }

    /// <summary>
    /// 根据名字获取鞋子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetShoesAnimClipByName(string name)
    {
        return GetAnimClipByName(3, name);
    }

    /// <summary>
    /// 根据名字获取衣服图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return GetSpriteDataByName(3, name);
    }

    /// <summary>
    /// 根据名字获取衣服动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetClothesAnimClipByName(string name)
    {
        return GetAnimClipByName(3, name);
    }

    /// <summary>
    /// 根据名字获取帽子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteByName(string name)
    {
        return GetSpriteDataByName(2, name);
    }

    /// <summary>
    /// 根据名字获取帽子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetHatAnimClipByName(string name)
    {
        return GetAnimClipByName(2, name);
    }

    protected Sprite GetSpriteDataByName(int type, string name)
    {
        if (name == null)
            return null;

        IconBeanDictionary dicData = null;
        SpriteAtlas spriteData = null;

        switch (type)
        {
            case 1:
                dicData = listIconMask;
                spriteData = maskAtlas;
                break;
            case 2:
                dicData = listIconHat;
                spriteData = hatAtlas;
                break;
            case 3:
                dicData = listIconClothes;
                spriteData = clothesAtlas;
                break;
            case 4:
                dicData = listIconShoes;
                spriteData = shoesAtlas;
                break;
        }
        if (dicData.TryGetValue(name, out Sprite value))
        {
            return value;
        }
        if (spriteData != null)
        {
            Sprite itemSprite = GetSpriteByName(name, spriteData);
            if (itemSprite != null)
                dicData.Add(name, itemSprite);
            return itemSprite;
        }
        string atlasName = "";

        switch (type)
        {
            case 1:
                atlasName = "AtlasForMask";
                break;
            case 2:
                atlasName = "AtlasForHat";
                break;
            case 3:
                atlasName = "AtlasForClothes";
                break;
            case 4:
                atlasName = "AtlasForShoes";
                break;
        }

        spriteData = LoadAssetUtil.SyncLoadAsset<SpriteAtlas>("sprite/dress", atlasName);
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
        if (spriteData != null)
            return GetSpriteDataByName(type, name);
        return null;
    }

    protected AnimationClip GetAnimClipByName(int type, string name)
    {
        if (name == null)
            return null;
        AnimBeanDictionary dicData = null;
        switch (type)
        {
            case 1:
                dicData = listMaskAnim;
                break;
            case 2:
                dicData = listHatAnim;
                break;
            case 3:
                dicData = listClothesAnim;
                break;
            case 4:
                dicData = listShoesAnim;
                break;
        }
        if (dicData.TryGetValue(name, out AnimationClip value))
        {
            return value;
        }

        AnimationClip anim = LoadAssetUtil.SyncLoadAsset<AnimationClip>("anim/dress", name);
        if (anim != null)
        {
            dicData.Add(name, anim);
        }
        return anim;
    }
}
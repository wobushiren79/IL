using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;

public class CharacterDressManager : BaseManager
{
    public SpriteAtlas maskAtlas;
    //面具列表
    //public IconBeanDictionary listIconMask;
    //面具动画列表
    public AnimBeanDictionary listMaskAnim;

    public SpriteAtlas shoesAtlas;
    //鞋子列表
    //public IconBeanDictionary listIconShoes;
    //鞋子动画列表
    public AnimBeanDictionary listShoesAnim;


    public SpriteAtlas clothesAtlas;
    //衣服列表
    //public IconBeanDictionary listIconClothes;
    //衣服动画列表
    public AnimBeanDictionary listClothesAnim;

    public SpriteAtlas hatAtlas;
    //帽子列表
    //public IconBeanDictionary listIconHat;
    //帽子动画列表
    public AnimBeanDictionary listHatAnim;

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
        return GetSpriteByName(name, maskAtlas);
    }

    /// <summary>
    /// 根据名字获取面具动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetMaskAnimClipByName(string name)
    {
        return GetAnimClipByName(name, listMaskAnim);
    }

    /// <summary>
    /// 根据名字获取鞋子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteByName(string name)
    {
        return GetSpriteByName(name, shoesAtlas);
    }

    /// <summary>
    /// 根据名字获取鞋子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetShoesAnimClipByName(string name)
    {
        return GetAnimClipByName(name, listShoesAnim);
    }

    /// <summary>
    /// 根据名字获取衣服图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return GetSpriteByName(name, clothesAtlas);
    }

    /// <summary>
    /// 根据名字获取衣服动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetClothesAnimClipByName(string name)
    {
        return GetAnimClipByName(name, listClothesAnim);
    }

    /// <summary>
    /// 根据名字获取帽子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteByName(string name)
    {
        return GetSpriteByName(name, hatAtlas);
    }

    /// <summary>
    /// 根据名字获取帽子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetHatAnimClipByName(string name)
    {
        return GetAnimClipByName(name, listHatAnim);
    }
}
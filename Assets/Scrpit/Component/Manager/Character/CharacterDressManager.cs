using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterDressManager : BaseManager
{
    //面具列表
    public IconBeanDictionary listIconMask;
    //鞋子列表
    public IconBeanDictionary listIconShoes;
    //衣服列表
    public IconBeanDictionary listIconClothes;
    //帽子列表
    public IconBeanDictionary listIconHat;


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
        return GetSpriteByName(name, listIconMask);
    }

    /// <summary>
    /// 根据名字获取鞋子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconShoes);
    }

    /// <summary>
    /// 根据名字获取衣服图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconClothes);
    }

    /// <summary>
    /// 根据名字获取帽子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconHat);
    }
}
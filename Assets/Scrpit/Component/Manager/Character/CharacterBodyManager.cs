using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterBodyManager : BaseManager
{
    //身体列表
    public List<IconBean> listIconBodyTrunk;
    //头发列表
    public List<IconBean> listIconBodyHair;
    //眼睛列表
    public List<IconBean> listIconBodyEye;
    //嘴巴列表
    public List<IconBean> listIconBodyMouth;

    /// <summary>
    /// 获取躯干
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTrunkSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyTrunk);
    }
    public Sprite GetTrunkSpriteByPosition(int position)
    {
        return GetSpriteByPosition(position, listIconBodyTrunk);
    }

    /// <summary>
    /// 获取头发
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHairSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyHair);
    }
    public Sprite GetHairSpriteByPosition(int position)
    {
        return GetSpriteByPosition(position, listIconBodyHair);
    }
    public IconBean GetHairIconBeanByPosition(int position)
    {
        return  BeanUtil.GetIconBeanByPosition(position, listIconBodyHair);
    }

    /// <summary>
    /// 获取眼睛
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetEyeSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyEye);
    }
    public Sprite GettEyeSpriteByPosition(int position)
    {
        return GetSpriteByPosition(position, listIconBodyEye);
    }
    public IconBean GetEyeIconBeanByPosition(int position)
    {
        return BeanUtil.GetIconBeanByPosition(position, listIconBodyEye);
    }

    /// <summary>
    /// 根据名字获取嘴巴
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMouthSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyMouth);
    }
    public Sprite GettMouthSpriteByPosition(int position)
    {
        return GetSpriteByPosition(position, listIconBodyMouth);
    }
    public IconBean GetMouthIconBeanByPosition(int position)
    {
        return BeanUtil.GetIconBeanByPosition(position, listIconBodyMouth);
    }
}
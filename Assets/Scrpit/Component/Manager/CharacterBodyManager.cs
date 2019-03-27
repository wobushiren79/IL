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
    /// 根据名字获取躯干
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTrunkSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyTrunk);
    }

    /// <summary>
    /// 根据名字获取头发
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHairSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyHair);
    }

    /// <summary>
    /// 根据名字获取眼睛
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetEyeSpriteByName(string name)
    {
        return GetSpriteByName(name, listIconBodyEye);
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


}
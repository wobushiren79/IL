using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
public class CharacterBodyManager : BaseManager
{
    //身体列表
    public SpriteAtlas trunkAtlas;
    //public IconBeanDictionary listIconBodyTrunk;
    //头发列表
    public SpriteAtlas hairAtlas;
    //public IconBeanDictionary listIconBodyHair;
    //眼睛列表
    public SpriteAtlas eyeAtlas;
    //public IconBeanDictionary listIconBodyEye;
    //嘴巴列表
    public SpriteAtlas mouthAtlas;
    //public IconBeanDictionary listIconBodyMouth;

    /// <summary>
    /// 获取躯干
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTrunkSpriteByName(string name)
    {
        return GetSpriteByName(name, trunkAtlas);
    }

    /// <summary>
    /// 获取头发
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHairSpriteByName(string name)
    {
        return GetSpriteByName(name, hairAtlas);
    }

    /// <summary>
    /// 获取随机头发
    /// </summary>
    /// <returns></returns>
    public string GetRandomHairStr()
    {
        int hairNumber = Random.Range(1, 82);
        return "character_hair_" + hairNumber;
    }

    /// <summary>
    /// 获取所有头发
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllHair()
    {
        List<string> listData = new List<string>();
        for (int i = 1; i < 82; i++)
        {
            listData.Add("character_hair_" + i);
        }
        return listData;
    }

    /// <summary>
    /// 获取眼睛
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetEyeSpriteByName(string name)
    {
        return GetSpriteByName(name, eyeAtlas);
    }
    /// <summary>
    /// 获取随机眼睛
    /// </summary>
    /// <returns></returns>
    public string GetRandomEyeStr()
    {
        int hairNumber = Random.Range(1, 60);
        return "character_eye_" + hairNumber;
    }

    /// <summary>
    /// 获取所有眼睛
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllEye()
    {
        List<string> listData = new List<string>();
        for (int i = 1; i < 60; i++)
        {
            listData.Add("character_eye_" + i);
        }
        return listData;
    }

    /// <summary>
    /// 根据名字获取嘴巴
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMouthSpriteByName(string name)
    {
        return GetSpriteByName(name, mouthAtlas);
    }
    /// <summary>
    /// 获取随机嘴巴
    /// </summary>
    /// <returns></returns>
    public string GetRandomMouthStr()
    {
        int hairNumber = Random.Range(1, 39);
        return "character_mouth_" + hairNumber;
    }

    /// <summary>
    /// 获取所有嘴巴
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllMouth()
    {
        List<string> listData = new List<string>();
        for (int i = 1; i < 39; i++)
        {
            listData.Add("character_mouth_" + i);
        }
        return listData;
    }

    /// <summary>
    /// 根据名字获取皮肤
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetSkinSpriteByName(string name)
    {
        return GetSpriteByName(name, trunkAtlas);
    }

    /// <summary>
    /// 获取所有嘴巴
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllSkin()
    {
        List<string> listData = new List<string>();
        for (int i = 1; i < 7; i++)
        {
            listData.Add("character_body_" + i);
        }
        return listData;
    }
}
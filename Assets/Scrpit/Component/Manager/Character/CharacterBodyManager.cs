using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
public class CharacterBodyManager : BaseManager
{
    //身体列表
    public SpriteAtlas trunkAtlas;
    public IconBeanDictionary listIconBodyTrunk = new IconBeanDictionary();
    //头发列表
    public SpriteAtlas hairAtlas;
    public IconBeanDictionary listIconBodyHair = new IconBeanDictionary();
    //眼睛列表
    public SpriteAtlas eyeAtlas;
    public IconBeanDictionary listIconBodyEye = new IconBeanDictionary();
    //嘴巴列表
    public SpriteAtlas mouthAtlas;
    public IconBeanDictionary listIconBodyMouth = new IconBeanDictionary();


    /// <summary>
    /// 获取躯干
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTrunkSpriteByName(string name)
    {
        return GetSpriteDataByName(1, name);
    }

    /// <summary>
    /// 获取所有皮肤
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllTrunk()
    {
        List<string> listData = new List<string>();
        for (int i = 1; i < 7; i++)
        {
            listData.Add("character_body_" + i);
        }
        return listData;
    }

    /// <summary>
    /// 获取头发
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHairSpriteByName(string name)
    {
        return GetSpriteDataByName(2, name);
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
    /// 获取创建角色头发
    /// </summary>
    /// <returns></returns>
    public List<Sprite> GetCreateCharacterHair()
    {
        List<Sprite> listData = new List<Sprite>();
        for (int i = 1; i < 20; i++)
        {
            Sprite sprite = GetHairSpriteByName("character_hair_" + i);
            listData.Add(sprite);
        }
        return listData;
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
        return GetSpriteDataByName(3, name); ;
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
    /// 获取创建角色眼睛
    /// </summary>
    /// <returns></returns>
    public List<Sprite> GetCreateCharacterEye()
    {
        List<Sprite> listData = new List<Sprite>();
        for (int i = 1; i < 20; i++)
        {
            Sprite sprite = GetEyeSpriteByName("character_eye_" + i);
            listData.Add(sprite);
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
        return GetSpriteDataByName(4, name); ;
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
    public List<Sprite> GetCreateCharacterMouth()
    {
        List<Sprite> listData = new List<Sprite>();
        for (int i = 1; i < 20; i++)
        {
            Sprite sprite = GetMouthSpriteByName("character_mouth_" + i);
            listData.Add(sprite);
        }
        return listData;
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

    protected Sprite GetSpriteDataByName(int type, string name)
    {
        if (name == null)
            return null;
        IconBeanDictionary dicData = null;
        SpriteAtlas spriteData = null;
        string atlasName = "";
        switch (type)
        {
            case 1:
                atlasName = "AtlasForTrunk";
                dicData = listIconBodyTrunk;
                spriteData = trunkAtlas;
                break;
            case 2:
                atlasName = "AtlasForHair";
                dicData = listIconBodyHair;
                spriteData = hairAtlas;
                break;
            case 3:
                atlasName = "AtlasForEye";
                dicData = listIconBodyEye;
                spriteData = eyeAtlas;
                break;
            case 4:
                atlasName = "AtlasForMouth";
                dicData = listIconBodyMouth;
                spriteData = mouthAtlas;
                break;
        }
        Sprite spData = GetSpriteByName(dicData,ref spriteData, atlasName, ProjectConfigInfo.ASSETBUNDLE_SPRITEATLAS, name);
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

}
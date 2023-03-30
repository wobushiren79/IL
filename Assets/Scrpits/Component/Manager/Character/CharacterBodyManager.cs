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

    public Dictionary<string, Texture2D> dicTex = new Dictionary<string, Texture2D>();
    /// <summary>
    /// 获取躯干
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTrunkSpriteByName(string name)
    {
        return GetSpriteDataByName(1, name);
    }

    public Texture GetTrunkTexByName(string name)
    {
        return TryGetTexture(name, 1);
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

    public Texture GetHairTexByName(string name)
    {
        return TryGetTexture(name, 2);
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
        }        return listData;
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

    public Texture2D GetEyeTexByName(string name)
    {
        return TryGetTexture( name, 3);
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
        return GetSpriteDataByName(4, name);
    }

    public Texture GetMouthTexByName(string name)
    {
        return TryGetTexture(name, 4);
    }

    /// <summary>
    /// 获取随机嘴巴
    /// </summary>
    /// <returns></returns>
    public string GetRandomMouthStr()
    {
        int hairNumber = Random.Range(1, 12);
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
        if (name.IsNull())
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
        Sprite spData = GetSpriteByName(dicData, ref spriteData, atlasName, ProjectConfigInfo.ASSETBUNDLE_SPRITEATLAS, name, "Assets/Texture/SpriteAtlas/" + atlasName + ".spriteatlas");
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
    public Texture2D TryGetTexture(string name, int type)
    {
        if (name.IsNull())
            return null;
        if (dicTex.TryGetValue(name, out Texture2D value))
        {
            return value;
        }
        Sprite spData = null;
        switch (type)
        {
            case 1:
                spData = GetTrunkSpriteByName(name);
                break;
            case 2:
                spData = GetHairSpriteByName(name);
                break;
            case 3:
                spData = GetEyeSpriteByName(name);
                break;
            case 4:
                spData = GetMouthSpriteByName(name);
                break;
        }
        if (spData == null)
            return null;
        value = TextureUtil.SpriteToTexture2D(spData);
        dicTex.Add(name,value);
        return value;
    }
}
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
using System.Collections.Generic;

public class IconDataManager : BaseManager
{
    //UI图标
    public SpriteAtlas iconAtlas;

    public SpriteAtlas backgroundAtlas;

    public IconBeanDictionary dicIcon = new IconBeanDictionary();

    public IconBeanDictionary dicBackground = new IconBeanDictionary();

    public Dictionary<string,Texture2D> dicTextureUI = new Dictionary<string, Texture2D>();
    /// <summary>
    /// 根据名字获取UI图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetIconSpriteByName(string name)
    {
        return GetSpriteByName(dicIcon, ref iconAtlas, "AtlasForIcon", "sprite/icon", name);
    }

    public Sprite GetBackgroundSpriteByName(string name)
    {
        return GetSpriteByName(dicBackground, ref backgroundAtlas, "AtlasForBackground", "sprite/background", name);
    }

    public Texture2D GetTextureUIByName(string name)
    {
        return GetModel(dicTextureUI,"texture/ui", name);
    }
}
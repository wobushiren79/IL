using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
public class IconDataManager : BaseManager
{
    //UI图标
    public SpriteAtlas iconAtlas;

    public SpriteAtlas backgroundAtlas;

    public IconBeanDictionary dicIcon = new IconBeanDictionary();

    public IconBeanDictionary dicBackground = new IconBeanDictionary();

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
}
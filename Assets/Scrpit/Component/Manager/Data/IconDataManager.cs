using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
public class IconDataManager : BaseManager
{
    //UI图标
    //public IconBeanDictionary listIcon;
    public SpriteAtlas iconAtlas;
    
    /// <summary>
    /// 根据名字获取UI图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetIconSpriteByName(string name)
    {
        return GetSpriteByName(name, iconAtlas);
    }
}
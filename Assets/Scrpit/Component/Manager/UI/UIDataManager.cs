using UnityEngine;
using UnityEditor;

public class UIDataManager : BaseManager
{
    //UI图标
    public IconBeanDictionary listUIIcon;
    
    /// <summary>
    /// 根据名字获取UI图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetUISpriteByName(string name)
    {
        return GetSpriteByName(name, listUIIcon);
    }
}
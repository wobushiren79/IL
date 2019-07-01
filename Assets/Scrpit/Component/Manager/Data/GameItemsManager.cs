using UnityEngine;
using UnityEditor;

public class GameItemsManager : BaseManager
{
    public IconBeanDictionary listItemsIcon;

    /// <summary>
    /// 通过名字获取Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemsSpriteByName(string name)
    {
        return GetSpriteByName(name, listItemsIcon);
    }
}
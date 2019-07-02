using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseManager : BaseMonoBehaviour
{
    /// <summary>
    /// 根据名字获取图标
    /// </summary>
    /// <param name="name"></param>
    /// <param name="listdata"></param>
    /// <returns></returns>
    public virtual Sprite GetSpriteByName(string name, List<IconBean> listdata)
    {
        IconBean iconData = BeanUtil.GetIconBeanByName(name, listdata);
        if (iconData == null)
            return null;
        return iconData.value;
    }

    /// <summary>
    /// 根据位置获取图标
    /// </summary>
    /// <param name="positon"></param>
    /// <param name="listdata"></param>
    /// <returns></returns>
    public virtual Sprite GetSpriteByPosition(int position, List<IconBean> listdata) {
        IconBean iconData = BeanUtil.GetIconBeanByPosition(position, listdata);
        if (iconData == null)
            return null;
        return iconData.value;
    }

    /// <summary>
    /// 通过名字获取Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetSpriteByName(string name,IconBeanDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out Sprite spIcon))
            return spIcon;
        else
            return null;  
    }

}
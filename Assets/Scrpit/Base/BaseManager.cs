using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseManager : BaseMonoBehaviour
{
    /// <summary>
    /// 基础获取
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
}
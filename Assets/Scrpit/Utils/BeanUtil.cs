using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BeanUtil
{
    /// <summary>
    /// 获取IconBean
    /// </summary>
    /// <param name="name"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public static IconBean GetIconBeanByName(string name,List<IconBean> listData)
    {
        if (listData == null || name == null)
            return null;
        for(int i=0;i< listData.Count; i++)
        {
            IconBean itemIcon= listData[i];
            if (itemIcon.key.Equals(name))
            {
                return itemIcon;
            }
        }
        return null;
    }
}

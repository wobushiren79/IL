using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnAttributesBean
{
    public long aesthetics;//客栈美观数
    public long praise;//好评数
    public long richness;//菜品丰富度

    /// <summary>
    /// 刷新菜品数据
    /// </summary>
    /// <param name="listOwnMenu"></param>
    public void RefreshRichNess(List<MenuOwnBean> listOwnMenu)
    {
        //菜品丰富度数据刷新
        richness = 0;
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemData = listOwnMenu[i];
            if (itemData.isSell)
            {
                richness += 1;
            }
        } 
    }
}
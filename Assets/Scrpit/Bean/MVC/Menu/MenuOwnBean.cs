using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MenuOwnBean 
{
    public long menuId;
    public bool isSell=true;//是否开放售卖
    public long sellNumber;//销售数量

    public MenuOwnBean()
    {
    }

    public MenuOwnBean(long menuId)
    {
        this.menuId = menuId;
    }
}
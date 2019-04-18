using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class MenuOwnBean 
{
    public long menuId;

    public MenuOwnBean()
    {
    }

    public MenuOwnBean(long menuId)
    {
        this.menuId = menuId;
    }
}
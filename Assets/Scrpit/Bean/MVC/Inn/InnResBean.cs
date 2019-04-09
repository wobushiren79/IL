using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class InnResBean:BaseBean
{
    //物品名称
    public long resId;

    //物品坐标
    public float positionX;
    public float positionY;
}
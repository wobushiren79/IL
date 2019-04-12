using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnResBean:BaseBean
{
    //物品名称
    public long resId;
    //起始点
    public Vector3Bean startPosition;
    //物品占地
    public List<Vector3Bean> listPosition;
}
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnBuildBean
{
    //墙数据
    public List<InnResBean> listWall;
    //地板数据
    public List<InnResBean> listFloor;
    //家具数据
    public List<InnResBean> listFurniture;
}
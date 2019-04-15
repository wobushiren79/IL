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

    /// <summary>
    /// 增加家具
    /// </summary>
    /// <param name="innRes"></param>
    public void AddFurniture(InnResBean innRes)
    {
        if (innRes == null)
            return;
        if (listFurniture == null)
            listFurniture = new List<InnResBean>();
        listFurniture.Add(innRes);
    }

    /// <summary>
    /// 获取家具
    /// </summary>
    /// <param name="innRes"></param>
    public List<InnResBean> GetFurnitureList()
    {
        if (listFurniture == null)
            listFurniture = new List<InnResBean>();
        return listFurniture;
    }
}
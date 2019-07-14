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

    public int innWidth = 9;//客栈宽
    public int innHeight = 9;//客栈高

    public int buildLevel = 0;

    /// <summary>
    /// 初始化墙壁
    /// </summary>
    public void InitWall()
    {
        List<InnResBean> doorList = GetDoorList();
        if (listWall == null)
        {
            //没有墙时处理
            listWall = new List<InnResBean>();
        }
        else
        {
            //扩建时的处理
            //先删除不在范围内的
            for (int i = 0; i < listWall.Count; i++)
            {
                InnResBean itemData = listWall[i];
                if (itemData.startPosition.x <= innWidth - 2
                    && itemData.startPosition.x > 0
                     && itemData.startPosition.y > 0
                      && itemData.startPosition.y <= innHeight - 2)
                {
                    listWall.RemoveAt(i);
                    i--;
                }
            }
        }
        //建造墙壁
        for (int i = 0; i < innWidth; i++)
        {
            for (int f = 0; f < innHeight; f++)
            {
                bool isBuild = false;
                if (i == 0 || i == innHeight - 1)
                {
                    isBuild = true;
                }
                else
                {
                    if (f == 0 || f == innHeight - 1)
                    {
                        isBuild = true;
                    }
                }
                if (isBuild)
                {
                    bool isHas = false;
                    for (int h = 0; h < listWall.Count; h++)
                    {
                        InnResBean itemWall = listWall[i];
                        if (itemWall.startPosition.x == i && itemWall.startPosition.y == f)
                        {
                            isHas = true;
                            break;
                        }
                    }
                    if (!isHas)
                    {
                        InnResBean itemData = new InnResBean();
                        itemData.id = 20001;
                        itemData.startPosition = new Vector3Bean(i, f);
                        listWall.Add(itemData);
                    }
                }
            }
        }
        ///移除墙壁
        for (int i = 0; i < doorList.Count; i++)
        {
            InnResBean itemData = doorList[i];
            for (int f = 0; f < itemData.listPosition.Count; f++)
            {
                Vector3Bean doorPositon = itemData.listPosition[f];
                for (int h = 0; h < listWall.Count; h++)
                {
                    Vector3Bean wallPostion = listWall[h].startPosition;
                    if (doorPositon.x == wallPostion.x + 1 && doorPositon.y == wallPostion.y)
                    {
                        listWall.Remove(listWall[h]);
                        h--;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初始化地板
    /// </summary>
    public void InitFloor()
    {
        listFloor = new List<InnResBean>();
        for (int i = 0; i < innWidth; i++)
        {
            for (int f = 0; f < innHeight; f++)
            {
                InnResBean itemData = new InnResBean();
                itemData.id = 10001;
                itemData.startPosition = new Vector3Bean(i, f);
                listFloor.Add(itemData);
            }
        }
    }

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


    /// <summary>
    /// 获取门
    /// </summary>
    /// <returns></returns>
    public List<InnResBean> GetDoorList()
    {
        List<InnResBean> doorList = new List<InnResBean>();
        List<InnResBean> allData = GetFurnitureList();
        for (int i = 0; i < allData.Count; i++)
        {
            InnResBean itemData = allData[i];
            if (itemData.id >= 90000 && itemData.id < 100000)
            {
                doorList.Add(itemData);
            }
        }
        return doorList;
    }
}
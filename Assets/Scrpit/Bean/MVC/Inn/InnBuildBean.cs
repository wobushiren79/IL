using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnBuildBean
{
    //墙数据
    public List<InnResBean> listWall = new List<InnResBean>();
    //地板数据
    public List<InnResBean> listFloor = new List<InnResBean>();
    //家具数据
    public List<InnResBean> listFurniture = new List<InnResBean>();

    public int innWidth = 9;//客栈宽
    public int innHeight = 9;//客栈高

    public int buildLevel = 0;

    public List<TimeBean> listBuildDay = new List<TimeBean>();
    public int buildInnWidth = 0;
    public int buildInnHeight = 0;

    /// <summary>
    /// 改变客栈大小
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void ChangeInnSize(InnBuildManager innBuildManager, int width, int height)
    {
        this.innWidth = width;
        this.innHeight = height;
        InitWall(GetDoorList(innBuildManager));
        InitFloor();
    }
    public void ChangeInnSize(List<InnResBean> listDoor, int width, int height)
    {
        this.innWidth = width;
        this.innHeight = height;
        InitWall(listDoor);
        InitFloor();
    }
    /// <summary>
    /// 初始化墙壁
    /// </summary>
    public void InitWall(List<InnResBean> listDoor)
    {
        //建造墙壁
        for (int i = 0; i < innWidth; i++)
        {
            for (int f = 0; f < innHeight; f++)
            {
                bool isBuild = false;
                if (i == 0 || i == innWidth - 1)
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
                    bool hasData = false;
                    foreach (InnResBean itemWall in listWall)
                    {
                        Vector3 startPosition = itemWall.GetStartPosition();
                        if (startPosition.x == i && startPosition.y == f)
                        {
                            hasData = true;
                            break;
                        }
                    }
                    if (!hasData)
                    {
                        InnResBean itemData = new InnResBean();
                        itemData.startPosition = new Vector3Bean(i, f);
                        itemData.id = 20001;
                        listWall.Add(itemData);
                    }
                }
            }
        }
        ///移除门所占墙壁
        for (int i = 0; i < listDoor.Count; i++)
        {
            InnResBean itemData = listDoor[i];
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
        if (listFloor == null)
            listFloor = new List<InnResBean>();
        for (int i = 0; i < innWidth; i++)
        {
            for (int f = 0; f < innHeight; f++)
            {
                bool hasFloor = false;
                foreach (InnResBean innItem in listFloor)
                {
                    //如果已经有这个点的地板
                    if (innItem.startPosition.x == i && innItem.startPosition.y == f)
                    {
                        hasFloor = true;
                        break;
                    }
                }
                if (!hasFloor)
                {
                    InnResBean itemData = new InnResBean(10001, new Vector3(i, f), null, Direction2DEnum.Left);
                    listFloor.Add(itemData);
                }
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
    /// 通过坐标获取地板
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public InnResBean GetFloorByPosition(Vector3 position)
    {
        if (listFloor == null)
            return null;
        foreach (InnResBean itemData in listFloor)
        {
            if (itemData.GetStartPosition() == position)
            {
                return itemData;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过坐标获取墙体
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public InnResBean GetWallByPosition(Vector3 position)
    {
        if (listWall == null)
            return null;
        foreach (InnResBean itemData in listWall)
        {
            if (itemData.GetStartPosition() == position)
            {
                return itemData;
            }
        }
        return null;
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
    public List<InnResBean> GetDoorList(InnBuildManager innBuildManager)
    {
        List<InnResBean> doorList = new List<InnResBean>();
        List<InnResBean> allData = GetFurnitureList();
        for (int i = 0; i < allData.Count; i++)
        {
            InnResBean itemData = allData[i];
            BuildItemBean buildItemData = innBuildManager.GetBuildDataById(itemData.id);
            if (buildItemData.build_type == (int)BuildItemTypeEnum.Door)
            {
                doorList.Add(itemData);
            }
        }
        return doorList;
    }

    /// <summary>
    /// 获取所有的墙
    /// </summary>
    /// <returns></returns>
    public List<InnResBean> GetWallList()
    {
        if (listWall == null)
            listWall = new List<InnResBean>();
        return listWall;
    }

    /// <summary>
    /// 通过坐标获取家具
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public InnResBean GetFurnitureByPosition(Vector3 position)
    {
        if (listFurniture == null)
            return null;
        foreach (InnResBean itemData in listFurniture)
        {
            foreach (Vector3Bean itemPosition in itemData.listPosition)
            {
                if (itemPosition.x == position.x && itemPosition.y == position.y)
                {
                    return itemData;
                }
            }
        }
        return null;
    }

}
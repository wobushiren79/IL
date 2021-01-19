using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnBuildBean
{
    //墙数据
    public List<InnResBean> listWall = new List<InnResBean>();
    public List<InnResBean> listSecondWall = new List<InnResBean>();
    //地板数据
    public List<InnResBean> listFloor = new List<InnResBean>();
    public List<InnResBean> listSecondFloor = new List<InnResBean>();
    //家具数据
    public List<InnResBean> listFurniture = new List<InnResBean>();
    public List<InnResBean> listSecondFurniture = new List<InnResBean>();

    public int innWidth = 9;//客栈宽
    public int innHeight = 9;//客栈高

    public int innSecondWidth = 0; //2楼宽
    public int innSecondHeight = 0; //2楼高

    public int buildLevel = 0;
    public int buildSecondLevel = 0;

    public List<TimeBean> listBuildDay = new List<TimeBean>();

    public int buildInnWidth = 0;
    public int buildInnHeight = 0;

    public int buildInnSecondWidth = 0;
    public int buildInnSecondHeight = 0;

    /// <summary>
    /// 改变客栈大小
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void ChangeInnSize(int layer, InnBuildManager innBuildManager, int width, int height)
    {
        if (layer == 1)
        {
            this.innWidth = width;
            this.innHeight = height;
            InitWall(listWall, layer, GetDoorList(innBuildManager));
            InitFloor(listFloor, layer);
        }
        else if (layer == 2)
        {
            this.innSecondWidth = width;
            this.innSecondHeight = height;
            InitWall(listSecondWall, layer, null);
            InitFloor(listSecondFloor, layer);
        }

    }

    public void ChangeInnSize(int layer, List<InnResBean> listDoor, int width, int height)
    {
        if (layer == 1)
        {
            this.innWidth = width;
            this.innHeight = height;
            InitWall(listWall, layer, listDoor);
            InitFloor(listFloor, layer);
        }
        else if (layer == 2)
        {
            this.innSecondWidth = width;
            this.innSecondHeight = height;
            InitWall(listSecondWall, layer, null);
            InitFloor(listSecondFloor, layer);
        }
    }

    /// <summary>
    /// 初始化墙壁
    /// </summary>
    public void InitWall(List<InnResBean> listWall, int layer, List<InnResBean> listDoor)
    {
        GetInnSize(layer,out int innInitWidth, out int innInitHeight,out int offsetH);
        //建造墙壁
        for (int i = 0; i < innInitWidth; i++)
        {
            for (int f = 0; f < innInitWidth; f++)
            {
                bool isBuild = false;
                if (i == 0 || i == innInitWidth - 1)
                {
                    isBuild = true;
                }
                else
                {
                    if (f == 0 || f == innInitHeight - 1)
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
                        if (startPosition.x == i && startPosition.y == f + offsetH)
                        {
                            hasData = true;
                            break;
                        }
                    }
                    if (!hasData)
                    {
                        InnResBean itemData = new InnResBean();
                        itemData.startPosition = new Vector3Bean(i, f + offsetH);
                        itemData.id = 20001;
                        listWall.Add(itemData);
                    }
                }
            }
        }
        if (listDoor != null)
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
    public void InitFloor(List<InnResBean> listFloor, int layer)
    {
        if (listFloor == null)
            listFloor = new List<InnResBean>();
        GetInnSize(layer, out int innInitWidth, out int innInitHeight, out int offsetH);

        for (int i = 0; i < innInitWidth; i++)
        {
            for (int f = 0; f < innInitHeight; f++)
            {
                bool hasFloor = false;
                foreach (InnResBean innItem in listFloor)
                {
                    //如果已经有这个点的地板
                    if (innItem.startPosition.x == i && innItem.startPosition.y == f + offsetH)
                    {
                        hasFloor = true;
                        break;
                    }
                }
                if (!hasFloor)
                {
                    InnResBean itemData = new InnResBean(10001, new Vector3(i, f + offsetH), null, Direction2DEnum.Left);
                    listFloor.Add(itemData);
                }
            }
        }
    }


    /// <summary>
    /// 增加家具
    /// </summary>
    /// <param name="innRes"></param>
    public void AddFurniture(int layer, InnResBean innRes)
    {
        if (innRes == null)
            return;
        List<InnResBean> listData = GetFurnitureList(layer);
        if (listData == null)
            listData = new List<InnResBean>();
        listData.Add(innRes);
    }

    /// <summary>
    /// 通过坐标获取地板
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public InnResBean GetFloorByPosition(int layer, Vector3 position)
    {
        List<InnResBean> listData = null;
        if (layer == 1)
        {
            listData = listFloor;
        }
        else if (layer == 2)
        {
            listData = listSecondFloor;
        }
        if (listFloor == null)
            return null;
        foreach (InnResBean itemData in listData)
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
    public InnResBean GetWallByPosition(int layer, Vector3 position)
    {
        List<InnResBean> listData = GetWallList(layer);
        foreach (InnResBean itemData in listData)
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
    public List<InnResBean> GetFurnitureList(int layer)
    {
        if (layer == 1)
        {
            if (listFurniture == null)
                listFurniture = new List<InnResBean>();
            return listFurniture;
        }
        else if (layer == 2)
        {
            if (listSecondFurniture == null)
                listSecondFurniture = new List<InnResBean>();
            return listSecondFurniture;
        }
        return new List<InnResBean>();
    }

    /// <summary>
    /// 获取门
    /// </summary>
    /// <returns></returns>
    public List<InnResBean> GetDoorList(InnBuildManager innBuildManager)
    {
        List<InnResBean> doorList = new List<InnResBean>();
        List<InnResBean> allData = GetFurnitureList(1);
        for (int i = 0; i < allData.Count; i++)
        {
            InnResBean itemData = allData[i];
            BuildItemBean buildItemData = InnBuildHandler.Instance.manager.GetBuildDataById(itemData.id);
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
    public List<InnResBean> GetWallList(int layer)
    {
        if (layer == 1)
        {
            if (listWall == null)
                listWall = new List<InnResBean>();
            return listWall;
        }
        else if (layer == 2)
        {
            if (listSecondWall == null)
                listSecondWall = new List<InnResBean>();
            return listSecondWall;
        }
        return null;
    }
    /// <summary>
    /// 获取所有的地板
    /// </summary>
    /// <returns></returns>
    public List<InnResBean> GetFloorList(int layer)
    {
        if (layer == 1)
        {
            if (listFloor == null)
                listFloor = new List<InnResBean>();
            return listFloor;
        }
        else if (layer == 2)
        {
            if (listSecondFloor == null)
                listSecondFloor = new List<InnResBean>();
            return listSecondFloor;
        }
        return null;
    }

    /// <summary>
    /// 通过坐标获取家具
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public InnResBean GetFurnitureByPosition(int layer, Vector3 position)
    {
        List<InnResBean> listData = GetFurnitureList(layer);
        if (listData == null)
            return null;
        foreach (InnResBean itemData in listData)
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


    /// <summary>
    /// 获取客栈大小
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="innWidth"></param>
    /// <param name="innHeight"></param>
    /// <param name="offsetHeight"></param>
    public void GetInnSize(int layer, out int innWidth, out int innHeight, out int offsetHeight)
    {
        innWidth = 0;
        innHeight = 0;
        offsetHeight = (layer - 1) * 100;
        if (layer == 1)
        {
            innWidth = this.innWidth;
            innHeight = this.innHeight;
        }
        else if (layer == 2)
        {
            innWidth = this.innSecondWidth;
            innHeight = this.innSecondHeight;
        }
    }
}
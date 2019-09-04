using UnityEngine;
using UnityEditor;

public class BuildTableCpt : BaseBuildItemCpt
{
    public enum TableStatusEnum
    {
        Idle=0,//空闲
        Ready=1,//有人,等待移动到座位并且点餐
        WaitFood=2,//已经点餐，等待上菜
        Eating=3,//吃饭中
        WaitClean=4,//等待清理
        Cleaning=5,//清理
    }

    public TableStatusEnum tableStatus = TableStatusEnum.Idle;

    public GameObject leftSeat;
    public GameObject leftTable;

    public GameObject rightSeat;
    public GameObject rightTable;

    public GameObject upSeat;
    public GameObject upTable;

    public GameObject downSeat;
    public GameObject downTable;

    /// <summary>
    /// 获取座位坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSeatPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftSeat.transform.position;
            case Direction2DEnum.Right:
                return rightSeat.transform.position;
            case Direction2DEnum.UP:
                return upSeat.transform.position;
            case Direction2DEnum.Down:
                return downSeat.transform.position;
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 获取桌子坐标
    /// </summary>
    /// <returns></returns>
    public GameObject GetTable()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftTable;
            case Direction2DEnum.Right:
                return rightTable;
            case Direction2DEnum.UP:
                return upTable;
            case Direction2DEnum.Down:
                return downTable;
        }
        return null;
    }

    /// <summary>
    /// 获取桌子坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTablePosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftTable.transform.position;
            case Direction2DEnum.Right:
                return rightTable.transform.position;
            case Direction2DEnum.UP:
                return upTable.transform.position;
            case Direction2DEnum.Down:
                return downTable.transform.position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 设置桌子状态
    /// </summary>
    /// <param name="tableStatus"></param>
    public void SetTableStatus(TableStatusEnum tableStatus)
    {
        this.tableStatus = tableStatus;
    }

    /// <summary>
    /// 清理桌子
    /// </summary>
    public void CleanTable()
    {
        FoodForCustomerCpt food = GetTable().GetComponentInChildren<FoodForCustomerCpt>();
        if (food != null)
            //删除桌子上的食物
            Destroy(food.gameObject);
        SetTableStatus(TableStatusEnum.Idle);
    }

}
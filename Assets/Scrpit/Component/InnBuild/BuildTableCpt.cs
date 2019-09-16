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

    public GameObject objLeftChair;
    public GameObject objLeftSeat;

    public GameObject objRightChair;
    public GameObject objRightSeat;

    public GameObject objDownChair;
    public GameObject objDownSeat;

    public GameObject objUpChair;
    public GameObject objUpSeat;


    public GameObject tablePosition;


    public override void SetDirection(Direction2DEnum direction)
    {
        base.SetDirection(direction);
        objLeftChair.SetActive(false);
        objRightChair.SetActive(false);
        objDownChair.SetActive(false);
        objUpChair.SetActive(false);
        switch (direction)
        {
            case Direction2DEnum.Left:
                objLeftChair.SetActive(true);
                break;
            case Direction2DEnum.Right:
                objRightChair.SetActive(true);
                break;
            case Direction2DEnum.Down:
                objDownChair.SetActive(true);
                break;
            case Direction2DEnum.UP:
                objUpChair.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 获取座位坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSeatPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return objLeftSeat.transform.position;
            case Direction2DEnum.Right:
                return objRightSeat.transform.position;
            case Direction2DEnum.UP:
                return objUpSeat.transform.position;
            case Direction2DEnum.Down:
                return objDownSeat.transform.position;
        }
        return Vector3.zero;
    }


    /// <summary>
    /// 获取桌子
    /// </summary>
    /// <returns></returns>
    public GameObject GetTable()
    {
        return tablePosition;
    }

    /// <summary>
    /// 获取桌子坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTablePosition()
    {
        return tablePosition.transform.position;
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
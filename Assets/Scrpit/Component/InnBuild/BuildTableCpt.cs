using UnityEngine;
using UnityEditor;

public class BuildTableCpt : BaseBuildItemCpt
{
    public enum TableStateEnum
    {
        Idle=0,//空闲
        Ready=1,//有人,等待移动到座位并且点餐
        Wait=2,//已经点餐，等待上菜
        Eating=3,//吃饭中
        Cleaning=4,//清理
    }
    public TableStateEnum tableState= TableStateEnum.Idle;

    public GameObject leftSeat;
    public GameObject rightSeat;
    public GameObject upSeat;
    public GameObject downSeat;

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
}
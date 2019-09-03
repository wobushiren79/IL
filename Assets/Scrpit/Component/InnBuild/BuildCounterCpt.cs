using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildCounterCpt : BaseBuildItemCpt
{
    //操作者
    public NpcAIWorkerCpt workerCpt;

    public GameObject leftPayPosition;
    public GameObject leftAccountingPosition;

    public GameObject rightPayPosition;
    public GameObject rightAccountingPosition;

    public GameObject upPayPosition;
    public GameObject upAccountingPosition;

    public GameObject downPayPosition;
    public GameObject downAccountingPosition;

    //排队算账的订单
    public List<OrderForCustomer> payQueue = new List<OrderForCustomer>();

    /// <summary>
    /// 获取付款位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPayPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return GameUtil.GetTransformInsidePosition2D(leftPayPosition.transform);
            case Direction2DEnum.Right:
                return GameUtil.GetTransformInsidePosition2D(rightPayPosition.transform);
            case Direction2DEnum.UP:
                return GameUtil.GetTransformInsidePosition2D(upPayPosition.transform);
            case Direction2DEnum.Down:
                return GameUtil.GetTransformInsidePosition2D(downPayPosition.transform);
        }
        return transform.position;
    }

    /// <summary>
    /// 获取算账位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAccountingPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftAccountingPosition.transform.position;
            case Direction2DEnum.Right:
                return rightAccountingPosition.transform.position;
            case Direction2DEnum.UP:
                return upAccountingPosition.transform.position;
            case Direction2DEnum.Down:
                return downAccountingPosition.transform.position;
        }
        return transform.position;
    }


}
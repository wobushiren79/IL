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

    //排队算账的人
    public List<NpcAICustomerCpt> payQueue = new List<NpcAICustomerCpt>();

    /// <summary>
    /// 获取付款位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPayPosition()
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                return leftPayPosition.transform.position;
            case Direction2DEnum.Right:
                return rightPayPosition.transform.position;
            case Direction2DEnum.UP:
                return upPayPosition.transform.position;
            case Direction2DEnum.Down:
                return downPayPosition.transform.position;
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
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForWaiterBean : CharacterWorkerBaseBean
{
    //清理次数
    public long cleanTotalNumber;
    //送餐次数
    public long sendTotalNumber;

    /// <summary>
    /// 增加清理次数
    /// </summary>
    /// <param name="number"></param>
    public void AddCleanNumber(int number)
    {
        cleanTotalNumber += number;
    }

    /// <summary>
    /// 增加送餐次数
    /// </summary>
    /// <param name="number"></param>
    public void AddSendNumber(int number)
    {
        sendTotalNumber += number;
    }

}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum ShowConditionEnum
{
    InnLevel,
}

public class ShowConditionBean : DataBean<ShowConditionEnum>
{
    public int innLevel;
    public bool isCondition;

    public ShowConditionBean() : base(ShowConditionEnum.InnLevel, "")
    {

    }
}

public class ShowConditionTools : DataTools
{

    /// <summary>
    /// 检测是否全部满足
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckIsMeetAllCondition(GameDataBean gameData, string data)
    {
        List<ShowConditionBean> listConditionData = GetListConditionData(data);
        foreach (var itemConditionData in listConditionData)
        {
            GetConditionDetails(itemConditionData);
            if (!itemConditionData.isCondition)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取出现条件
    /// </summary>
    /// <returns></returns>
    public static List<ShowConditionBean> GetListConditionData(string data)
    {
        List<ShowConditionBean> listConditionData = GetListData<ShowConditionBean, ShowConditionEnum>(data);
        return listConditionData;
    }

    /// <summary>
    /// 获取出现详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static ShowConditionBean GetConditionDetails(ShowConditionBean conditionData)
    {
        switch (conditionData.dataType)
        {
            case ShowConditionEnum.InnLevel:
                break;
        }
        return conditionData;
    }


}
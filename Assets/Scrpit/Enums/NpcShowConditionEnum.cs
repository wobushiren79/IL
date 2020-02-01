using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum NpcShowConditionEnum
{
    NpcNumber,
    InnLevel,
}

public class NpcShowConditionBean : DataBean<NpcShowConditionEnum>
{
    //Npc人数
    public int npcNumber = 1;
    public int innLevel;

    public NpcShowConditionBean() : base(NpcShowConditionEnum.NpcNumber,"")
    {

    }
}

public class NpcShowConditionTools : DataTools
{
    /// <summary>
    /// 获取出现条件
    /// </summary>
    /// <returns></returns>
    public static List<NpcShowConditionBean> GetListConditionData(string data)
    {
        List<NpcShowConditionBean> listConditionData = GetListData<NpcShowConditionBean, NpcShowConditionEnum>(data);
        return listConditionData;
    }

    /// <summary>
    /// 获取出现详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static NpcShowConditionBean GetConditionDetails(NpcShowConditionBean conditionData)
    {
        switch (conditionData.dataType)
        {
            case NpcShowConditionEnum.NpcNumber:
                GetConditionDetailsForNpcNumber(conditionData);
                break;
        }
        return conditionData;
    }

    /// <summary>
    /// 获取条件 - NPC数量
    /// </summary>
    /// <param name="conditionData"></param>
    private static void GetConditionDetailsForNpcNumber(NpcShowConditionBean conditionData)
    {
        conditionData.npcNumber =int.Parse(conditionData.data);
    }
}
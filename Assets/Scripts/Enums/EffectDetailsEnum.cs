using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EffectDetailsEnum 
{
    EffectPSName,//粒子特效名字
    ImpactNumber,//作用人数
    ImpactType,//作用人群类型 0自己 1友方 2敌方 
    DurationForRound,//持续回合数
}

public class EffectDetailsBean : DataBean<EffectDetailsEnum>
{

    public EffectDetailsBean() : base(EffectDetailsEnum.ImpactNumber, "")
    {

    }

}


public class EffectDetailsEnumTools : DataTools
{

    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<EffectDetailsBean> GetListEffectData(string data)
    {
        return GetListData<EffectDetailsBean, EffectDetailsEnum>(data);
    }


    /// <summary>
    /// 获取效果作用范围
    /// </summary>
    /// <param name="data"></param>
    /// <param name="impactNumber"></param>
    /// <param name="impactType"></param>
    public static void GetEffectRange(string data, out int impactNumber, out int impactType)
    {
        impactNumber = 1;
        impactType = 1;
        List<EffectDetailsBean> listData = GetListEffectData(data);
        foreach (EffectDetailsBean effectTypeData in listData)
        {
            if (effectTypeData.dataType == EffectDetailsEnum.ImpactNumber)
            {
                impactNumber = int.Parse(effectTypeData.data);
            }
            else if (effectTypeData.dataType == EffectDetailsEnum.ImpactType)
            {
                impactType = int.Parse(effectTypeData.data);
            }
        }
    }

    /// <summary>
    /// 获取战斗粒子详情
    /// </summary>
    /// <param name="data"></param>
    /// <param name="effectPSName"></param>
    /// <param name="durationForRound"></param>
    public static void GetEffectDetailsForCombat(string data,out string effectPSName, out int durationForRound)
    {
        effectPSName = "";
        durationForRound = 0;

        List<EffectDetailsBean> listData = GetListEffectData(data);
        foreach (EffectDetailsBean effectTypeData in listData)
        {
            if (effectTypeData.dataType == EffectDetailsEnum.EffectPSName)
            {
                effectPSName = effectTypeData.data;
            }
            else if (effectTypeData.dataType == EffectDetailsEnum.DurationForRound)
            {
                durationForRound = int.Parse(effectTypeData.data);
            }
        }
    }
}

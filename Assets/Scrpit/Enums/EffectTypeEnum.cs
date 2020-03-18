using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EffectTypeEnum 
{
    AddLife,//增加生命值
    Def,//增加防御力
    DefRate,//增加防御百分比
    Damage,//直接伤害
}

public class EffectTypeBean : DataBean<EffectTypeEnum>
{
    //说明
    public string effectDescribe;
    //图标
    public Sprite spIcon;
    //具体数值
    public float effectData;

    public EffectTypeBean() : base(EffectTypeEnum.AddLife, "")
    {

    }

}
public class EffectTypeEnumTools : DataTools
{
    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<EffectTypeBean> GetListEffectData(string data)
    {
        return GetListData<EffectTypeBean, EffectTypeEnum>(data);
    }

    /// <summary>
    /// 获取前置详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static EffectTypeBean GetEffectDetails(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.AddLife:
                effectTypeData = GetEffectDetailsForAddLife(iconDataManager,effectTypeData);
                break;
            case EffectTypeEnum.DefRate:
                effectTypeData = GetEffectDetailsForAddDef(iconDataManager, effectTypeData);
                break;
        }
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加生命的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForAddLife(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(501) , effectTypeData.data) ;
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_addlife_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加防御力的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForAddDef(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        float defRate = float.Parse(effectTypeData.data);
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(503), (defRate*100)+"%");
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("defend_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取所有效果的伤害加成
    /// </summary>
    /// <param name="listData"></param>
    public static int GetEffectDamageRate(List<EffectTypeBean> listData,int damage)
    {
        float damageRate = 1;
        float damageAdd = 0;
        foreach (EffectTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case EffectTypeEnum.Def:
                    damageAdd -= int.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DefRate:
                    damageRate -= float.Parse(itemData.data);
                    break;
        
            }
        }
        damage = (int)((damage + damageAdd) * damageRate);
        if (damage < 0)
            damage = 0;
        return damage; 
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EffectTypeEnum 
{
    AddLife,//增加生命值
    AddSpeed,//增加速度
    AddForce,//增加武力
    Def,//增加防御力
    DefRate,//增加防御百分比

    Damage,//直接伤害
    DamageRate,//直接伤害百分比
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
            case EffectTypeEnum.AddForce:
                effectTypeData = GetEffectDetailsForAddForce(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.AddSpeed:
                effectTypeData = GetEffectDetailsForAddSpeed(iconDataManager, effectTypeData);
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
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_defend_1");
        return effectTypeData;
    }
    private static EffectTypeBean GetEffectDetailsForAddSpeed(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(505), effectTypeData.data);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_speed_1");
        return effectTypeData;
    }
    private static EffectTypeBean GetEffectDetailsForAddForce(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(506), effectTypeData.data);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_force_1");
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

    /// <summary>
    /// 获取所有武力加成效果
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public static int GetEffectForceRate(List<EffectTypeBean> listData,int force)
    {
        float forceRate = 1;
        float forceAdd = 0;
        foreach (EffectTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case EffectTypeEnum.AddForce:
                    forceAdd += int.Parse(itemData.data);
                    break;
            }
        }
        force = (int)((force + forceAdd) * forceRate);
        if (force < 0)
            force = 0;
        return force;
    }

    /// <summary>
    /// 获取所有武力加成效果
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public static int GetEffectSpeedRate(List<EffectTypeBean> listData, int speed)
    {
        float speedRate = 1;
        float speedAdd = 0;
        foreach (EffectTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case EffectTypeEnum.AddSpeed:
                    speedAdd += int.Parse(itemData.data);
                    break;
            }
        }
        speed = (int)((speed + speedAdd) * speedRate);
        if (speed < 0)
            speed = 0;
        return speed;
    }

}
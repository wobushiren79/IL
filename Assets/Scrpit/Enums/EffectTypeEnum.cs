using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EffectTypeEnum 
{
    AddLife,//增加生命值
    AddSpeed,//增加速度
    AddForce,//增加武力

    SubSpeed,//减少速度
    SubForce,//减少速度

    Def,//增加防御力
    DefRate,//增加防御百分比

    Damage= 10001,//直接伤害
    DamageRate = 10002,//直接伤害百分比
    DamageRateForForce = 10003,//武力百分比加成
    DamageRateForSpeed = 10004,//速度百分比加成
}

public class EffectTypeBean : DataBean<EffectTypeEnum>
{
    //说明
    public string effectDescribe;
    //图标
    public Sprite spIcon;
    //图标颜色
    public Color colorIcon;
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
            case EffectTypeEnum.SubSpeed:
                effectTypeData = GetEffectDetailsForSubSpeed(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.SubForce:
                effectTypeData = GetEffectDetailsForSubForce(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.DefRate:
                effectTypeData = GetEffectDetailsForAddDef(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.Damage:
                effectTypeData = GetEffectDetailsForDamage(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.DamageRateForForce:
                effectTypeData = GetEffectDetailsForDamageRateForForce(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.DamageRateForSpeed:
                effectTypeData = GetEffectDetailsForDamageRateForSpeed(iconDataManager, effectTypeData);
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

    private static EffectTypeBean GetEffectDetailsForSubSpeed(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(531), effectTypeData.data);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_sub_speed_1");
        return effectTypeData;
    }
    private static EffectTypeBean GetEffectDetailsForSubForce(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(532), effectTypeData.data);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_sub_force_1");
        return effectTypeData;
    }
    
    private static EffectTypeBean GetEffectDetailsForDamage(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = int.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(504), effectTypeData.data);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_features_favorability");
        effectTypeData.colorIcon = Color.red;
        return effectTypeData;
    }

    private static EffectTypeBean GetEffectDetailsForDamageRateForForce(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(521), effectTypeData.data);
        effectTypeData.colorIcon = Color.red;
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_ability_force");
        return effectTypeData;
    }
    private static EffectTypeBean GetEffectDetailsForDamageRateForSpeed(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(522), effectTypeData.data);
        effectTypeData.colorIcon = Color.red;
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_ability_speed");
        return effectTypeData;
    }

    /// <summary>
    /// 获取角色造成的总伤害
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="actionCharacterData"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public static int GetTotalDamage(GameItemsManager gameItemsManager, CharacterBean actionCharacterData, List<EffectTypeBean> listData)
    {
        float damageAdd = 0;
        float damageAddRate = 1;
        actionCharacterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        foreach (EffectTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case EffectTypeEnum.Damage:
                    damageAdd +=  float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRate:
                    damageAddRate += float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForForce:
                    damageAdd += characterAttributes.force * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForSpeed:
                    damageAdd += characterAttributes.speed * float.Parse(itemData.data);
                    break;
            }
        }
        return (int)Mathf.Round(damageAdd * damageAddRate);
    }

    /// <summary>
    /// 获取增加的总生命值
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="actionCharacterData"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public static int GetTotalLife(GameItemsManager gameItemsManager, CharacterBean actionCharacterData, List<EffectTypeBean> listData)
    {
        float lifeAdd = 0;
        float lifeAddRate = 1;
        actionCharacterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        foreach (EffectTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case EffectTypeEnum.AddLife:
                    lifeAdd += float.Parse(itemData.data);
                    break;
            }
        }
        return (int)Mathf.Round(lifeAdd * lifeAddRate);
    }

    
    /// <summary>
    /// 获取所有效果的伤害加成
    /// </summary>
    /// <param name="listData"></param>
    public static int GetTotalDef(GameItemsManager gameItemsManager, CharacterBean characterData,  List<EffectTypeBean> listData,int damage)
    {
        float damageRate = 1;
        float damageAdd = 0;
        characterData.GetAttributes(gameItemsManager,out CharacterAttributesBean characterAttributes);
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
    public static int GetTotalForce(List<EffectTypeBean> listData,int force)
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
                case EffectTypeEnum.SubForce:
                    forceAdd -= int.Parse(itemData.data);
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
    public static int GetTotalSpeed(List<EffectTypeBean> listData, int speed)
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
                case EffectTypeEnum.SubSpeed:
                    speedAdd -= int.Parse(itemData.data);
                    break;
            }
        }
        speed = (int)((speed + speedAdd) * speedRate);
        if (speed < 0)
            speed = 0;
        return speed;
    }

    /// <summary>
    /// 检测是否是延迟执行的效果（用于延迟播放粒子特效）
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    public static bool CheckIsDelay(EffectTypeBean effectTypeData)
    {
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.AddLife:
                return true;
            case EffectTypeEnum.Damage:
                return true;
            case EffectTypeEnum.DamageRate:
                return true;
            case EffectTypeEnum.DamageRateForForce:
                return true;
            case EffectTypeEnum.DamageRateForSpeed:
                return true;
        }
        return false;
    }
}
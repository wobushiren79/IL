using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EffectTypeEnum 
{
    AddLife,//增加生命值
    AddLifeRate,//增加生命百分比

    AddLucky,
    AddCook,//增加厨
    AddSpeed,//增加速度
    AddAccount,//增加算
    AddCharm,//增加魅力
    AddForce,//增加武力

    AddLuckyRate,
    AddCookRate,
    AddSpeedRate,
    AddAccountRate,
    AddCharmRate,
    AddForceRate,

    SubLucky,
    SubCook,
    SubSpeed,
    SubAccount,
    SubCharm,
    SubForce,

    SubLuckyRate,
    SubCookRate,
    SubSpeedRate,
    SubAccountRate,
    SubCharmRate,
    SubForceRate,

    Def,//增加防御力
    DefRate,//增加防御百分比

    Damage ,//直接伤害
    DamageRate ,//直接伤害百分比
    DamageRateForLucky,
    DamageRateForCook,
    DamageRateForSpeed ,//速度百分比加成
    DamageRateForAccount,
    DamageRateForCharm,
    DamageRateForForce,//武力百分比加成
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
            case EffectTypeEnum.AddLifeRate:
                effectTypeData = GetEffectDetailsForAddLifeRate(iconDataManager, effectTypeData);
                break;

            case EffectTypeEnum.AddLucky:
            case EffectTypeEnum.AddCook:
            case EffectTypeEnum.AddSpeed:
            case EffectTypeEnum.AddAccount:
            case EffectTypeEnum.AddCharm:
            case EffectTypeEnum.AddForce:
                effectTypeData = GetEffectDetailsForAddAttributes(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.AddLuckyRate:
            case EffectTypeEnum.AddCookRate:
            case EffectTypeEnum.AddSpeedRate:
            case EffectTypeEnum.AddAccountRate:
            case EffectTypeEnum.AddCharmRate:
            case EffectTypeEnum.AddForceRate:
                effectTypeData = GetEffectDetailsForAddAttributesRate(iconDataManager, effectTypeData);
                break;

            case EffectTypeEnum.SubLucky:
            case EffectTypeEnum.SubCook:
            case EffectTypeEnum.SubSpeed:
            case EffectTypeEnum.SubAccount:
            case EffectTypeEnum.SubCharm:
            case EffectTypeEnum.SubForce:
                effectTypeData = GetEffectDetailsForSubAttributes(iconDataManager, effectTypeData);
                break;

            case EffectTypeEnum.SubLuckyRate:
            case EffectTypeEnum.SubCookRate:
            case EffectTypeEnum.SubSpeedRate:
            case EffectTypeEnum.SubAccountRate:
            case EffectTypeEnum.SubCharmRate:
            case EffectTypeEnum.SubForceRate:
                effectTypeData = GetEffectDetailsForSubAttributesRate(iconDataManager, effectTypeData);
                break;

            case EffectTypeEnum.DefRate:
                effectTypeData = GetEffectDetailsForAddDef(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.Damage:
                effectTypeData = GetEffectDetailsForDamage(iconDataManager, effectTypeData);
                break;
            case EffectTypeEnum.DamageRateForLucky:
            case EffectTypeEnum.DamageRateForCook:
            case EffectTypeEnum.DamageRateForSpeed:
            case EffectTypeEnum.DamageRateForAccount:
            case EffectTypeEnum.DamageRateForCharm:
            case EffectTypeEnum.DamageRateForForce:
                effectTypeData = GetEffectDetailsForDamageRateForAttributes(iconDataManager, effectTypeData);
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
    /// 获取增加生命的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForAddLifeRate(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        float addLifeRate = float.Parse(effectTypeData.data);
        effectTypeData.effectData = addLifeRate;
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(507), (addLifeRate * 100) + "%");
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
        effectTypeData.effectData = defRate;
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(503), (defRate*100)+"%");
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName("ui_effect_defend_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForAddAttributes(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.AddLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_lucky_1";
                break;
            case EffectTypeEnum.AddCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_cook_1";
                break;
            case EffectTypeEnum.AddSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_speed_1";
                break;
            case EffectTypeEnum.AddAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_account_1";
                break;
            case EffectTypeEnum.AddCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_charm_1";
                break;
            case EffectTypeEnum.AddForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_force_1";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(505), effectTypeData.data, attributesName);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }
    /// <summary>
    /// 获取增加属性百分比相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForAddAttributesRate(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.AddLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_lucky_2";
                break;
            case EffectTypeEnum.AddCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_cook_2";
                break;
            case EffectTypeEnum.AddSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_speed_2";
                break;
            case EffectTypeEnum.AddAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_account_2";
                break;
            case EffectTypeEnum.AddCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_charm_2";
                break;
            case EffectTypeEnum.AddForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_force_2";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(506), effectTypeData.data, attributesName);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }


    /// <summary>
    /// 获取减少属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForSubAttributes(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.SubLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_sub_lucky_1";
                break;
            case EffectTypeEnum.SubCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_sub_cook_1";
                break;
            case EffectTypeEnum.SubSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_sub_speed_1";
                break;
            case EffectTypeEnum.SubAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_sub_account_1";
                break;
            case EffectTypeEnum.SubCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_sub_charm_1";
                break;
            case EffectTypeEnum.SubForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_sub_force_1";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(531), effectTypeData.data, attributesName);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }

    /// <summary>
    /// 获取减少属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static EffectTypeBean GetEffectDetailsForSubAttributesRate(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.SubLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_sub_lucky_2";
                break;
            case EffectTypeEnum.SubCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_sub_cook_2";
                break;
            case EffectTypeEnum.SubSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_sub_speed_2";
                break;
            case EffectTypeEnum.SubAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_sub_account_2";
                break;
            case EffectTypeEnum.SubCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_sub_charm_2";
                break;
            case EffectTypeEnum.SubForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_sub_force_2";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(532), effectTypeData.data, attributesName);
        effectTypeData.spIcon = iconDataManager.GetIconSpriteByName(iconStr);
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

    private static EffectTypeBean GetEffectDetailsForDamageRateForAttributes(IconDataManager iconDataManager, EffectTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.DamageRateForLucky:
                attributesType = AttributesTypeEnum.Lucky;
                break;
            case EffectTypeEnum.DamageRateForCook:
                attributesType = AttributesTypeEnum.Cook;
                break;
            case EffectTypeEnum.DamageRateForSpeed:
                attributesType = AttributesTypeEnum.Speed;
                break;
            case EffectTypeEnum.DamageRateForAccount:
                attributesType = AttributesTypeEnum.Account;
                break;
            case EffectTypeEnum.DamageRateForCharm:
                attributesType = AttributesTypeEnum.Charm;
                break;
            case EffectTypeEnum.DamageRateForForce:
                attributesType = AttributesTypeEnum.Force;
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attibutesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(GameCommonInfo.GetUITextById(521), attibutesName, effectTypeData.data);
        effectTypeData.colorIcon = Color.red;
        effectTypeData.spIcon = AttributesTypeEnumTools.GetAttributesIcon(iconDataManager, attributesType);
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
                case EffectTypeEnum.DamageRateForLucky:
                    damageAdd += characterAttributes.lucky * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForCook:
                    damageAdd += characterAttributes.cook * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForSpeed:
                    damageAdd += characterAttributes.speed * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForAccount:
                    damageAdd += characterAttributes.account * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForCharm:
                    damageAdd += characterAttributes.charm * float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.DamageRateForForce:
                    damageAdd += characterAttributes.force * float.Parse(itemData.data);
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
                case EffectTypeEnum.AddLifeRate:
                    lifeAddRate += float.Parse(itemData.data);
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
                case EffectTypeEnum.AddForceRate:
                    forceRate+= float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.SubForce:
                    forceAdd -= int.Parse(itemData.data);
                    break;
                case EffectTypeEnum.SubForceRate:
                   forceRate -= float.Parse(itemData.data);
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
                case EffectTypeEnum.AddSpeedRate:
                    speedRate += float.Parse(itemData.data);
                    break;
                case EffectTypeEnum.SubSpeed:
                    speedAdd -= int.Parse(itemData.data);
                    break;
                case EffectTypeEnum.SubSpeedRate:
                    speedRate -= float.Parse(itemData.data);
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
            case EffectTypeEnum.AddLifeRate:
            case EffectTypeEnum.Damage:
            case EffectTypeEnum.DamageRate:
            case EffectTypeEnum.DamageRateForForce:
            case EffectTypeEnum.DamageRateForSpeed:
                return true;
        }
        return false;
    }
}
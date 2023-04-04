using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum BufferTypeEnum
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

    Damage,//直接伤害
    DamageRate,//直接伤害百分比
    DamageRateForLucky,
    DamageRateForCook,
    DamageRateForSpeed,//速度百分比加成
    DamageRateForAccount,
    DamageRateForCharm,
    DamageRateForForce,//武力百分比加成
}

public class BufferTypeBean : DataBean<BufferTypeEnum>
{
    //说明
    public string effectDescribe;
    //图标
    public Sprite spIcon;
    public Sprite spIconRemark;
    //图标颜色
    public Color colorIcon = Color.white;
    //具体数值
    public float effectData;

    public BufferTypeBean() : base(BufferTypeEnum.AddLife, "")
    {

    }

}
public class BufferTypeEnumTools : DataTools
{
    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<BufferTypeBean> GetListEffectData(string data)
    {
        return GetListData<BufferTypeBean, BufferTypeEnum>(data);
    }

    /// <summary>
    /// 获取前置详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static BufferTypeBean GetEffectDetails(BufferTypeBean effectTypeData, Sprite spRemark)
    {
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.AddLife:
                effectTypeData = GetEffectDetailsForAddLife(effectTypeData);
                break;
            case BufferTypeEnum.AddLifeRate:
                effectTypeData = GetEffectDetailsForAddLifeRate(effectTypeData);
                break;

            case BufferTypeEnum.AddLucky:
            case BufferTypeEnum.AddCook:
            case BufferTypeEnum.AddSpeed:
            case BufferTypeEnum.AddAccount:
            case BufferTypeEnum.AddCharm:
            case BufferTypeEnum.AddForce:
                effectTypeData = GetEffectDetailsForAddAttributes(effectTypeData);
                break;
            case BufferTypeEnum.AddLuckyRate:
            case BufferTypeEnum.AddCookRate:
            case BufferTypeEnum.AddSpeedRate:
            case BufferTypeEnum.AddAccountRate:
            case BufferTypeEnum.AddCharmRate:
            case BufferTypeEnum.AddForceRate:
                effectTypeData = GetEffectDetailsForAddAttributesRate(effectTypeData);
                break;

            case BufferTypeEnum.SubLucky:
            case BufferTypeEnum.SubCook:
            case BufferTypeEnum.SubSpeed:
            case BufferTypeEnum.SubAccount:
            case BufferTypeEnum.SubCharm:
            case BufferTypeEnum.SubForce:
                effectTypeData = GetEffectDetailsForSubAttributes(effectTypeData);
                break;

            case BufferTypeEnum.SubLuckyRate:
            case BufferTypeEnum.SubCookRate:
            case BufferTypeEnum.SubSpeedRate:
            case BufferTypeEnum.SubAccountRate:
            case BufferTypeEnum.SubCharmRate:
            case BufferTypeEnum.SubForceRate:
                effectTypeData = GetEffectDetailsForSubAttributesRate(effectTypeData);
                break;

            case BufferTypeEnum.DefRate:
                effectTypeData = GetEffectDetailsForAddDef(effectTypeData);
                break;
            case BufferTypeEnum.Damage:
                effectTypeData = GetEffectDetailsForDamage(effectTypeData, spRemark);
                break;
            case BufferTypeEnum.DamageRateForLucky:
            case BufferTypeEnum.DamageRateForCook:
            case BufferTypeEnum.DamageRateForSpeed:
            case BufferTypeEnum.DamageRateForAccount:
            case BufferTypeEnum.DamageRateForCharm:
            case BufferTypeEnum.DamageRateForForce:
                effectTypeData = GetEffectDetailsForDamageRateForAttributes(effectTypeData, spRemark);
                break;
        }
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加生命的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForAddLife(BufferTypeBean effectTypeData)
    {
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(501), effectTypeData.data);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName("ui_effect_addlife_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加生命的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForAddLifeRate(BufferTypeBean effectTypeData)
    {
        float addLifeRate = float.Parse(effectTypeData.data);
        effectTypeData.effectData = addLifeRate;
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(507), (addLifeRate * 100) + "");
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName("ui_effect_addlife_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加防御力的相关数据
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForAddDef(BufferTypeBean effectTypeData)
    {
        float defRate = float.Parse(effectTypeData.data);
        effectTypeData.effectData = defRate;
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(503), (defRate * 100) + "%");
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName("ui_effect_defend_1");
        return effectTypeData;
    }

    /// <summary>
    /// 获取增加属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForAddAttributes(BufferTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.AddLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_lucky_1";
                break;
            case BufferTypeEnum.AddCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_cook_1";
                break;
            case BufferTypeEnum.AddSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_speed_1";
                break;
            case BufferTypeEnum.AddAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_account_1";
                break;
            case BufferTypeEnum.AddCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_charm_1";
                break;
            case BufferTypeEnum.AddForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_force_1";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(505), effectTypeData.data, attributesName);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }
    /// <summary>
    /// 获取增加属性百分比相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForAddAttributesRate(BufferTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.AddLuckyRate:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_lucky_2";
                break;
            case BufferTypeEnum.AddCookRate:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_cook_2";
                break;
            case BufferTypeEnum.AddSpeedRate:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_speed_2";
                break;
            case BufferTypeEnum.AddAccountRate:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_account_2";
                break;
            case BufferTypeEnum.AddCharmRate:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_charm_2";
                break;
            case BufferTypeEnum.AddForceRate:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_force_2";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(506), effectTypeData.effectData * 100 + "", attributesName);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }


    /// <summary>
    /// 获取减少属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForSubAttributes(BufferTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.SubLucky:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_sub_lucky_1";
                break;
            case BufferTypeEnum.SubCook:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_sub_cook_1";
                break;
            case BufferTypeEnum.SubSpeed:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_sub_speed_1";
                break;
            case BufferTypeEnum.SubAccount:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_sub_account_1";
                break;
            case BufferTypeEnum.SubCharm:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_sub_charm_1";
                break;
            case BufferTypeEnum.SubForce:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_sub_force_1";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(531), effectTypeData.data, attributesName);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }

    /// <summary>
    /// 获取减少属性相关数据
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    private static BufferTypeBean GetEffectDetailsForSubAttributesRate(BufferTypeBean effectTypeData)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        string iconStr = "";
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.SubLuckyRate:
                attributesType = AttributesTypeEnum.Lucky;
                iconStr = "ui_effect_sub_lucky_2";
                break;
            case BufferTypeEnum.SubCookRate:
                attributesType = AttributesTypeEnum.Cook;
                iconStr = "ui_effect_sub_cook_2";
                break;
            case BufferTypeEnum.SubSpeedRate:
                attributesType = AttributesTypeEnum.Speed;
                iconStr = "ui_effect_sub_speed_2";
                break;
            case BufferTypeEnum.SubAccountRate:
                attributesType = AttributesTypeEnum.Account;
                iconStr = "ui_effect_sub_account_2";
                break;
            case BufferTypeEnum.SubCharmRate:
                attributesType = AttributesTypeEnum.Charm;
                iconStr = "ui_effect_sub_charm_2";
                break;
            case BufferTypeEnum.SubForceRate:
                attributesType = AttributesTypeEnum.Force;
                iconStr = "ui_effect_sub_force_2";
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attributesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(532), effectTypeData.effectData * 100 + "", attributesName);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName(iconStr);
        return effectTypeData;
    }

    private static BufferTypeBean GetEffectDetailsForDamage(BufferTypeBean effectTypeData, Sprite spRemark)
    {
        effectTypeData.effectData = int.Parse(effectTypeData.data);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(504), effectTypeData.data);
        effectTypeData.spIcon = IconHandler.Instance.GetIconSpriteByName("ui_features_favorability");
        effectTypeData.spIconRemark = spRemark;
        effectTypeData.colorIcon = Color.red;
        return effectTypeData;
    }

    private static BufferTypeBean GetEffectDetailsForDamageRateForAttributes(BufferTypeBean effectTypeData, Sprite spRemark)
    {
        AttributesTypeEnum attributesType = AttributesTypeEnum.Null;
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.DamageRateForLucky:
                attributesType = AttributesTypeEnum.Lucky;
                break;
            case BufferTypeEnum.DamageRateForCook:
                attributesType = AttributesTypeEnum.Cook;
                break;
            case BufferTypeEnum.DamageRateForSpeed:
                attributesType = AttributesTypeEnum.Speed;
                break;
            case BufferTypeEnum.DamageRateForAccount:
                attributesType = AttributesTypeEnum.Account;
                break;
            case BufferTypeEnum.DamageRateForCharm:
                attributesType = AttributesTypeEnum.Charm;
                break;
            case BufferTypeEnum.DamageRateForForce:
                attributesType = AttributesTypeEnum.Force;
                break;
        }
        effectTypeData.effectData = float.Parse(effectTypeData.data);
        string attibutesName = AttributesTypeEnumTools.GetAttributesName(attributesType);
        effectTypeData.effectDescribe = string.Format(TextHandler.Instance.manager.GetTextById(521), attibutesName, effectTypeData.data);
        effectTypeData.colorIcon = Color.red;
        effectTypeData.spIconRemark = spRemark;
        effectTypeData.spIcon = AttributesTypeEnumTools.GetAttributesIcon(attributesType);
        return effectTypeData;
    }

    /// <summary>
    /// 获取角色造成的总伤害
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="actionCharacterData"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public static int GetTotalDamage(NpcAIMiniGameCombatCpt actionNpc, List<BufferTypeBean> listData)
    {
        float damageAdd = 0;
        float damageAddRate = 1;
        actionNpc.characterData.GetAttributes(actionNpc.characterMiniGameData, out CharacterAttributesBean characterAttributes);
        foreach (BufferTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case BufferTypeEnum.Damage:
                    damageAdd += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRate:
                    damageAddRate += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForLucky:
                    damageAdd += characterAttributes.lucky * float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForCook:
                    damageAdd += characterAttributes.cook * float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForSpeed:
                    damageAdd += characterAttributes.speed * float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForAccount:
                    damageAdd += characterAttributes.account * float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForCharm:
                    damageAdd += characterAttributes.charm * float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DamageRateForForce:
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
    public static int GetTotalLife(CharacterBean actionCharacterData, List<BufferTypeBean> listData)
    {
        float lifeAdd = 0;
        float lifeAddRate = 0;
        actionCharacterData.GetAttributes(out CharacterAttributesBean characterAttributes);
        foreach (BufferTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case BufferTypeEnum.AddLife:
                    lifeAdd += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddLifeRate:
                    lifeAddRate += float.Parse(itemData.data);
                    break;
            }
        }
        return (int)Mathf.Round(lifeAdd + characterAttributes.life * lifeAddRate);
    }

    /// <summary>
    /// 获取所有效果的伤害加成
    /// </summary>
    /// <param name="listData"></param>
    public static int GetTotalDef(CharacterBean characterData, List<BufferTypeBean> listData, int damage)
    {
        float damageRate = 1;
        float damageAdd = 0;
        characterData.GetAttributes(out CharacterAttributesBean characterAttributes);
        foreach (BufferTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case BufferTypeEnum.Def:
                    damageAdd -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.DefRate:
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
    public static CharacterAttributesBean GetTotalAttributes(List<BufferTypeBean> listData, CharacterAttributesBean addAttributes)
    {
        float attibutesAddRateForLucky = 1;
        float attibutesAddForLucky = 0;

        float attibutesAddRateForCook = 1;
        float attibutesAddForCook = 0;

        float attibutesAddRateForSpeed = 1;
        float attibutesAddForSpeed = 0;

        float attibutesAddRateForAccount = 1;
        float attibutesAddForAccount = 0;

        float attibutesAddRateForCharm = 1;
        float attibutesAddForCharm = 0;

        float attibutesAddRateForForce = 1;
        float attibutesAddForForce = 0;
        foreach (BufferTypeBean itemData in listData)
        {
            switch (itemData.dataType)
            {
                case BufferTypeEnum.AddLucky:
                    attibutesAddForForce += int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddCook:
                    attibutesAddForCook += int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddSpeed:
                    attibutesAddForSpeed += int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddAccount:
                    attibutesAddForAccount += int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddCharm:
                    attibutesAddForCharm += int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddForce:
                    attibutesAddForForce += int.Parse(itemData.data);
                    break;
                //------------------------------------------------------------------------------
                case BufferTypeEnum.AddLuckyRate:
                    attibutesAddRateForLucky += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddCookRate:
                    attibutesAddRateForCook += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddSpeedRate:
                    attibutesAddRateForSpeed += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddAccountRate:
                    attibutesAddRateForAccount += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddCharmRate:
                    attibutesAddRateForCharm += float.Parse(itemData.data);
                    break;
                case BufferTypeEnum.AddForceRate:
                    attibutesAddRateForForce += float.Parse(itemData.data);
                    break;
                //------------------------------------------------------------------------------
                case BufferTypeEnum.SubLucky:
                    attibutesAddForForce -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.SubCook:
                    attibutesAddForCook -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.SubSpeed:
                    attibutesAddForSpeed -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.SubAccount:
                    attibutesAddForAccount -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.SubCharm:
                    attibutesAddForCharm -= int.Parse(itemData.data);
                    break;
                case BufferTypeEnum.SubForce:
                    attibutesAddForForce -= int.Parse(itemData.data);
                    break;
                //------------------------------------------------------------------------------
                case BufferTypeEnum.SubLuckyRate:
                    attibutesAddRateForLucky -= float.Parse(itemData.data);
                    if (attibutesAddRateForLucky < 0)
                        attibutesAddRateForLucky = 0;
                    break;
                case BufferTypeEnum.SubCookRate:
                    attibutesAddRateForCook -= float.Parse(itemData.data);
                    if (attibutesAddRateForCook < 0)
                        attibutesAddRateForCook = 0;
                    break;
                case BufferTypeEnum.SubSpeedRate:
                    attibutesAddRateForSpeed -= float.Parse(itemData.data);
                    if (attibutesAddRateForSpeed < 0)
                        attibutesAddRateForSpeed = 0;
                    break;
                case BufferTypeEnum.SubAccountRate:
                    attibutesAddRateForAccount -= float.Parse(itemData.data);
                    if (attibutesAddRateForAccount < 0)
                        attibutesAddRateForAccount = 0;
                    break;
                case BufferTypeEnum.SubCharmRate:
                    attibutesAddRateForCharm -= float.Parse(itemData.data);
                    if (attibutesAddRateForCharm < 0)
                        attibutesAddRateForCharm = 0;
                    break;
                case BufferTypeEnum.SubForceRate:
                    attibutesAddRateForForce -= float.Parse(itemData.data);
                    if (attibutesAddRateForForce < 0)
                        attibutesAddRateForForce = 0;
                    break;
            }
        }
        addAttributes.lucky = (int)((addAttributes.lucky + attibutesAddForLucky) * attibutesAddRateForLucky);
        addAttributes.cook = (int)((addAttributes.cook + attibutesAddForCook) * attibutesAddRateForCook);
        addAttributes.speed = (int)((addAttributes.speed + attibutesAddForSpeed) * attibutesAddRateForSpeed);
        addAttributes.account = (int)((addAttributes.account + attibutesAddForAccount) * attibutesAddRateForAccount);
        addAttributes.charm = (int)((addAttributes.charm + attibutesAddForCharm) * attibutesAddRateForCharm);
        addAttributes.force = (int)((addAttributes.force + attibutesAddForForce) * attibutesAddRateForForce);
        return addAttributes;
    }

    /// <summary>
    /// 检测是否是延迟执行的效果（用于延迟播放粒子特效）
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <returns></returns>
    public static bool CheckIsDelay(BufferTypeBean effectTypeData)
    {
        switch (effectTypeData.dataType)
        {
            case BufferTypeEnum.AddLife:
            case BufferTypeEnum.AddLifeRate:
            case BufferTypeEnum.Damage:
            case BufferTypeEnum.DamageRate:
            case BufferTypeEnum.DamageRateForLucky:
            case BufferTypeEnum.DamageRateForCook:
            case BufferTypeEnum.DamageRateForSpeed:
            case BufferTypeEnum.DamageRateForAccount:
            case BufferTypeEnum.DamageRateForCharm:
            case BufferTypeEnum.DamageRateForForce:
                return true;
        }
        return false;
    }
}
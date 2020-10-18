using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class NpcInfoBean : BaseBean
{
    public long npc_id;//npcId
    public int npc_type;//0默认NPC，
    public int sex;//性别
    public int face;//面向 1.左边 2右边

    public long mask_id;
    public long hand_id;
    public long hat_id;
    public long clothes_id;
    public long shoes_id;

    public string hair_id;
    public string hair_color;
    public string eye_id;
    public string eye_color;
    public string mouth_id;
    public string mouth_color;
    
    public string skin_color;


    public string title_name;//称号
    public string name;//npc名字

    public float position_x;
    public float position_y;

    public string talk_types;//人物对话ID
    public string skill_ids;

    public int attributes_loyal;
    public int attributes_life;
    public int attributes_cook;
    public int attributes_speed;
    public int attributes_account;
    public int attributes_charm;
    public int attributes_force;
    public int attributes_lucky;
    public int wage_l;
    public int wage_m;
    public int wage_s;
    //喜欢的东西
    public string love_items;
    //喜欢的菜单
    public string love_menus;
    //出现条件
    public string condition;
    //备注
    public string remark;
    /// <summary>
    /// 获取NPC类型
    /// </summary>
    /// <returns></returns>
    public NpcTypeEnum GetNpcType()
    {
        return (NpcTypeEnum)npc_type;
    }

    /// <summary>
    /// NPC信息转为角色信息
    /// </summary>
    /// <param name="npcInfo"></param>
    /// <returns></returns>
    public static CharacterBean NpcInfoToCharacterData(NpcInfoBean npcInfo)
    {
        CharacterBean characterData = new CharacterBean();
        characterData.baseInfo.characterType = npcInfo.npc_type;
        characterData.baseInfo.characterId = npcInfo.id + "";
        characterData.baseInfo.titleName = npcInfo.title_name;
        characterData.baseInfo.name = npcInfo.name;
        characterData.baseInfo.priceL = npcInfo.wage_l;
        characterData.baseInfo.priceM = npcInfo.wage_m;
        characterData.baseInfo.priceS = npcInfo.wage_s;
        //设置最喜欢的东西
        if (!CheckUtil.StringIsNull(npcInfo.love_items))
            characterData.baseInfo.listLoveItems = StringUtil.SplitBySubstringForArrayLong(npcInfo.love_items, ',').ToList();
        characterData.body = new CharacterBodyBean();
        characterData.body.hair = npcInfo.hair_id;
        //设置头发颜色
        ColorBean hairColor = new ColorBean(npcInfo.hair_color);
        if (hairColor != null)
            characterData.body.hairColor = hairColor;
        characterData.body.eye = npcInfo.eye_id;
        //设置眼睛颜色
        ColorBean eyeColor = new ColorBean(npcInfo.eye_color);
        if (eyeColor != null)
            characterData.body.eyeColor = eyeColor;
        characterData.body.mouth = npcInfo.mouth_id;
        //设置嘴巴颜色
        ColorBean mouthColor = new ColorBean(npcInfo.mouth_color);
        if (mouthColor != null)
            characterData.body.mouthColor = mouthColor;
        characterData.body.sex = npcInfo.sex;
        characterData.body.face = npcInfo.face;
        //设置皮肤颜色
        ColorBean skinColor = new ColorBean(npcInfo.skin_color);
        if (skinColor != null)
            characterData.body.skinColor = skinColor;
        //设置装备
        characterData.equips = new CharacterEquipBean();
        characterData.equips.maskId = npcInfo.mask_id;
        characterData.equips.handId = npcInfo.hand_id;
        characterData.equips.hatId = npcInfo.hat_id;
        characterData.equips.clothesId = npcInfo.clothes_id;
        characterData.equips.shoesId = npcInfo.shoes_id;
        characterData.equips.maskId = npcInfo.mask_id;

        //设置属性
        characterData.attributes = new CharacterAttributesBean();
        characterData.attributes.loyal = npcInfo.attributes_loyal;
        characterData.attributes.life = npcInfo.attributes_life;
        characterData.attributes.cook = npcInfo.attributes_cook;
        characterData.attributes.speed = npcInfo.attributes_speed;
        characterData.attributes.account = npcInfo.attributes_account;
        characterData.attributes.charm = npcInfo.attributes_charm;
        characterData.attributes.force = npcInfo.attributes_force;
        characterData.attributes.lucky = npcInfo.attributes_lucky;
        //设置技能
        characterData.attributes.listSkills = npcInfo.GetSkillIds();

        characterData.npcInfoData = npcInfo;
        return characterData;
    }

    /// <summary>
    /// 获取喜欢的菜品ID
    /// </summary>
    public List<long> GetLoveMenus()
    {
        long[] menusId = StringUtil.SplitBySubstringForArrayLong(love_menus, ',');
        return menusId.ToList();
    }

    /// <summary>
    /// 获取技能ID
    /// </summary>
    /// <returns></returns>
    public List<long> GetSkillIds() {
        long[] skillIds = StringUtil.SplitBySubstringForArrayLong(skill_ids, ',');
        return skillIds.ToList();
    }

    /// <summary>
    /// 获取对话的选项
    /// </summary>
    /// <returns></returns>
    public List<NpcTalkTypeEnum> GetTalkTypes()
    {
        NpcTalkTypeEnum[] talkTypes = StringUtil.SplitBySubstringForArrayEnum<NpcTalkTypeEnum>(talk_types, ',');
        return talkTypes.ToList();
    }
}
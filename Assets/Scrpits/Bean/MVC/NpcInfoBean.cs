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
    public int marry_status;
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
    /// 获取喜欢的菜品ID
    /// </summary>
    public List<long> GetLoveMenus()
    {
        long[] menusId = love_menus.SplitForArrayLong(',');
        return menusId.ToList();
    }

    /// <summary>
    /// 获取技能ID
    /// </summary>
    /// <returns></returns>
    public List<long> GetSkillIds() {
        long[] skillIds = skill_ids.SplitForArrayLong(',');
        return skillIds.ToList();
    }

    /// <summary>
    /// 获取对话的选项
    /// </summary>
    /// <returns></returns>
    public List<NpcTalkTypeEnum> GetTalkTypes()
    {
        NpcTalkTypeEnum[] talkTypes = talk_types.SplitForArrayEnum<NpcTalkTypeEnum>(',');
        return talkTypes.ToList();
    }

    /// <summary>
    /// 检测是否能结婚
    /// </summary>
    /// <returns></returns>
    public bool CheckCanMarry()
    {
        if (marry_status == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
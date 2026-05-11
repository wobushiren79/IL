using System;
using System.Linq;
using System.Collections.Generic;

public partial class NpcInfoBean
{
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
public partial class NpcInfoCfg
{
}

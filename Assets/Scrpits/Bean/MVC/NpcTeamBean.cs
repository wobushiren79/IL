using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class NpcTeamBean : BaseBean
{
    public long team_id;
    public int team_type;
    public string team_leader;
    public string team_members;
    public int team_number;
    public string condition;//出现条件
    public string talk_ids;//对话IDS
    public string shout_ids;//喊话的ID
    public string name;
    public string love_menus;//喜欢的菜单
    public float effect_time;//效果时间

    /// <summary>
    /// 获取小队所有成员信息
    /// </summary>
    /// <param name="npcInfoManager"></param>
    /// <returns></returns>
    public void GetTeamCharacterData(out List<CharacterBean> listLeader, out List<CharacterBean> listMembers)
    {
        listLeader = new List<CharacterBean>();
        listMembers = new List<CharacterBean>();
        foreach (long itemId in GetTeamLeaderId())
        {
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemId);
            if (characterData != null)
                listLeader.Add(characterData);
        }
        foreach (long itemId in GetTeamMembersId())
        {
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemId);
            if (characterData != null)
                listMembers.Add(characterData);
        }
    }

    public long[] GetTalkIds()
    {
        if (talk_ids == null)
            return new long[0];
        return talk_ids.SplitForArrayLong(',');
    }

    public long[] GetShoutIds()
    {
        if (shout_ids == null)
            return new long[0];
        return shout_ids.SplitForArrayLong(',');
    }

    public long[] GetTeamLeaderId()
    {
        if (team_leader == null)
            return new long[0];
        return team_leader.SplitForArrayLong(',');
    }

    public long[] GetTeamMembersId()
    {
        if (team_members == null)
            return new long[0];
        return team_members.SplitForArrayLong(',');
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
    /// 获取团队类型
    /// </summary>
    /// <returns></returns>
    public NpcTeamTypeEnum GetTeamType()
    {
        return (NpcTeamTypeEnum)team_type;
    }
}
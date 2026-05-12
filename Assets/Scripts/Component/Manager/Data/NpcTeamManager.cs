using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NpcTeamManager : BaseManager
{
    public List<NpcTeamBean> listCustomerTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listFriendTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listRascalTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listSundryTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listEntertainTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listDisappointedTeam = new List<NpcTeamBean>();
    public List<NpcTeamBean> listInfiniteTowersBossTeam = new List<NpcTeamBean>();

    private void Awake()
    {
        LoadAllTeamData();
    }

    private void LoadAllTeamData()
    {
        listCustomerTeam.Clear();
        listFriendTeam.Clear();
        listRascalTeam.Clear();
        listSundryTeam.Clear();
        listEntertainTeam.Clear();
        listDisappointedTeam.Clear();
        listInfiniteTowersBossTeam.Clear();

        var dicData = NpcTeamCfg.GetAllData();
        if (dicData == null)
            return;
        foreach (var item in dicData)
        {
            NpcTeamBean team = item.Value;
            if (team.valid != 1)
                continue;
            switch (team.GetTeamType())
            {
                case NpcTeamTypeEnum.Customer:
                    listCustomerTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.Friend:
                    listFriendTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.Rascal:
                    listRascalTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.Sundry:
                    listSundryTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.Entertain:
                    listEntertainTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.Disappointed:
                    listDisappointedTeam.Add(team);
                    break;
                case NpcTeamTypeEnum.InfiniteTowersBoss:
                    listInfiniteTowersBossTeam.Add(team);
                    break;
            }
        }
    }

    /// <summary>
    /// 随机获取满足出现条件的顾客队伍
    /// </summary>
    public List<NpcTeamBean> GetRandomTeamMeetConditionByType(NpcTeamTypeEnum npcTeamType, GameDataBean gameData)
    {
        List<NpcTeamBean> listData = null;
        switch (npcTeamType)
        {
            case NpcTeamTypeEnum.Customer:
                listData = listCustomerTeam;
                break;
            case NpcTeamTypeEnum.Friend:
                listData = listFriendTeam;
                break;
            case NpcTeamTypeEnum.Rascal:
                listData = listRascalTeam;
                break;
            case NpcTeamTypeEnum.Sundry:
                listData = listSundryTeam;
                break;
            case NpcTeamTypeEnum.Entertain:
                listData = listEntertainTeam;
                break;
            case NpcTeamTypeEnum.Disappointed:
                listData = listDisappointedTeam;
                break;
            case NpcTeamTypeEnum.InfiniteTowersBoss:
                listData = listInfiniteTowersBossTeam;
                break;
        }
        if (listData == null)
            return null;
        List<NpcTeamBean> listMeetData = GetMeetConditionTeam(gameData, listData);
        return listMeetData;
    }

    /// <summary>
    /// 根据ID获取顾客队伍
    /// </summary>
    public NpcTeamBean GetCustomerTeam(long teamId)
    {
        return GetTeam(teamId, listCustomerTeam);
    }
    public List<NpcTeamBean> GetCustomerTeam()
    {
        return listCustomerTeam;
    }

    /// <summary>
    /// 根据ID获取好友队伍
    /// </summary>
    public NpcTeamBean GetFriendTeam(long teamId)
    {
        return GetTeam(teamId, listFriendTeam);
    }

    /// <summary>
    /// 获取捣乱者队伍
    /// </summary>
    public NpcTeamBean GetRascalTeam(long teamId)
    {
        return GetTeam(teamId, listRascalTeam);
    }

    /// <summary>
    /// 获取杂项队伍
    /// </summary>
    public NpcTeamBean GetSundryTeam(long teamId)
    {
        return GetTeam(teamId, listSundryTeam);
    }

    /// <summary>
    /// 获取无尽之塔BOSS队伍
    /// </summary>
    public NpcTeamBean GetInfiniteTowerBossTeam(long teamId)
    {
        return GetTeam(teamId, listInfiniteTowersBossTeam);
    }

    /// <summary>
    /// 获取转换者队伍
    /// </summary>
    public NpcTeamBean GetConvertTeam(long teamId)
    {
        List<NpcTeamBean> listData = new List<NpcTeamBean>();
        listData.AddRange(listEntertainTeam);
        listData.AddRange(listDisappointedTeam);
        return GetTeam(teamId, listData);
    }

    /// <summary>
    /// 根据ID获取队伍
    /// </summary>
    public NpcTeamBean GetTeam(long teamId, List<NpcTeamBean> listData)
    {
        foreach (NpcTeamBean itemTeam in listData)
        {
            if (itemTeam.id == teamId)
            {
                return itemTeam;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取满足出现条件队伍
    /// </summary>
    public List<NpcTeamBean> GetMeetConditionTeam(GameDataBean gameData, List<NpcTeamBean> listData)
    {
        List<NpcTeamBean> listMeet = new List<NpcTeamBean>();
        foreach (NpcTeamBean itemTeam in listData)
        {
            if (itemTeam.condition.IsNull() || ShowConditionTools.CheckIsMeetAllCondition(gameData, itemTeam.condition))
            {
                listMeet.Add(itemTeam);
            }
        }
        return listMeet;
    }
}

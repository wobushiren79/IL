using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcTeamManager : BaseManager, INpcTeamView
{
    public NpcTeamController npcTeamController;

    public List<NpcTeamBean> listCustomerTeam;
    public List<NpcTeamBean> listFriendTeam;
    public List<NpcTeamBean> listRascalTeam;
    public List<NpcTeamBean> listSundryTeam;

    private void Awake()
    {
        npcTeamController = new NpcTeamController(this, this);
    }

    /// <summary>
    /// 随机获取满足出现条件的顾客队伍
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public NpcTeamBean GetRandomTeamMeetConditionByType(NpcTeamTypeEnum npcTeamType, GameDataBean gameData)
    {
        List<NpcTeamBean> listData =null;
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

        }
        if (listData == null)
            return null;
        List<NpcTeamBean> listMeetData = GetMeetConditionTeam(gameData, listData);
        return RandomUtil.GetRandomDataByList(listMeetData);
    }

    /// <summary>
    /// 根据ID获取顾客队伍
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public NpcTeamBean GetCustomerTeam(long teamId)
    {
        return GetTeam(teamId, listCustomerTeam);
    }

    /// <summary>
    ///  根据ID获取好友队伍
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public NpcTeamBean GetFriendTeam(long teamId)
    {
        return GetTeam(teamId, listFriendTeam);
    }

    /// <summary>
    /// 获取捣乱者队伍
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public NpcTeamBean GetRascalTeam(long teamId)
    {
        return GetTeam(teamId, listRascalTeam);
    }

    /// <summary>
    /// 获取杂项队伍
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public NpcTeamBean GetSundryTeam(long teamId)
    {
        return GetTeam(teamId, listSundryTeam);
    }

    /// <summary>
    /// 根据ID获取队伍
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public NpcTeamBean GetTeam(long teamId, List<NpcTeamBean> listData)
    {
        foreach (NpcTeamBean itemTeam in listData)
        {
            if (itemTeam.id==teamId)
            {
                return itemTeam;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取满足出现条件队伍
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="listData"></param>
    /// <returns></returns>
    public List<NpcTeamBean> GetMeetConditionTeam(GameDataBean gameData,  List<NpcTeamBean> listData)
    {
        List<NpcTeamBean> listMeet = new List<NpcTeamBean>();
        foreach (NpcTeamBean itemTeam in listData)
        {
            if (CheckUtil.StringIsNull(itemTeam.condition)&& ShowConditionTools.CheckIsMeetAllCondition(gameData, itemTeam.condition))
            {
                listMeet.Add(itemTeam);
            }
        }
        return listMeet;
    }

    #region 数据回调
    public void GetNpcTeamSuccess(NpcTeamTypeEnum npcTeam, List<NpcTeamBean> listData)
    {
        switch (npcTeam)
        {
            case NpcTeamTypeEnum.Customer:
                listCustomerTeam = listData;
                break;
            case NpcTeamTypeEnum.Friend:
                listFriendTeam = listData;
                break;
            case NpcTeamTypeEnum.Rascal:
                listRascalTeam = listData;
                break;
            case NpcTeamTypeEnum.Sundry:
                listSundryTeam = listData;
                break;
        }
    }

    public void GetNpcTeamFail()
    {

    }
    #endregion
}
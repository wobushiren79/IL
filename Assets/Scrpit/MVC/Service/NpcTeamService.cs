﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcTeamService : BaseMVCService
{
    public NpcTeamService() : base("npc_team", "npc_team_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 根据Id查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<NpcTeamBean> QueryDataById(long[] teamId)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(teamId, ",");
        return BaseQueryData<NpcTeamBean>("team_id", tableNameForMain + ".valid", "=", "1", tableNameForMain + ".id", "in", "(" + values + ")");
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public List<NpcTeamBean> QueryDataByType(int teamType)
    {
        return BaseQueryData<NpcTeamBean>("team_id", tableNameForMain + ".valid", "1", tableNameForMain + ".team_type", teamType + "");
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<NpcTeamBean> QueryAllData()
    {
        return BaseQueryAllData<NpcTeamBean>("team_id");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="teamId"></param>
    public bool DeleteDataById(long teamId)
    {
        return BaseDeleteDataWithLeft("id", "team_id", "" + teamId);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="teamData"></param>
    public void Update(NpcTeamBean teamData)
    {
        if (DeleteDataById(teamData.id))
        {
            InsertData(teamData);
        }
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="teamData"></param>
    public void InsertData(NpcTeamBean teamData)
    {
        List<string> listLeftName = new List<string>
        {
            "team_id",
            "name"
        };
        BaseInsertDataWithLeft(teamData, listLeftName);
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcTeamService : BaseMVCService
{
    public NpcTeamService() : base("npc_team", "npc_team_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<NpcTeamBean> QueryDataByType(int teamType)
    {
        return BaseQueryData<NpcTeamBean>("team_id", tableNameForMain + ".valid", "1", tableNameForMain + ".team_type", teamType + "");
    }
}
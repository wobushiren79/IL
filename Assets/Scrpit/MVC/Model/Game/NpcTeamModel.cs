using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcTeamModel : BaseMVCModel
{
    protected NpcTeamService npcTeamService;

    public override void InitData()
    {
        npcTeamService = new NpcTeamService();
    }

    public List<NpcTeamBean> GetNpcTeamByType(NpcTeamTypeEnum npcTeamType)
    {
        return npcTeamService.QueryDataByType((int)npcTeamType);
    }
    public List<NpcTeamBean> GetAllNpcTeam()
    {
        return npcTeamService.QueryAllData();
    }
}
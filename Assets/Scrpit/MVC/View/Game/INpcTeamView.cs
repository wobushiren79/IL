using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface INpcTeamView
{
    void GetNpcTeamSuccess(NpcTeamTypeEnum npcTeam, List<NpcTeamBean> listData);

    void GetNpcTeamFail();
}
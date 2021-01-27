using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface INpcTeamView
{
    void GetNpcTeamSuccess(NpcTeamTypeEnum npcTeam, List<NpcTeamBean> listData, Action<List<NpcTeamBean>> action);

    void GetAllNpcTeamSuccess(List<NpcTeamBean> listData, Action<List<NpcTeamBean>> action);

    void GetNpcTeamFail();
}
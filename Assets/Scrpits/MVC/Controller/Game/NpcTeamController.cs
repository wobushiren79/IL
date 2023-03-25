using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NpcTeamController : BaseMVCController<NpcTeamModel, INpcTeamView>
{

    public NpcTeamController(BaseMonoBehaviour content, INpcTeamView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 通过类型 获取NPC队伍
    /// </summary>
    /// <param name="npcTeamType"></param>
    public void GetNpcTeamByType(NpcTeamTypeEnum npcTeamType, Action<List<NpcTeamBean>> action)
    {
        List<NpcTeamBean> listData = GetModel().GetNpcTeamByType(npcTeamType);
        if (listData.IsNull())
        {
            GetView().GetNpcTeamFail();
        }
        else
        {
            GetView().GetNpcTeamSuccess(npcTeamType, listData, action);
        }
    }

    public void GetAllNpcTeam( Action<List<NpcTeamBean>> action)
    {
        List<NpcTeamBean> listData = GetModel().GetAllNpcTeam();
        if (listData.IsNull())
        {
            GetView().GetNpcTeamFail();
        }
        else
        {
            GetView().GetAllNpcTeamSuccess(listData, action);
        }
    }
}
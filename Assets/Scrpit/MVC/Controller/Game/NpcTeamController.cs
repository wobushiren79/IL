using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
    public void GetNpcTeamByType(NpcTeamTypeEnum npcTeamType)
    {
        List<NpcTeamBean> listData = GetModel().GetNpcTeamByType(npcTeamType);
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetNpcTeamFail();
        }
        else
        {
            GetView().GetNpcTeamSuccess(npcTeamType, listData);
        }
    }
}
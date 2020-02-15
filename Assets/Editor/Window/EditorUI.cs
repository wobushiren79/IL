using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EditorUI
{

    /// <summary>
    /// 团队创建 UI
    /// </summary>
    public static void GUICreateNpcTeam(NpcTeamBean npcTeamData, NpcTeamService npcTeamService)
    {
        GUILayout.Label("Npc团队创建", GUILayout.Width(100), GUILayout.Height(20));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcTeamService.InsertData(npcTeamData);
        }
        GUILayout.EndHorizontal();

        GUINpcTeam(npcTeamData);
    }

    /// <summary>
    /// 团队查询 UI
    /// </summary>
    public static List<NpcTeamBean> GUIFindNpcTeam(NpcTeamService npcTeamService, List<NpcTeamBean> listFindData)
    {
        GUILayout.Label("Npc团队查询", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("查询顾客团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Customer);
        }
        if (GUILayout.Button("查询好友团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Friend);
        }
        if (GUILayout.Button("查询捣乱团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Rascal);
        }
        GUILayout.EndHorizontal();
        if (listFindData != null)
        {
            NpcTeamBean itemRemoveData = null;
            foreach (NpcTeamBean itemData in listFindData)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    npcTeamService.Update(itemData);
                }
                if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    npcTeamService.DeleteDataById(itemData.id);
                    itemRemoveData = itemData;
                }
                GUILayout.EndHorizontal();
                GUINpcTeam(itemData);
                GUILayout.Space(20);
            }
            if (itemRemoveData != null)
                listFindData.Remove(itemRemoveData);
        }
        return listFindData;
    }

    /// <summary>
    /// 团队Iteam UI
    /// </summary>
    public static void GUINpcTeam(NpcTeamBean npcTeamData)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("团队类型 " + npcTeamData.team_type, GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_type = (int)(NpcTeamTypeEnum)EditorGUILayout.EnumPopup((NpcTeamTypeEnum)npcTeamData.team_type, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("团队名称", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.name = EditorGUILayout.TextArea(npcTeamData.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("团队ID", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.id = long.Parse(EditorGUILayout.TextArea(npcTeamData.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcTeamData.team_id = npcTeamData.id;
        GUILayout.Label("团队领袖IDs(,)", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_leader = EditorGUILayout.TextArea(npcTeamData.team_leader + "", GUILayout.Width(200), GUILayout.Height(20));
        GUILayout.Label("团队成员IDs(,)", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.team_members = EditorGUILayout.TextArea(npcTeamData.team_members + "", GUILayout.Width(200), GUILayout.Height(20));
        switch ((NpcTeamTypeEnum)npcTeamData.team_type)
        {
            case NpcTeamTypeEnum.Customer:
                GUILayout.Label("成员数量最大值", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.team_number = int.Parse(EditorGUILayout.TextArea(npcTeamData.team_number + "", GUILayout.Width(50), GUILayout.Height(20)));
                break;
            case NpcTeamTypeEnum.Friend:
                break;
            case NpcTeamTypeEnum.Rascal:
                GUILayout.Label("对话markId", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.talk_ids = EditorGUILayout.TextArea(npcTeamData.talk_ids + "", GUILayout.Width(200), GUILayout.Height(20));
                break;
        }

        GUILayout.Label("是否启用", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.valid = (int)(ValidEnum)EditorGUILayout.EnumPopup((ValidEnum)npcTeamData.valid, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.EndHorizontal();
    }
}
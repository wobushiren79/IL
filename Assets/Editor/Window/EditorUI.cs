using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class EditorUI
{
    /// <summary>
    /// NPC创建
    /// </summary>
    public static void GUINpcInfoCreate(
        NpcInfoService npcInfoService, GameItemsManager gameItemsManager,
        GameObject objNpcContainer, GameObject objNpcModel,
        NpcInfoBean npcInfo)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
        {
            CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(npcInfo);
            ShowNpc(gameItemsManager, objNpcContainer, objNpcModel, characterData);
        }
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcInfo.valid = 1;
            npcInfo.face = 1;
            npcInfoService.InsertData(npcInfo);
        }
        GUILayout.EndHorizontal();
        GUINpcInfoItem(gameItemsManager, objNpcContainer, npcInfo);
    }

    /// <summary>
    /// NPC查询
    /// </summary>
    public static void GUINpcInfoFind(
        NpcInfoService npcInfoService,
        GameItemsManager gameItemsManager,
        GameObject objNpcContainer, GameObject objNpcModel,
        string findIdsStr, List<NpcInfoBean> listNpcDataForFind,
        out string outFindIdStr, out List<NpcInfoBean> outListNpcDataForFind)
    {
        GUILayout.Label("查询NPC", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPCId", GUILayout.Width(100), GUILayout.Height(20));
        findIdsStr = EditorGUILayout.TextArea(findIdsStr + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIdsStr, ',');
            listNpcDataForFind = npcInfoService.QueryDataByIds(ids);
        }
        if (GUILayout.Button("查询全部", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryAllData();
        }
        if (GUILayout.Button("查询路人NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Passerby);
        }
        if (GUILayout.Button("查询小镇NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Town);
        }
        if (GUILayout.Button("查询特殊NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Special);
        }
        if (GUILayout.Button("查询小镇可招募NPC", GUILayout.Width(120), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.RecruitTown);
        }
        if (GUILayout.Button("查询团队顾客", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.GuestTeam);
        }
        if (GUILayout.Button("查询其他NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoService.QueryDataByType((int)NpcTypeEnum.Other);
        }

        GUILayout.EndHorizontal();
        foreach (NpcInfoBean itemData in listNpcDataForFind)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
            {
                CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(itemData);
                ShowNpc(gameItemsManager, objNpcContainer, objNpcModel, characterData);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.Update(itemData);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.DeleteData(itemData);
                listNpcDataForFind.Remove(itemData);
            }
            GUILayout.EndHorizontal();
            GUINpcInfoItem(gameItemsManager, objNpcContainer, itemData);
        }
        outFindIdStr = findIdsStr;
        outListNpcDataForFind = listNpcDataForFind;
    }

    /// <summary>
    /// Npc Item
    /// </summary>
    public static void GUINpcInfoItem(GameItemsManager gameItemsManager, GameObject objNpcContainer, NpcInfoBean npcInfo)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("获取场景位置数据", GUILayout.Width(120), GUILayout.Height(20)))
        {
            BaseNpcAI npcAI = objNpcContainer.GetComponentInChildren<BaseNpcAI>();
            npcInfo.position_x = npcAI.transform.position.x;
            npcInfo.position_y = npcAI.transform.position.y;
        }
        GUILayout.Label("NPCID：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.id = long.Parse(EditorGUILayout.TextArea(npcInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcInfo.npc_id = npcInfo.id;
        npcInfo.npc_type = (int)GUIEnum<NpcTypeEnum>("Npc类型：", npcInfo.npc_type);
        NpcTypeEnum npcType = (NpcTypeEnum)npcInfo.npc_type;
        if (npcType != NpcTypeEnum.Passerby)
        {
            GUILayout.Label("姓名：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.name = EditorGUILayout.TextArea(npcInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("性别：1男 2女", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.sex = int.Parse(EditorGUILayout.TextArea(npcInfo.sex + "", GUILayout.Width(30), GUILayout.Height(20)));
            GUILayout.Label("称号：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.title_name = EditorGUILayout.TextArea(npcInfo.title_name + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("朝向 1左 2右", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.face = int.Parse(EditorGUILayout.TextArea(npcInfo.face + "", GUILayout.Width(30), GUILayout.Height(20)));
            GUILayout.Label("位置XY：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.position_x = float.Parse(EditorGUILayout.TextArea(npcInfo.position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
            npcInfo.position_y = float.Parse(EditorGUILayout.TextArea(npcInfo.position_y + "", GUILayout.Width(100), GUILayout.Height(20)));

            GUILayout.Label("皮肤颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean skinColorData = new ColorBean(npcInfo.skin_color);
            Color skinColor = skinColorData.GetColor(); ;
            skinColor = EditorGUILayout.ColorField(skinColor);
            npcInfo.skin_color = skinColor.r + "," + skinColor.g + "," + skinColor.b + "," + skinColor.a;

            GUILayout.Label("头发：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.hair_id = EditorGUILayout.TextArea(npcInfo.hair_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string hairPath = "Assets/Texture/Character/Hair/";
            GUIPic(hairPath, npcInfo.hair_id);
            GUILayout.Label("头发颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean hairColorData = new ColorBean(npcInfo.hair_color);
            Color hairColor = hairColorData.GetColor(); ;
            hairColor = EditorGUILayout.ColorField(hairColor);
            npcInfo.hair_color = hairColor.r + "," + hairColor.g + "," + hairColor.b + "," + hairColor.a;

            GUILayout.Label("眼睛：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.eye_id = EditorGUILayout.TextArea(npcInfo.eye_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string eyePath = "Assets/Texture/Character/Eye/";
            GUIPic(eyePath, npcInfo.eye_id);
            GUILayout.Label("眼睛颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean eyeColorData = new ColorBean(npcInfo.eye_color);
            Color eyeColor = eyeColorData.GetColor(); ;
            eyeColor = EditorGUILayout.ColorField(eyeColor);
            npcInfo.eye_color = eyeColor.r + "," + eyeColor.g + "," + eyeColor.b + "," + eyeColor.a;

            GUILayout.Label("嘴巴：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.mouth_id = EditorGUILayout.TextArea(npcInfo.mouth_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string mouthPath = "Assets/Texture/Character/Mouth/";
            GUIPic(mouthPath, npcInfo.mouth_id);
            GUILayout.Label("嘴巴颜色：", GUILayout.Width(100), GUILayout.Height(20));
            ColorBean mouthColorData = new ColorBean(npcInfo.mouth_color);
            Color mouthColor = mouthColorData.GetColor(); ;
            mouthColor = EditorGUILayout.ColorField(mouthColor);
            npcInfo.mouth_color = mouthColor.r + "," + mouthColor.g + "," + mouthColor.b + "," + mouthColor.a;

            GUILayout.Label("喜欢的东西ID（用,分隔）：", GUILayout.Width(150), GUILayout.Height(20));
            npcInfo.love_items = EditorGUILayout.TextArea(npcInfo.love_items + "", GUILayout.Width(100), GUILayout.Height(20));



            GUILayout.Label("命：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_life = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_life + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("厨：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_cook = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_cook + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("速：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_speed = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_speed + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("算：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_account = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_account + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("魅：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_charm = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_charm + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("武：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_force = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_force + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("运：", GUILayout.Width(30), GUILayout.Height(20));
            npcInfo.attributes_lucky = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_lucky + "", GUILayout.Width(50), GUILayout.Height(20)));
            if (npcType == NpcTypeEnum.RecruitTown)
            {
                GUILayout.Label("忠诚：", GUILayout.Width(30), GUILayout.Height(20));
                npcInfo.attributes_loyal = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_loyal + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("工资 S：", GUILayout.Width(30), GUILayout.Height(20));
                npcInfo.wage_s = int.Parse(EditorGUILayout.TextArea(npcInfo.wage_s + "", GUILayout.Width(50), GUILayout.Height(20)));
            }

            GUILayout.Label("喜欢的菜品：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.love_menus = EditorGUILayout.TextArea(npcInfo.love_menus + "", GUILayout.Width(50), GUILayout.Height(20));
        }
        GUIText("|", 10);
        GUIText("面具：", 50);
        npcInfo.mask_id = long.Parse(EditorGUILayout.TextArea(npcInfo.mask_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string maskPath = "Assets/Texture/Character/Dress/Mask/";
        ItemsInfoBean maskInfo = gameItemsManager.GetItemsById(npcInfo.mask_id);
        if (maskInfo != null)
            GUIPic(maskPath, maskInfo.icon_key);
        GUIText("|", 10);
        GUIText("帽子：", 50);
        npcInfo.hat_id = long.Parse(EditorGUILayout.TextArea(npcInfo.hat_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string hatPath = "Assets/Texture/Character/Dress/Hat/";
        ItemsInfoBean hatInfo = gameItemsManager.GetItemsById(npcInfo.hat_id);
        if (hatInfo != null)
            GUIPic(hatPath, hatInfo.icon_key);
        GUIText("|", 10);
        GUIText("衣服：", 50);
        npcInfo.clothes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.clothes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string clothesPath = "Assets/Texture/Character/Dress/Clothes/";
        ItemsInfoBean clothesInfo = gameItemsManager.GetItemsById(npcInfo.clothes_id);
        if (clothesInfo != null)
            GUIPic(clothesPath, clothesInfo.icon_key);
        GUIText("|", 10);
        GUIText("鞋子：", 50);
        npcInfo.shoes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.shoes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string shoesPath = "Assets/Texture/Character/Dress/Shoes/";
        ItemsInfoBean shoesInfo = gameItemsManager.GetItemsById(npcInfo.shoes_id);
        if (shoesInfo != null)
            GUIPic(shoesPath, shoesInfo.icon_key);
        GUIText("|", 10);
        GUIText("武器：", 50);
        npcInfo.hand_id = long.Parse(EditorGUILayout.TextArea(npcInfo.hand_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        ItemsInfoBean handInfo = gameItemsManager.GetItemsById(npcInfo.hand_id);
        if (handInfo != null)
            GUIText(handInfo.name, 50);

        GUIText("|", 10);
        npcInfo.condition = GUIListData<ShowConditionEnum>("Npc出现条件", npcInfo.condition);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 团队创建 UI
    /// </summary>
    public static void GUINpcTeamCreate(NpcTeamService npcTeamService, NpcTeamBean npcTeamData)
    {
        GUILayout.Label("Npc团队创建", GUILayout.Width(100), GUILayout.Height(20));

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            npcTeamData.valid = 1;
            npcTeamService.InsertData(npcTeamData);
        }
        GUILayout.EndHorizontal();

        GUINpcTeamItem(npcTeamData);
    }

    /// <summary>
    /// 团队查询 UI
    /// </summary>
    public static void GUINpcTeamFind(
        TextInfoService textInfoService, NpcTeamService npcTeamService,
        string findIdsStr, List<NpcTeamBean> listFindData, Dictionary<long, List<TextInfoBean>> mapTeamTalkInfo,
        out string outFindIdsStr, out List<NpcTeamBean> outlistFindData, out Dictionary<long, List<TextInfoBean>> outMapTeamTalkInfo)
    {
        GUILayout.Label("Npc团队查询", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        outFindIdsStr = EditorGUILayout.TextArea(findIdsStr + "", GUILayout.Width(200), GUILayout.Height(20));

        if (GUILayout.Button("查询团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] findIds = StringUtil.SplitBySubstringForArrayLong(findIdsStr, ',');
            listFindData = npcTeamService.QueryDataById(findIds);
        }

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
        if (GUILayout.Button("查询杂项团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Sundry);
        }
        if (GUILayout.Button("查询助兴团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Entertain);
        }
        if (GUILayout.Button("查询扫兴团队", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindData = npcTeamService.QueryDataByType((int)NpcTeamTypeEnum.Disappointed);
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
                if (GUILayout.Button("查询团队对话", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    List<TextInfoBean> listNpcTeamTalkInfo = textInfoService.QueryDataByMarkId(TextEnum.Talk, itemData.GetTalkIds());
                    HandleTalkInfoDataByMarkId(listNpcTeamTalkInfo, mapTeamTalkInfo);
                }
                GUILayout.EndHorizontal();
                GUINpcTeamItem(itemData);
                GUILayout.Space(20);
            }
            if (itemRemoveData != null)
            {
                listFindData.Remove(itemRemoveData);
                itemRemoveData = null;
            }
        }
        outlistFindData = listFindData;
        outMapTeamTalkInfo = mapTeamTalkInfo;
    }

    /// <summary>
    /// 团队Iteam UI
    /// </summary>
    public static void GUINpcTeamItem(NpcTeamBean npcTeamData)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("是否启用", GUILayout.Width(100), GUILayout.Height(20));
        npcTeamData.valid = (int)(ValidEnum)EditorGUILayout.EnumPopup((ValidEnum)npcTeamData.valid, GUILayout.Width(100), GUILayout.Height(20));
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
            case NpcTeamTypeEnum.Sundry:
            case NpcTeamTypeEnum.Entertain:
            case NpcTeamTypeEnum.Disappointed:
                GUILayout.Label("对话markId(,)", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.talk_ids = EditorGUILayout.TextArea(npcTeamData.talk_ids + "", GUILayout.Width(200), GUILayout.Height(20));
                break;
        }
        GUIText("喜欢的菜品");
        npcTeamData.love_menus = EditorGUILayout.TextArea(npcTeamData.love_menus, GUILayout.Width(250), GUILayout.Height(20));
        npcTeamData.condition = GUIListData<ShowConditionEnum>("团队出现条件", npcTeamData.condition);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Npc对话逻辑添加
    /// </summary>
    /// <param name="npcId"></param>
    /// <param name="talkType"></param>
    /// <param name="mapNpcTalkInfoForFind"></param>
    public static void GUINpcTalkCreateByMarkId(long npcId, int talkType, Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NpcId:" + npcId, GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("talkType:" + talkType, GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加对话逻辑(警告：一定要先查询对应对话再添加)", GUILayout.Width(300), GUILayout.Height(20)))
        {
            long markId = npcId * 100000;
            markId += talkType * 10000;
            markId += (mapNpcTalkInfoForFind.Count + 1);
            List<TextInfoBean> listTemp = new List<TextInfoBean>();
            mapNpcTalkInfoForFind.Add(markId, listTemp);
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Npc对话查询 UI
    /// </summary>
    public static void GUINpcTalkFind(TextInfoService textInfoService,
        long npcId, TextTalkTypeEnum talkType, Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind,
        out TextTalkTypeEnum outTalkType)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NpcID:" + npcId, GUILayout.Width(120));
        bool isFind = false;
        if (GUILayout.Button("查询普通对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Normal;
        }
        if (GUILayout.Button("查询第一次对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.First;
        }
        if (GUILayout.Button("查询招募对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Recruit;
        }
        if (GUILayout.Button("查询礼物对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Gift;
        }
        if (GUILayout.Button("查询后续事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Special;
        }
        if (GUILayout.Button("查询捣乱事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Rascal;
        }
        if (GUILayout.Button("查询杂项事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            isFind = true;
            talkType = TextTalkTypeEnum.Sundry;
        }
        if (isFind)
        {
            List<TextInfoBean> listNpcTalkInfo = textInfoService.QueryDataByTalkType(TextEnum.Talk, talkType, npcId);
            HandleTalkInfoDataByMarkId(listNpcTalkInfo, mapNpcTalkInfoForFind);
        }
        GUILayout.EndHorizontal();
        outTalkType = talkType;
        if (mapNpcTalkInfoForFind == null)
            return;
        long deleteMarkId = 0;
        foreach (var mapItemTalkInfo in mapNpcTalkInfoForFind)
        {
            long markId = mapItemTalkInfo.Key;
            List<TextInfoBean> listTextData = mapItemTalkInfo.Value;
            if (GUILayout.Button("删除markId下所有对话", GUILayout.Width(150), GUILayout.Height(20)))
            {
                textInfoService.DeleteDataByMarkId(TextEnum.Talk, markId);
                deleteMarkId = markId;
            }
            GUINpcTextInfoItemForMarkId(textInfoService, npcId, talkType, markId, listTextData, out listTextData);
        }
        if (deleteMarkId != 0)
        {
            mapNpcTalkInfoForFind.Remove(deleteMarkId);
            deleteMarkId = 0;
        }
    }

    /// <summary>
    ///  团队对话查询
    /// </summary>
    public static void GUINpcTeamTalkFind(TextInfoService textInfoService,
        Dictionary<long, List<TextInfoBean>> mapNpcTalkInfoForFind)
    {
        if (mapNpcTalkInfoForFind == null)
            return;
        foreach (var mapItemTalkInfo in mapNpcTalkInfoForFind)
        {
            long markId = mapItemTalkInfo.Key;
            List<TextInfoBean> listTextData = mapItemTalkInfo.Value;
            GUINpcTextInfoItemForMarkId(textInfoService, 0, TextTalkTypeEnum.Normal, markId, listTextData, out listTextData);
        }
    }

    /// <summary>
    /// NPC 对话ITEM
    /// </summary>
    /// <param name="textInfoService"></param>
    /// <param name="markId"></param>
    /// <param name="listTextData"></param>
    public static void GUINpcTextInfoItemForMarkId(TextInfoService textInfoService,
        long userId, TextTalkTypeEnum talkType, long markId, List<TextInfoBean> listTextData,
        out List<TextInfoBean> outListTextData)
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("markId：" + markId, GUILayout.Width(150), GUILayout.Height(20));
        if (listTextData.Count > 0)
        {
            GUILayout.Label("对话类型：", GUILayout.Width(120), GUILayout.Height(20));
            listTextData[0].talk_type = (int)(TextTalkTypeEnum)EditorGUILayout.EnumPopup((TextTalkTypeEnum)listTextData[0].talk_type, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Label("条件-好感对话：", GUILayout.Width(120), GUILayout.Height(20));
            listTextData[0].condition_min_favorability = int.Parse(EditorGUILayout.TextArea(listTextData[0].condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));

        }
        if (listTextData != null)
            foreach (TextInfoBean itemTalkInfo in listTextData)
            {
                itemTalkInfo.talk_type = listTextData[0].talk_type;
                itemTalkInfo.condition_min_favorability = listTextData[0].condition_min_favorability;
            }
        if (GUILayout.Button("添加对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            TextInfoBean addText = new TextInfoBean();
            addText.mark_id = markId;
            addText.id = addText.mark_id * 1000 + (listTextData.Count + 1);
            addText.text_id = addText.id;
            addText.user_id = listTextData.Count > 0 ? listTextData[0].user_id : userId;
            addText.valid = 1;
            addText.text_order = 1;
            addText.talk_type = listTextData.Count > 0 ? listTextData[0].talk_type : (int)talkType;
            listTextData.Add(addText);
        }
        if (GUILayout.Button("保存当前所有对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            foreach (TextInfoBean itemTalkInfo in listTextData)
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
        }
        GUILayout.EndHorizontal();
        TextInfoBean removeTalkInfo = null;
        foreach (TextInfoBean itemTalkInfo in listTextData)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
            if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                removeTalkInfo = itemTalkInfo;
                textInfoService.DeleteDataById(TextEnum.Talk, itemTalkInfo.id);
            }
            GUILayout.Label("talkId：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.id = long.Parse(EditorGUILayout.TextArea(itemTalkInfo.id + "", GUILayout.Width(150), GUILayout.Height(20)));
            GUILayout.Label("对话顺序：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.text_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.text_order + "", GUILayout.Width(50), GUILayout.Height(20)));

            itemTalkInfo.type = (int)(TextInfoTypeEnum)EditorGUILayout.EnumPopup((TextInfoTypeEnum)itemTalkInfo.type, GUILayout.Width(100), GUILayout.Height(20));
            if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select)
            {
                GUILayout.Label("选择类型：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.select_type = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.select_type + "", GUILayout.Width(50), GUILayout.Height(20)));
            }
            GUILayout.Label("增加的好感：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.add_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.add_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("指定下一句对话：", GUILayout.Width(120), GUILayout.Height(20));
            itemTalkInfo.next_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.next_order + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("触发条件-最低好感：", GUILayout.Width(120), GUILayout.Height(20));
            itemTalkInfo.condition_min_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
            GUILayout.Label("预设名字：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.name = EditorGUILayout.TextArea(itemTalkInfo.name + "", GUILayout.Width(50), GUILayout.Height(20));
            GUILayout.Label("对话内容：", GUILayout.Width(100), GUILayout.Height(20));
            itemTalkInfo.content = EditorGUILayout.TextArea(itemTalkInfo.content + "", GUILayout.Width(500), GUILayout.Height(20));
            itemTalkInfo.reward_data = GUIListData<RewardTypeEnum>("奖励", itemTalkInfo.reward_data);
            if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select && itemTalkInfo.select_type == 1)
            {
                itemTalkInfo.pre_data = GUIListData<PreTypeEnum>("付出", itemTalkInfo.pre_data);
                itemTalkInfo.pre_data_minigame = GUIListData<PreTypeForMiniGameEnum>("小游戏数据", itemTalkInfo.pre_data_minigame);
            }

            if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
            {
                textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
            }
            if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                removeTalkInfo = itemTalkInfo;
                textInfoService.DeleteDataById(TextEnum.Talk, itemTalkInfo.id);
            }
            GUILayout.EndHorizontal();
        }
        if (removeTalkInfo != null)
        {
            listTextData.Remove(removeTalkInfo);
            removeTalkInfo = null;
        }
        outListTextData = listTextData;
    }

    /// <summary>
    /// 建造物品创建
    /// </summary>
    /// <param name="buildItemService"></param>
    /// <param name="buildItem"></param>
    public static void GUIBuildItemCreate(BuildItemService buildItemService, BuildItemBean buildItem)
    {
        GUILayout.BeginVertical();
        GUILayout.Label("建造物品创建：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("创建", GUILayout.Width(120), GUILayout.Height(20)))
        {
            buildItem.valid = 1;
            buildItemService.InsertData(buildItem);
        }
        GUIBuildItem(buildItem);
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 建造物品查询
    /// </summary>
    /// <param name="buildItemService"></param>
    /// <param name="listBuildItem"></param>
    /// <param name="outListBuildItem"></param>
    public static void GUIBuildItemFind(BuildItemService buildItemService,
        List<BuildItemBean> listBuildItem,
        out List<BuildItemBean> outListBuildItem)
    {
        GUILayout.Label("建造物品查询：", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        if (GUIButton("查询所有"))
        {
            listBuildItem = buildItemService.QueryAllData();
        }
        if (GUIButton("查询地板"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Floor);
        }
        if (GUIButton("查询墙壁"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Wall);
        }
        if (GUIButton("查询桌椅"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Table);
        }
        if (GUIButton("查询灶台"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Stove);
        }
        if (GUIButton("查询柜台"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Counter);
        }
        if (GUIButton("查询装饰"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Decoration);
        }
        if (GUIButton("查询正门"))
        {
            listBuildItem = buildItemService.QueryDataByType((int)BuildItemTypeEnum.Door);
        }
        GUILayout.EndHorizontal();
        if (listBuildItem != null)
        {
            BuildItemBean removeData = null;
            foreach (BuildItemBean itemData in listBuildItem)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                if (GUIButton("更新"))
                {
                    buildItemService.Update(itemData);
                }
                if (GUIButton("删除"))
                {
                    if (buildItemService.DeleteData(itemData.id))
                    {
                        removeData = itemData;
                    };
                }
                GUILayout.EndHorizontal();
                GUIBuildItem(itemData);
            }
            if (removeData != null)
            {
                listBuildItem.Remove(removeData);
            }
        }
        outListBuildItem = listBuildItem;
    }

    /// <summary>
    /// 建造物品展示
    /// </summary>
    /// <param name="buildItem"></param>
    public static void GUIBuildItem(BuildItemBean buildItem)
    {
        GUILayout.BeginHorizontal();
        GUIText("id：");
        buildItem.id = GUIEditorText(buildItem.id);
        buildItem.build_id = buildItem.id;
        buildItem.build_type = (int)GUIEnum<BuildItemTypeEnum>("类型：", buildItem.build_type);
        GUIText("模型ID：");
        buildItem.model_name = GUIEditorText(buildItem.model_name);
        GUIText(" 图标：", 200);
        buildItem.icon_key = GUIEditorText(buildItem.icon_key);
        string picPath = "";
        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Floor:
                picPath = "Assets/Texture/Tile/Floor/";
                break;
            case BuildItemTypeEnum.Wall:
                picPath = "Assets/Texture/Tile/Wall/";
                break;
            case BuildItemTypeEnum.Table:
                picPath = "Assets/Texture/InnBuild/TableAndChair/";
                break;
            case BuildItemTypeEnum.Stove:
                picPath = "Assets/Texture/InnBuild/Stove/";
                break;
            case BuildItemTypeEnum.Counter:
                picPath = "Assets/Texture/InnBuild/Counter/";
                break;
            case BuildItemTypeEnum.Decoration:
                picPath = "Assets/Texture/InnBuild/Decoration/";
                break;
            case BuildItemTypeEnum.Door:
                picPath = "Assets/Texture/InnBuild/Door/";
                break;
            default:
                break;
        }
        GUIPic(picPath, buildItem.icon_key);

        switch ((BuildItemTypeEnum)buildItem.build_type)
        {
            case BuildItemTypeEnum.Table:
            case BuildItemTypeEnum.Counter:
            case BuildItemTypeEnum.Stove:
            case BuildItemTypeEnum.Door:
            case BuildItemTypeEnum.Decoration:
                GUIText("icon_list");
                buildItem.icon_list = GUIEditorText(buildItem.icon_list, 300);
                break;
            case BuildItemTypeEnum.Floor:
            case BuildItemTypeEnum.Wall:
                GUIText("tile名字：");
                buildItem.tile_name = GUIEditorText(buildItem.tile_name);
                break;
            default:
                break;
        }
        GUIText("美观：", 50);
        buildItem.aesthetics = GUIEditorText(buildItem.aesthetics);
        GUIText("名称：", 50);
        buildItem.name = GUIEditorText(buildItem.name, 200);
        GUIText("形容：", 50);
        buildItem.content = GUIEditorText(buildItem.content, 300);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    public static void GUIMenuCreate(MenuInfoService menuInfoService, MenuInfoBean menuInfo)
    {
        GUIText("技能创建");
        if (GUIButton("创建"))
        {
            menuInfo.valid = 1;
            menuInfoService.InsertData(menuInfo);
        }
        GUIMenuItem(menuInfo);
        GUILayout.Space(20);
    }

    /// <summary>
    /// 菜单查询
    /// </summary>
    /// <param name="menuInfoService"></param>
    /// <param name="listData"></param>
    public static void GUIMenuFind(MenuInfoService menuInfoService, string findIds, List<MenuInfoBean> listData, out string outFindIds, out List<MenuInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        GUIText("查询IDs");
        findIds = GUIEditorText(findIds);
        if (GUIButton("查询指定ID"))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIds, ',');
            listData = menuInfoService.QueryDataByIds(ids);
        }
        if (GUIButton("查询所有菜单"))
        {
            listData = menuInfoService.QueryAllData();
        }
        GUILayout.EndHorizontal();
        if (!CheckUtil.ListIsNull(listData))
        {

            for (int i = 0; i < listData.Count; i++)
            {
                MenuInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUIButton("更新"))
                {
                    menuInfoService.Update(itemData);
                }
                if (GUIButton("删除"))
                {
                    menuInfoService.DeleteData(itemData.id);
                }
                GUILayout.EndHorizontal();
                GUIMenuItem(itemData);
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    /// <summary>
    /// 菜单展示
    /// </summary>
    public static void GUIMenuItem(MenuInfoBean menuInfo)
    {
        GUILayout.BeginHorizontal();
        GUIText("ID:");
        menuInfo.id = GUIEditorText(menuInfo.id);
        menuInfo.menu_id = menuInfo.id;
        GUIText("名称:");
        menuInfo.name = GUIEditorText(menuInfo.name, 150);
        GUIText("内容:");
        menuInfo.content = GUIEditorText(menuInfo.content, 300);
        GUIText("图片名称:");
        menuInfo.icon_key = GUIEditorText(menuInfo.icon_key, 150);
        string menuPicPath = "Assets/Texture/Food/";
        GUIPic(menuPicPath, menuInfo.icon_key);
        GUIText("烹饪时间:");
        menuInfo.cook_time = GUIEditorText(menuInfo.cook_time);
        GUIText("价格LMS:");
        menuInfo.price_l = GUIEditorText(menuInfo.price_l, 50);
        menuInfo.price_m = GUIEditorText(menuInfo.price_m, 50);
        menuInfo.price_s = GUIEditorText(menuInfo.price_s, 50);

        GUIText("材料 油盐:");
        menuInfo.ing_oilsalt = GUIEditorText(menuInfo.ing_oilsalt, 50);
        GUIText("材料 鲜肉:");
        menuInfo.ing_meat = GUIEditorText(menuInfo.ing_meat, 50);
        GUIText("材料 河鲜:");
        menuInfo.ing_riverfresh = GUIEditorText(menuInfo.ing_riverfresh, 50);
        GUIText("材料 海鲜:");
        menuInfo.ing_seafood = GUIEditorText(menuInfo.ing_seafood, 50);
        GUIText("材料 蔬菜:");
        menuInfo.ing_vegetables = GUIEditorText(menuInfo.ing_vegetables, 50);
        GUIText("材料 瓜果:");
        menuInfo.ing_melonfruit = GUIEditorText(menuInfo.ing_melonfruit, 50);
        GUIText("材料 酒水:");
        menuInfo.ing_waterwine = GUIEditorText(menuInfo.ing_waterwine, 50);
        GUIText("材料 面粉:");
        menuInfo.ing_flour = GUIEditorText(menuInfo.ing_flour, 50);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 技能创建
    /// </summary>
    /// <param name="skillInfoService"></param>
    /// <param name="skillInfo"></param>
    public static void GUISkillCreate(SkillInfoService skillInfoService, SkillInfoBean skillInfo)
    {
        if (GUIButton("创建"))
        {
            skillInfo.valid = 1;
            skillInfoService.InsertData(skillInfo);
        }
        GUISkillItem(skillInfo);
        GUILayout.Space(20);
    }

    /// <summary>
    /// 技能查询
    /// </summary>
    /// <param name="skillInfoService"></param>
    /// <param name="findIds"></param>
    /// <param name="listData"></param>
    /// <param name="outFindIds"></param>
    /// <param name="outListData"></param>
    public static void GUISkillFind(SkillInfoService skillInfoService, string findIds, List<SkillInfoBean> listData, out string outFindIds, out List<SkillInfoBean> outListData)
    {
        GUILayout.BeginHorizontal();
        GUIText("查询IDs");
        findIds = GUIEditorText(findIds);
        if (GUIButton("查询指定ID"))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIds, ',');
            listData = skillInfoService.QueryDataByIds(ids);
        }
        if (GUIButton("查询所有技能"))
        {
            listData = skillInfoService.QueryAllData();
        }
        GUILayout.EndHorizontal();
        if (!CheckUtil.ListIsNull(listData))
        {

            for (int i = 0; i < listData.Count; i++)
            {
                SkillInfoBean itemData = listData[i];
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUIButton("更新"))
                {
                    skillInfoService.Update(itemData);
                }
                if (GUIButton("删除"))
                {
                    skillInfoService.DeleteData(itemData.id);
                }
                GUISkillItem(itemData);
                GUILayout.EndHorizontal();
            }
        }
        outListData = listData;
        outFindIds = findIds;
    }

    /// <summary>
    /// 技能展示
    /// </summary>
    /// <param name="skillInfo"></param>
    public static void GUISkillItem(SkillInfoBean skillInfo)
    {
        GUILayout.BeginHorizontal();
        GUIText("ID:");
        skillInfo.id = GUIEditorText(skillInfo.id);
        skillInfo.skill_id = skillInfo.id;
        GUIText("名称");
        skillInfo.name = GUIEditorText(skillInfo.name);
        GUIText("介绍");
        skillInfo.content = GUIEditorText(skillInfo.content, 200);
        GUIText("图片名称:");
        skillInfo.icon_key = GUIEditorText(skillInfo.icon_key, 150);
        string menuPicPath = "Assets/Texture/Common/UI/";
        GUIPic(menuPicPath, skillInfo.icon_key);
        GUIText("使用数量");
        skillInfo.use_number = GUIEditorText(skillInfo.use_number);
        skillInfo.effect = GUIListData<EffectTypeEnum>("效果", skillInfo.effect);
        skillInfo.effect_details = GUIListData<EffectDetailsEnum>("效果详情", skillInfo.effect_details);
        skillInfo.pre_data = GUIListData<PreTypeEnum>("解锁条件", skillInfo.pre_data);
        GUILayout.EndHorizontal();
    }


    /// <summary>
    /// 按钮
    /// </summary>
    /// <param name="name"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static bool GUIButton(string name, int width, int height)
    {
        return GUILayout.Button(name, GUILayout.Width(width), GUILayout.Height(height));
    }
    public static bool GUIButton(string name, int width)
    {
        return GUIButton(name, width, 20);
    }
    public static bool GUIButton(string name)
    {
        return GUIButton(name, 100, 20);
    }

    /// <summary>
    /// 输入文本
    /// </summary>
    /// <param name="text"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static string GUIEditorText(string text, int width, int height)
    {
        return EditorGUILayout.TextArea(text, GUILayout.Width(width), GUILayout.Height(height));
    }
    public static string GUIEditorText(string text, int width)
    {
        return GUIEditorText(text, width, 20);
    }
    public static string GUIEditorText(string text)
    {
        return GUIEditorText(text, 100, 20);
    }
    public static long GUIEditorText(long text, int width, int height)
    {
        return long.Parse(GUIEditorText(text + "", width, height));
    }
    public static long GUIEditorText(long text, int width)
    {
        return GUIEditorText(text, width, 20);
    }
    public static long GUIEditorText(long text)
    {
        return GUIEditorText(text, 100, 20);
    }
    public static float GUIEditorText(float text, int width, int height)
    {
        return float.Parse(GUIEditorText(text + "", width, height));
    }
    public static float GUIEditorText(float text, int width)
    {
        return GUIEditorText(text, width, 20);
    }
    public static float GUIEditorText(float text)
    {
        return GUIEditorText(text, 100, 20);
    }
    public static int GUIEditorText(int text, int width, int height)
    {
        return int.Parse(GUIEditorText(text + "", width, height));
    }
    public static int GUIEditorText(int text, int width)
    {
        return GUIEditorText(text, width, 20);
    }
    public static int GUIEditorText(int text)
    {
        return GUIEditorText(text, 100, 20);
    }

    /// <summary>
    /// 文本
    /// </summary>
    /// <param name="text"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void GUIText(string text, int width, int height)
    {
        GUILayout.Label(text, GUILayout.Width(width), GUILayout.Height(height));
    }
    public static void GUIText(string text, int width)
    {
        GUIText(text, width, 20);
    }
    public static void GUIText(string text)
    {
        GUIText(text, 100, 20);
    }

    /// <summary>
    /// 枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="title"></param>
    /// <param name="type"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static T GUIEnum<T>(string title, int type, int width, int height) where T : Enum
    {
        return (T)EditorGUILayout.EnumPopup(title, EnumUtil.GetEnum<T>(type), GUILayout.Width(width), GUILayout.Height(height));
    }
    public static T GUIEnum<T>(string title, int type, int width) where T : Enum
    {
        return (T)EditorGUILayout.EnumPopup(title, EnumUtil.GetEnum<T>(type), GUILayout.Width(width), GUILayout.Height(20));
    }
    public static T GUIEnum<T>(string title, int type) where T : Enum
    {
        return (T)EditorGUILayout.EnumPopup(title, EnumUtil.GetEnum<T>(type), GUILayout.Width(300), GUILayout.Height(20));
    }

    /// <summary>
    /// 展示图片
    /// </summary>
    /// <param name="picPath"></param>
    /// <param name="picName"></param>
    public static void GUIPic(string picPath, string picName)
    {
        Texture2D iconTex = EditorGUIUtility.FindTexture(picPath + picName + ".png");
        if (iconTex)
            GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
    }

    /// <summary>
    /// 图片选择
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="iconName"></param>
    public static void GUIPicSelect(string iconName, Sprite spIcon)
    {
        spIcon = EditorGUILayout.ObjectField(new GUIContent(iconName, ""), spIcon, typeof(Sprite), true) as Sprite;
    }

    /// <summary>
    /// 通过markID处理聊天信息
    /// </summary>
    /// <param name="listNpcTalkInfo"></param>
    /// <param name="mapNpcTeamTalkInfoForFind"></param>
    public static void HandleTalkInfoDataByMarkId(List<TextInfoBean> listNpcTalkInfo, Dictionary<long, List<TextInfoBean>> mapNpcTeamTalkInfoForFind)
    {
        mapNpcTeamTalkInfoForFind.Clear();
        foreach (TextInfoBean itemTalkInfo in listNpcTalkInfo)
        {
            long markId = itemTalkInfo.mark_id;
            if (mapNpcTeamTalkInfoForFind.TryGetValue(markId, out List<TextInfoBean> value))
            {
                value.Add(itemTalkInfo);
            }
            else
            {
                List<TextInfoBean> listTemp = new List<TextInfoBean>();
                listTemp.Add(itemTalkInfo);
                mapNpcTeamTalkInfoForFind.Add(markId, listTemp);
            }
        }
    }

    /// <summary>
    ///  展示NPC
    /// </summary>
    public static GameObject ShowNpc(GameItemsManager gameItemsManager, GameObject objNpcContainer, GameObject objNpcModel, CharacterBean characterData)
    {
        CptUtil.RemoveChildsByActiveInEditor(objNpcContainer);
        GameObject objNpc = GameObject.Instantiate(objNpcModel, objNpcContainer.transform);
        objNpc.SetActive(true);
        objNpc.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);

        BaseNpcAI baseNpcAI = objNpc.GetComponent<BaseNpcAI>();
        baseNpcAI.Awake();

        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(baseNpcAI.gameObject, "Body");
        characterDress.Awake();

        CharacterBodyCpt characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(baseNpcAI.gameObject, "Body");
        characterBody.Awake();

        baseNpcAI.SetCharacterData(gameItemsManager, characterData);
        return objNpc;
    }

    /// <summary>
    /// 数据列表
    /// </summary>
    /// <param name="storeInfo"></param>
    public static string GUIListData<E>(string titleName, string content) where E : System.Enum
    {
        //前置相关
        EditorGUILayout.BeginVertical();
        GUILayout.Label(titleName + "：", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("添加", GUILayout.Width(100), GUILayout.Height(20)))
        {
            content += ("|" + EnumUtil.GetEnumName(EnumUtil.GetEnumValueByPosition<E>(0)) + ":" + "1|");
        }
        List<string> listConditionData = StringUtil.SplitBySubstringForListStr(content, '|');
        content = "";
        for (int i = 0; i < listConditionData.Count; i++)
        {
            string itemConditionData = listConditionData[i];
            if (CheckUtil.StringIsNull(itemConditionData))
            {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                listConditionData.RemoveAt(i);
                i--;
                continue;
            }
            List<string> listItemConditionData = StringUtil.SplitBySubstringForListStr(itemConditionData, ':');
            listItemConditionData[0] = EnumUtil.GetEnumName(EditorGUILayout.EnumPopup(EnumUtil.GetEnum<E>(listItemConditionData[0]), GUILayout.Width(300), GUILayout.Height(20)));
            listItemConditionData[1] = EditorGUILayout.TextArea(listItemConditionData[1] + "", GUILayout.Width(100), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            content += (listItemConditionData[0] + ":" + listItemConditionData[1]) + "|";
        }
        EditorGUILayout.EndVertical();
        return content;
    }
}
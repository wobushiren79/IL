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
        NpcInfoManager npcInfoManager, GameItemsManager gameItemsManager,
        GameObject objNpcContainer, GameObject objNpcModel,
        string findIdsStr, List<CharacterBean> listNpcDataForFind,
        out string outFindIdStr, out List<CharacterBean> outListNpcDataForFind)
    {
        GUILayout.Label("查询NPC", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPCId", GUILayout.Width(100), GUILayout.Height(20));
        findIdsStr = EditorGUILayout.TextArea(findIdsStr + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIdsStr, ',');
            listNpcDataForFind = npcInfoManager.GetCharacterDataByIds(ids);
        }
        if (GUILayout.Button("查询全部", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetAllCharacterData();
        }
        if (GUILayout.Button("查询路人NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.Passerby);
        }
        if (GUILayout.Button("查询小镇NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.Town);
        }
        if (GUILayout.Button("查询小镇可招募NPC", GUILayout.Width(120), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.RecruitTown);
        }
        if (GUILayout.Button("查询团队顾客", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.GuestTeam);
        }
        if (GUILayout.Button("查询其他NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listNpcDataForFind = npcInfoManager.GetCharacterDataByType((int)NpcTypeEnum.Other);
        }

        GUILayout.EndHorizontal();
        foreach (CharacterBean itemData in listNpcDataForFind)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ShowNpc(gameItemsManager, objNpcContainer, objNpcModel, itemData);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.Update(itemData.npcInfoData);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.DeleteData(itemData.npcInfoData);
                listNpcDataForFind.Remove(itemData);
            }
            GUILayout.EndHorizontal();
            GUINpcInfoItem(gameItemsManager, objNpcContainer, itemData.npcInfoData);
            itemData.body.hairColor = new ColorBean(itemData.npcInfoData.hair_color);
            itemData.body.eyeColor = new ColorBean(itemData.npcInfoData.eye_color);
            itemData.body.mouthColor = new ColorBean(itemData.npcInfoData.mouth_color);
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
        npcInfo.npc_type = (int)(NpcTypeEnum)EditorGUILayout.EnumPopup("Npc类型：", (NpcTypeEnum)npcInfo.npc_type);
        GUILayout.Label("姓名：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.name = EditorGUILayout.TextArea(npcInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("性别：1男 2女", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.sex = int.Parse(EditorGUILayout.TextArea(npcInfo.sex + "", GUILayout.Width(30), GUILayout.Height(20)));
        GUILayout.Label("朝向 1左 2右", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.face = int.Parse(EditorGUILayout.TextArea(npcInfo.face + "", GUILayout.Width(30), GUILayout.Height(20)));
        GUILayout.Label("称号：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.title_name = EditorGUILayout.TextArea(npcInfo.title_name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("位置XY：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.position_x = float.Parse(EditorGUILayout.TextArea(npcInfo.position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcInfo.position_y = float.Parse(EditorGUILayout.TextArea(npcInfo.position_y + "", GUILayout.Width(100), GUILayout.Height(20)));

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

        GUILayout.Label("面具：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.mask_id = long.Parse(EditorGUILayout.TextArea(npcInfo.mask_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string maskPath = "Assets/Texture/Character/Dress/Mask/";
        ItemsInfoBean maskInfo = gameItemsManager.GetItemsById(npcInfo.mask_id);
        if (maskInfo != null)
            GUIPic(maskPath, maskInfo.icon_key);

        GUILayout.Label("帽子：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.hat_id = long.Parse(EditorGUILayout.TextArea(npcInfo.hat_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string hatPath = "Assets/Texture/Character/Dress/Hat/";
        ItemsInfoBean hatInfo = gameItemsManager.GetItemsById(npcInfo.hat_id);
        if (hatInfo != null)
            GUIPic(hatPath, hatInfo.icon_key);

        GUILayout.Label("衣服：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.clothes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.clothes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string clothesPath = "Assets/Texture/Character/Dress/Clothes/";
        ItemsInfoBean clothesInfo = gameItemsManager.GetItemsById(npcInfo.clothes_id);
        if (clothesInfo != null)
            GUIPic(clothesPath, clothesInfo.icon_key);

        GUILayout.Label("鞋子：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.shoes_id = long.Parse(EditorGUILayout.TextArea(npcInfo.shoes_id + "", GUILayout.Width(100), GUILayout.Height(20)));
        string shoesPath = "Assets/Texture/Character/Dress/Shoes/";
        ItemsInfoBean shoesInfo = gameItemsManager.GetItemsById(npcInfo.shoes_id);
        if (shoesInfo != null)
            GUIPic(shoesPath, shoesInfo.icon_key);

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

        GUILayout.Label("喜欢的菜品：", GUILayout.Width(30), GUILayout.Height(20));
        npcInfo.attributes_life = int.Parse(EditorGUILayout.TextArea(npcInfo.attributes_life + "", GUILayout.Width(50), GUILayout.Height(20)));

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
                GUILayout.Label("对话markId(,)", GUILayout.Width(100), GUILayout.Height(20));
                npcTeamData.talk_ids = EditorGUILayout.TextArea(npcTeamData.talk_ids + "", GUILayout.Width(200), GUILayout.Height(20));
                break;
        }
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
        if(isFind)
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
            GUINpcTextInfoItemForMarkId(textInfoService,0, TextTalkTypeEnum.Normal, markId, listTextData,out listTextData);
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
            itemTalkInfo.pre_data = GUIListData<PreTypeEnum>("付出", itemTalkInfo.pre_data);
            itemTalkInfo.reward_data = GUIListData<RewardTypeEnum>("奖励", itemTalkInfo.reward_data);


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
    /// 展示图片
    /// </summary>
    /// <param name="picPath"></param>
    /// <param name="picName"></param>
    private static void GUIPic(string picPath, string picName)
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
    private static void GUIPicSelect(Sprite spIcon, string iconName)
    {
        spIcon = EditorGUILayout.ObjectField(new GUIContent(iconName, ""), spIcon, typeof(Sprite), true) as Sprite;
    }

    /// <summary>
    /// 通过markID处理聊天信息
    /// </summary>
    /// <param name="listNpcTalkInfo"></param>
    /// <param name="mapNpcTeamTalkInfoForFind"></param>
    private static void HandleTalkInfoDataByMarkId(List<TextInfoBean> listNpcTalkInfo, Dictionary<long, List<TextInfoBean>> mapNpcTeamTalkInfoForFind)
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
    private static GameObject ShowNpc(GameItemsManager gameItemsManager, GameObject objNpcContainer, GameObject objNpcModel, CharacterBean characterData)
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
    private static string GUIListData<E>(string titleName, string content) where E : System.Enum
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
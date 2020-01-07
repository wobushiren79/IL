using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using Cinemachine;
using System.Collections;

public class NpcCreateWindowEidtor : EditorWindow
{
    GameItemsManager gameItemsManager;
    NpcInfoManager npcInfoManager;
    NpcInfoService npcInfoService;
    TextInfoService textInfoService;

    private CinemachineVirtualCamera mCamera2D;
    private GameObject mObjContent;
    private GameObject mObjNpcModel;

    [MenuItem("Tools/Window/NpcCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(NpcCreateWindowEidtor));
    }

    public NpcCreateWindowEidtor()
    {
        this.titleContent = new GUIContent("Npc创建工具");
    }

    private void OnDestroy()
    {
        CptUtil.RemoveChildsByActiveInEditor(mObjContent);
    }

    private void OnEnable()
    {
        mCamera2D = GameObject.Find("Camera2D").GetComponent<CinemachineVirtualCamera>();
        gameItemsManager = new GameItemsManager();
        npcInfoManager = new NpcInfoManager();
        npcInfoService = new NpcInfoService();
        textInfoService = new TextInfoService();

        gameItemsManager.Awake();
        npcInfoManager.Awake();
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        npcInfoManager.npcInfoController.GetAllNpcInfo();
    }

    private void RefreshData()
    {
        listFindNpcData.Clear();
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        npcInfoManager.npcInfoController.GetAllNpcInfo();
    }

    public List<CharacterBean> listFindNpcData = new List<CharacterBean>();
    public Vector2 scrollPosition = Vector2.zero;

    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        if (GUILayout.Button("刷新", GUILayout.Width(100), GUILayout.Height(20)))
        {
            RefreshData();
        }

        mObjContent = EditorGUILayout.ObjectField(new GUIContent("Npc容器", ""), mObjContent, typeof(GameObject), true) as GameObject;
        mObjNpcModel = EditorGUILayout.ObjectField(new GUIContent("NPC模型", ""), mObjNpcModel, typeof(GameObject), true) as GameObject;
        GUICreateNpc();
        GUIFindNpc();
        GUINpcTalk();
        GUILayout.EndScrollView();
    }

    public NpcInfoBean createNpcInfo = new NpcInfoBean();
    private void GUICreateNpc()
    {
        GUINpcInfo(createNpcInfo, true);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
        {
            CharacterBean characterData = NpcInfoBean.NpcInfoToCharacterData(createNpcInfo);
            ShowNpc(characterData);
        }
        if (GUILayout.Button("创建", GUILayout.Width(100), GUILayout.Height(20)))
        {
            createNpcInfo.valid = 1;
            createNpcInfo.face = 1;
            npcInfoService.InsertData(createNpcInfo);
        }
        GUILayout.EndHorizontal();
    }

    string findIds = "";
    private void GUIFindNpc()
    {
        GUILayout.Label("查询NPC", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPCId", GUILayout.Width(100), GUILayout.Height(20));
        findIds = EditorGUILayout.TextArea(findIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIds, ',');
            listFindNpcData = npcInfoManager.GetCharacterDataByIds(ids);
            GetNpcTalkInfoList(TextTalkTypeEnum.Normal);
        }
        if (GUILayout.Button("查询全部", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindNpcData = npcInfoManager.GetAllCharacterData();
        }
        if (GUILayout.Button("查询路人NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindNpcData = npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.Passerby);
        }
        if (GUILayout.Button("查询小镇NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindNpcData = npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.Town);
        }
        if (GUILayout.Button("查询小镇可招募NPC", GUILayout.Width(120), GUILayout.Height(20)))
        {
            listFindNpcData = npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.RecruitTown);
        }
        if (GUILayout.Button("查询其他NPC", GUILayout.Width(100), GUILayout.Height(20)))
        {
            listFindNpcData = npcInfoManager.GetCharacterDataByType((int)NPCTypeEnum.Other);
        }

        GUILayout.EndHorizontal();
        foreach (CharacterBean itemData in listFindNpcData)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("显示", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ShowNpc(itemData);
            }
            if (GUILayout.Button("更新", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.Update(itemData.npcInfoData);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100), GUILayout.Height(20)))
            {
                npcInfoService.DeleteData(itemData.npcInfoData);
                listFindNpcData.Remove(itemData);
            }
            GUILayout.EndHorizontal();
            GUINpcInfo(itemData.npcInfoData, false);
            itemData.body.hairColor = new ColorBean(itemData.npcInfoData.hair_color);
            itemData.body.eyeColor = new ColorBean(itemData.npcInfoData.eye_color);
            itemData.body.mouthColor = new ColorBean(itemData.npcInfoData.mouth_color);
        }
    }

    private Sprite spCreateHair;
    private Sprite spCreateEye;
    private Sprite spCreateMouth;

    public Dictionary<long, List<TextInfoBean>> mapNpcTalkInfo = new Dictionary<long, List<TextInfoBean>>();
    private void GUINpcTalk()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NpcID:" + findIds, GUILayout.Width(120));
        if (GUILayout.Button("查询普通对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            GetNpcTalkInfoList(TextTalkTypeEnum.Normal);
        }
        if (GUILayout.Button("查询第一次对话",  GUILayout.Width(120), GUILayout.Height(20)))
        {
            GetNpcTalkInfoList(TextTalkTypeEnum.First);
        }
        if (GUILayout.Button("查询招募对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            GetNpcTalkInfoList(TextTalkTypeEnum.Recruit);
        }
        if (GUILayout.Button("查询礼物对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            GetNpcTalkInfoList(TextTalkTypeEnum.Gift);
        }
        if (GUILayout.Button("查询后续事件对话", GUILayout.Width(120), GUILayout.Height(20)))
        {
            GetNpcTalkInfoList(TextTalkTypeEnum.Special);
        }
        if (GUILayout.Button("添加对话逻辑(警告：一定要先查询对应对话再添加)", GUILayout.Width(300), GUILayout.Height(20)))
        {
            long markId = long.Parse(findIds) * 100000;
            markId += (mapNpcTalkInfo.Count + 1);
            markId += (int)mFindTalkType * 10000;
            List<TextInfoBean> listTemp = new List<TextInfoBean>();
            mapNpcTalkInfo.Add(markId, listTemp);
        }

        GUILayout.EndHorizontal();
        if (mapNpcTalkInfo == null || mapNpcTalkInfo.Count == 0)
            return;


        long removeMarkId = 0;
        long removeTalkId = 0;
        foreach (var mapItemTalkInfo in mapNpcTalkInfo)
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("markId：",GUILayout.Width(120), GUILayout.Height(20));
            long.Parse(EditorGUILayout.TextArea(mapItemTalkInfo.Key + "", GUILayout.Width(100), GUILayout.Height(20)));
            if (mapItemTalkInfo.Value.Count > 0)
            {
                GUILayout.Label("对话类型：", GUILayout.Width(120), GUILayout.Height(20));
                mapItemTalkInfo.Value[0].talk_type = (int)(TextTalkTypeEnum)EditorGUILayout.EnumPopup((TextTalkTypeEnum)mapItemTalkInfo.Value[0].talk_type, GUILayout.Width(100), GUILayout.Height(20));
                GUILayout.Label("条件-好感对话：", GUILayout.Width(120), GUILayout.Height(20));
                mapItemTalkInfo.Value[0].condition_min_favorability = int.Parse(EditorGUILayout.TextArea(mapItemTalkInfo.Value[0].condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));

            }
            if (mapItemTalkInfo.Value != null)
                foreach (TextInfoBean itemTalkInfo in mapItemTalkInfo.Value)
                {
                    itemTalkInfo.talk_type = mapItemTalkInfo.Value[0].talk_type;
                    itemTalkInfo.condition_min_favorability = mapItemTalkInfo.Value[0].condition_min_favorability;
                }
            if (GUILayout.Button("添加对话", GUILayout.Width(120), GUILayout.Height(20)))
            {
                TextInfoBean addText = new TextInfoBean();
                addText.mark_id = mapItemTalkInfo.Key;
                addText.id = addText.mark_id * 1000 + (mapItemTalkInfo.Value.Count + 1);
                addText.text_id = addText.id;
                addText.user_id = long.Parse(findIds);
                addText.valid = 1;
                addText.text_order = 1;
                addText.talk_type = (int)mFindTalkType;
                mapItemTalkInfo.Value.Add(addText);
            }
            if (GUILayout.Button("删除markId下所有对话", GUILayout.Width(150), GUILayout.Height(20)))
            {
                removeMarkId = mapItemTalkInfo.Key;
                textInfoService.DeleteDataByMarkId(TextEnum.Talk, removeMarkId);
            }
            GUILayout.EndHorizontal();

            foreach (TextInfoBean itemTalkInfo in mapItemTalkInfo.Value)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
                }
                if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    removeTalkId = itemTalkInfo.id;
                    textInfoService.DeleteDataById(TextEnum.Talk, removeTalkId);
                }
                GUILayout.Label("talkId：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.id = long.Parse(EditorGUILayout.TextArea(itemTalkInfo.id + "", GUILayout.Width(150), GUILayout.Height(20)));
                itemTalkInfo.type = (int)(TextInfoTypeEnum)EditorGUILayout.EnumPopup((TextInfoTypeEnum)itemTalkInfo.type, GUILayout.Width(100), GUILayout.Height(20));
                if (itemTalkInfo.type == (int)TextInfoTypeEnum.Select)
                {
                    GUILayout.Label("选择类型：", GUILayout.Width(100), GUILayout.Height(20));
                    itemTalkInfo.select_type = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.select_type + "", GUILayout.Width(50), GUILayout.Height(20)));
                    if (itemTalkInfo.select_type == 1)
                    {
                        GUILayout.Label("好处的前提条件：类型（1战斗），己方人数，敌方角色ID(-分隔),位置xy(-分隔),后续赢对话markID,后续输对话markID");

                        itemTalkInfo.add_pre = EditorGUILayout.TextArea(itemTalkInfo.add_pre + "", GUILayout.Width(300), GUILayout.Height(20));
                        GUILayout.Label("增加的金钱：L,M,S");
                        itemTalkInfo.add_money = EditorGUILayout.TextArea(itemTalkInfo.add_money + "", GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.Label("增加的角色：id,id,id");
                        itemTalkInfo.add_character = EditorGUILayout.TextArea(itemTalkInfo.add_character + "", GUILayout.Width(50), GUILayout.Height(20));
                        GUILayout.Label("增加的物品：id,id,id");
                        itemTalkInfo.add_items = EditorGUILayout.TextArea(itemTalkInfo.add_items + "", GUILayout.Width(50), GUILayout.Height(20));
                    }
                }
                GUILayout.Label("增加的好感：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.add_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.add_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("对话顺序：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.text_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.text_order + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("指定下一句对话：", GUILayout.Width(120), GUILayout.Height(20));
                itemTalkInfo.next_order = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.next_order + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("触发条件-最低好感：", GUILayout.Width(120), GUILayout.Height(20));
                itemTalkInfo.condition_min_favorability = int.Parse(EditorGUILayout.TextArea(itemTalkInfo.condition_min_favorability + "", GUILayout.Width(50), GUILayout.Height(20)));
                GUILayout.Label("预设名字：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.name = EditorGUILayout.TextArea(itemTalkInfo.name + "", GUILayout.Width(50), GUILayout.Height(20));
                GUILayout.Label("对话内容：", GUILayout.Width(100), GUILayout.Height(20));
                itemTalkInfo.content = EditorGUILayout.TextArea(itemTalkInfo.content + "", GUILayout.Width(500), GUILayout.Height(20));
                if (GUILayout.Button("更新", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    textInfoService.UpdateDataById(TextEnum.Talk, itemTalkInfo.id, itemTalkInfo);
                }
                if (GUILayout.Button("删除对话", GUILayout.Width(120), GUILayout.Height(20)))
                {
                    removeTalkId = itemTalkInfo.id;
                    textInfoService.DeleteDataById(TextEnum.Talk, removeTalkId);
                }
                GUILayout.EndHorizontal();
            }
        }
        if (removeMarkId != 0)
            mapNpcTalkInfo.Remove(removeMarkId);
        if (removeTalkId != 0)
        {
            GetNpcTalkInfoList(mFindTalkType);
        }
    }

    private TextTalkTypeEnum mFindTalkType;
    /// <summary>
    /// 获取NPC对话数据
    /// </summary>
    private void GetNpcTalkInfoList(TextTalkTypeEnum talkType)
    {
        mFindTalkType = talkType;
        List<TextInfoBean> listNpcTalkInfo = textInfoService.QueryDataByTalkType(TextEnum.Talk, talkType, long.Parse(findIds));
        mapNpcTalkInfo.Clear();
        foreach (TextInfoBean itemTalkInfo in listNpcTalkInfo)
        {
            long markId = itemTalkInfo.mark_id;
            if (mapNpcTalkInfo.TryGetValue(markId, out List<TextInfoBean> value))
            {
                value.Add(itemTalkInfo);
            }
            else
            {
                List<TextInfoBean> listTemp = new List<TextInfoBean>();
                listTemp.Add(itemTalkInfo);
                mapNpcTalkInfo.Add(markId, listTemp);
            }
        }
    }

    private void GUINpcInfo(NpcInfoBean npcInfo, bool canSelectPic)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("获取位置数据", GUILayout.Width(100), GUILayout.Height(20)))
        {
            BaseNpcAI npcAI = mObjContent.GetComponentInChildren<BaseNpcAI>();
            npcInfo.position_x = npcAI.transform.position.x;
            npcInfo.position_y = npcAI.transform.position.y;
        }
        GUILayout.Label("NPCID：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.id = long.Parse(EditorGUILayout.TextArea(npcInfo.id + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcInfo.npc_id = npcInfo.id;
        npcInfo.npc_type = (int)(NPCTypeEnum)EditorGUILayout.EnumPopup("Npc类型：", (NPCTypeEnum)npcInfo.npc_type);
        GUILayout.Label("姓名：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.name = EditorGUILayout.TextArea(npcInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("性别：1男 2女", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.sex =int.Parse(EditorGUILayout.TextArea(npcInfo.sex + "", GUILayout.Width(30), GUILayout.Height(20)));
        GUILayout.Label("朝向 1左 2右", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.face = int.Parse(EditorGUILayout.TextArea(npcInfo.face + "", GUILayout.Width(30), GUILayout.Height(20)));
        GUILayout.Label("称号：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.title_name = EditorGUILayout.TextArea(npcInfo.title_name + "", GUILayout.Width(100), GUILayout.Height(20));
        GUILayout.Label("位置XY：", GUILayout.Width(100), GUILayout.Height(20));
        npcInfo.position_x = float.Parse(EditorGUILayout.TextArea(npcInfo.position_x + "", GUILayout.Width(100), GUILayout.Height(20)));
        npcInfo.position_y = float.Parse(EditorGUILayout.TextArea(npcInfo.position_y + "", GUILayout.Width(100), GUILayout.Height(20)));

        if (canSelectPic)
        {
            spCreateHair = EditorGUILayout.ObjectField(new GUIContent("头发", ""), spCreateHair, typeof(Sprite), true) as Sprite;
            if (spCreateHair)
            {
                npcInfo.hair_id = EditorGUILayout.TextArea(spCreateHair.name + "", GUILayout.Width(150), GUILayout.Height(20));
            }
            spCreateEye = EditorGUILayout.ObjectField(new GUIContent("眼睛", ""), spCreateEye, typeof(Sprite), true) as Sprite;
            if (spCreateEye)
            {
                npcInfo.eye_id = EditorGUILayout.TextArea(spCreateEye.name + "", GUILayout.Width(150), GUILayout.Height(20));
            }
            spCreateMouth = EditorGUILayout.ObjectField(new GUIContent("嘴巴", ""), spCreateMouth, typeof(Sprite), true) as Sprite;
            if (spCreateMouth)
            {
                npcInfo.mouth_id = EditorGUILayout.TextArea(spCreateMouth.name + "", GUILayout.Width(150), GUILayout.Height(20));
            }
        }
        else
        {
            GUILayout.Label("头发：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.hair_id = EditorGUILayout.TextArea(npcInfo.hair_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string hairPath = "Assets/Texture/Character/Hair/";
            GUIPic(hairPath, npcInfo.hair_id);

            GUILayout.Label("眼睛：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.eye_id = EditorGUILayout.TextArea(npcInfo.eye_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string eyePath = "Assets/Texture/Character/Eye/";
            GUIPic(eyePath, npcInfo.eye_id);

            GUILayout.Label("嘴巴：", GUILayout.Width(100), GUILayout.Height(20));
            npcInfo.mouth_id = EditorGUILayout.TextArea(npcInfo.mouth_id + "", GUILayout.Width(200), GUILayout.Height(20));
            string mouthPath = "Assets/Texture/Character/Mouth/";
            GUIPic(mouthPath, npcInfo.mouth_id);
        }
        GUILayout.Label("头发颜色：", GUILayout.Width(100), GUILayout.Height(20));
        ColorBean hairColorData = new ColorBean(npcInfo.hair_color);
        Color hairColor = hairColorData.GetColor(); ;
        hairColor = EditorGUILayout.ColorField(hairColor);
        npcInfo.hair_color = hairColor.r + "," + hairColor.g + "," + hairColor.b + "," + hairColor.a;

        GUILayout.Label("眼睛颜色：", GUILayout.Width(100), GUILayout.Height(20));
        ColorBean eyeColorData = new ColorBean(npcInfo.eye_color);
        Color eyeColor = eyeColorData.GetColor(); ;
        eyeColor = EditorGUILayout.ColorField(eyeColor);
        npcInfo.eye_color = eyeColor.r + "," + eyeColor.g + "," + eyeColor.b + "," + eyeColor.a;

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
        GUILayout.EndHorizontal();

    }

    private void GUIPic(string picPath, string picName)
    {
        Texture2D iconTex = EditorGUIUtility.FindTexture(picPath + picName + ".png");
        if (iconTex)
            GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
    }

    private GameObject ShowNpc(CharacterBean characterData)
    {
        CptUtil.RemoveChildsByActiveInEditor(mObjContent);
        GameObject objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
        objNpc.SetActive(true);
        objNpc.transform.position = new Vector3(characterData.npcInfoData.position_x, characterData.npcInfoData.position_y);
        BaseNpcAI npcAI = objNpc.GetComponent<BaseNpcAI>();
        npcAI.gameItemsManager = gameItemsManager;
        npcAI.SetCharacterData(characterData);
        mCamera2D.Follow = objNpc.transform;
        Camera.main.transform.position = new Vector3(objNpc.transform.position.x, objNpc.transform.position.y, Camera.main.transform.position.z);
        return objNpc;
    }
}
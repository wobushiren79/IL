using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcCreateWindowEidtor : EditorWindow
{
    GameItemsManager gameItemsManager;
    NpcInfoManager npcInfoManager;

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
        gameItemsManager = new GameItemsManager();
        npcInfoManager = new NpcInfoManager();
        gameItemsManager.Awake();
        npcInfoManager.Awake();
        gameItemsManager.itemsInfoController.GetAllItemsInfo();
        npcInfoManager.npcInfoController.GetAllNpcInfo();
    }

    public List<CharacterBean> listFindNpcData = new List<CharacterBean>();
    public Vector2 scrollPosition = Vector2.zero;

    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        mObjContent = EditorGUILayout.ObjectField(new GUIContent("Npc容器", ""), mObjContent, typeof(GameObject), true) as GameObject;
        mObjNpcModel = EditorGUILayout.ObjectField(new GUIContent("NPC模型", ""), mObjNpcModel, typeof(GameObject), true) as GameObject;

        GUIFindNpc();

        GUILayout.EndScrollView();
    }

    string findIds = "";
    private  void GUIFindNpc()
    {
        GUILayout.Label("查询NPC");
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPCId");
        findIds = EditorGUILayout.TextArea(findIds + "", GUILayout.Width(100), GUILayout.Height(20));
        if (GUILayout.Button("查询", GUILayout.Width(100), GUILayout.Height(20)))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(findIds,',');
            listFindNpcData= npcInfoManager.GetCharacterDataByIds(ids);
        }
        foreach (CharacterBean itemData in listFindNpcData)
        {

        }
        GUILayout.EndHorizontal();
    }

    private void CreateNpc(CharacterBean characterData)
    {
        GameObject objNpc = Instantiate(mObjNpcModel, mObjContent.transform);
        objNpc.SetActive(true);
        BaseNpcAI npcAI = objNpc.GetComponent<BaseNpcAI>();
        npcAI.SetCharacterData(characterData);
    }
}
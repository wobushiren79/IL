using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemCreateWindowsEditor : EditorWindow
{
    GameItemsManager gameItemsManager;

    [MenuItem("Tools/Window/ItemCreate")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(ItemCreateWindowsEditor));
    }

    public ItemCreateWindowsEditor()
    {
        this.titleContent = new GUIContent("物品创建工具");
    }

    private void OnEnable()
    {
        gameItemsManager = new GameItemsManager();
        gameItemsManager.Awake();
        gameItemsManager.itemsInfoController.GetAllItemsInfo();  
    }

    private Vector2 scrollPosition = Vector2.zero;
    
    public Sprite spriteCreateIcon;
    public GeneralEnum createItemType;

    public List<ItemsInfoBean> listFindItem = new List<ItemsInfoBean>();
    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();

        GUICreateItem();
        GUILayout.Label("------------------------------------------------------------------------------------------------");
        GUIFindItem();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    
    /// <summary>
    /// 创建Item的UI
    /// </summary>
    private void GUICreateItem()
    {
        GUILayout.Label("创建物品");
        createItemType = (GeneralEnum)EditorGUILayout.EnumPopup("物品类型", createItemType);
        spriteCreateIcon = EditorGUILayout.ObjectField(new GUIContent("选择图片", ""), spriteCreateIcon, typeof(Sprite), true) as Sprite;   
    }


    /// <summary>
    /// 查询Item的UI
    /// </summary>
    private void GUIFindItem()
    {
        GUILayout.Label("查询物品");
        if (GUILayout.Button("查询"))
        {
          
        }
        if (GUILayout.Button("查询所有"))
        {
            listFindItem = gameItemsManager.GetAllItems();
        }
        if (GUILayout.Button("查询所有服装"))
        {
            listFindItem = gameItemsManager.GetClothesList();
        }

        for (int i=0;i< listFindItem.Count;i++)
        {
            ItemsInfoBean itemInfo = listFindItem[i];
 
            GUILayout.BeginHorizontal();
            GUILayout.Label("物品ID：");
            EditorGUILayout.TextArea(itemInfo.id+"", GUILayout.Width(200), GUILayout.Height(20));

            string path = "Assets/Texture/";
            switch ((GeneralEnum)itemInfo.items_type)
            {
                case GeneralEnum.Hat:
                    path += "Character/Dress/Hat/";
                    break;
                case GeneralEnum.Clothes:
                    path += "Character/Dress/Clothes/";
                    break;
                case GeneralEnum.Shoes:
                    path += "Character/Dress/Shoes/";
                    break;
                case GeneralEnum.Mask:
                    path += "Character/Dress/Mask/";
                    break;
                case GeneralEnum.Chef:
                    path += "Items/Chef/";
                    break;
                case GeneralEnum.Waiter:
                    path += "Items/Waiter/";
                    break;
                case GeneralEnum.Accouting:
                    path += "Items/Accouting/";
                    break;
                case GeneralEnum.Accost:
                    path += "Items/Accost/";
                    break;
                case GeneralEnum.Beater:
                    path += "Items/Beater/";
                    break;
                default:
                    path += "Items/";
                    break;

            }
            path+= (itemInfo.icon_key + ".png");
            Texture2D iconTex= EditorGUIUtility.FindTexture(path);
            if(iconTex)
            {
                GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
               //EditorGUI.DrawPreviewTexture(new Rect(0, 0, 100, 100), iconTex);
               //spIcon = EditorGUILayout.ObjectField(new GUIContent("图标", ""), spIcon, typeof(Sprite), true) as Sprite;
            }
            GUILayout.Label("名字：");
            EditorGUILayout.TextArea(itemInfo.name + "", GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }
    }


}
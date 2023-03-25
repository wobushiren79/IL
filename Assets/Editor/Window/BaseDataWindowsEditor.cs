using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataWindowsEditor : EditorWindow
{
    public BaseDataService baseDataService;
    public List<BaseDataBean> listBaseData;

    [MenuItem("Tools/Window/BaseData")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(BaseDataWindowsEditor));
    }

    private Vector2 scrollPosition = Vector2.zero;

    private void OnEnable()
    {
        baseDataService = new BaseDataService();
        listBaseData = baseDataService.QueryAllData();
    }

    private void OnGUI()
    {
        //滚动布局
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();
        if (EditorUI.GUIButton("刷新数据"))
        {
            listBaseData = baseDataService.QueryAllData();
        }
        EditorUI.GUIText("-------------------------------------------", 500);

        if (listBaseData != null)
        {
            foreach (BaseDataBean itemData in listBaseData)
            {
                GUILayout.BeginHorizontal();
                BaseDataTypeEnum baseDataType = (BaseDataTypeEnum)itemData.id;
                EditorUI.GUIText("ID:" + itemData.id, 120);
                switch (baseDataType)
                {
                    case BaseDataTypeEnum.WorkerForLevelUpExp1:
                        EditorUI.GUIText("职业升级经验(等级1)：", 150);
                        break;
                    case BaseDataTypeEnum.WorkerForLevelUpExp2:
                        EditorUI.GUIText("职业升级经验(等级2)：", 150);
                        break;
                    case BaseDataTypeEnum.WorkerForLevelUpExp3:
                        EditorUI.GUIText("职业升级经验(等级3)：", 150);
                        break;
                    case BaseDataTypeEnum.WorkerForLevelUpExp4:
                        EditorUI.GUIText("职业升级经验(等级4)：", 150);
                        break;
                    case BaseDataTypeEnum.WorkerForLevelUpExp5:
                        EditorUI.GUIText("职业升级经验(等级5)：", 150);
                        break;
                    case BaseDataTypeEnum.WorkerForLevelUpExp6:
                        EditorUI.GUIText("职业升级经验(等级6)：", 150);

                        break;

                    case BaseDataTypeEnum.MenuForLevelUpExp1:
                        EditorUI.GUIText("菜品升级经验(星级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForLevelUpExp2:
                        EditorUI.GUIText("菜品升级经验(月级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForLevelUpExp3:
                        EditorUI.GUIText("菜品升级经验(阳级)：", 150);
                        break;

                    case BaseDataTypeEnum.MenuForPriceAddRate1:
                        EditorUI.GUIText("菜品价格等级加成(星级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForPriceAddRate2:
                        EditorUI.GUIText("菜品价格等级加成(月级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForPriceAddRate3:
                        EditorUI.GUIText("菜品价格等级加成(阳级)：", 150);
                        break;

                    case BaseDataTypeEnum.MenuForLevelResearchExp1:
                        EditorUI.GUIText("菜品升级研究经验(星级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForLevelResearchExp2:
                        EditorUI.GUIText("菜品升级研究经验(月级)：", 150);
                        break;
                    case BaseDataTypeEnum.MenuForLevelResearchExp3:
                        EditorUI.GUIText("菜品升级研究经验(阳级)：", 150);
                        break;
                }
                itemData.content = EditorUI.GUIEditorText(itemData.content, 200);
                GUILayout.EndHorizontal();
           
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}
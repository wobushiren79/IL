using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class UIGameManager : BaseUIManager
{
    [Header("初始化")]
    public BaseSceneInit sceneInit;
    //UI控件
    [Header("控件")]
    public InfoPromptPopupShow infoPromptPopup;
    public InfoItemsPopupShow infoItemsPopup;
    public InfoFoodPopupShow infoFoodPopup;
    public InfoAchievementPopupShow infoAchievementPopup;
    public InfoAbilityPopupShow infoAbilityPopup;
    public ItemsSelectionBox itemsSelectionBox;

    public ToastAchievementShow toastAchievement;
    //数据
    [Header("数据")]
    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;
    public CharacterBodyManager characterBodyManager;
    public InnBuildManager innBuildManager;
    public InnFoodManager innFoodManager;
    public NpcInfoManager npcInfoManager;
    //UI相关
    public DialogManager dialogManager;
    public ToastManager toastManager;
    //相关处理
    [Header("处理")]
    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public ControlHandler controlHandler;
    public EventHandler eventHandler;

    [Header("建造")]
    public NpcCustomerBuilder npcCustomerBuilder;

    [Header("地形")]
    public NavMeshSurface navMesh;
}
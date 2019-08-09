using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class UIGameManager : BaseUIManager
{
    //UI控件
    [Header("控件")]
    public InfoPromptPopupShow infoPromptPopup;
    public InfoItemsPopupShow infoItemsPopup;
    public InfoFoodPopupShow infoFoodPopup;
    public InfoAchievementPopupShow infoAchievementPopup;
    public InfoAbilityPopupShow infoAbilityPopup;
    public ItemsSelectionBox itemsSelectionBox;
    public ToastView toastView;
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

    //相关处理
    [Header("处理")]
    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public ControlHandler controlHandler;

    [Header("地形")]
    public NavMeshSurface2d navMesh;
}
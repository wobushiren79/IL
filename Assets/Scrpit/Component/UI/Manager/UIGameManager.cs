using UnityEngine;
using UnityEditor;

public class UIGameManager : BaseUIManager
{
    //UI控件
    [Header("控件")]
    public InfoPromptPopupShow InfoPromptPopup;
    public InfoItemsPopupShow infoItemsPopup;
    public InfoFoodPopupShow infoFoodPopup;
    public InfoAchievementPopupShow infoAchievementPopup;
    public InfoAbilityPopupShow infoAbilityPopup;

    public ItemsSelectionBox itemsSelectionBox;

    //数据
    [Header("数据")]
    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;
    public CharacterBodyManager characterBodyManager;

    public InnFoodManager innFoodManager;
    //UI相关
    public DialogManager dialogManager;

    //相关处理
    [Header("处理")]
    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public ControlHandler controlHandler;
}
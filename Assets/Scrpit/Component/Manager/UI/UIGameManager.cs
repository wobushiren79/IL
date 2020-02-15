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
    public PopupItemsSelection popupItemsSelection;
    //数据
    [Header("数据")]
    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;
    public CharacterBodyManager characterBodyManager;
    public InnBuildManager innBuildManager;
    public InnFoodManager innFoodManager;
    public NpcInfoManager npcInfoManager;
    public StoreInfoManager storeInfoManager;
    public IconDataManager iconDataManager;
    public AchievementInfoManager achievementInfoManager;
    public TextInfoManager textInfoManager;
    //UI相关
    public DialogManager dialogManager;
    public ToastManager toastManager;
    //相关处理
    [Header("处理")]
    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public ControlHandler controlHandler;
    public EventHandler eventHandler;
    public AudioHandler audioHandler;

    [Header("建造")]
    public NpcCustomerBuilder npcCustomerBuilder;
    public NpcEventBuilder npcEventBuilder;

    [Header("地形")]
    public NavMeshSurface navMesh;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        achievementInfoManager = Find<AchievementInfoManager>(ImportantTypeEnum.GameDataManager);

        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        characterBodyManager = Find<CharacterBodyManager>(ImportantTypeEnum.CharacterManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        textInfoManager= Find<TextInfoManager>(ImportantTypeEnum.TextManager);

        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);

        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);

        infoPromptPopup = FindInChildren<InfoPromptPopupShow>(ImportantTypeEnum.Popup);
        infoItemsPopup = FindInChildren<InfoItemsPopupShow>(ImportantTypeEnum.Popup);
        infoFoodPopup = FindInChildren<InfoFoodPopupShow>(ImportantTypeEnum.Popup);
        infoAchievementPopup = FindInChildren<InfoAchievementPopupShow>(ImportantTypeEnum.Popup);
        infoAbilityPopup = FindInChildren<InfoAbilityPopupShow>(ImportantTypeEnum.Popup);
        popupItemsSelection = FindInChildren<PopupItemsSelection>(ImportantTypeEnum.Popup);


        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }
}
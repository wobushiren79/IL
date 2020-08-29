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
    public InfoSkillPopupShow infoSkillPopup;
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
    public SkillInfoManager skillInfoManager;
    public NpcTeamManager npcTeamManager;
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
    public GameDataHandler gameDataHandler;
    public SteamHandler steamHandler;
    public FPSHandler fpsHandler;
    public SkillInfoHandler skillInfoHandler;
    [Header("建造")]
    public NpcCustomerBuilder npcCustomerBuilder;
    public NpcEventBuilder npcEventBuilder;
    public NpcWorkerBuilder npcWorkerBuilder;
    public InnFurnitureBuilder innFurnitureBuilder;
    public InnFloorBuilder innFloorBuilder;
    public InnWallBuilder innWallBuilder;
    [Header("小游戏")]
    public MiniGameCombatHandler miniGameCombatHandler;

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
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager); 
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        textInfoManager= Find<TextInfoManager>(ImportantTypeEnum.TextManager);
        skillInfoManager = Find<SkillInfoManager>(ImportantTypeEnum.SkillManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);

        skillInfoHandler = Find<SkillInfoHandler>(ImportantTypeEnum.SkillHandler);
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        gameDataHandler = Find<GameDataHandler>(ImportantTypeEnum.GameDataHandler);
        steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
        fpsHandler = Find<FPSHandler>( ImportantTypeEnum.Camera);
        infoPromptPopup = FindInChildren<InfoPromptPopupShow>(ImportantTypeEnum.Popup);
        infoItemsPopup = FindInChildren<InfoItemsPopupShow>(ImportantTypeEnum.Popup);
        infoFoodPopup = FindInChildren<InfoFoodPopupShow>(ImportantTypeEnum.Popup);
        infoAchievementPopup = FindInChildren<InfoAchievementPopupShow>(ImportantTypeEnum.Popup);
        infoAbilityPopup = FindInChildren<InfoAbilityPopupShow>(ImportantTypeEnum.Popup);
        infoSkillPopup = FindInChildren<InfoSkillPopupShow>(ImportantTypeEnum.Popup);
        popupItemsSelection = FindInChildren<PopupItemsSelection>(ImportantTypeEnum.Popup);



        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
        npcWorkerBuilder = Find<NpcWorkerBuilder>(ImportantTypeEnum.NpcBuilder);

        innFurnitureBuilder = Find<InnFurnitureBuilder>(ImportantTypeEnum.InnBuilder);
        innFloorBuilder = Find<InnFloorBuilder>(ImportantTypeEnum.InnBuilder);
        innWallBuilder = Find<InnWallBuilder>(ImportantTypeEnum.InnBuilder);

        miniGameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
    }

}
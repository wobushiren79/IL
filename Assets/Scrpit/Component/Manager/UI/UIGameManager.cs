using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class UIGameManager : BaseUIManager
{
    [Header("初始化")]
    public BaseSceneInit sceneInit;
    //UI控件
    [Header("控件")]
    public PopupItemsSelection popupItemsSelection;
    //数据
    [Header("数据")]
    public GameDataManager gameDataManager;
    public InnBuildManager innBuildManager;
    public InnFoodManager innFoodManager;
    public NpcInfoManager npcInfoManager;
    public StoreInfoManager storeInfoManager;
    public IconDataManager iconDataManager;
    public AchievementInfoManager achievementInfoManager;
    public TextInfoManager textInfoManager;
    public SkillInfoManager skillInfoManager;
    public NpcTeamManager npcTeamManager;
    //相关处理
    [Header("处理")]
    public InnHandler innHandler;
    public GameTimeHandler gameTimeHandler;
    public ControlHandler controlHandler;
    public EventHandler eventHandler;
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
        sceneInit= Find<BaseSceneInit>(ImportantTypeEnum.Init);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        achievementInfoManager = Find<AchievementInfoManager>(ImportantTypeEnum.GameDataManager);

        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager); 
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        textInfoManager= Find<TextInfoManager>(ImportantTypeEnum.TextManager);
        skillInfoManager = Find<SkillInfoManager>(ImportantTypeEnum.SkillManager);

        skillInfoHandler = Find<SkillInfoHandler>(ImportantTypeEnum.SkillHandler);
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        gameDataHandler = Find<GameDataHandler>(ImportantTypeEnum.GameDataHandler);
        steamHandler = Find<SteamHandler>(ImportantTypeEnum.Steam);
        fpsHandler = Find<FPSHandler>( ImportantTypeEnum.Camera);

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
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

    [Header("地形")]
    public NavMeshSurface navMesh;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        characterBodyManager = Find<CharacterBodyManager>(ImportantTypeEnum.CharacterManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
        iconDataManager= Find<IconDataManager>(ImportantTypeEnum.UIManager);

        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);

        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }
}
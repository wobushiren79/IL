using UnityEngine;
using UnityEditor;

public class BaseSceneInit : BaseMonoBehaviour
{

    public UIGameManager uiGameManager;
    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public NpcInfoManager npcInfoManager;
    public DialogManager dialogManager;
    public StoryInfoManager storyInfoManager;

    public ControlHandler controlHandler;

    public void Start()
    {
        if (gameDataManager != null)
        {
            if (GameCommonInfo.GameData != null)
            {
                gameDataManager.gameData = GameCommonInfo.GameData;
            }
            else
            {
                gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.GameUserId);
            }
        }
    }

}
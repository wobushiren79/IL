using UnityEngine;
using UnityEditor;

public class SceneGameTownInit : BaseMonoBehaviour
{
    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;
    public NpcInfoManager npcInfoManager;

    public NpcImportantBuilder npcImportantBuilder;
    private void Start()
    {
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        //构建重要的NPC
        if (npcImportantBuilder!=null)
            npcImportantBuilder.BuildImportant();
    }
}
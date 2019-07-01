using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseMonoBehaviour
{

    // 角色着装管理
    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;

    private void Start()
    {
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetSimpleGameData();

    }
}
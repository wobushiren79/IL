using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseSceneInit
{
    private void Start()
    {
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetSimpleGameData();
    }
}
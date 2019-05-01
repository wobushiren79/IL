using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseMonoBehaviour
{

    // 角色着装管理
    public CharacterDressManager characterDressManager;
    public GameDataManager gameDataManager;

    private void Start()
    {
        if (characterDressManager != null)
            characterDressManager.equipInfoController.GetAllEquipInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetSimpleGameData();

    }
}
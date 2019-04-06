using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneGameInnInit : BaseManager
{
    public CharacterDressManager characterDressManager;
    public GameDataManager gameDataManager;
    public NpcInfoManager npcInfoManager;

    private void Start()
    {
        if (characterDressManager != null)
            characterDressManager.equipInfoController.GetAllEquipInfo();
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
    }

}
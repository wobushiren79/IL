using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseMonoBehaviour
{

    public long[] initEquipIdList = new long[] {
       10000, 10001,10002,10003,10004,
       20000, 20001,20002,20003,20004,
       30000, 30001,30002,30003,30004,
    };

    // 角色着装管理
    public CharacterDressManager characterDressManager;
    public GameDataManager gameDataManager;

    private void Start()
    {
        if (characterDressManager != null)
            characterDressManager.equipInfoController.GetEquipInfoByIds(initEquipIdList);
        if (gameDataManager != null)
            gameDataManager.gameDataController.GetSimpleGameData();
    }
}
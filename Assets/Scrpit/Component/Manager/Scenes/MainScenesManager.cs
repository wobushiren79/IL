using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainScenesManager : BaseManager
{
    public long[] initEquipIdList = new long[] {
       10000, 10001,10002,10003,10004
    };

    /// <summary>
    /// 角色着装管理
    /// </summary>
    public CharacterDressManager characterDressManager;

    private void Start()
    {
        characterDressManager.equipInfoController.GetEquipInfoByIds(initEquipIdList);
    }


}
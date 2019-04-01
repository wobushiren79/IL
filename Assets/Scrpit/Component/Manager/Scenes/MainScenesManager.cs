using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainScenesManager : BaseManager
{
    public long[] initEquipIdList = new long[] {
       10000, 10001,10002,10003,10004,
       20000, 20001,20002,20003,20004,
       30000, 30001,30002,30003,30004,
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
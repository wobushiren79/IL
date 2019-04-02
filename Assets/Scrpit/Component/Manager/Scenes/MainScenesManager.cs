using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainScenesManager : BaseManager,IGameDataView
{
    public long[] initEquipIdList = new long[] {
       10000, 10001,10002,10003,10004,
       20000, 20001,20002,20003,20004,
       30000, 30001,30002,30003,30004,
    };

    public GameDataBean gameData;

    /// <summary>
    /// 角色着装管理
    /// </summary>
    public CharacterDressManager characterDressManager;
    public GameDataController gameDataController;

    private void Awake()
    {
        gameDataController = new GameDataController(this, this);
    }

    private void Start()
    {
        characterDressManager.equipInfoController.GetEquipInfoByIds(initEquipIdList);
    }

    public void CreateNewData()
    {
        gameDataController.CreateUserData(gameData);
    }

    #region 数据回调
    public void GetGameDataFail()
    {
       
    }

    public void GetGameDataSimpleListFail()
    {
       
    }

    public void GetGameDataSimpleListSuccess(List<GameDataSimpleBean> listData)
    {
      
    }

    public void GetGameDataSuccess(GameDataBean gameData)
    {
       
    }

    public void SetGameDataFail()
    {
       
    }

    public void SetGameDataSuccess()
    {
        SceneUtil.SceneChange("GameInnScenes");
    }
    #endregion
}
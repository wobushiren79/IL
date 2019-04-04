using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MainScenesManager : BaseManager,IGameDataView{

    public long[] initEquipIdList = new long[] {
       10000, 10001,10002,10003,10004,
       20000, 20001,20002,20003,20004,
       30000, 30001,30002,30003,30004,
    };

    // 角色着装管理
    public CharacterDressManager characterDressManager;
    //游戏数据控制
    public GameDataController gameDataController;

    //简略游戏数据
    public List<GameDataSimpleBean> listGameDataSimple;

    private void Awake()
    {
        gameDataController = new GameDataController(this, this);
    }

    private void Start()
    {
        characterDressManager.equipInfoController.GetEquipInfoByIds(initEquipIdList);
        gameDataController.GetSimpleGameData();
    }

    public void CreateNewData(GameDataBean gameData)
    {
        gameDataController.CreateUserData(gameData);
    }

    public void DeleteGameDataByUserId(string userId)
    {
        gameDataController.DeleteUserDataByUserId(userId);
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
        this.listGameDataSimple = listData;
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

    public void DeleteGameDataSuccess()
    {
        gameDataController.GetSimpleGameData();
    }

    public void DeleteGameDataFail()
    {

    }
    #endregion
}
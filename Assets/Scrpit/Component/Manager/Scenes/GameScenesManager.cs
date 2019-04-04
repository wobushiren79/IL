using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameScenesManager : BaseManager,IGameDataView
{
    public GameDataBean gameData;

    public GameDataController gameDataController;

    private void Awake()
    {
        gameDataController = new GameDataController(this,this);
    }

    private void Start()
    {
        gameDataController.GetGameDataByUserId(GameCommonInfo.gameUserId);
    }

    #region 数据回调
    public void DeleteGameDataFail()
    {
    }

    public void DeleteGameDataSuccess()
    {
    }

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
        this.gameData = gameData;
    }

    public void SetGameDataFail()
    {
    }

    public void SetGameDataSuccess()
    {
    }
    #endregion
}
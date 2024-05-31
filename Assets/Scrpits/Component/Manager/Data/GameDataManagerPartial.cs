using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public partial class GameDataManager : BaseManager, IGameDataView, IUserRevenueView, IGameConfigView
{
    //游戏数据
    public GameDataBean gameData;

    //游戏数据控制
    public GameDataController controllerForGameData;
    public UserRevenueController controllerForUserRevenue;

    private void Awake()
    {
        gameData = new GameDataBean();
        controllerForGameData = new GameDataController(this, this);
        controllerForUserRevenue = new UserRevenueController(this, this);
        controllerForGameConfig = new GameConfigController(this,this);
        controllerForGameConfig.GetGameConfigData();
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public GameDataBean GetGameData()
    {
        return gameData;
    }

    /// <summary>
    ///  获取用户数据
    /// </summary>
    /// <param name="gameUserId"></param>
    public void GetGameDataByUserId(string gameUserId)
    {
        controllerForGameData.GetGameDataByUserId(gameUserId);
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void DeleteGameDataByUserId(string userId)
    {
        controllerForGameData.DeleteUserDataByUserId(userId);
    }

    /// <summary>
    /// 创建新数据
    /// </summary>
    /// <param name="gameDataBean"></param>
    public void CreateGameData(GameDataBean gameDataBean)
    {
        controllerForGameData.CreateUserData(gameDataBean);
    }


    /// <summary>
    /// 获取简介数据
    /// </summary>
    public void GetSimpleGameDataList(Action<List<GameDataSimpleBean>> action)
    {
        controllerForGameData.GetSimpleGameData(action);
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="innRecord">客栈流水</param>
    public void SaveGameData(InnRecordBean innRecordData)
    {
        controllerForGameData.SaveUserData(gameData);
        controllerForUserRevenue.SetUserRevenue(gameData.userId,innRecordData);
    }

    /// <summary>
    /// 获取营收数据
    /// </summary>
    /// <param name="year"></param>
    public void GetUserRevenueByYear(int year, Action<UserRevenueBean> action)
    {
        if (gameData == null)
            return;
        controllerForUserRevenue.GetUserRevenueByYear(gameData.userId, year, action);
    }

    /// <summary>
    /// 获取所有营收年份
    /// </summary>
    public void GetUserRevenueYear(Action<List<int>> action)
    {
        if (gameData == null)
            return;
        controllerForUserRevenue.GetUserRevenueYear(gameData.userId, action);
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

    public void GetGameDataSimpleListSuccess(List<GameDataSimpleBean> listData, Action<List<GameDataSimpleBean>> action)
    {
        action?.Invoke(listData);
    }

    public void GetGameDataSuccess(GameDataBean gameData)
    {
        if (gameData == null)
            gameData = new GameDataBean();
        this.gameData = gameData;
    }

    public void SetGameDataFail()
    {
    }

    public void SetGameDataSuccess()
    {

    }

    public void GetUserRevenueFail()
    {
    }

    public void GetUserRevenueYearSuccess(List<int> listYear, Action<List<int>> action)
    {
        action?.Invoke(listYear);
    }

    public void GetUserRevenueSuccess(UserRevenueBean userRevenue, Action<UserRevenueBean> action)
    {
        action?.Invoke(userRevenue);
    }
    #endregion
}
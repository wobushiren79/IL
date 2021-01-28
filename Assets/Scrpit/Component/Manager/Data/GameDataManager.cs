using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class GameDataManager : BaseManager, IGameDataView, IUserRevenueView
{
    //游戏数据控制
    public GameDataController gameDataController;
    public UserRevenueController userRevenueController;

    //游戏数据
    protected GameDataBean gameData;

    private void Awake()
    {
        gameData = new GameDataBean();
        gameDataController = new GameDataController(this, this);
        userRevenueController = new UserRevenueController(this, this);
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
        gameDataController.GetGameDataByUserId(gameUserId);
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void DeleteGameDataByUserId(string userId)
    {
        gameDataController.DeleteUserDataByUserId(userId);
    }

    /// <summary>
    /// 创建新数据
    /// </summary>
    /// <param name="gameDataBean"></param>
    public void CreateGameData(GameDataBean gameDataBean)
    {
        gameDataController.CreateUserData(gameDataBean);
    }


    /// <summary>
    /// 获取简介数据
    /// </summary>
    public void GetSimpleGameDataList(Action<List<GameDataSimpleBean>> action)
    {
        gameDataController.GetSimpleGameData(action);
    }


    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="innRecord">客栈流水</param>
    public void SaveGameData(InnRecordBean innRecordData)
    {
        gameDataController.SaveUserData(gameData);
        userRevenueController.SetUserRevenue(gameData.userId,innRecordData);
    }

    /// <summary>
    /// 获取营收数据
    /// </summary>
    /// <param name="year"></param>
    public void GetUserRevenueByYear(int year, Action<UserRevenueBean> action)
    {
        if (gameData == null)
            return;
        userRevenueController.GetUserRevenueByYear(gameData.userId, year, action);
    }

    /// <summary>
    /// 获取所有营收年份
    /// </summary>
    public void GetUserRevenueYear(Action<List<int>> action)
    {
        if (gameData == null)
            return;
        userRevenueController.GetUserRevenueYear(gameData.userId, action);
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
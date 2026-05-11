using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public partial class GameDataManager : BaseManager
{
    //游戏数据
    public GameDataBean gameData;

    //数据服务
    protected GameDataService gameDataService;
    protected UserRevenueService userRevenueService;

    private void Awake()
    {
        gameData = new GameDataBean();
        gameDataService = new GameDataService();
        userRevenueService = new UserRevenueService();
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
        gameData = gameDataService.QueryDataByUserId(gameUserId);
        if (gameData == null)
            gameData = new GameDataBean();
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void DeleteGameDataByUserId(string userId)
    {
        gameDataService.DeleteDataByUserId(userId);
    }

    /// <summary>
    /// 创建新数据
    /// </summary>
    /// <param name="gameDataBean"></param>
    public void CreateGameData(GameDataBean gameDataBean)
    {
        if (gameDataBean != null)
            gameDataService.UpdateDataByUserId(gameDataBean.userId, gameDataBean);
    }


    /// <summary>
    /// 获取简介数据
    /// </summary>
    public void GetSimpleGameDataList(Action<List<GameDataSimpleBean>> action)
    {
        GameListDataService gameListDataService = new GameListDataService();
        List<GameDataSimpleBean> listData = gameListDataService.QueryData();
        action?.Invoke(listData);
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="innRecord">客栈流水</param>
    public void SaveGameData(InnRecordBean innRecordData)
    {
        if (gameData != null)
            gameDataService.UpdateDataByUserId(gameData.userId, gameData);
        if (gameData != null && innRecordData != null)
        {
            UserRevenueBean userRevenueData = userRevenueService.QueryDataByYear(gameData.userId, innRecordData.year);
            if (userRevenueData == null)
            {
                userRevenueData = new UserRevenueBean();
                userRevenueData.year = innRecordData.year;
                userRevenueData.userId = gameData.userId;
            }
            if (userRevenueData.listMonthData == null)
                userRevenueData.listMonthData = new List<UserRevenueMonthBean>();
            bool hasMonthData = false;
            foreach (UserRevenueMonthBean itemMonth in userRevenueData.listMonthData)
            {
                if (itemMonth.month == innRecordData.month)
                {
                    if (itemMonth.listDayData == null)
                        itemMonth.listDayData = new List<InnRecordBean>();
                    itemMonth.listDayData.Add(innRecordData);
                    hasMonthData = true;
                }
            }
            if (!hasMonthData)
            {
                UserRevenueMonthBean itemMonth = new UserRevenueMonthBean();
                itemMonth.month = innRecordData.month;
                itemMonth.listDayData = new List<InnRecordBean>();
                itemMonth.listDayData.Add(innRecordData);
                userRevenueData.listMonthData.Add(itemMonth);
            }
            userRevenueService.UpdateDataByYear(userRevenueData);
        }
    }

    /// <summary>
    /// 获取营收数据
    /// </summary>
    /// <param name="year"></param>
    public void GetUserRevenueByYear(int year, Action<UserRevenueBean> action)
    {
        if (gameData == null)
            return;
        UserRevenueBean userRevenue = userRevenueService.QueryDataByYear(gameData.userId, year);
        action?.Invoke(userRevenue);
    }

    /// <summary>
    /// 获取所有营收年份
    /// </summary>
    public void GetUserRevenueYear(Action<List<int>> action)
    {
        if (gameData == null)
            return;
        List<int> listYear = userRevenueService.QueryYear(gameData.userId);
        action?.Invoke(listYear);
    }
}
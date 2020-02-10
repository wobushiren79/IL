using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class UserRevenueService : BaseDataStorageImpl<UserRevenueBean>
{
    public UserRevenueService()
    {

    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public UserRevenueBean QueryDataByYear(string userId, int year)
    {
        string filePath = "Revenue_" + year;
        return BaseLoadData(userId + "/" + filePath);
        //UserRevenueBean userRevenue = new UserRevenueBean();
        //userRevenue.year = year;
        //userRevenue.userId = userId;
        //userRevenue.listMonthData = new List<UserRevenueMonthBean>();
        //for (int i = 0; i < 4; i++)
        //{
        //    UserRevenueMonthBean userRevenueMonth = new UserRevenueMonthBean();
        //    userRevenueMonth.month = i + 1;
        //    userRevenueMonth.listDayData = new List<InnRecordBean>();
        //    int days = Random.Range(1, 30);
        //    for (int f = 0; f < days; f++)
        //    {
        //        InnRecordBean userRevenueDay = new InnRecordBean();
        //        userRevenueDay.year = userRevenue.year;
        //        userRevenueDay.month = userRevenueMonth.month;
        //        userRevenueDay.day = f + 1;
        //        userRevenueDay.status = Random.Range(0, 2);
        //        if (userRevenueDay.status != 0)
        //        {
        //            userRevenueDay.incomeL = Random.Range(0, 10);
        //            userRevenueDay.incomeM = Random.Range(10, 100);
        //            userRevenueDay.incomeS = Random.Range(100, 9999);

        //            userRevenueDay.expensesL = Random.Range(0, 10);
        //            userRevenueDay.expensesM = Random.Range(10, 100);
        //            userRevenueDay.expensesS = Random.Range(100, 9999);
        //            userRevenueDay.praiseExcitedNumber = 1;
        //            userRevenueDay.praiseHappyNumber = 2;
        //            userRevenueDay.praiseOkayNumber = 3;
        //            userRevenueDay.praiseOrdinaryNumber = 4;
        //            userRevenueDay.praiseDisappointedNumber = 5;
        //            userRevenueDay.praiseAngerNumber = 6;
        //            userRevenueDay.consumeIngOilsalt = 1;
        //            userRevenueDay.consumeIngMeat = 2;
        //            userRevenueDay.consumeIngRiverfresh = 3;
        //            userRevenueDay.consumeIngSeafood = 4;
        //            userRevenueDay.consumeIngVegetables = 5;
        //            userRevenueDay.consumeIngMelonfruit = 6;
        //            userRevenueDay.consumeIngWaterwine = 7;
        //            userRevenueDay.consumeIngFlour = 8;

        //            userRevenueDay.listSellNumber.Add(new GameItemsBean(1, 2, 3, 4, 5));
        //            userRevenueDay.listSellNumber.Add(new GameItemsBean(2, 2, 3, 4, 5));
        //            userRevenueDay.listSellNumber.Add(new GameItemsBean(3, 2, 3, 4, 5));
        //            userRevenueDay.listSellNumber.Add(new GameItemsBean(4, 2, 3, 4, 5));
        //        }
        //        userRevenueMonth.listDayData.Add(userRevenueDay);
        //    }
        //    userRevenue.listMonthData.Add(userRevenueMonth);
        //}
        //return userRevenue;
    }

    /// <summary>
    /// 查询所有年份
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public List<int> QueryYear(string userId)
    {
        FileInfo[] filesInfo = FileUtil.GetFilesByPath(dataStoragePath + "/" + userId);
        List<int> listYear = new List<int>();
        foreach (FileInfo itemFile in filesInfo)
        {
            string fileName = itemFile.Name;
            if (fileName.Contains("Revenue_"))
            {
                string yearStr = fileName.Replace("Revenue_", "");
                listYear.Add(int.Parse(yearStr));
            }
        }
        return listYear;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateDataByYear(UserRevenueBean gameData)
    {
        string filePath = "Revenue_" + gameData.year;
        FileUtil.CreateDirectory(dataStoragePath + "/" + gameData.userId);
        BaseSaveData(gameData.userId + "/" + filePath, gameData);
    }

    /// <summary>
    /// 删除用户数据
    /// </summary>
    public void DeleteDataByYear(string userId, int year)
    {
        string filePath = "Revenue_" + year;
        BaseDeleteFile(userId + "/" + filePath);
    }
}
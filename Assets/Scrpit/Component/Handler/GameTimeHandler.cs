using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameTimeHandler : BaseObservable<IBaseObserver>
{
    public enum DayEnum
    {
        Rest,
        Work,
    }

    public enum NotifyTypeEnum
    {
        NewDay,//新的一天
        Night//夜晚
    }

    public GameDataManager gameDataManager;

    public float hour;
    public float min;

    public DayEnum dayStauts = DayEnum.Rest;

    //是否停止时间
    public bool isStopTime = true;
    //时间流逝速度
    public float timeSclae = 1;

    private void Update()
    {
        if (!isStopTime)
        {
            TimeLapse();
        }
    }


    /// <summary>
    /// 进行下一天
    /// </summary>
    public void GoToNextDay(int nextDay)
    {
        TimeBean timeData = gameDataManager.gameData.gameTime;

        for (int i = 0; i < nextDay; i++)
        {
            timeData.day += 1;
            if (timeData.day > 30)
            {
                timeData.day = 1;
                timeData.month += 1;
            }
            if (timeData.month > 12)
            {
                timeData.month = 1;
                timeData.year += 1;
            }
        }
        SetNewDay();
    }

    /// <summary>
    /// 时间流逝
    /// </summary>
    public void TimeLapse()
    {
        min += Time.deltaTime * timeSclae;
        if (min >= 60)
        {
            min = 0;
            hour += 1;

            //晚上通知
            if (hour == 18)
            {
                NotifyAllObserver((int)NotifyTypeEnum.Night, null);
            }
        }
        if (hour >= 24)
        {
            SetTimeStatus(true);
            //TODO 一天时间结束处理
        }
        TimeBean timeData = gameDataManager.gameData.gameTime;
        timeData.SetTimeForHM((int)hour, (int)min);
    }

    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void SetNewDay()
    {
        SetTimeStatus(true);
        hour = 6;
        min = 0;
        //通知新的一天
        NotifyAllObserver((int)NotifyTypeEnum.NewDay, null);
    }

    /// <summary>
    /// 设置时间状态
    /// </summary>
    /// <param name="isStopTime"></param>
    public void SetTimeStatus(bool isStopTime)
    {
        this.isStopTime = isStopTime;
    }

    /// <summary>
    /// 获取日期
    /// </summary>
    /// <param name="hours"></param>
    /// <param name="min"></param>
    public void GetTime(out float hour, out float min)
    {
        hour = this.hour;
        min = this.min;
    }

    /// <summary>
    /// 获取日期
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void GetTime(out int year, out int month, out int day)
    {
        TimeBean timeData = gameDataManager.gameData.gameTime;
        year = timeData.year;
        month = timeData.month;
        day = timeData.day;
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    public void SetTime(int hour,int min)
    {
        this.hour = (float)hour;
        this.min = (float)min;
    }
}
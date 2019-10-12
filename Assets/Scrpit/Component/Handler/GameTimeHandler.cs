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
        Night,//夜晚
        EndDay,
        TimePoint,//整点报时
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
            if (timeData.day > 28)
            {
                timeData.day = 1;
                timeData.month += 1;
            }
            if (timeData.month > 4)
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
            NotifyAllObserver((int)NotifyTypeEnum.TimePoint, hour);
        }
        if (hour >= 24)
        {
            SetTimeStatus(true);
            //TODO 一天时间结束处理
            NotifyAllObserver((int)NotifyTypeEnum.EndDay, null);
        }
        TimeBean timeData = gameDataManager.gameData.gameTime;
        timeData.SetTimeForHM((int)hour, (int)min);
    }

    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void SetNewDay()
    {
        //初始化世界种子
        GameCommonInfo.RandomSeed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(GameCommonInfo.RandomSeed);
        //解除每日
        GameCommonInfo.DailyLimitData = new UserDailyLimitBean();
        //初始化时间
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
    /// 设置时间尺度
    /// </summary>
    /// <param name="scale"></param>
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    /// <summary>
    /// 设置时间彻底停止
    /// </summary>
    public void SetTimeStop()
    {
        SetTimeScale(0);
        SetTimeStatus(true);
    }

    /// <summary>
    /// 设置时间恢复
    /// </summary>
    public void SetTimeRestore()
    {
        SetTimeScale(1);
        SetTimeStatus(false);
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
    public void SetTime(int hour, int min)
    {
        this.hour = (float)hour;
        this.min = (float)min;
    }

    /// <summary>
    /// 添加时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    public void AddHour(int addHour)
    {
        if (hour < 18 && (hour + addHour) >= 18)
        {
            NotifyAllObserver((int)NotifyTypeEnum.Night, null);
        }
        hour += addHour;
    }
}
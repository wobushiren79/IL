using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

public class GameTimeHandler : BaseHandler<GameTimeHandler, GameTimeManager>
{
    public enum DayEnum
    {
        Rest = 0,
        Work = 1,
        End = 9,//结束 晚上12点后
        None = 99,
    }

    public enum NotifyTypeEnum
    {
        NewDay,//新的一天
        EndDay,
        TimePoint,//整点报时
    }

    protected Action<NotifyTypeEnum,float>  notifyForTime;

    public float hour;
    public float min;

    public DayEnum dayStauts = DayEnum.None;

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
    /// 注册时间通知
    /// </summary>
    /// <param name="notifyForTime"></param>
    public void RegisterNotifyForTime(Action<NotifyTypeEnum,float> notifyForTime)
    {
        this.notifyForTime += notifyForTime;
    }
    public void UnRegisterNotifyForTime(Action<NotifyTypeEnum, float> notifyForTime)
    {
        this.notifyForTime -= notifyForTime;
    }
    /// <summary>
    /// 进行下一天
    /// </summary>
    public void GoToNextDay(int nextDay)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        TimeBean timeData = gameData.gameTime;

        for (int i = 0; i < nextDay; i++)
        {
            timeData.day += 1;
            if (timeData.day > 42)
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
    /// 获取当前日期之后几天的日期
    /// </summary>
    /// <param name="afterDay"></param>
    /// <returns></returns>
    public TimeBean GetAfterDay(int afterDay)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        TimeBean timeData = gameData.gameTime;
        int tempYear = timeData.year;
        int tempMonth = timeData.month;
        int tempDay = timeData.day;
        for (int i = 0; i < afterDay; i++)
        {
            tempDay += 1;
            if (tempDay > 42)
            {
                tempDay = 1;
                tempMonth += 1;
            }
            if (tempMonth > 4)
            {
                tempMonth = 1;
                tempYear += 1;
            }
        }
        TimeBean tempTimeData = new TimeBean();
        tempTimeData.SetTimeForYMD(tempYear, tempMonth, tempDay);
        return tempTimeData;
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

            //整点通知
            notifyForTime?.Invoke(NotifyTypeEnum.TimePoint, hour);
        }
        if (hour >= 24)
        {
            SetTimeStatus(true);
            SetTimeScale(1);
            SystemUtil.GCCollect();
            //TODO 一天时间结束处理
            notifyForTime?.Invoke(NotifyTypeEnum.EndDay,-1);
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        TimeBean timeData = gameData.gameTime;
        timeData.SetTimeForHM((int)hour, (int)min);
    }

    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void SetNewDay()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //初始化世界种子
        GameCommonInfo.RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        GameCommonInfo.InitRandomSeed();
        //解除每日
        GameCommonInfo.DailyLimitData.InitData(gameData);
        //初始化时间
        SetTimeStatus(true);
        hour = 6;
        min = 0;
        //如果有建筑日则建筑日减一天
        InnBuildBean innBuildData = gameData.GetInnBuildData();
        if (innBuildData.listBuildDay.Count > 0)
        {
            //检测当前日子是否包含在建筑日内
            TimeBean timeData = gameData.gameTime;
            bool isBuildDay = false;
            foreach (TimeBean itemTime in innBuildData.listBuildDay)
            {
                if (itemTime.year == timeData.year && itemTime.month == timeData.month && itemTime.day == timeData.day)
                {
                    isBuildDay = true;
                }
            }
            if (!isBuildDay)
            {
                innBuildData.listBuildDay.Clear();
                //检测是1楼还是2楼
                if (innBuildData.buildInnWidth != 0 || innBuildData.buildInnHeight != 0)
                {
                    innBuildData.ChangeInnSize(1, innBuildData.buildInnWidth, innBuildData.buildInnHeight);
                    innBuildData.buildInnWidth = 0;
                    innBuildData.buildInnHeight = 0;
                }
                else if (innBuildData.buildInnSecondWidth != 0 || innBuildData.buildInnSecondHeight != 0)
                {
                    innBuildData.ChangeInnSize(2, innBuildData.buildInnSecondWidth, innBuildData.buildInnSecondHeight);
                    innBuildData.buildInnSecondWidth = 0;
                    innBuildData.buildInnSecondHeight = 0;
                }


                InnBuildHandler.Instance.builderForFloor.StartBuild();
                InnBuildHandler.Instance.builderForWall.StartBuild();
            }
        }
        //通知新的一天
        notifyForTime?.Invoke(NotifyTypeEnum.NewDay, -1);
        SystemUtil.GCCollect();
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

    public float GetTimeScale()
    {
        return Time.timeScale;
    }

    /// <summary>
    /// 设置时间停止
    /// </summary>
    public void SetTimeStop()
    {
        SetTimeStatus(true);
    }

    /// <summary>
    /// 设置时间恢复
    /// </summary>
    public void SetTimeRestore()
    {
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        TimeBean timeData = gameData.gameTime;
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        this.hour = (float)hour;
        this.min = (float)min;
        gameData.gameTime.SetTimeForHM((int)this.hour, (int)this.min);

        GameLightHandler.instance.CheckTime();
    }

    /// <summary>
    /// 添加时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    public void AddHour(int addHour)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        hour += addHour;
        gameData.gameTime.hour = (int)hour;

        GameLightHandler.instance.CheckTime();
    }

    /// <summary>
    /// 获取当天状态
    /// </summary>
    /// <returns></returns>
    public DayEnum GetDayStatus()
    {
        return dayStauts;
    }

    /// <summary>
    /// 设置按天状态
    /// </summary>
    /// <param name="dayStauts"></param>
    public void SetDayStatus(DayEnum dayStauts)
    {
        this.dayStauts = dayStauts;
        GameCommonInfo.CurrentDayData.dayStatus = this.dayStauts;
    }
}
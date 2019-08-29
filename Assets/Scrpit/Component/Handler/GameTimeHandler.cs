using UnityEngine;
using UnityEditor;
using System.Collections;
public class GameTimeHandler : BaseMonoBehaviour
{
    public enum DayEnum
    {
        Rest,
        Work,
    }

    public GameDataManager gameDataManager;

    public BaseUIManager uiManager;

    public float hours;
    public float min;

    public DayEnum dayStauts = DayEnum.Rest;

    //是否停止时间
    public bool isStopTime = true;
    //时间流逝速度
    public float timeSclae = 1;

    private void Start()
    {
        SetNewDay();
    }

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
            hours += 1;
        }
        if (hours >= 24)
        {
            SetTimeStatus(true);
            //TODO 一天时间结束处理
        }
    }

    /// <summary>
    /// 开始新的一天
    /// </summary>
    public void SetNewDay()
    {
        SetTimeStatus(true);
        hours = 6;
        min = 0;
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
    public void GetTime(out float hours, out float min)
    {
        hours = this.hours;
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
}
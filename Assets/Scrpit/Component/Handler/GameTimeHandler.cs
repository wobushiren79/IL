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
    public InnHandler innHandler;

    public float hours;
    public float min;
    public DayEnum dayStauts = DayEnum.Rest;
    //时钟
    public ClockView clockView;

    //是否停止时间
    public bool isStopTime = true;

    public float timeSclae = 1;

    private void Start()
    {
        StartNewDay(true);
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
    }


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
            isStopTime = true;
            if (uiManager != null)
                if (uiManager.GetOpenUI() != null && uiManager.GetOpenUI().name.Equals("Settle"))
                {

                }
                else
                {
                    uiManager.OpenUIAndCloseOtherByName("Settle");
                }
            if (innHandler != null)
                innHandler.CloseInn();
        }
        clockView.SetTime((int)hours, (int)min);
        float sunColor = 1;
        if (hours < 12)
        {
            sunColor = hours / 12f + (min / 720f);
        }
        else if (hours >= 12 && hours <= 15)
        {
            sunColor = 1;
        }
        else
        {
            sunColor = 1 - (hours - 15f) / 12f - (min / 720f);
        }
        // sun.color = new Color(sunColor, sunColor, sunColor, 1);
    }

    public void StartNewDay(bool isStopTime)
    {
        hours = 6;
        min = 0;
        // sun.color = new Color(1, 1, 1, 1);
        clockView.SetTime((int)hours, (int)min);
        this.isStopTime = isStopTime;
    }


    /// <summary>
    /// 获取日期
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void GetTimeForDate(out int year, out int month, out int day)
    {
        TimeBean timeData = gameDataManager.gameData.gameTime;
        year = timeData.year;
        month = timeData.month;
        day = timeData.day;
    }
}
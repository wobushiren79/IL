using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TimeBean 
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;

    public TimeBean()
    {

    }

    public TimeBean(int year,int month,int day,int hour,int minute,int second)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }

    public void SetTimeForYMD(int year,int month,int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }

    public void SetTimeForHM(int hour, int minute)
    {
        this.hour = hour;
        this.minute = minute;
    }

    /// <summary>
    /// 增加时间-针对小时
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    public void AddTimeForHMS(int addHour, int addMin, int Addsecond)
    {
        second += Addsecond;
        if (second >= 60)
        {
            addMin += (second / 60);
            second = second % 60;
        }

        minute += addMin;
        if (minute >= 60)
        {
            addHour += (minute / 60);
            minute = minute % 60;
        }

        hour += addHour;
    }

    /// <summary>
    /// 获取时间-秒
    /// </summary>
    /// <returns></returns>
    public long GetTimeForTotalS()
    {
        long totalS = 0;
        totalS += second;
        totalS += (minute * 60);
        totalS += (hour * 60 * 60);
        return totalS;
    }
}
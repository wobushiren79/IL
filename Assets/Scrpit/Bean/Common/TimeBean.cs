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
}
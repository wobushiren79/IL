using UnityEngine;
using UnityEditor;

public class CurrentDayBean 
{
    public int year;
    public int month;
    public int day;
    public GameTimeHandler.DayEnum dayStatus = GameTimeHandler.DayEnum.None;//当天状态
    public WeatherBean weatherToday;//当天天气
}
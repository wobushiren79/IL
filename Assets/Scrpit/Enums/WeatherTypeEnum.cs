using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum WeatherTypeEnum 
{
    Sunny = 1,//普通
    Cloudy = 2,//多云

    LightRain = 11,//小雨
    Rain = 22,//下雨
    Thunderstorm = 23,//雷雨

    Fog = 31,//雾

    LightSnow = 41,//小雪
    Snow = 42,//下雪

    Wind = 51,//大风
    Defoliation = 52,//落叶
}

public class WeatherTypeEnumTools
{
    /// <summary>
    /// 获取指定天气人流量加成
    /// </summary>
    /// <returns></returns>
    public static float GetWeatherAddition(WeatherTypeEnum weatherType)
    {
        float addition = 0;
        switch (weatherType)
        {
            case WeatherTypeEnum.Sunny:
                addition = 0;
                break;
            case WeatherTypeEnum.Cloudy:
                addition = 1;
                break;
            case WeatherTypeEnum.LightRain:
                addition = -1;
                break;
            case WeatherTypeEnum.Rain:
                addition = -1.5f;
                break;
            case WeatherTypeEnum.Thunderstorm:
                break;
            case WeatherTypeEnum.Fog:
                addition = -2;
                break;
            case WeatherTypeEnum.LightSnow:
                addition = -1;
                break;
            case WeatherTypeEnum.Snow:
                addition = -1.5f;
                break;
            case WeatherTypeEnum.Wind:
                addition = -0.5f;
                break;
            case WeatherTypeEnum.Defoliation:
                addition = 0f;
                break;
        }
        return addition;
    }

    /// <summary>
    /// 根据月份获取天气
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public static List<WeatherTypeEnum> GetWeahterListByMonth(int month)
    {
        List<WeatherTypeEnum> listWeather = new List<WeatherTypeEnum>();
        switch ((SeasonsEnum)month)
        {
            case SeasonsEnum.Spring:
                listWeather.Add(WeatherTypeEnum.LightRain);
                listWeather.Add(WeatherTypeEnum.Rain);
                listWeather.Add(WeatherTypeEnum.Fog);
                listWeather.Add(WeatherTypeEnum.Wind);
                break;
            case SeasonsEnum.Summer:
                listWeather.Add(WeatherTypeEnum.LightRain);
                listWeather.Add(WeatherTypeEnum.Rain);
                listWeather.Add(WeatherTypeEnum.Thunderstorm);
                listWeather.Add(WeatherTypeEnum.Fog);
                listWeather.Add(WeatherTypeEnum.Wind);
                break;
            case SeasonsEnum.Autumn:
                listWeather.Add(WeatherTypeEnum.LightRain);
                listWeather.Add(WeatherTypeEnum.Rain);
                listWeather.Add(WeatherTypeEnum.Fog);
                listWeather.Add(WeatherTypeEnum.Wind);
                listWeather.Add(WeatherTypeEnum.Defoliation);
                break;
            case SeasonsEnum.Winter:
                listWeather.Add(WeatherTypeEnum.LightSnow);
                listWeather.Add(WeatherTypeEnum.Snow);
                break;
            default:
                listWeather.AddRange(EnumUtil.GetEnumValue<WeatherTypeEnum>());
                break;
        }
        return listWeather;
    }
}

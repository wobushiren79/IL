using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class WeatherHandler : BaseMonoBehaviour
{
    public enum WeatherTypeEnum
    {
        Sunny = 1,//普通
        Rain = 2,//下雨
        Snow = 3,//下雪
        Fog = 4,//起雾
        Wind = 5,//大风
    }

    [Header("控件")]
    //太阳光
    public SunLightCpt sunLight;
    [Header("数据")]
    public GameTimeHandler gameTimeHandler;
    public List<WeatherCpt> listWeather = new List<WeatherCpt>();

    public WeatherTypeEnum weatherType = WeatherTypeEnum.Sunny;
    public WeatherBean weatherData = new WeatherBean();

    private void Update()
    {
        switch (weatherType)
        {
            case WeatherTypeEnum.Sunny:
                SetWeahterSunny();
                break;
            default:
                SetWeahterSunny();
                break;
        }
    }

    /// <summary>
    /// 随机化一个天气
    /// </summary>
    public WeatherBean RandomWeather()
    {
        WeatherTypeEnum weatherStatusRandom = WeatherTypeEnum.Sunny;
        int weatherRandom = 0;
        if (weatherRandom == 0)
        {
            weatherStatusRandom = WeatherTypeEnum.Rain;
        }
        WeatherBean weatherData = new WeatherBean();
        weatherData.weatherType = (int)weatherStatusRandom;
        weatherData.weatherSize = 1;
        SetWeahter(weatherData);
        return weatherData;
    }

    /// <summary>
    /// 设置天气
    /// </summary>
    /// <param name="weatherStatus"></param>
    public void SetWeahter(WeatherBean weatherData)
    {
        this.weatherData = weatherData;
        weatherType = (WeatherTypeEnum)weatherData.weatherType;
        foreach (WeatherCpt itemWeather in listWeather)
        {
            if (itemWeather.name.Equals(EnumUtil.GetEnumName(weatherType)))
            {
                itemWeather.OpenWeather();
            }
            else
            {
                itemWeather.CloseWeather();
            }
        }
    }

    /// <summary>
    /// 设置晴天天气
    /// </summary>
    public void SetWeahterSunny()
    {
        if (gameTimeHandler == null || sunLight == null)
            return;
        gameTimeHandler.GetTime(out float hour, out float min);
        if (hour >= 0 && hour < 7)
        {
            sunLight.SetSunColor(0.7f, 0.7f, 0.8f);
        }
        else if (hour >= 7 && hour < 10)
        {
            ChangeColor(new Vector3(0.7f, 0.7f, 0.8f), new Vector3(1, 1, 1), 3f, (hour - 7) + (min / 60f), out Vector3 outColor);
            sunLight.SetSunColor(outColor.x, outColor.y, outColor.z);
        }
        else if (hour >= 10 && hour < 17)
        {
            sunLight.SetSunColor(1, 1, 1);
        }
        else if (hour >= 17 && hour < 24)
        {
            ChangeColor(new Vector3(1, 1, 1), new Vector3(0.2f, 0.2f, 0.3f), 7f, (hour - 17) + (min / 60f), out Vector3 outColor);
            sunLight.SetSunColor(outColor.x, outColor.y, outColor.z);
        }
        else
        {
            sunLight.SetSunColor(0.2f, 0.2f, 0.3f);
        }
    }

    private void ChangeColor(Vector3 oldChange, Vector3 newChange, float changeHour, float valueHour, out Vector3 outColor)
    {
        float tempR = newChange.x - oldChange.x;
        float tempG = newChange.y - oldChange.y;
        float tempB = newChange.z - oldChange.z;

        float itemR = tempR / changeHour;
        float itemG = tempG / changeHour;
        float itemB = tempB / changeHour;

        outColor = new Vector3(valueHour * itemR + oldChange.x, valueHour * itemG + oldChange.y, valueHour * itemB + oldChange.z);
    }
}
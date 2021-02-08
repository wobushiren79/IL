using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class GameWeatherHandler : BaseHandler<GameWeatherHandler, GameWeatherManager>
{

    /// <summary>
    /// 随机化一个天气
    /// </summary>
    public WeatherBean RandomWeather()
    {
        WeatherTypeEnum weatherStatusRandom = WeatherTypeEnum.Sunny;
        GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);

        float weatherRate = UnityEngine.Random.Range(0f, 1f);
        if (weatherRate <= 0.5f)
        {
            weatherStatusRandom = WeatherTypeEnum.Sunny;
        }
        else if (weatherRate > 0.5f && weatherRate <= 0.8f)
        {
            weatherStatusRandom = WeatherTypeEnum.Cloudy;
        }
        else
        {
            List<WeatherTypeEnum> listWeather = WeatherTypeEnumTools.GetWeahterListByMonth(month);
            weatherStatusRandom = RandomUtil.GetRandomDataByList(listWeather);
        }
        WeatherBean weatherData = new WeatherBean(weatherStatusRandom);
        SetWeather(weatherData);
        return weatherData;
    }

    /// <summary>
    /// 设置天气
    /// </summary>
    /// <param name="weatherStatus"></param>
    public void SetWeather(WeatherBean weatherData)
    {
        if (weatherData == null)
            return;
        manager.weatherData = weatherData;

        CloseWeather();

        Color sunLightOffset = Color.black;
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Sunny:
            case WeatherTypeEnum.Cloudy:
                manager.weatherSunny.OpenWeather(weatherData);
                break;
            case WeatherTypeEnum.LightRain:
            case WeatherTypeEnum.Rain:
            case WeatherTypeEnum.Thunderstorm:
                manager.weatherRain.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.2f, -0.2f, -0.2f, 1);
                break;
            case WeatherTypeEnum.Fog:
                manager.weatherFog.OpenWeather(weatherData);
                break;
            case WeatherTypeEnum.LightSnow:
            case WeatherTypeEnum.Snow:
                manager.weatherSnow.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.1f, -0.1f, -0.1f, 1);
                break;
            case WeatherTypeEnum.Wind:
            case WeatherTypeEnum.Defoliation:
                manager.weatherWind.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.1f, -0.1f, -0.1f, 1);
                break;
        }
        GameLightHandler.Instance.sunLight.SetOffsetColor(sunLightOffset);
    }

    /// <summary>
    /// 关闭所有天气
    /// </summary>
    public void CloseWeather()
    {
        manager.weatherSunny.CloseWeather();
        manager.weatherRain.CloseWeather();
        manager.weatherFog.CloseWeather();
        manager.weatherSnow.CloseWeather();
        manager.weatherWind.CloseWeather();
    }

}
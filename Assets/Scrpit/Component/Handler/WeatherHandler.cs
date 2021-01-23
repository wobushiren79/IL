using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class WeatherHandler : BaseMonoBehaviour
{


    public WeatherForSunnyCpt weatherSunny;
    public WeatherForRainCpt weatherRain;
    public WeatherForFogCpt weatherFog;
    public WeatherForSnowCpt weatherSnow;
    public WeatherForWindCpt weatherWind;

    public WeatherBean weatherData;

    protected GameTimeHandler gameTimeHandler;
    //太阳光
    protected SunLightCpt sunLight;

    private void Awake()
    {
        sunLight = Find<SunLightCpt>(ImportantTypeEnum.Sun);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
    }

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
        SetWeahter(weatherData);
        return weatherData;
    }

    /// <summary>
    /// 设置天气
    /// </summary>
    /// <param name="weatherStatus"></param>
    public void SetWeahter(WeatherBean weatherData)
    {
        if (weatherData == null)
            return;
        this.weatherData = weatherData;

        weatherSunny.CloseWeather();
        weatherRain.CloseWeather();
        weatherFog.CloseWeather();
        weatherSnow.CloseWeather();
        weatherWind.CloseWeather();

        Color sunLightOffset = Color.black;
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Sunny:
            case WeatherTypeEnum.Cloudy:
                weatherSunny.OpenWeather(weatherData);
                break;
            case WeatherTypeEnum.LightRain:
            case WeatherTypeEnum.Rain:
            case WeatherTypeEnum.Thunderstorm:
                weatherRain.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.2f,-0.2f,-0.2f,1);
                break;
            case WeatherTypeEnum.Fog:
                weatherFog.OpenWeather(weatherData);
                break;
            case WeatherTypeEnum.LightSnow:
            case WeatherTypeEnum.Snow:
                weatherSnow.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.1f, -0.1f, -0.1f, 1);
                break;
            case WeatherTypeEnum.Wind:
            case WeatherTypeEnum.Defoliation:
                weatherWind.OpenWeather(weatherData);
                sunLightOffset = new Color(-0.1f, -0.1f, -0.1f, 1);
                break;
        }
        sunLight.SetOffsetColor(sunLightOffset);
    }


}
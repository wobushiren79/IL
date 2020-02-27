using UnityEngine;
using UnityEditor;

public class WeatherForSunnyCpt : WeatherCpt
{
    public GameObject objCloudy;

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        objCloudy.SetActive(false);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Snow:
                SetSunny();
                break;
            case WeatherTypeEnum.Cloudy:
                SetCloudy();
                break;
        }
    }

    public void SetCloudy()
    {
        objCloudy.SetActive(true);
    }

    public void SetSunny()
    {

    }
}
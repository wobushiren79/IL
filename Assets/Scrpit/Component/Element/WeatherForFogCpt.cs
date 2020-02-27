using UnityEngine;
using UnityEditor;

public class WeatherForFogCpt : WeatherCpt
{
    public GameObject objFog;

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        objFog.SetActive(false);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Fog:
                SetFog();
                break;
        }
    }

    public void SetFog()
    {
        objFog.SetActive(true);
    }

}
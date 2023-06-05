using UnityEditor;
using UnityEngine;

public class SceneBaseManager : BaseManager
{
    public Vector3 windPosition
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.wind_position);
        }
    }

    public Vector3 windScale
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.wind_scale);
        }
    }

    public Vector3 windScaleRange
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.wind_scaleRange);
        }
    }

    public Vector3 fogScaleRange
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.fog_scaleRange);
        }
    }

    public Vector3 rainPosition
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.rain_position);
        }
    }
    public Vector3 rainScale
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.rain_scale);
        }
    }

    public Vector3 snowPosition
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.snow_position);
        }
    }
    public Vector3 snowScale
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.snow_scale);
        }
    }

    public Vector3 sunnyPosition
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.sunny_position);
        }
    }
    public Vector3 sunnyScale
    {
        get
        {
            WeatherInfoBean weatherInfo = GetWeatherInfo();
            return weatherInfo.GetVector3(weatherInfo.sunny_scale);
        }
    }

    protected WeatherInfoBean GetWeatherInfo()
    {
        ScenesEnum scenes = SceneUtil.GetCurrentScene();
        WeatherInfoBean weatherInfo = WeatherInfoCfg.GetItemData((long)scenes);
        return weatherInfo;
    }
}
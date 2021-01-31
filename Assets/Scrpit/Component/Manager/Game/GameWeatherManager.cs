using UnityEditor;
using UnityEngine;

public class GameWeatherManager : BaseManager
{
    public WeatherForSunnyCpt _weatherSunny;
    public WeatherForRainCpt _weatherRain;
    public WeatherForFogCpt _weatherFog;
    public WeatherForSnowCpt _weatherSnow;
    public WeatherForWindCpt _weatherWind;

    public WeatherBean weatherData;

    public WeatherForSunnyCpt weatherSunny
    {
        get
        {
            if (_weatherSunny==null)
            {
                _weatherSunny = CreateWeather<WeatherForSunnyCpt>("weather/weather", "Sunny");
            }
            return _weatherSunny;
        }
    }

    public WeatherForRainCpt weatherRain
    {
        get
        {
            if (_weatherRain == null)
            {
                _weatherRain = CreateWeather<WeatherForRainCpt>("weather/weather", "Rain");
            }
            return _weatherRain;
        }
    }

    public WeatherForFogCpt weatherFog
    {
        get
        {
            if (_weatherFog == null)
            {
                _weatherFog = CreateWeather<WeatherForFogCpt>("weather/weather", "Fog");
            }
            return _weatherFog;
        }
    }

    public WeatherForSnowCpt weatherSnow
    {
        get
        {
            if (_weatherSnow == null)
            {
                _weatherSnow = CreateWeather<WeatherForSnowCpt>("weather/weather", "Snow");
            }
            return _weatherSnow;
        }
    }

    public WeatherForWindCpt weatherWind
    {
        get
        {
            if (_weatherWind == null)
            {
                _weatherWind = CreateWeather<WeatherForWindCpt>("weather/weather", "Wind");
            }
            return _weatherWind;
        }
    }

    protected T CreateWeather<T>(string path,string name)
    {
        GameObject objMdoel = GetModel<GameObject>(path, name);
        GameObject objItem = Instantiate(gameObject, objMdoel);
        objItem.name = name;
        return objItem.GetComponent<T>();
    }
}
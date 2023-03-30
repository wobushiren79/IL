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
                _weatherSunny = CreateWeather<WeatherForSunnyCpt>("Assets/Prefabs/Effects/Weather/Sunny.prefab");
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
                _weatherRain = CreateWeather<WeatherForRainCpt>("Assets/Prefabs/Effects/Weather/Rain.prefab");
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
                _weatherFog = CreateWeather<WeatherForFogCpt>("Assets/Prefabs/Effects/Weather/Fog.prefab");
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
                _weatherSnow = CreateWeather<WeatherForSnowCpt>("Assets/Prefabs/Effects/Weather/Snow.prefab");
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
                _weatherWind = CreateWeather<WeatherForWindCpt>("Assets/Prefabs/Effects/Weather/Wind.prefab");
            }
            return _weatherWind;
        }
    }

    protected T CreateWeather<T>(string path)
    {
        GameObject objMdoel = LoadAddressablesUtil.LoadAssetSync<GameObject>(path);
        GameObject objItem = Instantiate(gameObject, objMdoel);
        objItem.name = name;
        return objItem.GetComponent<T>();
    }
}
using UnityEngine;
using UnityEditor;

public class WeatherCpt : BaseMonoBehaviour
{
    public WeatherBean weatherData;

    public virtual void OpenWeather(WeatherBean weatherData)
    {
        this.weatherData = weatherData;
        gameObject.SetActive(true);
    }

    public virtual void CloseWeather()
    {
        gameObject.SetActive(false);
        this.weatherData = null;
    }
}
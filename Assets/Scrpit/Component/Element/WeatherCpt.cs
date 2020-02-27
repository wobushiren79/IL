using UnityEngine;
using UnityEditor;

public class WeatherCpt : BaseMonoBehaviour
{
    public WeatherBean weatherData;

    protected AudioHandler audioHandler;

    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

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
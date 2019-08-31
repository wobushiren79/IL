using UnityEngine;
using UnityEditor;

public class WeatherCpt : BaseMonoBehaviour
{
    public virtual void OpenWeather()
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseWeather()
    {
        gameObject.SetActive(false);
    }
}
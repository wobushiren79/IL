using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class WeatherBean 
{
    public WeatherTypeEnum weatherType;//天气类型  
    public int weatherSize;//天气大小
    public float weatherAddition;//天气加成

    public WeatherBean(WeatherTypeEnum weatherType)
    {
        this.weatherType = weatherType;
        weatherAddition = WeatherTypeEnumTools.GetWeatherAddition(weatherType);
        weatherSize = 1;
    }
}
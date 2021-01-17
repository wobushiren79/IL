using UnityEngine;
using UnityEditor;

public class WeatherForWindCpt : WeatherCpt
{
    public GameObject objWind_1;
    public ParticleSystem psWind_1;
    public GameObject objWind_2;
    public ParticleSystem psWind_2;
    public GameObject objDefoliation_1;
    public ParticleSystem psDefoliation_1;

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        objWind_1.SetActive(true);
        objWind_2.SetActive(false);
        objDefoliation_1.SetActive(false);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Wind:
                SetWind();
                break;
            case WeatherTypeEnum.Defoliation:
                SetDefoliation();
                break;
        }
        
            AudioHandler.Instance.PlayEnvironment(AudioEnvironmentEnum.Wind,GameCommonInfo.GameConfig.environmentVolume);
    }

    public override void CloseWeather()
    {
        
            AudioHandler.Instance.StopEnvironment();
        objWind_1.SetActive(false);
        objWind_2.SetActive(false);
        objDefoliation_1.SetActive(false);
        base.CloseWeather();
    }

    /// <summary>
    /// 设置大风
    /// </summary>
    public void SetWind()
    {
        objWind_2.SetActive(true);
    }

    /// <summary>
    /// 设置落叶
    /// </summary>
    public void SetDefoliation()
    {
        objDefoliation_1.SetActive(true);
    }

}
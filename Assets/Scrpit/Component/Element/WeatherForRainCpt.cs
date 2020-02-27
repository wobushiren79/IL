using UnityEngine;
using UnityEditor;
using System.Collections;
public class WeatherForRainCpt : WeatherCpt
{
    public GameObject objRain;
    public ParticleSystem psRain;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        objRain.SetActive(true);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.LightRain:
                SetLightRain();
                break;
            case WeatherTypeEnum.Rain:
                SetRain();
                break;
            case WeatherTypeEnum.Thunderstorm:
                SetThunderstorm();
                break;
        }
        if (audioHandler != null)
            audioHandler.PlayEnvironment(AudioEnvironmentEnum.Rain);
    }

    public override void CloseWeather()
    {
        if (audioHandler != null)
            audioHandler.StopEnvironment();
        StopAllCoroutines();
        base.CloseWeather();
    }

    public void SetLightRain()
    {
        ParticleSystem.EmissionModule emissionModule = psRain.emission;
        emissionModule.rateOverTime = Random.Range(100, 200);
    }

    public void SetRain()
    {
        ParticleSystem.EmissionModule emissionModule = psRain.emission;
        emissionModule.rateOverTime = Random.Range(500, 1000);
    }

    public void SetThunderstorm()
    {
        ParticleSystem.EmissionModule emissionModule = psRain.emission;
        emissionModule.rateOverTime = 2000;
        StartCoroutine(CoroutineForThunderstorm());
    }

    /// <summary>
    /// 协程-雷雨
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForThunderstorm()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            if (audioHandler != null)
                audioHandler.PlaySound(AudioSoundEnum.Thunderstorm);
        } 
    }
}
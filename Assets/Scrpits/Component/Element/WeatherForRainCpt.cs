using UnityEngine;
using UnityEditor;
using System.Collections;

using DG.Tweening;

public class WeatherForRainCpt : WeatherCpt
{
    public GameObject objRain;
    public ParticleSystem psRain;
    public GameObject objThunder;
    public UnityEngine.Rendering.Universal.Light2D lightThunder;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        InitPS();
        if (objRain != null)
            objRain.SetActive(true);
        if (objThunder != null)
            objThunder.SetActive(false);
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
        AudioHandler.Instance.PlayEnvironment(AudioEnvironmentEnum.Rain);
    }

    public override void CloseWeather()
    {
        
        AudioHandler.Instance.StopEnvironment();
        StopAllCoroutines(); 
        if (objRain != null)
            objRain.SetActive(false);
        if (objThunder != null)
            objThunder.SetActive(false);
        base.CloseWeather();
    }

    public void InitPS()
    {
        SceneBaseManager sceneBaseManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psRain.shape;
        shapeModule.position = sceneBaseManager.positionForRain;
        shapeModule.scale = sceneBaseManager.scaleForRain;
    }

    /// <summary>
    /// 设置小雨
    /// </summary>
    public void SetLightRain()
    {
        ParticleSystem.EmissionModule emissionModule = psRain.emission;      
        emissionModule.rateOverTime = Random.Range(100, 200);
    }

    /// <summary>
    /// 设置中雨
    /// </summary>
    public void SetRain()
    {
        ParticleSystem.EmissionModule emissionModule = psRain.emission;
        emissionModule.rateOverTime = Random.Range(500, 1000);
    }

    /// <summary>
    /// 设置雷雨
    /// </summary>
    public void SetThunderstorm()
    {
        if (objThunder != null)
            objThunder.SetActive(true);
        lightThunder.intensity = 0;
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
            yield return new WaitForSeconds(Random.Range(10, 30));
            //打雷特效
            
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Thunderstorm);
            float intensity = 0;
            DOTween
                .To(() => intensity, x => { intensity = x; lightThunder.intensity = intensity; }, 0.5f, 1f)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
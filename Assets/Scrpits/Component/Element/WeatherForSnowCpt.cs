using UnityEngine;
using UnityEditor;

public class WeatherForSnowCpt : WeatherCpt
{
    public GameObject objSnow;
    public ParticleSystem psSnow;

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        if (objSnow != null)
            objSnow.SetActive(true);
        InitPS();
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.LightSnow:
                SetLightSnow();
                break;
            case WeatherTypeEnum.Snow:
                SetSnow();
                break;
        }
    }

    public override void CloseWeather()
    {
        if (objSnow != null)
            objSnow.SetActive(false);
        base.CloseWeather();
    }

    /// <summary>
    /// 设置小雪
    /// </summary>
    public void SetLightSnow()
    {
        ParticleSystem.EmissionModule emissionModule = psSnow.emission;
        emissionModule.rateOverTime = Random.Range(50, 100);
    }

    /// <summary>
    /// 设置中雨
    /// </summary>
    public void SetSnow()
    {
        ParticleSystem.EmissionModule emissionModule = psSnow.emission;
        emissionModule.rateOverTime = Random.Range(200, 300);
    }

    public void InitPS()
    {
        SceneBaseManager sceneBaseManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psSnow.shape;
        shapeModule.position = sceneBaseManager.positionForSnow;
        shapeModule.scale = sceneBaseManager.scaleForSnow;
    }
}
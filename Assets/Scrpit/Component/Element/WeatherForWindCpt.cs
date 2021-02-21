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
        objWind_1.SetActive(false);
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
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        AudioHandler.Instance.PlayEnvironment(AudioEnvironmentEnum.Wind, gameConfig.environmentVolume);
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
        objWind_1.SetActive(true);
        SceneBaseManager sceneManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModuleWind_1 = psWind_1.shape;
        shapeModuleWind_1.position = sceneManager.positionForWind;
        shapeModuleWind_1.scale = sceneManager.scaleForWind;

        objWind_2.SetActive(true);
        ParticleSystem.ShapeModule shapeModuleWind_2 = psWind_1.shape;
        shapeModuleWind_2.scale = sceneManager.scaleRangeForWind;
    }

    /// <summary>
    /// 设置落叶
    /// </summary>
    public void SetDefoliation()
    {
        objWind_1.SetActive(true);
        SceneBaseManager sceneManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psWind_1.shape;
        shapeModule.position = sceneManager.positionForWind;
        shapeModule.scale = sceneManager.scaleForWind;

        objDefoliation_1.SetActive(true);
        ParticleSystem.ShapeModule shapeModuleDefoliation = psDefoliation_1.shape;
        shapeModuleDefoliation.position = sceneManager.positionForWind;
        shapeModuleDefoliation.scale = sceneManager.scaleForWind;
    }



}
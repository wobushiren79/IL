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
        if(objWind_1)
            objWind_1.SetActive(false);
        if (objWind_2)
            objWind_2.SetActive(false);
        if (objDefoliation_1)
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
        AudioHandler.Instance.PlayEnvironment(AudioEnvironmentEnum.Wind);
    }

    public override void CloseWeather()
    {
        AudioHandler.Instance.StopEnvironment();
        if (objWind_1)
            objWind_1.SetActive(false);
        if (objWind_2)
            objWind_2.SetActive(false);
        if (objDefoliation_1)
            objDefoliation_1.SetActive(false);
        base.CloseWeather();
    }

    /// <summary>
    /// 设置大风
    /// </summary>
    public void SetWind()
    {
        if (objWind_1)
            objWind_1.SetActive(true);
        SceneBaseManager sceneManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModuleWind_1 = psWind_1.shape;
        shapeModuleWind_1.position = sceneManager.windPosition;
        shapeModuleWind_1.scale = sceneManager.windScale;

        if (objWind_2)
            objWind_2.SetActive(true);
        ParticleSystem.ShapeModule shapeModuleWind_2 = psWind_1.shape;
        shapeModuleWind_2.scale = sceneManager.windScaleRange;
    }

    /// <summary>
    /// 设置落叶
    /// </summary>
    public void SetDefoliation()
    {
        if (objWind_1)
            objWind_1.SetActive(true);
        SceneBaseManager sceneManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psWind_1.shape;
        shapeModule.position = sceneManager.windPosition;
        shapeModule.scale = sceneManager.windScale;

        if (objDefoliation_1)
            objDefoliation_1.SetActive(true);
        ParticleSystem.ShapeModule shapeModuleDefoliation = psDefoliation_1.shape;
        shapeModuleDefoliation.position = sceneManager.windPosition;
        shapeModuleDefoliation.scale = sceneManager.windScale;
    }



}
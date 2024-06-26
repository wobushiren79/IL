﻿using UnityEngine;
using UnityEditor;

public class InteractiveSceneChangeCpt : BaseInteractiveCpt
{

    public string interactiveContent;
    //需要跳转的场景
    public ScenesEnum changeScene;

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (changeScene == ScenesEnum.GameSquareScene || changeScene == ScenesEnum.GameForestScene)
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(1361));
            }
            else if (changeScene == ScenesEnum.GameCourtyardScene)
            {
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                InnCourtyardBean innCourtyard = gameData.GetInnCourtyardData();
                //如果还没有扩建后院
                if (innCourtyard.courtyardLevel <= 0)
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(1362));
                    return;
                }
                GameScenesHandler.Instance.ChangeScene(changeScene);
            }
            else
            {
                if (changeScene == ScenesEnum.GameMountainScene)
                {
                    //如果山顶 特殊天气不能前往
                    WeatherBean weatherData = GameCommonInfo.CurrentDayData.weatherToday;
                    if (weatherData != null &&
                        (weatherData.weatherType == WeatherTypeEnum.Cloudy
                        || weatherData.weatherType == WeatherTypeEnum.Sunny))
                    {
                        GameScenesHandler.Instance.ChangeScene(changeScene);
                    }
                    else
                    {
                        UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1321));
                    }
                }
                else
                {
                    GameScenesHandler.Instance.ChangeScene(changeScene);
                }
            }
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}
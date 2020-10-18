﻿using UnityEngine;
using UnityEditor;

public class SceneForMountainHandler : BaseHandler
{
    public MoveByGameTimeCpt sun;
    public MoveByGameTimeCpt moon;

    protected GameTimeHandler gameTimeHandler;

    private void Awake()
    {
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
    }

    private void Update()
    {
        if (gameTimeHandler == null)
            return;
        gameTimeHandler.GetTime(out float hour, out float min);
        if (hour >= 6 && hour < 21)
        {
            sun.gameObject.SetActive(true);
            moon.gameObject.SetActive(false);
        }
        else if (hour >= 21 && hour <= 24)
        {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        }
        else
        {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(false);
        }
    }


}
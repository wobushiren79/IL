﻿using UnityEngine;
using UnityEditor;

public class InteractiveStairsCpt : BaseInteractiveCpt
{
    public BuildStairsCpt buildStairsCpt;

    protected InnEntranceHandler innEntranceHandler;

    private void Awake()
    {
        innEntranceHandler = Find<InnEntranceHandler>(ImportantTypeEnum.InnHandler);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            innEntranceHandler.GetStairsPosition(buildStairsCpt.remarkId, out Vector3 layerFirstPosition, out Vector3 layerSecondPosition);
            if (buildStairsCpt.layer == 1)
            {
                characterInt.transform.position = layerSecondPosition;
            }
            else
            {
                characterInt.transform.position = layerFirstPosition;
            }
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        if (buildStairsCpt.layer == 1)
        {
            characterInt.ShowInteractive(GameCommonInfo.GetUITextById(95));
        }
        else
        {
            characterInt.ShowInteractive(GameCommonInfo.GetUITextById(96));
        }
    }
}
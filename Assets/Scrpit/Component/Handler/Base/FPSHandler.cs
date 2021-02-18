using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHandler : BaseHandler<FPSHandler, BaseManager>
{

    protected override void Awake()
    {
        base.Awake();
        //Screen.SetResolution(1280, 800, false);	        
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        SetData(gameConfig.statusForFrames, gameConfig.frames);
    }

    public void SetData(bool isLock, int fps)
    {
        if (isLock)
        {
            Application.targetFrameRate = fps;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
    }

    public void SetData(int isLock, int fps)
    {
        if (isLock == 1)
        {
            Application.targetFrameRate = fps;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
    }

}

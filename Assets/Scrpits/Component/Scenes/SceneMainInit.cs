﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseSceneInit
{
    public override void Start()
    {
        base.Start();
        //打开UI
        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
        //播放主界面音乐
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Main);
    }

}
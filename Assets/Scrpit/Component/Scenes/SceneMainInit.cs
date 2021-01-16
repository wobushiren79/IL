using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseSceneInit
{
    public override void Start()
    {
        base.Start();
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
  
        //打开UI
        UIHandler.Instance.manager.OpenUI(UIEnum.MainStart);
        //播放主界面音乐
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Main);
    }

}
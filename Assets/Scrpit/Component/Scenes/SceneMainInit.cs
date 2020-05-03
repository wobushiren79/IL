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
        audioHandler.PlayMusicForLoop( AudioMusicEnum.LangTaoSha);
    }
}
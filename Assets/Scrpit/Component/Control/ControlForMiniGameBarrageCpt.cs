using UnityEngine;
using UnityEditor;

public class ControlForMiniGameBarrageCpt : ControlForMoveCpt
{
    /// <summary>
    /// 设置镜头对象
    /// </summary>
    /// <param name="npcCpt"></param>
    public void SetCameraFollowObj(NpcAIMiniGameBarrageCpt npcCpt)
    {
        base.SetCameraFollowObj(npcCpt.gameObject);
        characterMoveCpt = npcCpt.characterMoveCpt;
        characterMoveCpt.isManualMove = true;
        npcAI = npcCpt;
    }

    public override void StartControl()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        if (cameraFollowObj != null)
            GameCameraHandler.Instance.manager.camera2D.Follow = cameraFollowObj.transform;
        InitCharacter();
    }
}
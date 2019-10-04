using UnityEngine;
using UnityEditor;

public class ControlForMiniGameBarrageCpt : ControlForMoveCpt
{
    /// <summary>
    /// 设置镜头对象
    /// </summary>
    /// <param name="npcCpt"></param>
    public  void SetCameraFollowObj(NpcAIMiniGameBarrageCpt npcCpt)
    {
        base.SetCameraFollowObj(npcCpt.gameObject);
        characterMoveCpt = npcCpt.characterMoveCpt;
        characterMoveCpt.navMeshAgent = null;
        characterMoveCpt.isManualMove = true;
        npcAI = npcCpt;
    }
}
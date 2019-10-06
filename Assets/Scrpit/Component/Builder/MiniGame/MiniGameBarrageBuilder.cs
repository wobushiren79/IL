using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBarrageBuilder : BaseMonoBehaviour
{
    //发射器容器
    public GameObject objEjectorContainer;
    //发射器模型
    public GameObject objEjectorModel;

    //玩家容器
    public GameObject objPlayerContainer;
    //玩家模型
    public GameObject objPlayerModel;

    //所有的发射器
    public List<BarrageEjectorCpt> listEjector = new List<BarrageEjectorCpt>();
    //所有的游玩对象
    public List<NpcAIMiniGameBarrageCpt> listGamePlayer = new List<NpcAIMiniGameBarrageCpt>();

    //控制器
    public ControlForMiniGameBarrageCpt controlForMiniGameBarrageCpt;
    /// <summary>
    /// 获取所有的发射器
    /// </summary>
    /// <returns></returns>
    public List<BarrageEjectorCpt> GetEjector()
    {
        return listEjector;
    }

    /// <summary>
    /// 获取所有玩家
    /// </summary>
    /// <returns></returns>
    public List<NpcAIMiniGameBarrageCpt> GetPlayerList()
    {
        return listGamePlayer;
    }

    /// <summary>
    /// 创建所有玩家
    /// </summary>
    /// <param name="userData"></param>
    /// <param name="listEnemy"></param>
    public void CreateAllPlayer(MiniGameCharacterBean userData, List<MiniGameCharacterBean> listEnemyData)
    {
        //创建操作角色
        NpcAIMiniGameBarrageCpt npcCpt = CreatePlayer(userData, new Vector3(0, 6, 0));
        controlForMiniGameBarrageCpt.SetCameraFollowObj(npcCpt);
    }

    /// <summary>
    /// 创建一个发射器
    /// </summary>
    /// <param name="position"></param>
    public BarrageEjectorCpt CreateEjector(Vector3 ejectorPosition)
    {
        GameObject objEjector = Instantiate(objEjectorContainer, objEjectorModel);
        objEjector.transform.position = ejectorPosition;
        BarrageEjectorCpt ejectorCpt = objEjector.GetComponent<BarrageEjectorCpt>();
        listEjector.Add(ejectorCpt);
        return ejectorCpt;
    }

    /// <summary>
    /// 创建一个NPC
    /// </summary>
    public NpcAIMiniGameBarrageCpt CreatePlayer(MiniGameCharacterBean characterData, Vector3 position)
    {
        GameObject objPlayer = Instantiate(objPlayerContainer, objPlayerModel, position);
        NpcAIMiniGameBarrageCpt npcCpt = objPlayer.GetComponent<NpcAIMiniGameBarrageCpt>();
        npcCpt.SetData(characterData);
        listGamePlayer.Add(npcCpt);
        return npcCpt;
    }

    /// <summary>
    /// 清理所有玩家
    /// </summary>
    public void DestoryPlayer()
    {
        CptUtil.RemoveChild(objPlayerContainer.transform);
        listGamePlayer.Clear();
    }

    /// <summary>
    /// 清理所有的发射器
    /// </summary>
    public void DestoryEjector()
    {
        CptUtil.RemoveChild(objEjectorContainer.transform);
        listEjector.Clear();
    }

}
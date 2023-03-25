using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBarrageBuilder : BaseMiniGameBuilder
{
    //发射器容器
    public GameObject objEjectorContainer;
    //发射器模型
    public GameObject objEjectorModel;

    //玩家容器
    public GameObject objPlayerContainer;
    //玩家模型
    public GameObject objPlayerModel;

    //玩家容器
    public GameObject objBulletContainer;

    //所有的发射器
    public List<MiniGameBarrageEjectorCpt> listEjector = new List<MiniGameBarrageEjectorCpt>();
    //所有的游玩对象
    public List<NpcAIMiniGameBarrageCpt> listGamePlayer = new List<NpcAIMiniGameBarrageCpt>();

    public Sprite spStone;
    public RuntimeAnimatorController animatorControllerForStone;
    public Sprite spArrow;
    public RuntimeAnimatorController animatorControllerForArrow;
    public Sprite spDarts;
    public RuntimeAnimatorController animatorControllerForDarts;
    public Sprite spFireball;
    public RuntimeAnimatorController animatorControllerForFireball;
    public Sprite spEgg;
    public RuntimeAnimatorController animatorControllerForEgg;
    /// <summary>
    /// 获取所有的发射器
    /// </summary>
    /// <returns></returns>
    public List<MiniGameBarrageEjectorCpt> GetEjector()
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
    public void CreateAllCharacter(List<MiniGameCharacterBean> listUserData, Vector3 userStartPosition)
    {
        //创建操作角色
        foreach (MiniGameCharacterBean itemUserData in listUserData)
        {
            NpcAIMiniGameBarrageCpt npcCpt = CreatePlayer(itemUserData, userStartPosition);
            ControlForMiniGameBarrageCpt controlForMiniGameBarrageCpt= GameControlHandler.Instance.manager.GetControl<ControlForMiniGameBarrageCpt>( GameControlHandler.ControlEnum.MiniGameBarrage);
            controlForMiniGameBarrageCpt.SetCameraFollowObj(npcCpt);
        }
    }

    /// <summary>
    /// 创建一个发射器
    /// </summary>
    /// <param name="position"></param>
    public MiniGameBarrageEjectorCpt CreateEjector(Vector3 ejectorPosition)
    {
        GameObject objEjector = Instantiate(objEjectorContainer, objEjectorModel);
        objEjector.transform.position = ejectorPosition;
        MiniGameBarrageEjectorCpt ejectorCpt = objEjector.GetComponent<MiniGameBarrageEjectorCpt>();
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

    public void DestoryBullet()
    {
       CptUtil.RemoveChild(objBulletContainer.transform);
    }

    /// <summary>
    /// 删除所有
    /// </summary>
    public override void DestroyAll()
    {
        base.DestroyAll();
        DestoryBullet();
        DestoryPlayer();
        DestoryEjector();
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SceneArenaManager : SceneBaseManager
{
    public Transform arena_1_CombatPosition;
    public Transform arena_1_EjectorPosition_1;
    public Transform arena_1_EjectorPosition_2;
    public Transform arena_1_EjectorPosition_3;
    public Transform arena_1_EjectorPosition_4;
    public Transform arena_1_EjectorPosition_5;
    public Transform arena_1_EjectorPosition_6;
    public Transform arena_1_BarrageUserPosition;

    public Transform arena_2_PlayerStartPosition;
    public Transform arena_2_AuditorStartPosition;
    public Transform arena_2_CompereStartPosition_1;
    public Transform arena_2_CompereStartPosition_2;

    public GameObject arena_2_Obj_CallBoard_Container;
    public GameObject arena_2_Obj_AuditTable_Container;
    public GameObject arena_2_Obj_Stove_Container;


    public MiniGameAccountEjectorCpt arena_3_AccountEjector;
    public Transform arena_3_PlayerPosition;
    public Transform arena_3_CameraPosition;
    public Transform arena_3_MoneyPosition;

    /// <summary>
    /// 获取竞技场1的战斗地点
    /// </summary>
    /// <param name="vector3"></param>
    public void GetArenaForCombatBy1(out Vector3 vector3)
    {
        vector3 = arena_1_CombatPosition.position;
    }

    /// <summary>
    /// 获取竞技场1的发射台位置
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetArenaForBarrageEjectorBy1(int number)
    {
        List<Vector3> listEjectorPosition = new List<Vector3>();
        if (number == 1)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_1.position);
        }
        else if (number == 2)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_2.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_3.position);
        }
        else if (number == 3)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_4.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_5.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_6.position);
        }
        return listEjectorPosition;
    }

    /// <summary>
    /// 获取竞技场1弹幕游戏用户起始位置
    /// </summary>
    /// <returns></returns>
    public void GetArenaForBarrageUserPositionBy1(out Vector3 vector3)
    {
        vector3 = arena_1_BarrageUserPosition.position;
    }

    /// <summary>
    /// 获取竞技场2的烹饪游戏玩家起始位置
    /// </summary>
    /// <param name="vector3"></param>
    public void GetArenaForCookingPlayerPositionBy2(out Vector3 vector3)
    {
        vector3 = arena_2_PlayerStartPosition.position;
    }

    /// <summary>
    ///  获取竞技场2的烹饪游戏评审员起始位置
    /// </summary>
    /// <param name="auditorStartPosition"></param>
    public void GetArenaForCookingAuditorPositionBy2(out Vector3 auditorStartPosition)
    {
        auditorStartPosition = arena_2_AuditorStartPosition.position;
    }

    /// <summary>
    ///  获取竞技场2的烹饪游戏主持人起始位置
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public List<Vector3> GetArenaForCookingComperePositionBy2(int number)
    {
        List<Vector3> listPosition = new List<Vector3>();
        if (number == 1)
        {
            listPosition.Add(arena_2_CompereStartPosition_1.position);
        }
        else if (number == 2)
        {
            listPosition.Add(arena_2_CompereStartPosition_1.position);
            listPosition.Add(arena_2_CompereStartPosition_2.position);
        }
        return listPosition;
    }

    /// <summary>
    /// 获取竞技场2的通告板
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingCallBoardCpt> GetArenaForCookingCallBoardBy2()
    {
        MiniGameCookingCallBoardCpt[] listCallBoard = arena_2_Obj_CallBoard_Container.GetComponentsInChildren<MiniGameCookingCallBoardCpt>();
        return TypeConversionUtil.ArrayToList(listCallBoard);
    }

    /// <summary>
    /// 获取竞技场2的评审桌子
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingAuditTableCpt> GetArenaForCookingAuditTableBy2()
    {
        MiniGameCookingAuditTableCpt[] listCallBoard = arena_2_Obj_AuditTable_Container.GetComponentsInChildren<MiniGameCookingAuditTableCpt>();
        return TypeConversionUtil.ArrayToList(listCallBoard);
    }

    /// <summary>
    /// 获取竞技场2的灶台
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCookingStoveCpt> GetArenaForCookingStoveBy2()
    {
        MiniGameCookingStoveCpt[] listCallBoard = arena_2_Obj_Stove_Container.GetComponentsInChildren<MiniGameCookingStoveCpt>();
        return TypeConversionUtil.ArrayToList(listCallBoard);
    }

    /// <summary>
    /// 获取竞技场3的玩家位置
    /// </summary>
    /// <param name="playerPosition"></param>
    public void GetArenaForAccountPlayerBy3(out Vector3 playerPosition)
    {
        playerPosition = arena_3_PlayerPosition.position;
    }

    /// <summary>
    /// 获取竞技场3的摄像头位置
    /// </summary>
    /// <param name="cameraPosition"></param>
    public void GetArenaForAccountCameraBy3(out Vector3 cameraPosition)
    {
        cameraPosition = arena_3_CameraPosition.position;
    }

    /// <summary>
    /// 获取竞技场3的金钱生成位置
    /// </summary>
    /// <param name="tfMoneyPosition"></param>
    public void GetArenaForAccountMoneyBy3(out Transform tfMoneyPosition)
    {
        tfMoneyPosition = arena_3_MoneyPosition;
    }

    /// <summary>
    /// 获取竞技场3的发射器
    /// </summary>
    /// <param name="ejectorCpt"></param>
    public void GetArenaForAccountEjectorBy3(out MiniGameAccountEjectorCpt ejectorCpt)
    {
        ejectorCpt = arena_3_AccountEjector;
    }
}
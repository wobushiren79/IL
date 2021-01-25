using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface IGameDataView
{
    /// <summary>
    /// 获取简略游戏数据列表成功
    /// </summary>
    /// <param name="listData"></param>
    void GetGameDataSimpleListSuccess(List<GameDataSimpleBean> listData, Action<List<GameDataSimpleBean>> action);

    /// <summary>
    /// 获取简略游戏数据列表失败
    /// </summary>
    void GetGameDataSimpleListFail();

    /// <summary>
    /// 获取游戏数据成功
    /// </summary>
    /// <param name="gameData"></param>
    void GetGameDataSuccess(GameDataBean gameData);

    /// <summary>
    /// 获取游戏数据失败
    /// </summary>
    void GetGameDataFail();

    /// <summary>
    /// 设置游戏数据成功
    /// </summary>
    void SetGameDataSuccess();

    /// <summary>
    /// 设置游戏数据失败
    /// </summary>
    void SetGameDataFail();

    /// <summary>
    /// 删除游戏数据成功
    /// </summary>
    void DeleteGameDataSuccess();

    /// <summary>
    /// 删除游戏数据失败
    /// </summary>
    void DeleteGameDataFail();
}
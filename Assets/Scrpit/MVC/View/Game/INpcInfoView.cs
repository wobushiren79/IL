using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface INpcInfoView
{
    /// <summary>
    /// 获取NPC信息成功
    /// </summary>
    /// <param name="type">-1.所有， 0,普通</param>
    /// <param name="listData"></param>
    void GetNpcInfoSuccess(int type,List<NpcInfoBean> listData);

    /// <summary>
    /// 获取NPC信息失败
    /// </summary>
    void GetNpcInfoFail(int type);
}
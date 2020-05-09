using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NpcInfoService: BaseMVCService
{

    public NpcInfoService() : base("npc_info", "npc_info_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<NpcInfoBean> QueryAllData()
    {
        return BaseQueryData<NpcInfoBean>("npc_id", tableNameForMain + ".valid", "1");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<NpcInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<NpcInfoBean>("npc_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<NpcInfoBean> QueryDataById(long id)
    {
       return  QueryDataByIds(new long[] { id });
    }

    /// <summary>
    /// 根据Type查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<NpcInfoBean> QueryDataByType(int[] type)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(type, ",");
        return BaseQueryData<NpcInfoBean>("npc_id", tableNameForMain + ".npc_type", "IN", "(" + values + ")");
    }

    public List<NpcInfoBean> QueryDataByType(int type)
    {
        return QueryDataByType(new int[] { type } );
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void DeleteData(NpcInfoBean npcInfo)
    {
        if (npcInfo == null)
            return;
        BaseDeleteDataById(npcInfo.id);
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void InsertData(NpcInfoBean itemsInfo)
    {
        List<string> listLeftName = new List<string>
        {
            "npc_id",
            "title_name",
            "name"
        };
        BaseInsertDataWithLeft(itemsInfo, listLeftName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="npcInfoData"></param>
    public void Update(NpcInfoBean npcInfoData)
    {
        DeleteData(npcInfoData);
        InsertData(npcInfoData);
    }
}
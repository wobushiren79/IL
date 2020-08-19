﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class StoreInfoBean : BaseBean
{
    public long store_id;
    public int type;//类型 9市场
    public int store_goods_type;//商店商品类型（不同商店有不同类型）
    public string mark;
    public long mark_id;
    public int mark_type;
    public int mark_x;
    public int mark_y;
    public int get_number;//获的 购买一次获得的数量
    public long price_l;
    public long price_m;
    public long price_s;
    public long guild_coin;
    public long trophy_elementary;
    public long trophy_intermediate;
    public long trophy_advanced;
    public long trophy_legendary;

    public string pre_ach_ids;//前置成就ID
    public string pre_data;//前置条件
    public string pre_data_minigame;//小游戏前置条件
    public string reward_data;//奖励

    public string icon_key;//图标KEY
    public string name;
    public string content;


    /// <summary>
    /// 获取前置成就ID
    /// </summary>
    /// <returns></returns>
    public List<long> GetPreAchIds()
    {
        if (pre_ach_ids == null)
            return null;
        List<string> listIdsStr = StringUtil.SplitBySubstringForListStr(pre_ach_ids, ',');
        List<long> listData = TypeConversionUtil.ListStrToListLong(listIdsStr);
        return listData;
    }


    /// <summary>
    /// 检测是否满足所有前置成就ID
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public bool CheckPreAchIds(GameDataBean gameData)
    {
        List<long> achIds = GetPreAchIds();
        if (CheckUtil.ListIsNull(achIds))
        {
            return true;
        }
        foreach (long achId in achIds)
        {
            if (!gameData.GetAchievementData().CheckHasAchievement(achId))
            {
                return false;
            }
        }
        return true;
    }

}
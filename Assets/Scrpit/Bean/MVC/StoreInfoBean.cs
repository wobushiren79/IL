using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class StoreInfoBean : BaseBean
{
    public int type;//类型 9市场
    public string mark;
    public long mark_id;
    public int mark_type;
    public int mark_x;
    public int mark_y;
    public long price_l;
    public long price_m;
    public long price_s;
    public long guild_coin;

    public string pre_ach_ids;//前置成就ID

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
        List<string> listIdsStr = StringUtil.SplitBySubstring(pre_ach_ids, ',');
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
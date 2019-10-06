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

    //小游戏专用数据
    public int minigame_player_number;//玩家数量
    public float minigame_win_survivaltime;//生存时间
    public float minigame_win_life;//胜利生命值

    //弹幕游戏专用数据
    public int barrage_launch_interval;//发射间隔
    public float barrage_launch_speed;//发射速度
    public string barrage_launch_types;//发射器子弹类型


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
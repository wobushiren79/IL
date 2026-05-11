using System;
using System.Collections.Generic;
public partial class StoreInfoBean
{
     /// <summary>
    /// 获取前置成就ID
    /// </summary>
    /// <returns></returns>
    public List<long> GetPreAchIds()
    {
        if (pre_ach_ids == null)
            return null;
        List<string> listIdsStr = pre_ach_ids.SplitForListStr(',');
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
        if (achIds.IsNull())
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
public partial class StoreInfoCfg
{
}

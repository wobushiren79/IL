using System;
using System.Collections.Generic;
public partial class AchievementInfoBean
{
    public AchievementTypeEnum GetAchievementType()
    {
        return (AchievementTypeEnum)type;
    }

    public long[]  GetPreAchIds()
    {
        return pre_ach_ids.SplitForArrayLong(',');
    }
}
public partial class AchievementInfoCfg
{
}

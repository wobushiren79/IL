using System;
using System.Collections.Generic;
public partial class SkillInfoBean
{
    /// <summary>
    /// 获取剩下的使用次数
    /// </summary>
    /// <param name="usedNumber"></param>
    /// <returns></returns>
    public int GetRestNumber(int usedNumber)
    {
        int number = GetUseNumber() - usedNumber;
        if (number < 0)
            number = 0;
        return number;
    }

    /// <summary>
    /// 获取使用次数
    /// </summary>
    /// <returns></returns>
    public int GetUseNumber()
    {
        if (use_number == 0)
            use_number = 1;
        return use_number;
    }
}
public partial class SkillInfoCfg
{
}

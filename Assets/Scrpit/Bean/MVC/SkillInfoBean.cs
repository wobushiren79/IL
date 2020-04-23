using UnityEngine;
using UnityEditor;

public class SkillInfoBean : BaseBean
{
    public long skill_id;

    //图标
    public string icon_key;
    //附加特性
    public string effect;
    public string effect_details;
    //使用次数
    public int use_number;
    //前置解锁条件
    public string pre_data;

    //名字
    public string name;
    //描述
    public string content;

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
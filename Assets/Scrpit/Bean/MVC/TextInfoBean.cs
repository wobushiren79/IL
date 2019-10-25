using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class TextInfoBean : BaseBean
{
    //类型 0默认文本 1选择对话    4书本详情  5黑幕标题
    public int type;
    //文本编号
    public long mark_id;
    //文本发起对象ID
    public long user_id;
    //文本发起人名字
    public string name;
    //文本内容
    public string content;
    //文本顺序
    public int text_order;
    //指定下一顺序
    public int next_order;
    //停留时间
    public float wait_time;
    //选择对话 的类型 0提示文本  1选项
    public int select_type;
    //选择结果指向
    public int select_result;

    //是否停止时间
    public bool is_stoptime;

    //触发条件
    public bool condition_first_meet;
    //触发条件 好感区间
    public int condition_min_favorability;
    public int condition_max_favorability;

    //选择加的好感
    public int add_favorability;
    public string add_money;


    /// <summary>
    /// 获取增加的钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void GetAddMoney(out long moneyL, out long moneyM, out long moneyS)
    {
        if (CheckUtil.StringIsNull(add_money))
        {
            moneyL = 0;
            moneyM = 0;
            moneyS = 0;
            return;
        }
        long[] listData= StringUtil.SplitBySubstringForArrayLong(add_money,',');
        moneyL = listData[0];
        moneyM = listData[1];
        moneyS = listData[2];
    }
}
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class TextInfoBean : BaseBean
{
    public long text_id;
    //类型 0默认文本 1选择对话    4书本详情  5黑幕标题
    public int type;
    //对话类型 0普通对话，1送礼回复对话  2招募对话
    public int talk_type;
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



    //是否停止时间
    public int is_stoptime;

    //触发条件
    public int condition_first_meet;
    //触发条件 好感区间
    public int condition_min_favorability;
    public int condition_max_favorability;

    //好处的前提条件 类型，人数
    public string add_pre;
    //增加的好感
    public int add_favorability;
    //增加的金钱
    public string add_money;
    //增加的角色
    public string add_character;
    //增加的物品
    public string add_items;

    //场景人物表情
    public string scene_expression;

    public TextInfoBean() {

    }
    public TextInfoBean(int selectType,string content)
    {
        text_order = 1;
        type = 1;
        this.select_type = selectType;
        this.content = content;
    }
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
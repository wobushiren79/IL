using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ItemsInfoBean : BaseBean
{
    public long intact_id;//套装ID
    public long items_id;//装备ID

    public int items_type;//装备类型 1帽子 2衣服  3鞋子
    public string icon_key;//装备图标

    public float add_waiter;//增加 跑堂
    public float add_account;//增加 算账
    public float add_shout;//增加 吆喝
    public float add_force;//增加 武力
    public float add_chop;//增加 切菜
    public float add_cook;//增加 做菜

    public int intact_number;//套装触发数量

    public string name;//名字
    public string content;//内容
}
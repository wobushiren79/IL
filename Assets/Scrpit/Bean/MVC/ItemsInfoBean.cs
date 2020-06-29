﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ItemsInfoBean : BaseBean
{
    public long intact_id;//套装ID
    public long items_id;//装备ID

    public int items_type;//装备类型 1帽子 2衣服  3鞋子
    public string icon_key;//装备图标
    public string anim_key;//动画名字

    public int add_life;//添加生命值
    public int add_cook;//增加 做菜
    public int add_speed;//增加 跑堂
    public int add_account;//增加 算账
    public int add_charm;//增加 吆喝
    public int add_force;//增加 武力
    public int add_lucky;//增加 切菜
    public int add_loyal;//增加信任

    public int rotation_angle;//选择角度

    public long add_id;//增加的内容ID
    public string effect;//效果 增加效果
    public string effect_details;//效果详情

    //public int intact_number;//套装触发数量

    public string name;//名字
    public string content;//内容
    public int rarity;//稀有度


    /// <summary>
    /// 获取类型
    /// </summary>
    /// <returns></returns>
    public GeneralEnum GetItemsType()
    {
        return (GeneralEnum)items_type;
    }

    /// <summary>
    /// 获取物品稀有度
    /// </summary>
    /// <returns></returns>
    public RarityEnum GetItemRarity()
    {
        return (RarityEnum)rarity;
    }
}
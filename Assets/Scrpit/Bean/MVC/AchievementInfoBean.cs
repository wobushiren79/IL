using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class AchievementInfoBean : BaseBean
{
    public long ach_id;
    public long pre_ach_id;//前置成就
    public int type;//类型 1通用  2菜品
    public string icon_key;
    public string icon_key_remark;
    public long remark_id;

    //拥有的金钱
    public long achieve_money_s;
    public long achieve_money_m;
    public long achieve_money_l;
    //需要支付的金钱
    public long achieve_pay_s;
    public long achieve_pay_m;
    public long achieve_pay_l;
    //销售数量
    public long achieve_sell_number;

    //奖励
    //公会硬币
    public long reward_guildcoin;
    //奖励道具
    public string reward_items_ids;
    //奖励建筑材料
    public string reward_build_ids;


    public string name;
    public string content;


    public List<long> GetRewardItems()
    {
        List<string> listData = StringUtil.SplitBySubstring(reward_items_ids, ',');
        return TypeConversionUtil.ListStrToListLong(listData);
    }

    public List<long> GetRewardBuild()
    {
        List<string> listData = StringUtil.SplitBySubstring(reward_build_ids, ',');
        return TypeConversionUtil.ListStrToListLong(listData);
    }

    /// <summary>
    /// 检测是否满足条件
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public bool CheckAchievement(GameDataBean gameData)
    {
        //检测拥有的钱
        if (achieve_money_s != 0 && achieve_money_s > gameData.moneyS)
        {
            return false;
        }
        if (achieve_money_m != 0 && achieve_money_m > gameData.moneyM)
        {
            return false;
        }
        if (achieve_money_l != 0 && achieve_money_l > gameData.moneyL)
        {
            return false;
        }
        //检测支付的钱
        if (achieve_pay_s != 0 && achieve_pay_s > gameData.moneyS)
        {
            return false;
        }
        if (achieve_pay_m != 0 && achieve_pay_m > gameData.moneyM)
        {
            return false;
        }
        if (achieve_pay_l != 0 && achieve_pay_l > gameData.moneyL)
        {
            return false;
        }
        //销售数量要求
        if (achieve_sell_number != 0)
        {
            MenuOwnBean menuOwn = gameData.GetMenuById(remark_id);
            if (menuOwn != null && achieve_sell_number > menuOwn.sellNumber)
                return false;
            else if (menuOwn == null)
            {
                return false;
            }
        }
        return true;
    }


}
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class AchievementInfoBean : BaseBean
{
    public long ach_id;
    public long pre_ach_id;//前置成就
    public int type;//类型 1通用  2菜品
    public string icon_key;

    //拥有的金钱
    public long achieve_money_s;
    public long achieve_money_m;
    public long achieve_money_l;
    //需要支付的金钱
    public long achieve_pay_s;
    public long achieve_pay_m;
    public long achieve_pay_l;

    public string name;
    public string content;

   /// <summary>
   /// 检测是否满足条件
   /// </summary>
   /// <param name="gameData"></param>
   /// <returns></returns>
    public bool CheckAchievement(GameDataBean gameData)
    {
        return false;
    }
}
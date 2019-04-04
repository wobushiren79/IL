using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBean 
{
   public string userId;//用户ID
   public long money;//1黄金=10白银  1白银=1000文
   public string innName;//客栈名称
   public CharacterBean userCharacter;// 老板
   public List<CharacterBean> staffCharacterList;//员工
   public TimeBean gameTime;//游戏时间

    public static void GetMoneyDetails(long money,out long L,out long M, out long S)
    {
        long temp1 = money % 10;
        long temp2 = money % 100/10;
        long temp3 = money % 1000/100;
        long temp4 = money % 10000 / 1000;
        S = temp3 * 100 + temp2 * 10+ temp1;
        M = temp4;
        L = money / 10000;
    }
}
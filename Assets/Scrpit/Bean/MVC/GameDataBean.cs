using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBean 
{
   public string userId;//用户ID
   public long money;//1黄金=10白银  1白银=1000文
   public CharacterBean userCharacter;// 老板
   public List<CharacterBean> staffCharacterList;//员工
   public TimeBean gameTime;//游戏时间
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCharacterForCookingBean : MiniGameCharacterBean
{
    //烹饪游戏
    public MenuInfoBean cookingMenuInfo;

    //游玩成绩
    public MiniGameCookingSettleBean settleDataForPre;
    public MiniGameCookingSettleBean settleDataForMaking;
    public MiniGameCookingSettleBean settleDataForEnd;

    //分数 题 色 香 味
    public List<int> listScoreForTheme=new List<int>();
    public List<int> listScoreForColor = new List<int>();
    public List<int> listScoreForSweet = new List<int>();
    public List<int> listScoreForTaste = new List<int>();
}
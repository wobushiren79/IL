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

    public int scoreForTheme = 0;
    public int scoreForColor = 0;
    public int scoreForSweet = 0;
    public int scoreForTaste = 0;
    public int scoreForTotal = 0;

    //期望分数
    public int scoreForExpect = 0;

    public void InitScore()
    {
        scoreForTheme = GetScore(listScoreForTheme);
        scoreForColor = GetScore(listScoreForColor);
        scoreForSweet = GetScore(listScoreForSweet);
        scoreForTaste = GetScore(listScoreForTaste);
        scoreForTotal = (scoreForTheme + scoreForColor + scoreForSweet + scoreForTaste) / 4;
    }

    private int GetScore(List<int> listData)
    {
        if (CheckUtil.ListIsNull(listData))
        {
            scoreForExpect = Random.Range(50,90);
            //如果是敌人
            if (characterType == 0)
            {
               return Random.Range(scoreForExpect-5, scoreForExpect+5);
            }
            return 0;
        }
       
        int score = 0;
        foreach (int itemScore in listData)
        {
            score += itemScore;
        }
        return score / listData.Count;
    }
}
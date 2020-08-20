using UnityEngine;
using UnityEditor;
using System;

public class UIGambleTrickySize : UIBaseGamble<GambleTrickySizeBean,GambleTrickySizeHandler,GambleTrickySizeBuilder>
{
    public GambleTrickySizeItem gambleTrickySizeItem;

    public override void OpenUI()
    {
        base.OpenUI();
        //根据等级设置杯子数量
        int level = int.Parse(remarkData);
        gambleData = new GambleTrickySizeBean
        {
            winRate = 0.5f - (0.08f * level),
            winRewardRate = level * 0.5f + 1.5f,
            //betMaxForMoneyS = 100 * ((long)Math.Pow(10, level - 1))
            betMaxForMoneyS = 100 * level * 5
        };
        SetData(gambleData);
        gambleBuilder.SetCup(gambleTrickySizeItem);
        gambleHandler.InitGame(gambleData);
    }

    public override void CloseUI()
    {
        base.CloseUI();
   
    }

    public override void SetData(GambleTrickySizeBean gambleData)
    {
        base.SetData(gambleData);
    }

}
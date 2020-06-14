using UnityEngine;
using UnityEditor;
using System;

public class GambleTrickySizeBuilder : BaseGambleBuilder
{
    public GambleTrickySizeItem gambleCup;




    public void SetCup(GambleTrickySizeItem gambleCup)
    {
        this.gambleCup = gambleCup;
    }
    public GambleTrickySizeItem GetCup()
    {
        return gambleCup;
    }

    public void InitCup()
    {
        gambleCup.SetStatus(GambleTrickySizeItem.CupStatusEnum.Idle);
    }
}
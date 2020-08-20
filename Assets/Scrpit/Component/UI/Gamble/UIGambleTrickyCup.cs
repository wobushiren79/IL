using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class UIGambleTrickyCup : UIBaseGamble<GambleTrickyCupBean, GambleTrickyCupHandler, GambleTrickyCupBuilder>
{
    public GameObject objItemCupContainer;
    public GameObject objItemCupModel;

    public override void OpenUI()
    {
        if (gambleBuilder)
            gambleBuilder.CleanAllCup();
        base.OpenUI();
        //根据等级设置杯子数量
        int level = int.Parse(remarkData);
        gambleData = new GambleTrickyCupBean
        {
            cupNumber = 2 + level,
            changeNumber = level * 10,
            winRewardRate = level * 0.5f + 1.5f,
            betMaxForMoneyS = 100 * ((long)Math.Pow(10, level - 1))
        };
        SetData(gambleData);
        gambleHandler.InitGame(gambleData);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        gambleBuilder.CleanAllCup();
    }

    public override void SetData(GambleTrickyCupBean gambleData)
    {
        base.SetData(gambleData);
        CreateCupList();
    }

    public void CreateCupList()
    {
        float width = ((RectTransform)objItemCupContainer.transform).sizeDelta.x;
        float itemW = width / (gambleData.cupNumber + 1);
        int randomCup = UnityEngine.Random.Range(0, gambleData.cupNumber);
        for (int i = 0; i < gambleData.cupNumber; i++)
        {
            GameObject objItemCup = Instantiate(objItemCupContainer, objItemCupModel);
            GambleTrickyCupItem itemCup = objItemCup.GetComponent<GambleTrickyCupItem>();

            //随机设置一个杯子有骰子
            if (i == randomCup)
            {
                itemCup.hasDice = true;
            }
            else
            {
                itemCup.hasDice = false;
            }

            //设置杯子位置
            RectTransform rtItem = ((RectTransform)objItemCup.transform);
            rtItem.anchoredPosition = new Vector2(itemW * (i + 1) - width / 2, rtItem.anchoredPosition.y);

            gambleBuilder.AddCup(itemCup);
        }
    }

}
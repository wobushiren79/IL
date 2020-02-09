using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using static GameTimeHandler;
using System.Collections.Generic;

public class InfoRecordPopupShow : PopupShowView
{
    public Text tvDate;
    public Text tvStatus;

    public Text tvIncomeL;
    public Text tvIncomeM;
    public Text tvIncomeS;

    public Text tvExpensesL;
    public Text tvExpensesM;
    public Text tvExpensesS;

    public Text tvPraiseExcite;
    public Text tvPraiseHappy;
    public Text tvPraiseOkay;
    public Text tvPraiseOrdinary;
    public Text tvPraiseDisappointed;
    public Text tvPraiseAnger;

    public Text tvConsumeIngOilsalt;
    public Text tvConsumeIngMeat;
    public Text tvConsumeIngRiverfresh;
    public Text tvConsumeIngSeafood;
    public Text tvConsumeIngVegetablest;
    public Text tvConsumeIngMelonfruit;
    public Text tvConsumeIngWaterwine;
    public Text tvConsumeIngFlour;

    public GameObject objFoodContainer;
    public GameObject objFoodItem;

    protected InnFoodManager innFoodManager;

    private void Awake()
    {
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="innRecordData"></param>
    public void SetData(InnRecordBean innRecordData)
    {
        SetDate(innRecordData.year, innRecordData.month, innRecordData.day);
        SetStatus((DayEnum)innRecordData.status);
        SetTotalIncome(innRecordData.incomeL, innRecordData.incomeM, innRecordData.incomeS);
        SetTotalExpenses(innRecordData.expensesL, innRecordData.expensesM, innRecordData.expensesS);
        SetPraise(
            innRecordData.praiseExcitedNumber,
            innRecordData.praiseHappyNumber,
            innRecordData.praiseOkayNumber,
            innRecordData.praiseOrdinaryNumber,
            innRecordData.praiseDisappointedNumber,
            innRecordData.praiseAngerNumber);
        SetConsumeIng(
            innRecordData.consumeIngOilsalt,
            innRecordData.consumeIngMeat,
            innRecordData.consumeIngRiverfresh,
            innRecordData.consumeIngSeafood,
            innRecordData.consumeIngVegetablest,
            innRecordData.consumeIngMelonfruit,
            innRecordData.consumeIngWaterwine,
            innRecordData.consumeIngFlour);
        SetFoodSellList(innRecordData.listSellNumber);
    }

    /// <summary>
    /// 设置日期
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void SetDate(int year, int month, int day)
    {
        if (tvDate != null)
        {
            string monthStr = "";
            switch ((SeasonsEnum)month)
            {
                case SeasonsEnum.Spring:
                    monthStr = GameCommonInfo.GetUITextById(33);
                    break;
                case SeasonsEnum.Summer:
                    monthStr = GameCommonInfo.GetUITextById(34);
                    break;
                case SeasonsEnum.Autumn:
                    monthStr = GameCommonInfo.GetUITextById(35);
                    break;
                case SeasonsEnum.Winter:
                    monthStr = GameCommonInfo.GetUITextById(36);
                    break;
            }
            tvDate.text = year + GameCommonInfo.GetUITextById(29) + monthStr + GameCommonInfo.GetUITextById(30) + day + GameCommonInfo.GetUITextById(31);
        }
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(DayEnum status)
    {
        if (tvStatus == null)
            return;
        string statusStr = "";
        switch (status)
        {
            case DayEnum.None:
                break;
            case DayEnum.Work:
                statusStr = GameCommonInfo.GetUITextById(72);
                tvStatus.color = Color.green;
                break;
            case DayEnum.Rest:
                statusStr = GameCommonInfo.GetUITextById(73);
                tvStatus.color = Color.red;
                break;
        }
        tvStatus.text = statusStr;
    }

    /// <summary>
    /// 设置总收入
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void SetTotalIncome(long incomeL, long incomeM, long incomeS)
    {
        if (tvIncomeL != null)
            tvIncomeL.text = "" + incomeL;
        if (tvIncomeM != null)
            tvIncomeM.text = "" + incomeM;
        if (tvIncomeS != null)
            tvIncomeS.text = "" + incomeS;
    }

    /// <summary>
    /// 设置总支出
    /// </summary>
    /// <param name="expensesL"></param>
    /// <param name="expensesM"></param>
    /// <param name="expensesS"></param>
    public void SetTotalExpenses(long expensesL, long expensesM, long expensesS)
    {
        if (tvExpensesL != null)
            tvExpensesL.text = "" + expensesL;
        if (tvExpensesM != null)
            tvExpensesM.text = "" + expensesM;
        if (tvExpensesS != null)
            tvExpensesS.text = "" + expensesS;
    }

    /// <summary>
    /// 设置好感
    /// </summary>
    /// <param name="praiseExciteNumber"></param>
    /// <param name="praiseHappyNumber"></param>
    /// <param name="praiseOkayNumber"></param>
    /// <param name="praiseOrdinaryNumber"></param>
    /// <param name="praiseDisappointedNumber"></param>
    /// <param name="praiseAngerNumber"></param>
    public void SetPraise(
        long praiseExciteNumber,
        long praiseHappyNumber,
        long praiseOkayNumber,
        long praiseOrdinaryNumber,
        long praiseDisappointedNumber,
        long praiseAngerNumber)
    {
        if (tvPraiseExcite)
        {
            tvPraiseExcite.text = "" + praiseExciteNumber;
        }
        if (tvPraiseHappy)
        {
            tvPraiseHappy.text = "" + praiseHappyNumber;
        }
        if (tvPraiseOkay)
        {
            tvPraiseOkay.text = "" + praiseOkayNumber;
        }
        if (tvPraiseOrdinary)
        {
            tvPraiseOrdinary.text = "" + praiseOrdinaryNumber;
        }
        if (tvPraiseDisappointed)
        {
            tvPraiseDisappointed.text = "" + praiseDisappointedNumber;
        }
        if (tvPraiseAnger)
        {
            tvPraiseAnger.text = "" + praiseAngerNumber;
        }
    }

    /// <summary>
    /// 设置消耗素材
    /// </summary>
    /// <param name="consumeIngOilsalt"></param>
    /// <param name="consumeIngMeat"></param>
    /// <param name="consumeIngRiverfresh"></param>
    /// <param name="consumeIngSeafood"></param>
    /// <param name="consumeIngVegetablest"></param>
    /// <param name="consumeIngMelonfruit"></param>
    /// <param name="consumeIngWaterwine"></param>
    /// <param name="consumeIngFlour"></param>
    public void SetConsumeIng(
        int consumeIngOilsalt,
        int consumeIngMeat,
        int consumeIngRiverfresh,
        int consumeIngSeafood,
        int consumeIngVegetablest,
        int consumeIngMelonfruit,
        int consumeIngWaterwine,
        int consumeIngFlour)
    {
        if (tvConsumeIngOilsalt)
        {
            tvConsumeIngOilsalt.text = consumeIngOilsalt + "";
        }
        if (tvConsumeIngMeat)
        {
            tvConsumeIngMeat.text = consumeIngMeat + "";
        }
        if (tvConsumeIngRiverfresh)
        {
            tvConsumeIngRiverfresh.text = consumeIngRiverfresh + "";
        }
        if (tvConsumeIngSeafood)
        {
            tvConsumeIngSeafood.text = consumeIngSeafood + "";
        }
        if (tvConsumeIngVegetablest)
        {
            tvConsumeIngVegetablest.text = consumeIngVegetablest + "";
        }
        if (tvConsumeIngMelonfruit)
        {
            tvConsumeIngMelonfruit.text = consumeIngMelonfruit + "";
        }
        if (tvConsumeIngWaterwine)
        {
            tvConsumeIngWaterwine.text = consumeIngWaterwine + "";
        }
        if (tvConsumeIngFlour)
        {
            tvConsumeIngFlour.text = consumeIngFlour + "";
        }
    }

    /// <summary>
    /// 设置销售食物
    /// </summary>
    /// <param name="listData"></param>
    public void SetFoodSellList(List<GameItemsBean> listData)
    {
        CptUtil.RemoveChildsByActive(objFoodContainer);
        if (listData == null)
            return;
        foreach (GameItemsBean itemData in listData)
        {
            GameObject objItem = Instantiate(objFoodContainer, objFoodItem);
            ItemPopupRecordFoodCpt itemCpt = objItem.GetComponent<ItemPopupRecordFoodCpt>();
            MenuInfoBean menuInfo=  innFoodManager.GetFoodDataById(itemData.itemId);
            Sprite spIcon = innFoodManager.GetFoodSpriteByName(menuInfo.icon_key);
            itemCpt.SetData(spIcon, menuInfo.name, itemData.itemNumber, itemData.priceL, itemData.priceM, itemData.priceS);
        }
    }
}
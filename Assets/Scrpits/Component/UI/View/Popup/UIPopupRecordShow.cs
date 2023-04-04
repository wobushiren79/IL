using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using static GameTimeHandler;
using System.Collections.Generic;

public class UIPopupRecordShow : PopupShowView
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
    public Text tvConsumeIngVegetables;
    public Text tvConsumeIngMelonfruit;
    public Text tvConsumeIngWaterwine;
    public Text tvConsumeIngFlour;

    public GameObject objShowContainer;
    public GameObject objShowItem;


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="innRecordData"></param>
    public void SetData(InnRecordBean innRecordData)
    {
        SetDate(innRecordData.year, innRecordData.month, innRecordData.day);
        SetStatus((DayEnum)innRecordData.status);

        innRecordData.GetTotalIncome(out long incomeL, out long incomeM, out long incomeS);
        SetTotalIncome(incomeL, incomeM, incomeS);

        innRecordData.GetTotalExpenses(out long expensesL, out long expensesM, out long expensesS);
        SetTotalExpenses(expensesL, expensesM, expensesS);

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
            innRecordData.consumeIngVegetables,
            innRecordData.consumeIngMelonfruit,
            innRecordData.consumeIngWaterwine,
            innRecordData.consumeIngFlour);
        CptUtil.RemoveChildsByActive(objShowContainer);
        SetHotel(innRecordData.incomeForHotelL, innRecordData.incomeForHotelM, innRecordData.incomeForHotelS);
        SetFoodSellList(innRecordData.listSellNumber, innRecordData.incomeL, innRecordData.incomeM, innRecordData.incomeS);
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
                    monthStr = TextHandler.Instance.manager.GetTextById(33);
                    break;
                case SeasonsEnum.Summer:
                    monthStr = TextHandler.Instance.manager.GetTextById(34);
                    break;
                case SeasonsEnum.Autumn:
                    monthStr = TextHandler.Instance.manager.GetTextById(35);
                    break;
                case SeasonsEnum.Winter:
                    monthStr = TextHandler.Instance.manager.GetTextById(36);
                    break;
            }
            tvDate.text = year + TextHandler.Instance.manager.GetTextById(29) + monthStr + TextHandler.Instance.manager.GetTextById(30) + day + TextHandler.Instance.manager.GetTextById(31);
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
                statusStr = TextHandler.Instance.manager.GetTextById(72);
                tvStatus.color = Color.green;
                break;
            case DayEnum.Rest:
                statusStr = TextHandler.Instance.manager.GetTextById(73);
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
        SetMoney(tvIncomeL, incomeL);
        SetMoney(tvIncomeM, incomeM);
        SetMoney(tvIncomeS, incomeS);
    }

    /// <summary>
    /// 设置总支出
    /// </summary>
    /// <param name="expensesL"></param>
    /// <param name="expensesM"></param>
    /// <param name="expensesS"></param>
    public void SetTotalExpenses(long expensesL, long expensesM, long expensesS)
    {
        SetMoney(tvExpensesL, expensesL);
        SetMoney(tvExpensesM, expensesM);
        SetMoney(tvExpensesS, expensesS);
    }


    protected void SetMoney(Text tvMoney, long money)
    {
        if (tvMoney != null)
        {
            if (money == 0)
            {
                //tvMoney.gameObject.SetActive(false);
            }
            else
            {
                // tvMoney.gameObject.SetActive(true);
            }
            tvMoney.text = "" + money;
        }
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
    /// <param name="consumeIngVegetables"></param>
    /// <param name="consumeIngMelonfruit"></param>
    /// <param name="consumeIngWaterwine"></param>
    /// <param name="consumeIngFlour"></param>
    public void SetConsumeIng(
        int consumeIngOilsalt,
        int consumeIngMeat,
        int consumeIngRiverfresh,
        int consumeIngSeafood,
        int consumeIngVegetables,
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
        if (tvConsumeIngVegetables)
        {
            tvConsumeIngVegetables.text = consumeIngVegetables + "";
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
    public void SetFoodSellList(List<GameItemsBean> listData, long priceL, long priceM, long priceS)
    {
        if (listData == null)
            return;
        if (listData.Count >= 20)
        {
            GameObject objItem = Instantiate(objShowContainer, objShowItem);
            ItemPopupRecordCpt itemCpt = objItem.GetComponent<ItemPopupRecordCpt>();
            Sprite spIcon = IconHandler.Instance.GetIconSpriteByName("ach_sellmenunumber_2");
            itemCpt.SetData(spIcon, TextHandler.Instance.manager.GetTextById(332), 0, priceL, priceM, priceS);
        }
        else
        {
            for (int i = 0; i < listData.Count; i++)
            {
                GameItemsBean itemData = listData[i];
                GameObject objItem = Instantiate(objShowContainer, objShowItem);
                ItemPopupRecordCpt itemCpt = objItem.GetComponent<ItemPopupRecordCpt>();
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(itemData.itemId);
                Sprite spIcon = InnFoodHandler.Instance.manager.GetFoodSpriteByName(menuInfo.icon_key);
                itemCpt.SetData(spIcon, menuInfo.name, itemData.itemNumber, itemData.priceL, itemData.priceM, itemData.priceS);
            }
        }
    }

    /// <summary>
    /// 设置入住
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void SetHotel(long priceL, long priceM, long priceS)
    {
        if (priceS != 0)
        {
            GameObject objItem = Instantiate(objShowContainer, objShowItem);
            ItemPopupRecordCpt itemCpt = objItem.GetComponent<ItemPopupRecordCpt>();
            Sprite spIcon = IconHandler.Instance.GetIconSpriteByName("worker_waiter_bed_pro_2");
            itemCpt.SetData(spIcon, TextHandler.Instance.manager.GetTextById(351), 0, priceL, priceM, priceS);
        }
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIGameStatisticsForRevenue : BaseUIChildComponent<UIGameStatistics>, IRadioGroupCallBack, GameDataManager.IUserRevenueCallBack
{
    public RadioGroupView rgMonth;
    public Dropdown ddYear;
    //柱状图
    public CartogramBarView cartogramBar;

    protected GameDataManager gameDataManager;

    public List<int> listYear;
    public UserRevenueBean userRevenueData;

    private void Awake()
    {
        gameDataManager = uiComponent.uiGameManager.gameDataManager;
        if (rgMonth != null)
            rgMonth.SetCallBack(this);
        if (ddYear != null)
            ddYear.onValueChanged.AddListener(OnValueChangedForYear);
    }

    public override void Open()
    {
        base.Open();
        gameDataManager.SetUserRevenueCallBack(this);
        gameDataManager.GetUserRevenueYear();
    }

    /// <summary>
    /// 年改变
    /// </summary>
    /// <param name="position"></param>
    public void OnValueChangedForYear(int position)
    {
        gameDataManager.GetUserRevenueByYear(listYear[position]);
    }

    /// <summary>
    /// 获取相应月份数据
    /// </summary>
    /// <param name="month"></param>
    /// <param name="userRevenueData"></param>
    /// <returns></returns>
    public UserRevenueMonthBean GetUserRevenueMonthData(int month, UserRevenueBean userRevenueData)
    {
        if (userRevenueData == null || userRevenueData.listMonthData == null)
            return null;
        foreach (UserRevenueMonthBean monthData in userRevenueData.listMonthData)
        {
            if (month == monthData.month)
            {
                return monthData;
            }
        }
        return null;
    }

    #region 季节选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiComponent.uiGameManager.audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        int month = 1;
        switch (rbview.name)
        {
            case "Spring":
                month = 1;
                break;
            case "Summer":
                month = 2;
                break;
            case "Autumn":
                month = 3;
                break;
            case "Winter":
                month = 4;
                break;
        }
        UserRevenueMonthBean userRevenueMonthData = GetUserRevenueMonthData(month, userRevenueData);
        if (userRevenueMonthData == null)
        {
            cartogramBar.SetData(new List<CartogramDataBean>());
        }
        else
        {
            ((RevenueCartogramBarView)cartogramBar).SetData(userRevenueMonthData.GetListCartogramDataForIncome(), userRevenueMonthData.listDayData);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 营收数据回调
    public void GetUserRevenueSuccess(UserRevenueBean userRevenueData)
    {
        this.userRevenueData = userRevenueData;
        rgMonth.SetPosition(0, true);
    }

    public void GetUserRevenueYearSuccess(List<int> listYear)
    {
        this.listYear = listYear;
        ddYear.ClearOptions();
        if (this.listYear == null || this.listYear.Count == 0)
        {
            this.listYear = new List<int>
            {
                 gameDataManager.gameData.gameTime.year
            };
        }
        List<Dropdown.OptionData> listOptionData = new List<Dropdown.OptionData>();
        foreach (int itemYear in this.listYear)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData
            {
                text = string.Format(GameCommonInfo.GetUITextById(46), itemYear + "")
            };
            listOptionData.Add(optionData);
        }
        ddYear.AddOptions(listOptionData);
        //ddYear.value = 0;
        ddYear.SetValueWithoutNotify(0);
        OnValueChangedForYear(0);
    }
    #endregion
}
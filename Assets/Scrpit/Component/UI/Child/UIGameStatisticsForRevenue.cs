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

    private void Start()
    {

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
        cartogramBar.SetData(userRevenueMonthData.GetListCartogramData());
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
                1,
                2
            };
        }
        foreach (int itemYear in this.listYear)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData
            {
                text = string.Format(GameCommonInfo.GetUITextById(46), itemYear + "")
            };
            ddYear.options.Add(optionData);
        }
        ddYear.value = 1;
        //OnValueChangedForYear(0);
    }
    #endregion
}
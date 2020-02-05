using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIGameStatisticsForRevenue : BaseUIChildComponent<UIGameStatistics>, IRadioGroupCallBack
{
    public RadioGroupView rgMonth;
    public Dropdown ddYear;
    //柱状图
    public CartogramBarView cartogramBar;

    protected GameDataManager gameDataManager;

    private void Awake()
    {
        gameDataManager = uiComponent.uiGameManager.gameDataManager;
    }

    private void Start()
    {
        if (ddYear != null)
            ddYear.onValueChanged.AddListener(OnValueChangedForYear);
    }

    public override void Open()
    {
        base.Open();
        List<CartogramDataBean> listCartogramData = new List<CartogramDataBean>();
        for (int i = 0; i < 30; i++)
        {
            CartogramDataBean cartogramData = new CartogramDataBean();
            cartogramData.value_1 = i * 10;
            listCartogramData.Add(cartogramData);
        }
        cartogramBar.SetData(listCartogramData);
    }

    /// <summary>
    /// 年改变
    /// </summary>
    /// <param name="position"></param>
    public void OnValueChangedForYear(int position)
    {

    }

    #region 季节选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        switch (rbview.name)
        {
            case "Spring":
                break;
            case "Summer":
                break;
            case "Autumn":
                break;
            case "Winter":
                break;
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}
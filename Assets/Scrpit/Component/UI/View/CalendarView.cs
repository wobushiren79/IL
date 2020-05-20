using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class CalendarView : BaseMonoBehaviour, IDateInfoView
{
    public Text tvYear;
    public Text tvSeasons;

    public GameObject objDayContent;
    public GameObject objDayModel;

    public int year;
    public int month;
    public int day;

    public DateInfoController dateInfoController;
    protected GameDataManager gameDataManager;

    private List<ItemGameCalendarCpt> mListItemDay = new List<ItemGameCalendarCpt>();

    private void OnGUI()
    {
        //防止拖动分辨率 UI变形
        RectTransform rtfDayContent = objDayContent.GetComponent<RectTransform>();
        GridLayoutGroup glgDayContent = objDayContent.GetComponent<GridLayoutGroup>();
        float itemW = rtfDayContent.rect.width / 7f;
        float itemH = rtfDayContent.rect.height / 6f;
        glgDayContent.cellSize = new Vector2(itemW, itemH);
    }

    private void Awake()
    {
        dateInfoController = new DateInfoController(this, this);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void InitData(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        SetYear(year);
        SetSeasons(month);
        InitMonth(month);
    }

    /// <summary>
    /// 改变数据
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void ChangeData(int year, int month, int day)
    {
        tvYear.transform.DOKill();
        tvSeasons.transform.DOKill();
        tvYear.transform.localScale = new Vector3(1f, 1f, 1f);
        tvSeasons.transform.localScale = new Vector3(1f, 1f, 1f);
        this.day = day;
        if (this.year != year)
        {
            this.year = year;
            tvYear.transform
                .DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f)
                .From()
                .SetEase(Ease.OutBack);
            SetYear(year);
        }
        if (this.month != month)
        {
            this.month = month;
            this.day = day;
            tvSeasons.transform
                .DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f)
                .From()
                .SetEase(Ease.OutBack);
            SetSeasons(month);
            InitMonth(month);
        }
        else
        {
            SetCurrentDay(day);
        }
    }

    /// <summary>
    /// 设置建筑日
    /// </summary>
    /// <param name="buildDay"></param>
    public void SetBuildDay()
    {
        InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
        if (innBuildData.listBuildDay.Count > 0)
        {
            foreach (TimeBean itemBuildDay in innBuildData.listBuildDay)
            {
                foreach (ItemGameCalendarCpt itemGameCalendar in mListItemDay)
                {
                    if (itemGameCalendar.dateInfo.day == itemBuildDay.day
                        && itemGameCalendar.dateInfo.month== itemBuildDay.month
                        && year == itemBuildDay.year)
                    {
                        itemGameCalendar.SetRemark("建");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置年份
    /// </summary>
    /// <param name="year"></param>
    public void SetYear(int year)
    {
        if (tvYear != null)
            tvYear.text = GameCommonInfo.GetUITextById(32) + year + GameCommonInfo.GetUITextById(29);
    }

    /// <summary>
    /// 设置季度
    /// </summary>
    /// <param name="month"></param>
    public void SetSeasons(int month)
    {
        if (tvSeasons == null)
            return;
        switch ((SeasonsEnum)month)
        {
            case SeasonsEnum.Spring:
                tvSeasons.text = GameCommonInfo.GetUITextById(33);
                tvSeasons.color = new Color(0.22f, 0.87f, 0f);
                break;
            case SeasonsEnum.Summer:
                tvSeasons.text = GameCommonInfo.GetUITextById(34);
                tvSeasons.color = new Color(1f, 0.8f, 0f);
                break;
            case SeasonsEnum.Autumn:
                tvSeasons.text = GameCommonInfo.GetUITextById(35);
                tvSeasons.color = new Color(1f, 0.32f, 0f);
                break;
            case SeasonsEnum.Winter:
                tvSeasons.text = GameCommonInfo.GetUITextById(36);
                tvSeasons.color = new Color(0f, 0.9f, 1f);
                break;
            default:
                tvSeasons.text = "";
                break;
        }
    }

    /// <summary>
    /// 初始化月份
    /// </summary>
    public void InitMonth(int month)
    {
        CptUtil.RemoveChildsByActive(objDayContent.transform);
        mListItemDay.Clear();
        dateInfoController.GetDateInfoByMonth(month);
    }

    /// <summary>
    /// 设置当前天
    /// </summary>
    /// <param name="day"></param>
    public void SetCurrentDay(int day)
    {
        if (mListItemDay == null)
            return;
        foreach (ItemGameCalendarCpt itemDay in mListItemDay)
        {
            if (itemDay.dateInfo != null && itemDay.dateInfo.day == day)
            {
                itemDay.SetItemStatus(true);
            }
            else
            {
                itemDay.SetItemStatus(false);
            }
        }
    }

    #region 日期数据回调
    public void GetDateInfoSuccess(List<DateInfoBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            DateInfoBean itemData = listData[i];
            GameObject objDay = Instantiate(objDayModel, objDayContent.transform);
            objDay.SetActive(true);
            //设置数据
            ItemGameCalendarCpt calendarCpt = objDay.GetComponent<ItemGameCalendarCpt>();
            if (calendarCpt != null)
            {
                calendarCpt.SetData(itemData);
                mListItemDay.Add(calendarCpt);
                if (day == itemData.day)
                    calendarCpt.SetItemStatus(true);
            }
        }
        //如果当天是修建日，则需在日历上显示
        SetBuildDay();

        OnGUI();
    }

    public void GetDateInfoFail()
    {

    }
    #endregion
}
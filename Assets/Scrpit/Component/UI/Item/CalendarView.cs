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
    public float timeAnimChange = 3;
    public float timeDelay = 2;
    public DateInfoController dateInfoController;

    private List<ItemGameCalendarCpt> mListItemDay = new List<ItemGameCalendarCpt>();

    private void OnGUI()
    {
        //防止拖动分辨率 UI变形
        RectTransform rtfDayContent = objDayContent.GetComponent<RectTransform>();
        GridLayoutGroup glgDayContent = objDayContent.GetComponent<GridLayoutGroup>();
        float itemW = rtfDayContent.rect.width / 7f;
        float itemH = rtfDayContent.rect.height / 4f;
        glgDayContent.cellSize = new Vector2(itemW, itemH);
    }

    private void Awake()
    {
        dateInfoController = new DateInfoController(this, this);
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
        StopAllCoroutines();
        //延迟执行
        StartCoroutine(StartDelay(year, month, day));
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
        switch (month)
        {
            case 1:
                tvSeasons.text = GameCommonInfo.GetUITextById(33);
                tvSeasons.color = new Color(0.35f, 0.64f, 0.25f);
                break;
            case 2:
                tvSeasons.text = GameCommonInfo.GetUITextById(34);
                tvSeasons.color = new Color(0.9f, 0.8f, 0.15f);
                break;
            case 3:
                tvSeasons.text = GameCommonInfo.GetUITextById(35);
                tvSeasons.color = new Color(0.9f, 0.55f, 0.27f);
                break;
            case 4:
                tvSeasons.text = GameCommonInfo.GetUITextById(36);
                tvSeasons.color = new Color(0.3f, 0.4f, 0.6f);
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
    /// 延迟执行
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDelay(int year, int month, int day)
    {
        yield return new WaitForSeconds(timeDelay);
        tvYear.transform.DOKill();
        tvSeasons.transform.DOKill();
        tvYear.transform.localScale = new Vector3(1f, 1f, 1f);
        tvSeasons.transform.localScale = new Vector3(1f, 1f, 1f);
        if (this.year != year)
        {
            this.year = year;
            tvYear.transform
                .DOScale(new Vector3(0.5f, 0.5f, 0.5f),1f)
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
        if (this.day != day)
        {
            this.day = day;
            SetCurrentDay(day);
        }
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
    }

    public void GetDateInfoFail()
    {

    }
    #endregion
}
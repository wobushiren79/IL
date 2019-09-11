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

    private void Awake()
    {
        dateInfoController = new DateInfoController(this, this);
    }

    public void InitData(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        SetYear(year);
        SetSeasons(month);
        InitDays(month);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            ChangeData(year,month+ 1, day );
        }
    }

    public void ChangeData(int year, int month, int day)
    {
        StopAllCoroutines();
        //延迟执行
        StartCoroutine(StartDelay( year,month,day));
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
    /// 初始化天数
    /// </summary>
    public void InitDays(int month)
    {
        CptUtil.RemoveChildsByActive(objDayContent.transform);
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
            tvYear.transform
                .DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f)
                .From()
                .SetEase(Ease.OutBack);
            SetYear(year);
        }
        if (this.month != month)
        {
            tvSeasons.transform
                .DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f)
                .From()
                .SetEase(Ease.OutBack);
            SetSeasons(month);
        }
        this.year = year;
        this.month = month;
        this.day = day;
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
                calendarCpt.SetData(itemData);
        }
    }

    private void OnGUI()
    {
        //防止拖动分辨率 UI变形
        RectTransform rtfDayContent = objDayContent.GetComponent<RectTransform>();
        GridLayoutGroup glgDayContent = objDayContent.GetComponent<GridLayoutGroup>();
        float itemW = rtfDayContent.rect.width / 7f;
        float itemH = rtfDayContent.rect.height / 4f;
        glgDayContent.cellSize = new Vector2(itemW, itemH);
    }

    public void GetDateInfoFail()
    {
    }
    #endregion
}
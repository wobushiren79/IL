using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class UIGameDate : BaseUIComponent
{
    [Header("控件")]
    public Text tvYear;
    public Text tvMonth;
    public Text tvDay;

    public GameObject objDialog;
    public Button btWork;
    public Button btRest;

    [Header("数据")]
    public float animTime = 1;//动画时间
    public float animDelay = 2;//动画延迟
    public GameTimeHandler gameTimeHandler;//时间控制

    private void Start()
    {
        if (btWork != null)
            btWork.onClick.AddListener(InnWork);
        if (btRest != null)
            btRest.onClick.AddListener(InnRest);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (gameTimeHandler != null)
        {
            gameTimeHandler.GetTimeForDate(out int year, out int month, out int day);
            SetYear(year);
            SetMonth(month);
            SetDay(day);
            gameTimeHandler.GoToNextDay(1);
            gameTimeHandler.GetTimeForDate(out int newYear, out int newMonth, out int newDay);
            SetDate(newYear, newMonth, newDay);
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
        objDialog.SetActive(false);
    }

    public void SetDate(int year, int month, int day)
    {
        float totalAnimTime = 1;
        if (int.TryParse(tvDay.text, out int oldDay) && day != oldDay)
        {
            DOTween.To(() => oldDay,
            newPoint =>
            {
                SetDay(newPoint);
            },
            day,
            animTime)
           .SetDelay(animDelay);
            totalAnimTime = animDelay + animTime;
        }
        if (int.TryParse(tvMonth.text, out int oldMonth) && month != oldMonth)
        {
            DOTween.To(() => oldMonth,
            newPoint =>
            {
                SetMonth(newPoint);
            },
            month,
            animTime)
            .SetDelay(animDelay + animTime);
            totalAnimTime = animDelay + 2 * animTime;
        }
        if (int.TryParse(tvYear.text, out int oldYearh) && year != oldYearh)
        {
            DOTween.To(() => oldYearh,
            newPoint =>
            {
                SetYear(newPoint);
            },
            year,
            animTime)
            .SetDelay(animDelay + 2 * animTime);
            totalAnimTime = animDelay + 3 * animTime;
        }
        //展示是否营业框

        StartCoroutine(ShowDialog(totalAnimTime));

    }

    /// <summary>
    /// 延迟展示确认框
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator ShowDialog(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        objDialog.SetActive(true);
        objDialog.transform.localScale = new Vector3(1,1,1);
        objDialog.transform.DOScale(Vector3.zero,0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 设置年
    /// </summary>
    /// <param name="year"></param>
    public void SetYear(int year)
    {
        if (tvYear != null)
        {
            tvYear.text = "" + year;
        }
    }

    /// <summary>
    /// 设置月
    /// </summary>
    /// <param name="month"></param>
    public void SetMonth(int month)
    {
        if (tvMonth != null)
        {
            tvMonth.text = "" + month;
        }
    }

    /// <summary>
    /// 设置日
    /// </summary>
    /// <param name="day"></param>
    public void SetDay(int day)
    {
        if (tvDay != null)
        {
            tvDay.text = "" + day;
        }
    }

    public void InnWork()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

    public void InnRest()
    {
        GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
        ControlHandler controlHandler = GetUIMananger<UIGameManager>().controlHandler;

        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Rest;
        gameTimeHandler.StartNewDay(true);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
        controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
    }

}
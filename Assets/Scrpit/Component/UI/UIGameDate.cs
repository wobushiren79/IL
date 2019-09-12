using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class UIGameDate : BaseUIComponent
{
    [Header("控件")]
    public CalendarView calendarView;
    public Text tvDialogContent;

    public GameObject objDialog;
    public Button btWork;
    public Button btRest;

    [Header("数据")]
    public float animTime = 1;//动画时间
    public float animDelay = 2;//动画延迟

    private void Start()
    {
        if (tvDialogContent != null)
            tvDialogContent.text = GameCommonInfo.GetUITextById(3005);
        if (btWork != null)
            btWork.onClick.AddListener(InnWork);
        if (btRest != null)
            btRest.onClick.AddListener(InnRest);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameTimeHandler gameTimeHandler =   GetUIMananger<UIGameManager>().gameTimeHandler;
        if (gameTimeHandler != null)
        {
            gameTimeHandler.GetTime(out int year, out int month, out int day);
            calendarView.InitData(year, month, day);
            //保存数据
           
            //进入下一天
            gameTimeHandler.GoToNextDay(1);
            gameTimeHandler.GetTime(out int newYear, out int newMonth, out int newDay);
            SetDate(newYear, newMonth, newDay);
        }
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        objDialog.SetActive(false);
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.RestoreControl();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void SetDate(int year, int month, int day)
    {
        calendarView.ChangeData(year,month,day);
        //展示是否营业框
        StartCoroutine(ShowDialog(3));
    }

    /// <summary>
    /// 延迟展示确认框
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator ShowDialog(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
        // 第一天默认不营业
        gameTimeHandler.GetTime(out int year, out int month, out int day);
        if (year == 221 && day == 1 && day == 1)
        {
            InnRest();
        }
        else
        {
            objDialog.SetActive(true);
            objDialog.transform.localScale = new Vector3(1, 1, 1);
            objDialog.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutBack);
        }
    }

    public void InnWork()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameAttendance));
    }

    public void InnRest()
    {
        GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
        ControlHandler controlHandler = GetUIMananger<UIGameManager>().controlHandler;
        NpcCustomerBuilder npcCustomerBuilder = GetUIMananger<UIGameManager>().npcCustomerBuilder;

        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Rest;
        gameTimeHandler.SetTimeStatus(false);

        //没有触发事件
        if (!EventHandler.Instance.EventTriggerForStory())
        {
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
            controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
            //开始建造NPC
            npcCustomerBuilder.StartBuildCustomer();
        }
    }

}
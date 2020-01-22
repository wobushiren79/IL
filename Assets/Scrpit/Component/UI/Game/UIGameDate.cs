using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class UIGameDate : BaseUIComponent
{
    [Header("控件")]
    public GameObject objContent;
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
        AnimForInit();
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameTimeHandler != null)
        {
            gameTimeHandler.GetTime(out int year, out int month, out int day);
            //设置日历
            calendarView.InitData(year, month, day);
            //保存数据

            //下一天
            StartCoroutine(CoroutineForNextDay());
        }
        if (GetUIManager<UIGameManager>().controlHandler != null)
            GetUIManager<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        objDialog.SetActive(false);
        if (GetUIManager<UIGameManager>().controlHandler != null)
            GetUIManager<UIGameManager>().controlHandler.RestoreControl();
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        if (objContent != null)
            objContent.transform.DOScaleY(0,0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 协程-下一天
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForNextDay()
    {
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        yield return new WaitForSeconds(3);
        //进入下一天
        gameTimeHandler.GoToNextDay(1);
        gameTimeHandler.GetTime(out int newYear, out int newMonth, out int newDay);
        calendarView.ChangeData(newYear, newMonth, newDay);

        //展示是否营业框
        yield return new WaitForSeconds(3);
        // 第一天默认不营业
        gameTimeHandler.GetTime(out int year, out int month, out int day);
        if (year == 221 && day == 1 && day == 1)
        {
            InnRest();
        }
        //如果时建设中则不营业
        else if (gameDataManager.gameData.GetInnBuildData().listBuildDay.Count > 0)
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

    /// <summary>
    /// 营业
    /// </summary>
    public void InnWork()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameAttendance));
    }

    /// <summary>
    /// 休息
    /// </summary>
    public void InnRest()
    {
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        NpcCustomerBuilder npcCustomerBuilder = GetUIManager<UIGameManager>().npcCustomerBuilder;
        EventHandler eventHandler = GetUIManager<UIGameManager>().eventHandler;
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Rest;
        gameTimeHandler.SetTimeStatus(false);

        //没有触发事件
        if (!eventHandler.EventTriggerForStory())
        {
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
            controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
            //开始建造NPC
            npcCustomerBuilder.StartBuildCustomer();
        }
    }

}
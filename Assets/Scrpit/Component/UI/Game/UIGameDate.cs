using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class UIGameDate : UIGameComponent
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

    protected GameTimeHandler gameTimeHandler;
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected ControlHandler controlHandler;
    protected NpcCustomerBuilder npcCustomerBuilder;
    protected EventHandler eventHandler;
    protected AudioHandler audioHandler;
    protected InnHandler innHandler;

    public override void Awake()
    {
        base.Awake();
        gameTimeHandler = uiGameManager.gameTimeHandler;
        gameDataManager = uiGameManager.gameDataManager;
        gameItemsManager = uiGameManager.gameItemsManager;
        controlHandler = uiGameManager.controlHandler;
        npcCustomerBuilder = uiGameManager.npcCustomerBuilder;
        eventHandler = uiGameManager.eventHandler;
        audioHandler = uiGameManager.audioHandler;
        innHandler = uiGameManager.innHandler;
    }

    private void Start()
    {
        if (tvDialogContent != null)
            tvDialogContent.text = GameCommonInfo.GetUITextById(3005);
        if (btWork != null)
            btWork.onClick.AddListener(OnClickInnWork);
        if (btRest != null)
            btRest.onClick.AddListener(OnClickInnRest);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit();

        if (gameTimeHandler != null)
        {
            gameTimeHandler.GetTime(out int year, out int month, out int day);
            //设置日历
            calendarView.InitData(year, month, day);
            //保存数据

            //下一天
            StartCoroutine(CoroutineForNextDay());
        }
        if (controlHandler != null)
            controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        objDialog.SetActive(false);
        if (controlHandler != null)
            controlHandler.RestoreControl();
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        if (objContent != null)
            objContent.transform.DOScaleY(0, 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 按钮-工作
    /// </summary>
    public void OnClickInnWork()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        InnWork();
    }

    /// <summary>
    /// 按钮-休息
    /// </summary>
    public void OnClickInnRest()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        InnRest();
    }

    /// <summary>
    /// 协程-下一天
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForNextDay()
    {
        yield return new WaitForSeconds(3);
        //进入下一天
        gameTimeHandler.GoToNextDay(1);
        gameTimeHandler.GetTime(out int newYear, out int newMonth, out int newDay);
        calendarView.ChangeData(newYear, newMonth, newDay);
        audioHandler.PlaySound( AudioSoundEnum.ButtonForShow);
        //展示是否营业框
        yield return new WaitForSeconds(1.5f);
        // 第一天默认不营业
        gameTimeHandler.GetTime(out int year, out int month, out int day);

        List<CharacterBean> listWorker= gameDataManager.gameData.GetAllCharacterData();
        //如果有请假的员工 新的一天结束请假
        foreach (CharacterBean itemWork in listWorker)
        {
            if(itemWork.baseInfo.GetWorkerStatus()== WorkerStatusEnum.Vacation)
            {
                itemWork.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
            }
        }
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
        List<CharacterBean> listWorker = gameDataManager.gameData.GetAllCharacterData();
        //计算员工请假概率
        foreach (CharacterBean itemWork in listWorker)
        {
            if (itemWork.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Rest
                || itemWork.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
            {
                if (itemWork.CalculationWorkerVacation(gameItemsManager, gameDataManager))
                {
                    long vacationId = Random.Range(1101, 1111);
                    string vacationStr = string.Format(GameCommonInfo.GetUITextById(vacationId), itemWork.baseInfo.name);
                    uiGameManager.toastManager.ToastHint(vacationStr);
                    itemWork.baseInfo.SetWorkerStatus(WorkerStatusEnum.Vacation);
                }
            }
        }
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameAttendance));
    }

    /// <summary>
    /// 休息
    /// </summary>
    public void InnRest()
    {

        gameTimeHandler.SetDayStatus(GameTimeHandler.DayEnum.Rest);
        gameTimeHandler.SetTimeStatus(false);

        //没有触发事件
        if (!eventHandler.EventTriggerForStory())
        {
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
            //设置位置
            Vector3 startPosition = innHandler.GetRandomEntrancePosition();
            BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
            baseControl.SetFollowPosition(startPosition + new Vector3(0,-2,0));
        }
    }

}
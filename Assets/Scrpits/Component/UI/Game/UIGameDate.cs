﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class UIGameDate : BaseUIComponent
{
    [Header("控件")]
    public GameObject objContent;
    public CalendarView calendarView;
    public Text tvDialogContent;

    public GameObject objDialog;
    public Button btWork;
    public Button btRest;
    public Button btBrith;

    [Header("数据")]
    protected float animTimeForWaitNext = 1f;//动画时间
    protected float animTimeForShowDialog = 2f;//动画延迟

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (tvDialogContent != null)
            tvDialogContent.text = TextHandler.Instance.manager.GetTextById(3005);
        if (btWork != null)
            btWork.onClick.AddListener(OnClickInnWork);
        if (btRest != null)
            btRest.onClick.AddListener(OnClickInnRest);
        if (btBrith != null)
            btBrith.onClick.AddListener(OnClickBirth);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit();
        btBrith.gameObject.SetActive(false);
        GameTimeHandler.Instance.SetTimeStop();
        GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);
        //设置日历
        calendarView.InitData(year, month, day);
        //下一天
        StartCoroutine(CoroutineForNextDay());
        //停止控制
        GameControlHandler.Instance.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        objDialog.SetActive(false);
        GameControlHandler.Instance.RestoreControl();
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
    /// 生孩子初始化
    /// </summary>
    public void BirthForInit()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //如果是结婚之后 并且当天还拥有爱爱次数
        if (gameData.GetFamilyData().CheckMarry(gameData.gameTime) && GameCommonInfo.DailyLimitData.CheckBirthNumber(1))
        {
            btBrith.gameObject.SetActive(true);
        }
        else
        {
            btBrith.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 按钮-工作
    /// </summary>
    public void OnClickInnWork()
    {

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        InnWork();
    }

    /// <summary>
    /// 按钮-休息
    /// </summary>
    public void OnClickInnRest()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        InnRest();
    }

    /// <summary>
    /// 按钮-生孩子
    /// </summary>
    public void OnClickBirth()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        int year = gameData.GetGameTimeForYear();
        if (year == 0)
            year = 1;
        float playSpeed = Mathf.Clamp(10f / year, 2, 10);
        MiniGameBirthBean gameBirthData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Birth) as MiniGameBirthBean;

        gameBirthData.fireNumber = 10;
        gameBirthData.enmeySpeed = 3;
        gameBirthData.enmeyBuildInterval = 0.8f;
        gameBirthData.playSpeed = playSpeed;
        gameBirthData.addBirthPro = 0.01f;
        gameBirthData.gameReason = MiniGameReasonEnum.Other;

        gameBirthData.InitForMiniGame();
        MiniGameHandler.Instance.handlerForBirth.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForBirth.InitGame(gameBirthData);
    }


    /// <summary>
    /// 协程-下一天
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForNextDay()
    {
        yield return new WaitForSeconds(animTimeForWaitNext);
        //进入下一天
        GameTimeHandler.Instance.GoToNextDay(1);
        GameTimeHandler.Instance.GetTime(out int newYear, out int newMonth, out int newDay);
        calendarView.ChangeData(newYear, newMonth, newDay);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForShow);
        //展示是否营业框
        yield return new WaitForSeconds(animTimeForShowDialog);

        GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterBean> listWorker = gameData.GetAllCharacterData();
        //如果有请假的员工 新的一天结束请假
        foreach (CharacterBean itemWork in listWorker)
        {
            if (itemWork.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Vacation)
            {
                itemWork.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
            }
        }
        FamilyDataBean familyData = gameData.GetFamilyData();
        // 第一年和第二年 第一天默认不营业
        if ((year == 221 || year == 222) && month == 1 && day == 1)
        {
            InnRest();
        }
        //如果时建设中则不营业
        else if (gameData.GetInnBuildData().listBuildDay.Count > 0)
        {
            InnRest();
        }
        //如果是结婚日
        else if (familyData.timeForMarry.year == year && familyData.timeForMarry.month == month && familyData.timeForMarry.day == day)
        {
            InnRest();
        }
        else
        {
            objDialog.SetActive(true);
            objDialog.transform.localScale = new Vector3(1, 1, 1);
            objDialog.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutBack);
            BirthForInit();
        }

        HandleCourtyardData();
    }

    /// <summary>
    /// 处理后院数据
    /// </summary>
    public void HandleCourtyardData()
    {
        //检测所有的收获物品
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
        innCourtyardData.AddDay(1);
    }

    /// <summary>
    /// 营业
    /// </summary>
    public void InnWork()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterBean> listWorker = gameData.listWorkerCharacter;
        //计算员工请假概率
        foreach (CharacterBean itemWork in listWorker)
        {
            if (itemWork.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Rest
                || itemWork.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
            {
                if (itemWork.CalculationWorkerVacation())
                {
                    long vacationId = Random.Range(1101, 1111);
                    string vacationStr = string.Format(TextHandler.Instance.manager.GetTextById(vacationId), itemWork.baseInfo.name);
                    UIHandler.Instance.ToastHint<ToastView>(vacationStr);
                    itemWork.baseInfo.SetWorkerStatus(WorkerStatusEnum.Vacation);
                }
            }
        }
        UIHandler.Instance.OpenUIAndCloseOther<UIGameAttendance>();
    }

    /// <summary>
    /// 休息
    /// </summary>
    public void InnRest()
    {
        GameTimeHandler.Instance.SetDayStatus(GameTimeHandler.DayEnum.Rest);
        GameTimeHandler.Instance.SetTimeStatus(false);

        //设置位置
        Vector3 startPosition = InnHandler.Instance.GetRandomEntrancePosition();
        BaseControl baseControl = GameControlHandler.Instance.StartControl<BaseControl>(GameControlHandler.ControlEnum.Normal);
        baseControl.SetFollowPosition(startPosition + new Vector3(0, -2, 0));

        //触发事件检测 
        if (!GameEventHandler.Instance.EventTriggerForStory())
        {
            //没有触发事件 直接打开UI
            UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
        }
        else
        {
            //触发了事件
        }
    }

    /// <summary>
    /// 小游戏回掉
    /// </summary>
    /// <param name="miniGameStatus"></param>
    /// <param name="data"></param>
    protected void NotifyForMiniGameStatus(MiniGameStatusEnum miniGameStatus, params object[] data)
    {
        if (miniGameStatus == MiniGameStatusEnum.GameClose)
        {
            UIGameDate ui = UIHandler.Instance.GetUI<UIGameDate>();
            ui.gameObject.SetActive(true);
            objDialog.SetActive(true);
            BirthForInit();
            GameTimeHandler.Instance.SetTimeStop();
            GameControlHandler.Instance.StopControl();
        }
    }
}
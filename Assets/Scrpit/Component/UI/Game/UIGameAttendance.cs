using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameAttendance : UIBaseOne, ItemGameAttendanceCpt.ICallBack
{
    public Text tvPriceL;
    public Text tvPriceM;
    public Text tvPirceS;
    public Text tvNumber;

    public Button btSubmit;
    public Button btSelectAll;
    public Button btUnSelectAll;

    public ScrollGridVertical gridVertical;

    //出勤金钱
    public long attendancePriceL;
    public long attendancePriceM;
    public long attendancePriceS;

    public int attendanceNumber;//出勤人数
    public List<CharacterBean> listData = new List<CharacterBean>();
    public override void Start()
    {
        base.Start();
        if (btSubmit != null)
            btSubmit.onClick.AddListener(StartWork);
        if (btSelectAll != null)
            btSelectAll.onClick.AddListener(OnClickForSelectAll);
        if (btUnSelectAll != null)
            btUnSelectAll.onClick.AddListener(OnClickForUnSelectAll);
        if (gridVertical != null)
            gridVertical.AddCellListener(OnCellForItem);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        InitData();
        InitAttendancePrice();
    }

    public void InitAttendancePrice()
    {
        attendanceNumber = 0;
        attendancePriceL = 0;
        attendancePriceM = 0;
        attendancePriceS = 0;
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean characterData = listData[i];
            //初始化数据
            if (characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
            {
                attendancePriceL += characterData.baseInfo.priceL;
                attendancePriceM += characterData.baseInfo.priceM;
                attendancePriceS += characterData.baseInfo.priceS;
                attendanceNumber += 1;
            }
        }
        tvPriceL.text = attendancePriceL + "";
        tvPriceM.text = attendancePriceM + "";
        tvPirceS.text = attendancePriceS + "";
        tvNumber.text = TextHandler.Instance.manager.GetTextById(4003) + attendanceNumber;
    }

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        CharacterBean characterData = listData[itemCell.index];
        ItemGameAttendanceCpt workerItem = itemCell.GetComponent<ItemGameAttendanceCpt>();
        workerItem.SetData(characterData);
        workerItem.SetCallBack(this);
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    public void StartWork()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //如果出勤人数太少
        if (attendanceNumber <= 0)
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1013));
            return;
        }
        if (!gameData.HasEnoughMoney(attendancePriceL, attendancePriceM, attendancePriceS))
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1014));
            return;
        }

        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        foreach (CharacterBean itemCharacter in listCharacter)
        {
            //没有出勤的人员减少忠诚
            if (itemCharacter.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Rest
                || itemCharacter.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Research)
            {
                itemCharacter.attributes.AddLoyal(-2);
            }
            //增加出勤天数
            else if (itemCharacter.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
            {
                itemCharacter.baseInfo.AddWorkDay(1);
            }
        }
        //支付出勤费用
        gameData.PayMoney(attendancePriceL, attendancePriceM, attendancePriceS);
        //记录出勤费用
        InnHandler.Instance.GetInnRecord().AddPayWage(attendancePriceL, attendancePriceM, attendancePriceS);
        //设置当天状态
        GameTimeHandler.Instance.SetDayStatus(GameTimeHandler.DayEnum.Work);
        //设置是否停止时间
        GameTimeHandler.Instance.SetTimeStatus(false);
        //打开客栈
        InnHandler.Instance.OpenInn();
        //放开控制
        GameControlHandler.Instance.StartControl<ControlForWorkCpt>(GameControlHandler.ControlEnum.Work);
        //开启主UI
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

    public void InitData()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listData = gameData.GetAllCharacterData();
        gridVertical.SetCellCount(listData.Count);
    }


    public void OnClickForSelectAll()
    {
        ChangeAllSelectStatus(true);
    }

    public void OnClickForUnSelectAll()
    {
        ChangeAllSelectStatus(false);
    }

    protected void ChangeAllSelectStatus(bool isSelect)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean characterData = listData[i];
            WorkerStatusEnum workerStatus = characterData.baseInfo.GetWorkerStatus();
            if (workerStatus == WorkerStatusEnum.Work)
            {
                if (!isSelect)
                    characterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
            }
            else if (workerStatus == WorkerStatusEnum.Rest)
            {
                if (isSelect)
                    characterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Work);
            }
        }
        gridVertical.RefreshAllCells();
        InitAttendancePrice();
    }

    #region  出勤回调
    public void AttendanceChange(ItemGameAttendanceCpt itemView, WorkerStatusEnum workerStatus, CharacterBean characterBean)
    {
        InitAttendancePrice();
    }
    #endregion
}
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

    public GameObject objListContent;
    public GameObject objItemWorkModel;

    //出勤金钱
    public long attendancePriceL;
    public long attendancePriceM;
    public long attendancePriceS;

    public int attendanceNumber;//出勤人数

    public override void Start()
    {
        base.Start();
        if (btSubmit != null)
            btSubmit.onClick.AddListener(StartWork);
        if (btSelectAll != null)
            btSelectAll.onClick.AddListener(OnClickForSelectAll);
        if (btUnSelectAll != null)
            btUnSelectAll.onClick.AddListener(OnClickForUnSelectAll);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        attendanceNumber = 0;
        attendancePriceL = 0;
        attendancePriceM = 0;
        attendancePriceS = 0;
        InitData();
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
        CptUtil.RemoveChildsByActive(objListContent);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterBean> listData = gameData.GetAllCharacterData();
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            CreateWorkerItem(itemData);
        }
        SetTotalData();
    }

    public void CreateWorkerItem(CharacterBean characterData)
    {
        if (objListContent == null || objItemWorkModel == null)
            return;
        GameObject objWorkerItem = Instantiate(objListContent, objItemWorkModel);
        ItemGameAttendanceCpt workerItem = objWorkerItem.GetComponent<ItemGameAttendanceCpt>();
        if (workerItem != null)
        {
            workerItem.SetData(characterData);
            workerItem.SetCallBack(this);
        }
        //初始化数据
        if (characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
        {
            attendancePriceL += characterData.baseInfo.priceL;
            attendancePriceM += characterData.baseInfo.priceM;
            attendancePriceS += characterData.baseInfo.priceS;
            attendanceNumber += 1;
        }

    }

    public void SetTotalData()
    {
        tvPriceL.text = attendancePriceL + "";
        tvPriceM.text = attendancePriceM + "";
        tvPirceS.text = attendancePriceS + "";
        tvNumber.text = TextHandler.Instance.manager.GetTextById(4003) + attendanceNumber;
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
        ItemGameAttendanceCpt[] listAttendance = objListContent.GetComponentsInChildren<ItemGameAttendanceCpt>();
        for (int i = 0; i < listAttendance.Length; i++)
        {
            ItemGameAttendanceCpt itemAttendance= listAttendance[i];
            if (itemAttendance.gameObject.activeSelf)
            {
                itemAttendance.ChangeSelectStauts(isSelect);
            }
        }
    }

    #region  出勤回调
    public void AttendanceChange(ItemGameAttendanceCpt itemView, WorkerStatusEnum workerStatus, CharacterBean characterBean)
    {
        if (workerStatus == WorkerStatusEnum.Work)
        {
            attendancePriceL += characterBean.baseInfo.priceL;
            attendancePriceM += characterBean.baseInfo.priceM;
            attendancePriceS += characterBean.baseInfo.priceS;
            attendanceNumber += 1;
        }
        else
        {
            attendancePriceL -= characterBean.baseInfo.priceL;
            attendancePriceM -= characterBean.baseInfo.priceM;
            attendancePriceS -= characterBean.baseInfo.priceS;
            attendanceNumber -= 1;
        }
        if (attendancePriceL < 0)
            attendancePriceL = 0;
        if (attendancePriceM < 0)
            attendancePriceM = 0;
        if (attendancePriceS < 0)
            attendancePriceS = 0;
        if (attendanceNumber < 0)
            attendanceNumber = 0;
        SetTotalData();
    }
    #endregion
}
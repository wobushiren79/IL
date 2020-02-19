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

    public GameObject objListContent;
    public GameObject objItemWorkModle;

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
        uiGameManager.audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        //如果出勤人数太少
        if (attendanceNumber <= 0)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1013));
            return;
        }
        if (!uiGameManager.gameDataManager.gameData.HasEnoughMoney(attendancePriceL, attendancePriceM, attendancePriceS))
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1014));
            return;
        }
        //支付出勤费用
        uiGameManager.gameDataManager.gameData.PayMoney(attendancePriceL, attendancePriceM, attendancePriceS);
        //设置当天状态
        uiGameManager.gameTimeHandler.SetDayStatus(GameTimeHandler.DayEnum.Work);
        //设置是否停止时间
        uiGameManager.gameTimeHandler.SetTimeStatus(false);
        //开启主UI
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
        //打开客栈
        uiGameManager.innHandler.OpenInn();
        //放开控制
        uiGameManager.controlHandler.StartControl(ControlHandler.ControlEnum.Work);
    }

    public void InitData()
    {
        CptUtil.RemoveChildsByActive(objListContent.transform);
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameDataManager == null)
            return;
        List<CharacterBean> listData = gameDataManager.gameData.GetAllCharacterData();
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            CreateWorkerItem(itemData);
        }
        SetTotalData();
    }

    public void CreateWorkerItem(CharacterBean characterData)
    {
        if (objListContent == null || objItemWorkModle == null)
            return;
        GameObject objWorkerItem = Instantiate(objItemWorkModle, objListContent.transform);
        objWorkerItem.SetActive(true);
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
        tvNumber.text = GameCommonInfo.GetUITextById(4003) + attendanceNumber;
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
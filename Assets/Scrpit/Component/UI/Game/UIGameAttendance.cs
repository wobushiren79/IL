using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameAttendance : BaseUIComponent, ItemGameAttendanceCpt.ICallBack
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

    private void Start()
    {
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
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        InnHandler innHandler = GetUIManager<UIGameManager>().innHandler;
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        NpcCustomerBuilder npcCustomerBuilder = GetUIManager<UIGameManager>().npcCustomerBuilder;
        //如果出勤人数太少
        if (attendanceNumber <= 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1013));
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(attendancePriceL, attendancePriceM, attendancePriceS))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1014));
            return;
        }
        //支付出勤费用
        gameDataManager.gameData.PayMoney(attendancePriceL, attendancePriceM, attendancePriceS);
        //设置当天状态
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Work;
        //设置是否停止时间
        gameTimeHandler.SetTimeStatus(false);
        //开启主UI
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
        //打开客栈
        innHandler.OpenInn();
        //放开控制
        controlHandler.StartControl(ControlHandler.ControlEnum.Work);
        //开始建造NPC
        npcCustomerBuilder.StartBuildCustomer();
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
        if (characterData.baseInfo.isAttendance)
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
    public void AttendanceChange(ItemGameAttendanceCpt itemView, bool isAttendance, CharacterBean characterBean)
    {
        if (isAttendance)
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
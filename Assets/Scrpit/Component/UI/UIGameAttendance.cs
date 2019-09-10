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
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
        InnHandler innHandler = GetUIMananger<UIGameManager>().innHandler;
        ControlHandler controlHandler = GetUIMananger<UIGameManager>().controlHandler;
        ToastView toastView = GetUIMananger<UIGameManager>().toastView;

        //如果出勤人数太少
        if (attendanceNumber <= 0)
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1013));
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(attendancePriceL, attendancePriceM, attendancePriceS))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1014));
            return;
        }
        gameDataManager.gameData.PayMoney(attendancePriceL, attendancePriceM, attendancePriceS);
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Work;
        gameTimeHandler.SetTimeStatus(false);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
        innHandler.OpenInn();
        controlHandler.StartControl(ControlHandler.ControlEnum.Work);
    }

    public void InitData()
    {
        CptUtil.RemoveChildsByActive(objListContent.transform);
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        if (gameDataManager == null)
            return;
        List<CharacterBean> listData = new List<CharacterBean>();
        listData.Add(gameDataManager.gameData.userCharacter);
        listData.AddRange(gameDataManager.gameData.workCharacterList);
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
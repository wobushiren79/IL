using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PickForMoneyDialogView : DialogView
{
    public Button btSubMoneyL;
    public Button btAddMoneyL;
    public Text tvMoneyL;

    public Button btSubMoneyM;
    public Button btAddMoneyM;
    public Text tvMoneyM;

    public Button btSubMoneyS;
    public Button btAddMoneyS;
    public Text tvMoneyS;

    //每次增量
    public int cvMoneyL = 1;
    public int cvMoneyM = 1;
    public int cvMoneyS = 1;

    //最大金钱数
    public long maxMoneyL = 1;
    public long maxMoneyM = 1;
    public long maxMoneyS = 1;

    public long moneyL = 0;
    public long moneyM = 0;
    public long moneyS = 0;
    
    protected ToastManager toastManager;

    public override void Awake()
    {
        base.Awake();
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
    }

    public override void InitData()
    {
        base.InitData();
        if (btSubMoneyL != null)
            btSubMoneyL.onClick.AddListener(SubMoneyForL);
        if (btAddMoneyL != null)
            btAddMoneyL.onClick.AddListener(AddMoneyForL);

        if (btSubMoneyM != null)
            btSubMoneyM.onClick.AddListener(SubMoneyForM);
        if (btAddMoneyM != null)
            btAddMoneyM.onClick.AddListener(AddMoneyForM);

        if (btSubMoneyS != null)
            btSubMoneyS.onClick.AddListener(SubMoneyForS);
        if (btAddMoneyS != null)
            btAddMoneyS.onClick.AddListener(AddMoneyForS);
        RefreshUI();
    }

    public void SubMoneyForL()
    {
        ChangeMoney(1, -cvMoneyL);
    }
    public void AddMoneyForL()
    {
        ChangeMoney(1, cvMoneyL);
    }
    public void SubMoneyForM()
    {
        ChangeMoney(2, -cvMoneyM);
    }
    public void AddMoneyForM()
    {
        ChangeMoney(2, cvMoneyM);
    }
    public void SubMoneyForS()
    {
        ChangeMoney(3, -cvMoneyS);
    }
    public void AddMoneyForS()
    {
        ChangeMoney(3, cvMoneyS);
    }

    public void RefreshUI()
    {
        SetMoney(moneyL, moneyM, moneyS);
    }

    public override void SubmitOnClick()
    {
        if (moneyL == 0 && moneyM == 0 && moneyS == 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1033));
            return;
        }
        base.SubmitOnClick();
    }

    public void GetPickMoney(out long moneyL, out long moneyM, out long moneyS)
    {
        moneyL = this.moneyL;
        moneyM = this.moneyM;
        moneyS = this.moneyS;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cvMoneyL"></param>
    /// <param name="cvMoneyM"></param>
    /// <param name="cvMoneyS"></param>
    public void SetData(int cvMoneyL, int cvMoneyM, int cvMoneyS)
    {
        this.cvMoneyL = cvMoneyL;
        this.cvMoneyM = cvMoneyM;
        this.cvMoneyS = cvMoneyS;
    }

    /// <summary>
    /// 设置最大金钱修改数
    /// </summary>
    /// <param name="maxMoneyL"></param>
    /// <param name="maxMoneyM"></param>
    /// <param name="maxMoneyS"></param>
    public void SetMaxMoney(long maxMoneyL, long maxMoneyM, long maxMoneyS)
    {
        this.maxMoneyL = maxMoneyL;
        this.maxMoneyM = maxMoneyM;
        this.maxMoneyS = maxMoneyS;
    }

    /// <summary>
    /// 修改金钱
    /// </summary>
    /// <param name="type"></param>
    /// <param name="changeValue"></param>
    public void ChangeMoney(int type, int changeValue)
    {
        switch (type)
        {
            case 1:
                moneyL += changeValue;
                if (moneyL < 0)
                    moneyL = 0;
                //上限设置
                if (maxMoneyL != 0 && moneyL > maxMoneyL)
                    moneyL = maxMoneyL;
                break;
            case 2:
                moneyM += changeValue;
                if (moneyM < 0)
                    moneyM = 0;
                //上限设置
                if (maxMoneyM != 0 && moneyM > maxMoneyM)
                    moneyM = maxMoneyM;
                break;
            case 3:
                moneyS += changeValue;
                if (moneyS < 0)
                    moneyS = 0;
                //上限设置
                if (maxMoneyS != 0 && moneyS > maxMoneyS)
                    moneyS= maxMoneyS;
                break;
        }
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        RefreshUI();
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (tvMoneyL != null)
        {
            tvMoneyL.text = moneyL + "";
        }
        if (tvMoneyM != null)
        {
            tvMoneyM.text = moneyM + "";
        }
        if (tvMoneyS != null)
        {
            tvMoneyS.text = moneyS + "";
        }
    }
}
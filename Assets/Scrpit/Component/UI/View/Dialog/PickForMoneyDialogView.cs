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

    public int cvMoneyL = 1;
    public int cvMoneyM = 1;
    public int cvMoneyS = 1;

    public long moneyL = 0;
    public long moneyM = 0;
    public long moneyS = 0;

    protected GameDataManager gameDataManager;
    protected ToastManager toastManager;

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
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
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1041));
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(moneyL, moneyM, moneyS))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(moneyL, moneyM, moneyS);
        base.SubmitOnClick();
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
                break;
            case 2:
                moneyM += changeValue;
                if (moneyM < 0)
                    moneyM = 0;
                break;
            case 3:
                moneyS += changeValue;
                if (moneyS < 0)
                    moneyS = 0;
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
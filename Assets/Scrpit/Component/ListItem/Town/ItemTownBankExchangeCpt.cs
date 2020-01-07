using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownBankExchangeCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public enum ExchangeMoneyEnum
    {
        SToM,//文转银
        MToS,//银转文
        MToL,//银转金
        LToM,//金转银
    }

    public Button btSubmit;
    public Image ivOldMoney;
    public Text tvOldMoney;
    public Image ivNewMoney;
    public InputField etNewMoney;
    public Image ivRate;
    public Text tvRate;
    public Text tvLimit;

    public Sprite spRateUp;
    public Sprite spRateDown;
    public Sprite spRateUnbiased;

    //交换汇率
    public float exchangeRate = 1;
    public ExchangeMoneyEnum exchangeType = ExchangeMoneyEnum.SToM;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(ExchangeSubmit);
        if (etNewMoney != null)
            etNewMoney.onValueChanged.AddListener(ExchangeMoney);
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        if (tvLimit != null && exchangeType == ExchangeMoneyEnum.MToL)
        {
            tvLimit.gameObject.SetActive(true);
            tvLimit.text = GameCommonInfo.GetUITextById(19) + ":" + GameCommonInfo.DailyLimitData.exchangeMoneyL;
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="exchangeType"></param>
    /// <param name="exchangeRate"></param>
    public void SetData(ExchangeMoneyEnum exchangeType, int exchangeOld, int exchangeNew)
    {
        SetExchangeType(exchangeType);
        SetRate(exchangeOld, exchangeNew);
        RefreshUI();
    }

    /// <summary>
    /// 设置交换类型
    /// </summary>
    /// <param name="exchangeType"></param>
    public void SetExchangeType(ExchangeMoneyEnum exchangeType)
    {
        this.exchangeType = exchangeType;
    }

    /// <summary>
    /// 设置交换汇率
    /// </summary>
    /// <param name="exchangeRate"></param>
    public void SetRate(int exchangeOld, int exchangeNew)
    {
        this.exchangeRate = (float)exchangeOld / (float)exchangeNew;

        Sprite spRate;
        Color colorRate = Color.gray;
        tvRate.text = exchangeOld + ":" + exchangeNew;
        switch (exchangeType)
        {
            //文换银 正常比例是1000:1
            case ExchangeMoneyEnum.SToM:

                if (exchangeRate > 1000)
                {
                    spRate = spRateUp;
                    colorRate = Color.red;
                }
                else if (exchangeRate < 1000)
                {
                    spRate = spRateDown;
                    colorRate = Color.green;
                }
                else
                {
                    spRate = spRateUnbiased;
                    colorRate = Color.gray;
                }
                break;
            //银换文 正常比例是1:1000
            case ExchangeMoneyEnum.MToS:
                if (exchangeRate > 0.001f)
                {
                    spRate = spRateUp;
                    colorRate = Color.red;
                }
                else if (exchangeRate < 0.001f)
                {
                    spRate = spRateDown;
                    colorRate = Color.green;
                }
                else
                {
                    spRate = spRateUnbiased;
                    colorRate = Color.gray;
                }
                break;
            //银换金 正常比例1:10
            case ExchangeMoneyEnum.MToL:
                if (exchangeRate > 10)
                {
                    spRate = spRateUp;
                    colorRate = Color.red;
                }
                else if (exchangeRate < 10)
                {
                    spRate = spRateDown;
                    colorRate = Color.green;
                }
                else
                {
                    spRate = spRateUnbiased;
                    colorRate = Color.gray;
                }
                break;
            //金换银 正常比例10:1
            case ExchangeMoneyEnum.LToM:
                if (exchangeRate > 0.1f)
                {
                    spRate = spRateUp;
                    colorRate = Color.red;
                }
                else if (exchangeRate < 0.1f)
                {
                    spRate = spRateDown;
                    colorRate = Color.green;
                }
                else
                {
                    spRate = spRateUnbiased;
                    colorRate = Color.gray;
                }
                break;
            default:
                spRate = spRateUnbiased;
                colorRate = Color.gray;
                break;
        }
        ivRate.sprite = spRate;
        tvRate.color = colorRate;
    }

    /// <summary>
    /// 金钱改变时
    /// </summary>
    /// <param name="money"></param>
    public void ExchangeMoney(string value)
    {
        int valueInt = 0;
        if (value.Contains("-"))
        {
            etNewMoney.text = "";
        }
        else
        {
            if (int.TryParse(value, out int outInt))
            {
                valueInt = outInt;
            }
        }
        tvOldMoney.text = Mathf.CeilToInt(exchangeRate * valueInt) + "";
    }

    /// <summary>
    /// 提交交易
    /// </summary>
    public void ExchangeSubmit()
    {
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;

        mPayMoneyL = 0;
        mPayMoneyM = 0;
        mPayMoneyS = 0;
        mExchangeMoneyL = 0;
        mExchangeMoneyM = 0;
        mExchangeMoneyS = 0;
        mExchangeMoneyStr = "";

        int.TryParse(etNewMoney.text, out int exchangeMoney);
        int.TryParse(tvOldMoney.text, out int payMoney);

        //兑换金额不对
        if (payMoney == 0 || exchangeMoney == 0)
        {
            toastManager.ToastHint(ivOldMoney.sprite, GameCommonInfo.GetUITextById(1041));
            return;
        }
        //是否有足够的金额兑换
        string moneyOldUnit = "";
        string moneyNewUnit = "";
        switch (exchangeType)
        {
            case ExchangeMoneyEnum.SToM:
                mPayMoneyS = payMoney;
                mExchangeMoneyM = exchangeMoney;
                moneyOldUnit = GameCommonInfo.GetUITextById(18);
                moneyNewUnit = GameCommonInfo.GetUITextById(17);
                break;
            case ExchangeMoneyEnum.MToS:
                mPayMoneyM = payMoney;
                mExchangeMoneyS = exchangeMoney;
                moneyOldUnit = GameCommonInfo.GetUITextById(17);
                moneyNewUnit = GameCommonInfo.GetUITextById(18);
                break;
            case ExchangeMoneyEnum.MToL:
                mPayMoneyM = payMoney;
                mExchangeMoneyL = exchangeMoney;
                moneyOldUnit = GameCommonInfo.GetUITextById(17);
                moneyNewUnit = GameCommonInfo.GetUITextById(16);
                break;
            case ExchangeMoneyEnum.LToM:
                mPayMoneyL = payMoney;
                mExchangeMoneyM = exchangeMoney;
                moneyOldUnit = GameCommonInfo.GetUITextById(16);
                moneyNewUnit = GameCommonInfo.GetUITextById(17);
                break;
        }
        //判断是否有足够的金钱兑换
        if (!gameDataManager.gameData.HasEnoughMoney(mPayMoneyL, mPayMoneyM, mPayMoneyS))
        {
            toastManager.ToastHint(ivOldMoney.sprite, GameCommonInfo.GetUITextById(1042));
            return;
        }
        //判断是否超过限额
        if (mExchangeMoneyL > GameCommonInfo.DailyLimitData.exchangeMoneyL)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1044));
            return;
        }

        DialogBean dialogBean = new DialogBean();
        mExchangeMoneyStr = exchangeMoney + moneyNewUnit + "";
        dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3041), payMoney + moneyOldUnit + "", mExchangeMoneyStr);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    private long mPayMoneyL = 0;
    private long mPayMoneyM = 0;
    private long mPayMoneyS = 0;
    private long mExchangeMoneyL = 0;
    private long mExchangeMoneyM = 0;
    private long mExchangeMoneyS = 0;
    private string mExchangeMoneyStr = "";

    #region 确认对话框回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;

        gameDataManager.gameData.PayMoney(mPayMoneyL, mPayMoneyM, mPayMoneyS);
        gameDataManager.gameData.AddMoney(mExchangeMoneyL, mExchangeMoneyM, mExchangeMoneyS);
        GameCommonInfo.DailyLimitData.exchangeMoneyL -= (int)mExchangeMoneyL;
        //成功提示
        toastManager.ToastHint(ivNewMoney.sprite, string.Format(GameCommonInfo.GetUITextById(1043), mExchangeMoneyStr));
        //刷新UI
        RefreshUI();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}
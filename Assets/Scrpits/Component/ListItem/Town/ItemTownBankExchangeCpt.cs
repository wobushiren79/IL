﻿using UnityEngine;
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
    public double exchangeRate = 1;
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
            tvLimit.text = TextHandler.Instance.manager.GetTextById(19) + ":" + GameCommonInfo.DailyLimitData.exchangeMoneyL;
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
        etNewMoney.text = "";
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
        this.exchangeRate = decimal.ToDouble((decimal)exchangeOld / (decimal)exchangeNew);
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
        tvOldMoney.text = Mathf.CeilToInt((float)(exchangeRate * valueInt)) + "";
    }

    /// <summary>
    /// 提交交易
    /// </summary>
    public void ExchangeSubmit()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

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
            UIHandler.Instance.ToastHint<ToastView>(ivOldMoney.sprite, TextHandler.Instance.manager.GetTextById(1041));
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
                moneyOldUnit = TextHandler.Instance.manager.GetTextById(18);
                moneyNewUnit = TextHandler.Instance.manager.GetTextById(17);
                break;
            case ExchangeMoneyEnum.MToS:
                mPayMoneyM = payMoney;
                mExchangeMoneyS = exchangeMoney;
                moneyOldUnit = TextHandler.Instance.manager.GetTextById(17);
                moneyNewUnit = TextHandler.Instance.manager.GetTextById(18);
                break;
            case ExchangeMoneyEnum.MToL:
                mPayMoneyM = payMoney;
                mExchangeMoneyL = exchangeMoney;
                moneyOldUnit = TextHandler.Instance.manager.GetTextById(17);
                moneyNewUnit = TextHandler.Instance.manager.GetTextById(16);
                break;
            case ExchangeMoneyEnum.LToM:
                mPayMoneyL = payMoney;
                mExchangeMoneyM = exchangeMoney;
                moneyOldUnit = TextHandler.Instance.manager.GetTextById(16);
                moneyNewUnit = TextHandler.Instance.manager.GetTextById(17);
                break;
        }
        //判断是否有足够的金钱兑换
        if (!gameData.HasEnoughMoney(mPayMoneyL, mPayMoneyM, mPayMoneyS))
        {
            UIHandler.Instance.ToastHint<ToastView>(ivOldMoney.sprite, TextHandler.Instance.manager.GetTextById(1042));
            return;
        }
        //判断是否超过限额
        if (mExchangeMoneyL > GameCommonInfo.DailyLimitData.exchangeMoneyL)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1044));
            return;
        }

        DialogBean dialogData = new DialogBean();
        mExchangeMoneyStr = exchangeMoney + moneyNewUnit + "";
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3041), payMoney + moneyOldUnit + "", mExchangeMoneyStr);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        gameData.PayMoney(mPayMoneyL, mPayMoneyM, mPayMoneyS);
        gameData.AddMoney(mExchangeMoneyL, mExchangeMoneyM, mExchangeMoneyS);
        GameCommonInfo.DailyLimitData.exchangeMoneyL -= (int)mExchangeMoneyL;
        //成功提示
        UIHandler.Instance.ToastHint<ToastView>(ivNewMoney.sprite, string.Format(TextHandler.Instance.manager.GetTextById(1043), mExchangeMoneyStr));
        //刷新UI
        RefreshUI();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}
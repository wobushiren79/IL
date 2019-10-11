using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownBankExchangeCpt : ItemGameBaseCpt
{
    public enum ExchangeMoneyEnum
    {
        SToM,//文转银
        MToS,//银转文
        MToL,//银转金
        LToM,//金转银
    }

    public Button btSubmit;
    public Text tvOldMoney;
    public InputField etNewMoney;
    public Image ivRate;
    public Text tvRate;

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

    public void SetData(ExchangeMoneyEnum exchangeType, float exchangeRate)
    {
        SetExchangeType(exchangeType);
        SetRate(exchangeRate);
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
    public void SetRate(float exchangeRate)
    {
        this.exchangeRate = exchangeRate;
        tvRate.text = "" + exchangeRate;
        Sprite spRate;
        switch (exchangeType)
        {
            //文换银 正常比例是1000:1
            case ExchangeMoneyEnum.SToM:
                if (exchangeRate > 1000)
                    spRate = spRateDown;
                else if (exchangeRate <1000)
                    spRate = spRateUp;
                else
                    spRate = spRateUnbiased;
                break;
            //银换文 正常比例是1:1000
            case ExchangeMoneyEnum.MToS:
                if(exchangeRate>0.0001)
                    spRate = spRateDown;
                else if(exchangeRate < 0.0001)
                    spRate = spRateDown;
                spRate = spRateUnbiased;
                break;
            case ExchangeMoneyEnum.MToL:
                spRate = spRateUnbiased;
                break;
            case ExchangeMoneyEnum.LToM:
                spRate = spRateUnbiased;
                break;
            default:
                spRate = spRateUnbiased;
                break;
        }
        ivRate.sprite = spRate;
    }

    /// <summary>
    /// 金钱改变时
    /// </summary>
    /// <param name="money"></param>
    public void ExchangeMoney(string value)
    {
        if (CheckUtil.StringIsNull(value) || value.Contains("-"))
        {
            etNewMoney.text = "0";
        }
        else
        {
            tvOldMoney.text = (exchangeRate * int.Parse(value)) + "";
        }
    }

    /// <summary>
    /// 提交交易
    /// </summary>
    public void ExchangeSubmit()
    {

    }
}
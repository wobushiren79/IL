using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsAccountingInfo : BaseMonoBehaviour
{
    [Header("控件")]
    public Text tvAccountingTotalNumber;
    public Text tvTotalMoneyL;
    public Text tvTotalMoneyM;
    public Text tvTotalMoneyS;

    public Text tvAccountingSuccessNumber;
    public Text tvMoreMoneyL;
    public Text tvMoreMoneyM;
    public Text tvMoreMoneyS;

    public Text tvAccountingFailNumber;
    public Text tvLoseMoneyL;
    public Text tvLoseMoneyM;
    public Text tvLoseMoneyS;

    [Header("数据")]
    public CharacterWorkerForAccountingBean accountingData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="accountingData"></param>
    public void SetData(CharacterWorkerForAccountingBean accountingData)
    {
        this.accountingData = accountingData;
        SetTotalData(accountingData.accountingTotalNumber,
            accountingData.accountingMoneyL, accountingData.accountingMoneyM, accountingData.accountingMoneyS);
        SetSuccessData(accountingData.accountingSuccessNumber,
          accountingData.moreMoneyL, accountingData.moreMoneyM, accountingData.moreMoneyS);
        SetFailData(accountingData.accountingErrorNumber,
          accountingData.loseMoneyL, accountingData.lostMoneyM, accountingData.lostMoneyS);
    }

    /// <summary>
    /// 设置总共数据
    /// </summary>
    /// <param name="totalNumber"></param>
    /// <param name="totalMoneL"></param>
    /// <param name="totalMoneM"></param>
    /// <param name="totalMoneS"></param>
    public void SetTotalData(long totalNumber,long totalMoneL, long totalMoneM, long totalMoneS)
    {
        if (tvAccountingTotalNumber != null)
            tvAccountingTotalNumber.text = totalNumber + "";
        if (tvTotalMoneyL != null)
            tvTotalMoneyL.text = totalMoneL + "";
        if (tvTotalMoneyM != null)
            tvTotalMoneyM.text = totalMoneM + "";
        if (tvTotalMoneyS != null)
            tvTotalMoneyS.text = totalMoneS + "";
    }

    /// <summary>
    /// 设置成功数据
    /// </summary>
    /// <param name="successNumber"></param>
    /// <param name="moreMoneL"></param>
    /// <param name="moreMoneM"></param>
    /// <param name="moreMoneS"></param>
    public void SetSuccessData(long successNumber, long moreMoneL, long moreMoneM, long moreMoneS)
    {
        if (tvAccountingSuccessNumber != null)
            tvAccountingSuccessNumber.text = successNumber + "";
        if (tvMoreMoneyL != null)
            tvMoreMoneyL.text = moreMoneL + "";
        if (tvMoreMoneyM != null)
            tvMoreMoneyM.text = moreMoneM + "";
        if (tvMoreMoneyS != null)
            tvMoreMoneyS.text = moreMoneS + "";
    }

    /// <summary>
    /// 设置成功数据
    /// </summary>
    /// <param name="successNumber"></param>
    /// <param name="moreMoneL"></param>
    /// <param name="moreMoneM"></param>
    /// <param name="moreMoneS"></param>
    public void SetFailData(long failNumber, long loseMoneL, long loseMoneM, long loseMoneS)
    {
        if (tvAccountingFailNumber != null)
            tvAccountingFailNumber.text = failNumber + "";
        if (tvLoseMoneyL != null)
            tvLoseMoneyL.text = loseMoneL + "";
        if (tvLoseMoneyM != null)
            tvLoseMoneyM.text = loseMoneM + "";
        if (tvLoseMoneyS != null)
            tvLoseMoneyS.text = loseMoneS + "";
    }
}
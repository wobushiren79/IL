using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsAccountantInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{
    [Header("数据")]
    public CharacterWorkerForAccountantBean accountingData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="accountingData"></param>
    public void SetData(CharacterWorkerForAccountantBean accountingData)
    {
        this.accountingData = accountingData;
        CptUtil.RemoveChildsByActive(objItemContent);

        AddTotalData(accountingData.accountingTotalNumber,
            accountingData.accountingMoneyL, accountingData.accountingMoneyM, accountingData.accountingMoneyS);
        AddSuccessData(accountingData.accountingSuccessNumber,
          accountingData.moreMoneyL, accountingData.moreMoneyM, accountingData.moreMoneyS);
        AddFailData(accountingData.accountingErrorNumber,
          accountingData.loseMoneyL, accountingData.lostMoneyM, accountingData.lostMoneyS);
        //AddAccountingTime(accountingData.accountingTotalTime);
    }

    /// <summary>
    /// 设置总共数据
    /// </summary>
    /// <param name="totalNumber"></param>
    /// <param name="totalMoneL"></param>
    /// <param name="totalMoneM"></param>
    /// <param name="totalMoneS"></param>
    public void AddTotalData(long totalNumber, long totalMoneyL, long totalMoneyM, long totalMoneyS)
    {
        Sprite spIcon1 = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon1, TextHandler.Instance.manager.GetTextById(316), totalNumber + "");
        Sprite spIcon2 = GetSpriteByName("money_1");
        CreateMoneyItem(spIcon2, TextHandler.Instance.manager.GetTextById(317), totalMoneyL, totalMoneyM, totalMoneyS);
    }

    /// <summary>
    /// 设置成功数据
    /// </summary>
    /// <param name="successNumber"></param>
    /// <param name="moreMoneL"></param>
    /// <param name="moreMoneM"></param>
    /// <param name="moreMoneS"></param>
    public void AddSuccessData(long successNumber, long moreMoneyL, long moreMoneyM, long moreMoneyS)
    {
        Sprite spIcon1 = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon1, TextHandler.Instance.manager.GetTextById(318), Color.green, successNumber + "");
        Sprite spIcon2 = GetSpriteByName("money_1");
        CreateMoneyItem(spIcon2, TextHandler.Instance.manager.GetTextById(319), Color.green, moreMoneyL, moreMoneyM, moreMoneyS);
    }

    /// <summary>
    /// 设置成功数据
    /// </summary>
    /// <param name="successNumber"></param>
    /// <param name="moreMoneL"></param>
    /// <param name="moreMoneM"></param>
    /// <param name="moreMoneS"></param>
    public void AddFailData(long failNumber, long loseMoneyL, long loseMoneyM, long loseMoneyS)
    {
        Sprite spIcon1 = GetSpriteByName("worker_accounting_pro_0");
        CreateTextItem(spIcon1, TextHandler.Instance.manager.GetTextById(320), Color.red, failNumber + "");
        Sprite spIcon2 = GetSpriteByName("money_1");
        CreateMoneyItem(spIcon2, TextHandler.Instance.manager.GetTextById(321), Color.red, loseMoneyL, loseMoneyM, loseMoneyS);
    }

    /// <summary>
    /// 设置结算时间
    /// </summary>
    /// <param name="time"></param>
    public void AddAccountingTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, TextHandler.Instance.manager.GetTextById(322), time + TextHandler.Instance.manager.GetTextById(38));
    }
}
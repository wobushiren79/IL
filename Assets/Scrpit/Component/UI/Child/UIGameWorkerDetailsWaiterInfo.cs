using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsWaiterInfo : BaseMonoBehaviour
{
    public Text tvSendNumber;
    public Text tvCleanNumber;

    public Text tvCleanTime;

    public CharacterWorkerForWaiterBean waiterData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="waiterData"></param>
    public void SetData(CharacterWorkerForWaiterBean waiterData)
    {
        this.waiterData = waiterData;
        SetSendNumber(waiterData.sendTotalNumber);
        SetCleanNumber(waiterData.cleanTotalNumber);
        SetCleanTime(waiterData.cleanTotalTime);
    }

    /// <summary>
    /// 设置送餐数量
    /// </summary>
    /// <param name="numebr"></param>
    public void SetSendNumber(long numebr)
    {
        if (tvSendNumber != null)
            tvSendNumber.text = numebr + "";
    }

    /// <summary>
    /// 设置清理数量
    /// </summary>
    /// <param name="number"></param>
    public void SetCleanNumber(long number)
    {
        if (tvCleanNumber != null)
            tvCleanNumber.text = number + "";
    }

    /// <summary>
    /// 设置清理时间
    /// </summary>
    /// <param name="time"></param>
    public void SetCleanTime(float time)
    {
        if (tvCleanTime != null)
            tvCleanTime.text = time + GameCommonInfo.GetUITextById(38);
    }
}
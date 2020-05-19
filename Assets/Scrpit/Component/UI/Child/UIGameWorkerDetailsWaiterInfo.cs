using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsWaiterInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{
    public CharacterWorkerForWaiterBean waiterData;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="waiterData"></param>
    public void SetData(CharacterWorkerForWaiterBean waiterData)
    {
        this.waiterData = waiterData;
        CptUtil.RemoveChildsByActive(objItemContent);
        AddSendNumber(waiterData.sendTotalNumber);
        AddCleanNumber(waiterData.cleanTotalNumber);
       //AddCleanTime(waiterData.cleanTotalTime);
    }

    /// <summary>
    /// 设置送餐数量
    /// </summary>
    /// <param name="numebr"></param>
    public void AddSendNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_send_pro_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(313), number + "");
    }

    /// <summary>
    /// 设置清理数量
    /// </summary>
    /// <param name="number"></param>
    public void AddCleanNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_clear_pro_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(314), number + "");
    }

    /// <summary>
    /// 设置清理时间
    /// </summary>
    /// <param name="time"></param>
    public void AddCleanTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(315), time + GameCommonInfo.GetUITextById(38));
    }
}
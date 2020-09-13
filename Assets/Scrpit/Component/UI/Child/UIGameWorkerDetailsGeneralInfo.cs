using UnityEngine;
using UnityEditor;

public class UIGameWorkerDetailsGeneralInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{

    public CharacterBean characterData;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="waiterData"></param>
    public void SetData(CharacterBean characterData)
    {
        this.characterData = characterData;
        CptUtil.RemoveChildsByActive(objItemContent);
        AddWorkDays(characterData.baseInfo.workDay);

        AddDazeNumber(characterData.baseInfo.dazeNumber);
    }

    /// <summary>
    /// 设置送餐数量
    /// </summary>
    /// <param name="numebr"></param>
    public void AddWorkDays(long days)
    {
        Sprite spIcon = GetSpriteByName("time_wait_1_0");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(337), days + "");
    }

    /// <summary>
    /// 设置送餐数量
    /// </summary>
    /// <param name="numebr"></param>
    public void AddDazeNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("daze_1");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(349), number + "");
    }
}
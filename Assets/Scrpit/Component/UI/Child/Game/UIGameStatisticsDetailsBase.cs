using UnityEngine;
using UnityEditor;

public class UIGameStatisticsDetailsBase<T> : BaseUIChildComponent<T>
        where T : BaseUIComponent
{
    public GameObject objItemContent;
    public GameObject objItemTextModel;
    public GameObject ObjItemMoneyModel;

    /// <summary>
    /// 创建文本Item
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    protected void CreateTextItem(Sprite spIcon, Color colorIcon, string name, Color colorName, string content)
    {
        GameObject objItem = Instantiate(objItemContent, objItemTextModel);
        ItemGameStatisticsDataForTextCpt itemCpt = objItem.GetComponent<ItemGameStatisticsDataForTextCpt>();
        itemCpt.SetData(spIcon, colorIcon, name, colorName, content);
    }

    protected void CreateTextItem(Sprite spIcon, Color colorIcon, string name, string content)
    {
        CreateTextItem(spIcon, colorIcon, name, Color.black, content);
    }

    protected void CreateTextItem(Sprite spIcon, string name, string content)
    {
        CreateTextItem(spIcon, Color.white, name, Color.black, content);
    }

    protected void CreateTextItem(Sprite spIcon, string name, Color colorName, string content)
    {
        CreateTextItem(spIcon, Color.white, name, colorName, content);
    }

    /// <summary>
    /// 创建金钱Item
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="spColor"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    protected void CreateMoneyItem(Sprite spIcon, Color colorIcon, string name, Color colorName, long moneyL, long moneyM, long moneyS)
    {
        GameObject objItem = Instantiate(objItemContent, ObjItemMoneyModel);
        ItemGameStatisticsDataForMoneyCpt itemCpt = objItem.GetComponent<ItemGameStatisticsDataForMoneyCpt>();
        itemCpt.SetData(spIcon, colorIcon, name, colorName, moneyL, moneyM, moneyS);
    }
    protected void CreateMoneyItem(Sprite spIcon, string name, long moneyL, long moneyM, long moneyS)
    {
        CreateMoneyItem(spIcon, Color.white, name, Color.black, moneyL, moneyM, moneyS);
    }
    protected void CreateMoneyItem(Sprite spIcon, string name, Color colorName, long moneyL, long moneyM, long moneyS)
    {
        CreateMoneyItem(spIcon, Color.white, name, colorName, moneyL, moneyM, moneyS);
    }
    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected Sprite GetSpriteByName(string name)
    {
        return IconDataHandler.Instance.manager.GetIconSpriteByName(name);
    }
}
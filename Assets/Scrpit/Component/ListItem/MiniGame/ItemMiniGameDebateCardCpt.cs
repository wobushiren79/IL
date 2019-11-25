using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameDebateCardCpt : ItemGameBaseCpt
{
    public enum DebateCardTypeEnun
    {
        Rock,//石头
        Paper,//布
        Scissors//剪刀
    }

    [Header("控件")]
    public Image ivIcon;
    public Text tvName;

    [Header("数据")]
    public Sprite spRock;
    public Sprite spPaper;
    public Sprite spScissors;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cardType"></param>
    public void SetData(DebateCardTypeEnun cardType)
    {
        Sprite spIcon = null;
        string name = "???";
        switch (cardType)
        {
            case DebateCardTypeEnun.Rock:
                spIcon = spRock;
                name = GameCommonInfo.GetUITextById(261);
                break;
            case DebateCardTypeEnun.Paper:
                spIcon = spPaper;
                name = GameCommonInfo.GetUITextById(262);
                break;
            case DebateCardTypeEnun.Scissors:
                spIcon = spScissors;
                name = GameCommonInfo.GetUITextById(263);
                break;
        }
        SetIcon(spIcon);
        SetName(name);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
        {
            ivIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }
}
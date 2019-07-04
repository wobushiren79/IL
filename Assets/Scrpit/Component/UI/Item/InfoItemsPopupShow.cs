using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class InfoItemsPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvType;

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetData(Sprite spIcon, ItemsInfoBean data)
    {
        if (data == null)
            return;
        if (spIcon != null)
            SetIcon(spIcon);
        SetName(data.name);
        SetContent(data.content);
        SetType(data.items_type);
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    public void SetType(int type)
    {
        string typeStr = "类型：";
        switch (type)
        {
            case 1:
            case 2:
            case 3:
                typeStr += "装备";
                break;
            case 11:
                typeStr += "书籍";
                break;
            case 12:
                typeStr += "料理";
                break;
        }
        if (tvType != null)
            tvType.text = typeStr;
    }
}
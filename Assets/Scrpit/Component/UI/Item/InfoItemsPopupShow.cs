using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class InfoItemsPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvType;

    public GameObject objCook;
    public Text tvCook;
    public GameObject objSpeed;
    public Text tvSpeed;
    public GameObject objAccount;
    public Text tvAccount;
    public GameObject objCharm;
    public Text tvCharm;
    public GameObject objForce;
    public Text tvForce;
    public GameObject objLucky;
    public Text tvLucky;
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
        SetAttributes( data);
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

    public void SetAttributes(ItemsInfoBean data)
    {
        SetItemAttributes(objCook, tvCook, data.add_cook, GameCommonInfo.GetUITextById(1));
        SetItemAttributes(objSpeed, tvSpeed, data.add_speed, GameCommonInfo.GetUITextById(2));
        SetItemAttributes(objAccount, tvAccount, data.add_account, GameCommonInfo.GetUITextById(3));
        SetItemAttributes(objCharm, tvCharm, data.add_charm, GameCommonInfo.GetUITextById(4));
        SetItemAttributes(objForce, tvForce, data.add_force, GameCommonInfo.GetUITextById(5));
        SetItemAttributes(objLucky, tvLucky, data.add_lucky, GameCommonInfo.GetUITextById(6));
    }

    private void SetItemAttributes(GameObject objAttributes, Text tvAttributes, int attributes, string attributesStr)
    {
        if (objCook != null && tvAttributes != null)
        {
            if (attributes == 0)
            {
                objAttributes.SetActive(false);
            }
            else
            {
                objAttributes.SetActive(true);
            }
            tvAttributes.text = attributesStr + "+" + attributes;
        }
    }
}
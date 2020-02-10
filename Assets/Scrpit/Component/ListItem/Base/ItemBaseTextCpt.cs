using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemBaseTextCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;

    public void SetData(Sprite spIcon, Color spColor, string name, Color colorName, string content)
    {
        SetIcon(spIcon, spColor);
        SetName(name, colorName);
        SetContent(content);
    }
    public void SetData(Sprite spIcon, Color colorIcon, string name, string content)
    {
        SetData(spIcon, colorIcon, name, Color.black, content);
    }
    public void SetData(Sprite spIcon, string name, string content)
    {
        SetData(spIcon, Color.white, name, Color.black, content);
    }


    public void SetIcon(Sprite spIcon, Color spColor)
    {
        if (ivIcon != null)
        {
            ivIcon.sprite = spIcon;
            ivIcon.color = spColor;
        }
    }

    public void SetName(string name, Color spColor)
    {
        if (tvName != null)
        {
            tvName.text = name;
            tvName.color = spColor;
        }
    }

    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }
}
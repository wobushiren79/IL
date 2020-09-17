using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemBaseTextCpt : ItemGameBaseCpt
{
    public Image ivIcon;
    public Image ivBackground;
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
        SetData(spIcon, colorIcon, name, tvName.color, content);
    }
    public void SetData(Sprite spIcon, string name, string content)
    {
        SetData(spIcon, ivIcon.color, name, tvName.color, content);
    }
    public void SetData(Sprite spIcon, string name, Color colorName, string content)
    {
        SetData(spIcon, ivIcon.color, name, colorName, content);
    }

    public virtual void SetIcon(Sprite spIcon, Color spColor)
    {
        if (ivIcon != null)
        {
            if (spIcon == null)
            {
                ivIcon.gameObject.SetActive(false);
            }
            else
            {
                ivIcon.gameObject.SetActive(true);
                ivIcon.sprite = spIcon;
                ivIcon.color = spColor;
            }
        }
    }

    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
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

    public void SetBackground(Sprite spBackground)
    {
        if (ivBackground != null)
        {
            ivBackground.sprite = spBackground;
        }
    }
}
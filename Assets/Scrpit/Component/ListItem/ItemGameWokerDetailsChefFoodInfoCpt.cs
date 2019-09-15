using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameWokerDetailsChefFoodInfoCpt : BaseMonoBehaviour
{
    public Text tvName;
    public Image ivIcon;
    public Text tvNumber;

    public void SetData(string name, long number, Sprite spIcon)
    {
        SetName(name);
        SetNumber(number);
        SetIcon(spIcon);
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        if (tvNumber != null)
            tvNumber.text = number + "";
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
        {
            if (spIcon == null)
            {
                ivIcon.color = new Color(1, 1, 1, 0);
                return;
            }
            ivIcon.sprite = spIcon;
        }
    }
}
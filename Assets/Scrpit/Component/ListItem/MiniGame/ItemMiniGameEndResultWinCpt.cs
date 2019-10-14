using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameEndResultWinCpt : BaseMonoBehaviour
{
    public Text tvContent;
    public Image ivIcon;

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }
}
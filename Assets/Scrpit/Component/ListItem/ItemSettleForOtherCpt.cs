using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemSettleForOtherCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvConsume;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="number"></param>
    /// <param name="icon"></param>
    /// <param name="name"></param>
    public void SetData(string  number, Sprite icon, string name,Color numerColor)
    {
        if (ivIcon != null)
            ivIcon.sprite = icon;
        if (tvName != null)
            tvName.text = name;
        if (tvConsume != null)
        {
            tvConsume.text = number;
            tvConsume.color = numerColor;
        }
    }
}
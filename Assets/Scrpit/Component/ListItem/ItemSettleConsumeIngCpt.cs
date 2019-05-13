using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemSettleConsumeIngCpt : BaseMonoBehaviour
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
    public void SetData(int number, Sprite icon, string name)
    {
        if (ivIcon != null)
            ivIcon.sprite = icon;
        if (tvName != null)
            tvName.text = name;
        if (tvConsume != null)
            tvConsume.text = ("-" + number);
    }
}
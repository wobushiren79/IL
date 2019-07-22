using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGamePopupAchRewardCpt : BaseMonoBehaviour
{
    public Text tvName;
    public Image ivIcon;

    public void SetData(string name, long number, Sprite spIcon)
    {
        //设置名字
        if (tvName != null)
        {
            tvName.text = name;
            if (number != 0)
            {
                tvName.text += (" x" + number);
            }
        }

        //设置图标
        if (ivIcon != null)
        {
            if (spIcon != null)
                ivIcon.sprite = spIcon;
        }
    }
}
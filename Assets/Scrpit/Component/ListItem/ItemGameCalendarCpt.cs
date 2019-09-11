using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameCalendarCpt : BaseMonoBehaviour
{
    public Image ivBackground;
    public Text tvDay;
    public Text tvDetails;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="dateInfo"></param>
    public void SetData(DateInfoBean dateInfo)
    {
        if (dateInfo == null)
            return;
        SetDay(dateInfo.day);
        SetDetails(dateInfo.content);
    }

    /// <summary>
    /// 设置天数
    /// </summary>
    /// <param name="day"></param>
    public void SetDay(int day)
    {
        if (tvDay != null)
            tvDay.text = day+"";
    }

    /// <summary>
    /// 设置详情说明
    /// </summary>
    /// <param name="content"></param>
    public void SetDetails(string content)
    {
        if (tvDetails != null)
            tvDetails.text = content + "";
    }

    
}


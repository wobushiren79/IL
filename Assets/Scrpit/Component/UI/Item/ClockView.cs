using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ClockView : BaseMonoBehaviour
{
    //时钟的指正
    public Image ivClockHand;
    //时间
    public Text tvClock;
    //日期
    public Text tvMonthAndDay;
    // 当前时间
    public int timeHour;


    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="hour">24小时</param>
    /// <param name="min"></param>
    public void SetTime(int month, int day, int hour, int min)
    {
        timeHour = hour;
        if (hour > 24 || hour < 0)
        {
            return;
        }
        if (ivClockHand == null)
            return;
        float rotationZ = 0;
        if (hour >= 0 && hour < 6)
        {
            rotationZ = -90;
        }
        else
        {
            int startHour = hour - 6;
            rotationZ = 90 - startHour * 10;
        }
        ivClockHand.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        string hourStr = hour < 10 ? "0" + hour : "" + hour;
        string minStr = min < 10 ? "0" + min : "" + min;
        tvClock.text = hourStr + ":" + minStr;

        SetMonthAndDay(month, day);
    }

    public void SetMonthAndDay(int month, int day)
    {
        if (tvMonthAndDay == null)
            return;
        string seasons = "";
        switch (month)
        {
            case 1:
                seasons = GameCommonInfo.GetUITextById(33);
                tvMonthAndDay.color = new Color(0.22f, 0.87f, 0f);
                break;
            case 2:
                seasons = GameCommonInfo.GetUITextById(34);
                tvMonthAndDay.color = new Color(1f, 0.8f, 0f);
                break;
            case 3:
                seasons = GameCommonInfo.GetUITextById(35);
                tvMonthAndDay.color = new Color(1f, 0.32f, 0f);
                break;
            case 4:
                seasons = GameCommonInfo.GetUITextById(36);
                tvMonthAndDay.color = new Color(0f, 0.9f, 1f);
                break;
        }
        tvMonthAndDay.text = seasons + " " + day + GameCommonInfo.GetUITextById(31);
    }
}
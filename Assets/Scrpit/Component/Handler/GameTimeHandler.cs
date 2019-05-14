using UnityEngine;
using UnityEditor;
using System.Collections;
public class GameTimeHandler : BaseMonoBehaviour
{
    public enum DayEnum
    {
        Rest,
        Work,
    }
    public float hours;
    public float min;
    public DayEnum dayStauts = DayEnum.Rest;
    //时钟
    public ClockView clockView;
    //太阳
    public Light sun;

    //是否停止时间
    public bool isStopTime = false;

    private void Start()
    {
        StartNewDay();
    }

    private void Update()
    {
        if (!isStopTime)
        {
            TimeLapse();
        }
    }

    public void TimeLapse()
    {
        min += Time.deltaTime;
        if (min >= 60)
        {
            min = 0;
            hours += 1;
        }
        if (hours >= 24)
        {
            isStopTime = true;
        }
        clockView.SetTime((int)hours, (int)min);
        float sunColor = 1;
        if (hours < 12)
        {
            sunColor = hours / 12f + (min/720f);
        }
        else if (hours >= 12 && hours <= 15)
        {
            sunColor = 1;
        }
        else
        {
            sunColor = 1 - (hours - 15f) / 12f - (min / 720f);
        }

        sun.color = new Color(sunColor, sunColor, sunColor, 1);
    }

    public void StartNewDay()
    {
        hours = 6;
        min = 0;
        sun.color = new Color(1, 1, 1, 1);
    }
}
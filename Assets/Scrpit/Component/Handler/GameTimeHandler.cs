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
    public DayEnum dayStauts= DayEnum.Rest;
    public ClockView clockView;

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
    }

    public void StartNewDay()
    {
        hours =6;
        min = 0;
    }
}
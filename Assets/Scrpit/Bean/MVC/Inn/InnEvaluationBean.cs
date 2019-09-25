using UnityEngine;
using UnityEditor;

public class InnEvaluationBean
{
    public float mood = 100;//1-100 ; 0以下为愤怒 1-20失望 20-40不高兴 40-60 普通 60-80 满意  80-100非常满意

    /// <summary>
    /// 根据心情不同给出不同的评价
    /// </summary>
    /// <returns></returns>
    public float GetPraise()
    {
        float praise = 0;
        if (mood >= 80)
            praise = 0.1f;
        else if (mood >= 60 && mood < 80)
            praise = 0.05f;
        else if (mood > 20 && mood <= 40)
            praise = -0.05f;
        else if (mood > 0 && mood <= 20)
            praise = -0.1f;
        else if (mood <= 0)
            praise = -0.2f;
        return praise;
    }
}
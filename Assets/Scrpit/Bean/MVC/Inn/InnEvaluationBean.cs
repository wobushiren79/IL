using UnityEngine;
using UnityEditor;

public class InnEvaluationBean
{
    public float mood = 100;//1-100 ; 0以下为愤怒 1-20失望 20-40不高兴 40-60 普通 60-80 满意  80-100非常满意

    /// <summary>
    /// 根据心情不同给出不同的评价
    /// </summary>
    /// <returns></returns>
    public PraiseTypeEnum GetPraise()
    {
        PraiseTypeEnum praise = PraiseTypeEnum.Excited;
        if (mood > 80)
            praise = PraiseTypeEnum.Excited;
        else if (mood > 60 && mood <= 80)
            praise = PraiseTypeEnum.Happy;
        else if (mood > 40 && mood <= 60)
            praise = PraiseTypeEnum.Okay;
        else if (mood > 20 && mood <= 40)
            praise = PraiseTypeEnum.Ordinary;
        else if (mood > 0 && mood <= 20)
            praise = PraiseTypeEnum.Disappointed;
        else if (mood <= 0)
            praise = PraiseTypeEnum.Anger;
        return praise;
    }
}
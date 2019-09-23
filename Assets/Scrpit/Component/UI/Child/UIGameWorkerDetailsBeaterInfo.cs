using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameWorkerDetailsBeaterInfo : BaseMonoBehaviour
{
    public Text tvFightNumber;
    public Text tvFightWinNumber;
    public Text tvFightLoseNumber;

    public Text tvFightTime;
    public Text tvRestTime;


    [Header("数据")]
    public CharacterWorkerForBeaterBean beaterInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="beaterInfo"></param>
    public void SetData(CharacterWorkerForBeaterBean beaterInfo)
    {
        this.beaterInfo = beaterInfo;
        SetFightNumber(beaterInfo.fightTotalNumber);
        SetFightWinNumber(beaterInfo.fightWinNumber);
        SetFightLoseNumber(beaterInfo.fightLoseNumber);
        SetFightTime(beaterInfo.fightTotalTime);
        SetRestTime(beaterInfo.restTotalTime);
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void SetFightNumber(long number)
    {
        if (tvFightNumber != null)
        {
            tvFightNumber.text = number + "";
        }
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void SetFightWinNumber(long number)
    {
        if (tvFightWinNumber != null)
        {
            tvFightWinNumber.text = number + "";
        }
    }

    /// <summary>
    /// 设置打架次数
    /// </summary>
    /// <param name="number"></param>
    public void SetFightLoseNumber(long number)
    {
        if (tvFightLoseNumber != null)
        {
            tvFightLoseNumber.text = number + "";
        }
    }

    /// <summary>
    /// 设置打架时间
    /// </summary>
    /// <param name="time"></param>
    public void SetFightTime(float time)
    {
        if (tvFightTime != null)
            tvFightTime.text = time + GameCommonInfo.GetUITextById(38);
    }

    /// <summary>
    /// 设置休息时间
    /// </summary>
    /// <param name="time"></param>
    public void SetRestTime(float time)
    {
        if (tvRestTime != null)
            tvRestTime.text = time + GameCommonInfo.GetUITextById(38);
    }
}
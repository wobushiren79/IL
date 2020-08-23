using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemGameCalendarCpt : BaseMonoBehaviour
{
    public Image ivBackground;
    public Text tvDay;
    public Text tvRemark;
    public Text tvDetails;
    public Text tvWeather;

    public DateInfoBean dateInfo;

    public WeatherTypeEnum weather= WeatherTypeEnum.Null;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="dateInfo"></param>
    public void SetData(DateInfoBean dateInfo)
    {
        if (dateInfo == null)
            return;
        this.dateInfo = dateInfo;
        SetDay(dateInfo.day);
        SetDetails(dateInfo.content);
    }

    /// <summary>
    /// 设置天气
    /// </summary>
    /// <param name="weather"></param>
    public void SetWeather(WeatherTypeEnum weather)
    {
        this.weather = weather;
        if (tvWeather != null)
            tvWeather.text = WeatherTypeEnumTools.GetWeahterName(weather);
    }

    /// <summary>
    /// 设置备用
    /// </summary>
    /// <param name="remark"></param>
    public void SetRemark(string remark)
    {
        if (tvRemark == null)
            return;
        tvRemark.gameObject.SetActive(true);
        tvRemark.text = remark;
    }


    public void SetItemStatus(bool isCurrentDay)
    {
        if (isCurrentDay)
        {
            ivBackground.DOKill();
            ivBackground.color = Color.white;
            ivBackground.DOColor(new Color(1f, 0.5f, 0.5f),1);
            tvDay.DOKill();
            tvDay.transform.localScale=new Vector3(1,1,1);
            tvDay.transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1).From().SetEase(Ease.OutBack);
        }
        else
        {
            ivBackground.color = Color.white;
        }
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


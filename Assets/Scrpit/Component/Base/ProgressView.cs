using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ProgressView : BaseMonoBehaviour
{

    public enum ProgressType
    {
        Percentage,//百分比
        Degree,//进度
    }

    public ProgressType progressType;
    public Text tvContent;
    public Slider sliderPro;

    public void SetData(float maxData, float data)
    {
        float pro = data / maxData;
        switch (progressType)
        {
            case ProgressType.Percentage:
                SetContent((Math.Round(pro, 4) * 100) + "%");
                break;
            case ProgressType.Degree:
                SetContent(data + "/" + maxData);
                break;
        }
        SetSlider(pro);
    }

    /// <summary>
    /// 设置文字显示
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="pro"></param>
    public void SetSlider(float pro)
    {
        if (sliderPro != null)
            sliderPro.value = pro;
    }
}
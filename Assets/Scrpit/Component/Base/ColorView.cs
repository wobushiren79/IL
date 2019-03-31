using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ColorView : BaseMonoBehaviour
{
    private CallBack callBack;
    public Slider colorR;
    public Slider colorG;
    public Slider colorB;

    private void Start()
    {
        if (colorR != null)
            colorR.onValueChanged.AddListener(ChangeValue);
        if (colorG != null)
            colorG.onValueChanged.AddListener(ChangeValue);
        if (colorB != null)
            colorB.onValueChanged.AddListener(ChangeValue);
    }

    public void ChangeValue(float value)
    {
        if (this.callBack != null)
            this.callBack.ColorChange(this,colorR.value, colorG.value, colorB.value);
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(CallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 获取颜色
    /// </summary>
    /// <returns></returns>
    public ColorBean GetColorBean()
    {
        if (colorR == null || colorG == null || colorB == null)
            return null;
        ColorBean colorBean = new ColorBean();
        colorBean.r = colorR.value;
        colorBean.g = colorG.value;
        colorBean.b = colorB.value;
        colorBean.a = 1;
        return colorBean;
    }
    public Color GetColor()
    {
        if (colorR == null || colorG == null || colorB == null)
            return new Color();
        Color color = new Color(colorR.value, colorG.value, colorB.value);
        return color;
    }

    /// <summary>
    /// 回调
    /// </summary>
    public interface CallBack
    {
        void ColorChange(ColorView colorView,float r, float g, float b);
    }
}
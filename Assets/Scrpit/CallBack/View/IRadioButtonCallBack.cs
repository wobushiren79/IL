using UnityEngine;
using UnityEditor;

public interface IRadioButtonCallBack
{
    /// <summary>
    /// 单选按钮点击
    /// </summary>
    /// <param name="position"></param>
    /// <param name="view"></param>
    void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates);
}
using UnityEngine;
using UnityEditor;

public interface IRadioGroupCallBack 
{
    /// <summary>
    /// 按钮选择
    /// </summary>
    /// <param name="position"></param>
    /// <param name="view"></param>
    void RadioButtonSelected(int position,RadioButtonView view);

    /// <summary>
    /// 按钮未选择
    /// </summary>
    /// <param name="position"></param>
    /// <param name="view"></param>
    void RadioButtonUnSelected(int position, RadioButtonView view);
}
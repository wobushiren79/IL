using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class InputTextDialogView : DialogView
{
    public InputField etContent;

    public void SetData(string text)
    {
        SetText(text);
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        if(etContent)
            etContent.text = text;
    }

    /// <summary>
    /// 获取文本
    /// </summary>
    /// <returns></returns>
    public string GetText()
    {
        if (etContent)
        {
            return etContent.text;
        }
        return null;
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class InputTextDialogView : DialogView
{
    public InputField etContent;
    public Text tvHint;


    public override void InitData()
    {
        base.InitData();
        if (!dialogData.content.IsNull())
        {
            SetText(dialogData.content);
        }
        if (!dialogData.title.IsNull())
        {
            SetHint(dialogData.title);
        }
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        if (etContent)
            etContent.text = text;
    }

    public void SetHint(string hint)
    {
        if (tvHint)
            tvHint.text = hint;
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

    public override void SubmitOnClick()
    {
        if (etContent.text.IsNull())
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1025));
            return;
        }
        base.SubmitOnClick();

    }
}
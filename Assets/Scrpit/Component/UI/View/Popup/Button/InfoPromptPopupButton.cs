using UnityEngine;
using UnityEditor;

public class InfoPromptPopupButton : PopupButtonView
{
    /// <summary>
    /// 内容
    /// </summary>
    public string content;

    public void SetContent(string content)
    {
        this.content = content;
    }

    public override void OpenPopup()
    {
        if (popupShow != null)
            ((InfoPromptPopupShow)popupShow).SetContent(content);
        else
            LogUtil.Log("popupShow is null");
    }

    public override void ClosePopup()
    {

    }

}
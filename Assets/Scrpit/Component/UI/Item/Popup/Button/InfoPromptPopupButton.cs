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
        ((InfoPromptPopupShow)popupShow).SetContent(content);
    }

    public override void ClosePopup()
    {

    }

}
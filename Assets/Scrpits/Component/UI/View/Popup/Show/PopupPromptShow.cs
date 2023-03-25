using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PopupPromptShow : PopupShowView
{
    public Text tvContent;

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

}
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIGameTextForBook : BaseUIChildComponent<UIGameText>
{
    public Text tvBookName;
    public Text tvBookContent;
    public Button btBookBack;

    private void Awake()
    {
        if (btBookBack != null)
            btBookBack.onClick.AddListener(OnClickBack);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="bookName"></param>
    /// <param name="bookContent"></param>
    public void SetData(string bookName, string bookContent)
    {
        SetBookName(bookName);
        SetBookContent(bookContent);
    }

    /// <summary>
    /// 设置书名
    /// </summary>
    /// <param name="bookName"></param>
    public void SetBookName(string bookName)
    {

        if (tvBookName != null)
            tvBookName.text = bookName;
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="bookContent"></param>
    public void SetBookContent(string bookContent)
    {
        if (tvBookContent != null)
            tvBookContent.text = bookContent;
    }

    public void OnClickBack()
    {
        uiComponent.NextText();
       // uiComponent.uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}
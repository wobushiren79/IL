using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ItemGameTextSelectCpt : BaseMonoBehaviour
{
    [Header("控件")]
    public Text tvContent;
    public Button btSubmit;
    private UIGameText mUIGameText;

    [Header("数据")]
    public TextInfoBean textData;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(Submit);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemData"></param>
    public void SetData(TextInfoBean itemData,UIGameText uiGameText)
    {
        this.textData = itemData;
        this.mUIGameText = uiGameText;
        SetText(itemData.content);
    }

    /// <summary>
    ///  设置文本数据
    /// </summary>
    /// <param name="content"></param>
    public void SetText(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    public void Submit()
    {
        int textOrderNext = textData.select_result;
        mUIGameText.NextText(textOrderNext);
    }
}
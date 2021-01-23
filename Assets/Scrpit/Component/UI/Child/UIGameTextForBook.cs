using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class UIGameTextForBook : BaseUIChildComponent<UIGameText>
{
    public Text tvBookName;
    public Text tvBookContent;
    public Button btBookBack;

    public override void Awake()
    {
        base.Awake();
        if (btBookBack != null)
            btBookBack.onClick.AddListener(OnClickBack);
    }
    public override void Open()
    {
        base.Open();
        transform.DOKill();
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScaleX(0, 0.2f).From();
        GameTimeHandler.Instance.SetTimeStop();
    }

    private void OnDisable()
    {
        GameTimeHandler.Instance.SetTimeRestore();
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
        // uiComponent.UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }
}
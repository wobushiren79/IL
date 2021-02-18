using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class UITextController : BaseMVCController<UITextModel, IUITextView>
{
    private Dictionary<long, UITextBean> dicText = new Dictionary<long, UITextBean>();

    public UITextController(BaseMonoBehaviour content, IUITextView view) : base(content, view)
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void InitData()
    {

    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public void GetAllData()
    {
        dicText = new Dictionary<long, UITextBean>();
        List<UITextBean> listData = GetModel().GetAllData();
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            UITextBean itemData = listData[i];
            dicText.Add(itemData.id, itemData);
        }
    }

    /// <summary>
    /// 根据ID获取文字内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTextById(long id)
    {
        if (dicText == null)
            return null;
        if (dicText.TryGetValue(id, out UITextBean value))
        {
            return value.content;
        }
        else
        {
            LogUtil.LogError("没有找到ID为" + id + "的UI内容");
            return null;
        }
    }

}
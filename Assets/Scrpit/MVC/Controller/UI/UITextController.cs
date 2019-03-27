using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class UITextController : BaseMVCController<UITextModel,IUITextView>
{
    private Dictionary<long, UITextBean> mMapData;

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
    public void RefreshData()
    {
        mMapData = new Dictionary<long, UITextBean>();
        List<UITextBean> listData = GetModel().GetAllData();
        if (listData == null)
            return;
        foreach (UITextBean itemData in listData)
        {
            mMapData.Add(itemData.id, itemData);
        }
    }

    /// <summary>
    /// 根据ID获取文字内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTextById(long id)
    {
        if (mMapData == null)
            return null;
        UITextBean itemData=null;
        try
        {
             itemData = mMapData[id];
        }
        catch (Exception e)
        {
            LogUtil.LogError("没有找到ID为"+id+"的UI内容");
        }
        if (itemData == null)
            return null;
        return itemData.content;
    }
}
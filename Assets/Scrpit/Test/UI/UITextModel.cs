using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITextModel : BaseMVCModel
{
    private UITextService mUITextService;

    public override void InitData()
    {
        mUITextService = new UITextService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<UITextBean> GetAllData()
    {
        List<UITextBean> listData = mUITextService.QueryAllData();
        return listData;
    }

}
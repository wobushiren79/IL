using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoController : BaseMVCController<TextInfoModel,ITextInfoView>
{
    public TextInfoController(BaseMonoBehaviour content, ITextInfoView view) : base(content, view)
    {
    }

    public override void InitData()
    {
        
    }

    public void GetTextForLook(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForLook(markId);
        if(listData != null)
            GetView().GetTextInfoSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForTalk(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalk(markId);
        if (listData != null)
            GetView().GetTextInfoSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForStory(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForStory(markId);
        if (listData != null)
            GetView().GetTextInfoSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }
}
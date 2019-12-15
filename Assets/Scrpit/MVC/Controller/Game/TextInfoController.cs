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
            GetView().GetTextInfoForLookSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForTalkByUserId(long userId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByUserId(userId);
        if (listData != null)
            GetView().GetTextInfoForTalkByUserIdSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForTalkByMarkId(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByMarkId(markId);
        if (listData != null)
            GetView().GetTextInfoForTalkByMarkIdSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForStory(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForStory(markId);
        if (listData != null)
            GetView().GetTextInfoForStorySuccess(listData);
        else
            GetView().GetTextInfoFail();
    }
}
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

    public void GetTextForTalk(long userId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalk(userId);
        if (listData != null)
            GetView().GetTextInfoForTalkSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForTalkByFavorability(long characterId,int favorability)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByFavorability(characterId, favorability);
        if (listData != null)
            GetView().GetTextInfoForTalkSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    public void GetTextForTalkByFirst(long characterId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByFirst(characterId);
        if (listData != null)
            GetView().GetTextInfoForTalkSuccess(listData);
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
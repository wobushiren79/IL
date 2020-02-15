using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoController : BaseMVCController<TextInfoModel, ITextInfoView>
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
        if (listData != null)
            GetView().GetTextInfoForLookSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 通过用户ID查询对话数据
    /// </summary>
    /// <param name="userId"></param>
    public void GetTextForTalkByUserId(long userId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByUserId(userId);
        if (listData != null)
            GetView().GetTextInfoForTalkByUserIdSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 通过最小好感获取对话
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="minFavorability"></param>
    public void GetTextForTalkByMinFavorability(long userId, int minFavorability)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByMinFavorability(userId, minFavorability);
        if (listData != null)
            GetView().GetTextInfoForTalkByUserIdSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 通过标记ID查询对话数据
    /// </summary>
    /// <param name="markId"></param>
    public void GetTextForTalkByMarkId(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByMarkId(markId);
        if (listData != null)
            GetView().GetTextInfoForTalkByMarkIdSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 通过用户ID查询该用户的第一次对话数据
    /// </summary>
    /// <param name="userId"></param>
    public void GetTextForTalkByFirstMeet(long userId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByFirstMeet(userId);
        if (listData != null)
            GetView().GetTextInfoForTalkByFirstMeetSuccess(listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 根据类型查询对话数据
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="textTalkType"></param>
    public void GetTextForTalkByType(long userId, TextTalkTypeEnum textTalkType)
    {
        List<TextInfoBean> listData = GetModel().GetTextForTalkByType(userId, textTalkType);
        if (listData != null)
            GetView().GetTextInfoForTalkByTypeSuccess(textTalkType, listData);
        else
            GetView().GetTextInfoFail();
    }

    /// <summary>
    /// 获取故事文本
    /// </summary>
    /// <param name="markId"></param>
    public void GetTextForStory(long markId)
    {
        List<TextInfoBean> listData = GetModel().GetTextForStory(markId);
        if (listData != null)
            GetView().GetTextInfoForStorySuccess(listData);
        else
            GetView().GetTextInfoFail();
    }
}
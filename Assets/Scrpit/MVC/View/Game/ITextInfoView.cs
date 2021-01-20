using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface ITextInfoView
{
    void GetTextInfoForLookSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoForTalkByUserIdSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoForTalkByTypeSuccess(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoForTalkByMarkIdSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoForTalkByFirstMeetSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoForStorySuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action);
    void GetTextInfoFail();
}
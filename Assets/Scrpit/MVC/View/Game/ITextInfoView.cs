using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface ITextInfoView
{
    void GetTextInfoForLookSuccess(List<TextInfoBean> listData);
    void GetTextInfoForTalkSuccess(List<TextInfoBean> listData);
    void GetTextInfoForStorySuccess(List<TextInfoBean> listData);
    void GetTextInfoFail();
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoHandler : BaseHandler, TextInfoManager.ICallBack
{
    protected TextInfoManager textInfoManager;
    protected ICallBack callBack;

    public void Awake()
    {
        textInfoManager = Find<TextInfoManager>(ImportantTypeEnum.TextManager);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    public void GetTextInfoFotTalkByMarkId(long markId)
    {
        textInfoManager.SetCallBack(this);
        textInfoManager.GetTextForTalkByMarkId(markId);
    }

    public void SetTextInfoForLook(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForStory(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByFirstMeet(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByMarkId(List<TextInfoBean> listData)
    {
        if (callBack != null)
        {
            callBack.GetTextInfoSuccess(listData);
        }
    }

    public void SetTextInfoForTalkByUserId(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByType(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkOptions(List<TextInfoBean> listData)
    {
    }

    public interface ICallBack
    {
        void GetTextInfoSuccess(List<TextInfoBean> listData);

        void GetTextInfoFail();
    }

}
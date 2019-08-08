using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoModel : BaseMVCModel
{
    private TextInfoService mTextInfoService;

    public override void InitData()
    {
        mTextInfoService = new TextInfoService();
    }

    public List<TextInfoBean> GetTextForLook(long markId)
    {
        return mTextInfoService.QueryDataByMarkId(TextEnum.Look, markId);
    }

    public List<TextInfoBean> GetTextForTalk(long markId)
    {
       return mTextInfoService.QueryDataByMarkId(TextEnum.Talk, markId);
    }

    public List<TextInfoBean> GetTextForStory(long markId)
    {
        return mTextInfoService.QueryDataByMarkId(TextEnum.Story, markId);
    }
}
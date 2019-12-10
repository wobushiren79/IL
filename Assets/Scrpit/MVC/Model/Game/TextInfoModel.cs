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

    public List<TextInfoBean> GetTextForTalk(long userId)
    {
       return mTextInfoService.QueryDataByUserId(TextEnum.Talk, userId);
    }

    public List<TextInfoBean> GetTextForStory(long markId)
    {
        return mTextInfoService.QueryDataByMarkId(TextEnum.Story, markId);
    }

    /// <summary>
    /// 通过好感区间值查询对话
    /// </summary>
    /// <param name="characterId"></param>
    /// <param name="favorability"></param>
    /// <returns></returns>
    public List<TextInfoBean> GetTextForTalkByFavorability(long characterId,int favorability)
    {
        return mTextInfoService.QueryDataForFirstOrderByFavorability(characterId, favorability);
    }

    /// <summary>
    /// 通过是否首次对话查询
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public List<TextInfoBean> GetTextForTalkByFirst(long characterId)
    {
        return mTextInfoService.QueryDataForFirstOrderByFirstMeet(characterId);
    }


}
﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoManager : BaseManager, ITextInfoView
{
    //查询的对话内容
    public List<TextInfoBean> listTextData;

    public Dictionary<long, List<TextInfoBean>> mapTalkNormalData;
    public Dictionary<long, List<TextInfoBean>> mapTalkGiftData;
    public Dictionary<long, List<TextInfoBean>> mapTalkRecruitData;
    public Dictionary<long, List<TextInfoBean>> mapTalkRascalData;
    public Dictionary<long, List<TextInfoBean>> mapTalkExchangeData;

    protected TextInfoController textInfoController;
    protected ICallBack callBack;

    private void Awake()
    {
        textInfoController = new TextInfoController(this, this);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="id">当 textEnum为Look 或 Story时 为markId</param>
    public void GetTextById(TextEnum textEnum, long id)
    {
        switch (textEnum)
        {
            case TextEnum.Look:
                textInfoController.GetTextForLook(id);
                break;
            case TextEnum.Talk:
                textInfoController.GetTextForTalkByMarkId(id);
                break;
            case TextEnum.Story:
                textInfoController.GetTextForStory(id);
                break;
        }
    }

    /// <summary>
    /// 获取小镇居民第一次见面时的对话
    /// </summary>
    public void GetTextForTownFirstMeet(long userId)
    {
        textInfoController.GetTextForTalkByFirstMeet(userId);
    }

    /// <summary>
    /// 获取交流对话选项
    /// </summary>
    public void GetTextForTalkOptions(GameDataBean gameData, NpcInfoBean npcInfo)
    {
        //如果不是第一次对话则有以下选项
        listTextData = new List<TextInfoBean>();

        List<NpcTalkTypeEnum>  npcTalkTypes= npcInfo.GetTalkTypes();
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk,0, GameCommonInfo.GetUITextById(99101)));
        foreach (NpcTalkTypeEnum itemType in npcTalkTypes)
        {
            switch (itemType)
            {
                case NpcTalkTypeEnum.Talk:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99102)));
                    break;
                case NpcTalkTypeEnum.OneTalk:
                    if (!GameCommonInfo.DailyLimitData.CheckIsTalkNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99102)));
                    }
                    break;
                case NpcTalkTypeEnum.Recruit:
                    if (!gameData.CheckHasWorker(npcInfo.id + ""))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99104)));
                    }
                    break;
                case NpcTalkTypeEnum.Gift:
                    if (!GameCommonInfo.DailyLimitData.CheckIsGiftNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99105)));
                    }
                    break;
                case NpcTalkTypeEnum.GuildCoinExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99201)));
                    break;
                case NpcTalkTypeEnum.TrophyExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99202)));
                    break;
                case NpcTalkTypeEnum.InfiniteTowerEquipExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99203)));
                    break;
                case NpcTalkTypeEnum.InfiniteTowerItemsExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99204)));
                    break;
            }
        }
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, GameCommonInfo.GetUITextById(99103)));
        if (callBack != null)
            callBack.SetTextInfoForTalkOptions(listTextData);
        //继续查询该人物的所有对话
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(npcInfo.id);
        GetTextForTalkByMinFavorability(npcInfo.id, characterFavorability.favorabilityLevel);
    }

    /// <summary>
    /// 根据markID获取对话
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="favorabilityLevel"></param>
    public void GetTextForTalkByMarkId(long markId)
    {
        textInfoController.GetTextForTalkByMarkId(markId);
    }

    /// <summary>
    /// 根据好感获取对话
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="favorabilityLevel"></param>
    public void GetTextForTalkByMinFavorability(long userId, int favorabilityLevel)
    {
        textInfoController.GetTextForTalkByMinFavorability(userId, favorabilityLevel);
    }

    /// <summary>
    /// 获取捣乱对话
    /// </summary>
    /// <param name="userId"></param>
    public void GetTextForTalkType(long userId, TextTalkTypeEnum textTalkType)
    {
        textInfoController.GetTextForTalkByType(userId, textTalkType);
    }

    /// <summary>
    /// 通过等级获取赠送对话
    /// </summary>
    /// <param name="level"></param>
    public List<TextInfoBean> GetGiftTalkByFavorability(int favorability)
    {
        List<List<TextInfoBean>> listData = new List<List<TextInfoBean>>();
        foreach (var itemData in mapTalkGiftData)
        {
            if (itemData.Value[0].add_favorability == favorability)
            {
                listData.Add(itemData.Value);
                break;
            }
        }
        if (listData.Count == 0)
        {
            return new List<TextInfoBean>();
        }
        else
        {
            return RandomUtil.GetRandomDataByList(listData);
        }
    }

    /// <summary>
    /// 根据顺序获取文本数据
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<TextInfoBean> GetTextDataByOrder(int order)
    {
        List<TextInfoBean> listData = new List<TextInfoBean>();
        if (listTextData == null)
            return listData;
        foreach (TextInfoBean itemData in listTextData)
        {
            if (itemData.text_order == order)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    #region 数据回调
    public void GetTextInfoFail()
    {
    }

    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        if (callBack != null)
            callBack.SetTextInfoForLook(listData);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        if (callBack != null)
            callBack.SetTextInfoForStory(listData);
    }

    public void GetTextInfoForTalkByFirstMeetSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        if (callBack != null)
            callBack.SetTextInfoForTalkByFirstMeet(listData);
    }

    public void GetTextInfoForTalkByMarkIdSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        if (callBack != null)
            callBack.SetTextInfoForTalkByMarkId(listData);
    }

    public void GetTextInfoForTalkByUserIdSuccess(List<TextInfoBean> listData)
    {
        mapTalkNormalData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkGiftData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkRecruitData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkRascalData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkExchangeData = new Dictionary<long, List<TextInfoBean>>();
        foreach (TextInfoBean itemTalkInfo in listData)
        {
            long markId = itemTalkInfo.mark_id;
            Dictionary<long, List<TextInfoBean>> addMap = new Dictionary<long, List<TextInfoBean>>();
            switch (itemTalkInfo.GetTextTalkType())
            {
                case TextTalkTypeEnum.Normal:
                    addMap = mapTalkNormalData;
                    break;
                case TextTalkTypeEnum.Gift:
                    addMap = mapTalkGiftData;
                    break;
                case TextTalkTypeEnum.Recruit:
                    addMap = mapTalkRecruitData;
                    break;
                case TextTalkTypeEnum.Rascal:
                    addMap = mapTalkRascalData;
                    break;
                case TextTalkTypeEnum.Exchange:
                    addMap = mapTalkExchangeData;
                    break;
            }
            if (addMap.TryGetValue(markId, out List<TextInfoBean> value))
            {
                value.Add(itemTalkInfo);
            }
            else
            {
                List<TextInfoBean> listTemp = new List<TextInfoBean>();
                listTemp.Add(itemTalkInfo);
                addMap.Add(markId, listTemp);
            }
        }
        if (callBack != null)
            callBack.SetTextInfoForTalkByUserId(listData);
    }

    public void GetTextInfoForTalkByTypeSuccess(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData)
    {
        listTextData = listData;
        if (callBack != null)
            callBack.SetTextInfoForTalkByType(textTalkType, listData);
    }

    public interface ICallBack
    {
        void SetTextInfoForLook(List<TextInfoBean> listData);

        void SetTextInfoForStory(List<TextInfoBean> listData);

        void SetTextInfoForTalkByFirstMeet(List<TextInfoBean> listData);
        void SetTextInfoForTalkByMarkId(List<TextInfoBean> listData);
        void SetTextInfoForTalkByUserId(List<TextInfoBean> listData);
        void SetTextInfoForTalkByType(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData);
        void SetTextInfoForTalkOptions(List<TextInfoBean> listData);

    }
    #endregion
}
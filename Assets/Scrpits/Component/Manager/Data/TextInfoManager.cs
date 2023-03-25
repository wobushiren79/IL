using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

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

    private void Awake()
    {
        textInfoController = new TextInfoController(this, this);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="id">当 textEnum为Look 或 Story时 为markId</param>
    public void GetTextById(TextEnum textEnum, long id, Action<List<TextInfoBean>> action)
    {
        switch (textEnum)
        {
            case TextEnum.Look:
                textInfoController.GetTextForLook(id, action);
                break;
            case TextEnum.Talk:
                textInfoController.GetTextForTalkByMarkId(id, action);
                break;
            case TextEnum.Story:
                textInfoController.GetTextForStory(id, action);
                break;
        }
    }

    /// <summary>
    /// 获取小镇居民第一次见面时的对话
    /// </summary>
    public void GetTextForTownFirstMeet(long userId, Action<List<TextInfoBean>> action)
    {
        textInfoController.GetTextForTalkByFirstMeet(userId, action);
    }

    /// <summary>
    /// 获取交流对话选项
    /// </summary>
    public void GetTextForTalkOptions(GameDataBean gameData, NpcInfoBean npcInfo, Action<List<TextInfoBean>> action)
    {
        //如果不是第一次对话则有以下选项
        listTextData = new List<TextInfoBean>();

        List<NpcTalkTypeEnum>  npcTalkTypes= npcInfo.GetTalkTypes();
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk,0, TextHandler.Instance.manager.GetTextById(99101)));
        foreach (NpcTalkTypeEnum itemType in npcTalkTypes)
        {
            switch (itemType)
            {
                case NpcTalkTypeEnum.Talk:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99102)));
                    break;
                case NpcTalkTypeEnum.OneTalk:
                    if (!GameCommonInfo.DailyLimitData.CheckIsTalkNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99102)));
                    }
                    break;
                case NpcTalkTypeEnum.Recruit:
                    if (!gameData.CheckHasWorker(npcInfo.id + ""))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99104)));
                    }
                    break;
                case NpcTalkTypeEnum.Gift:
                    if (!GameCommonInfo.DailyLimitData.CheckIsGiftNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99105)));
                    }
                    break;
                case NpcTalkTypeEnum.GuildCoinExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99201)));
                    break;
                case NpcTalkTypeEnum.TrophyExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99202)));
                    break;
                case NpcTalkTypeEnum.EquipExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99203)));
                    break;
                case NpcTalkTypeEnum.ItemsExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99204)));
                    break;
            }
        }

        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(npcInfo.id);
        FamilyDataBean familyData= gameData.GetFamilyData();
        //如果满足了结婚条件。添加结婚对话
        if (
            //该角色是否可以结婚
            npcInfo.CheckCanMarry() 
            //好感是否达到要求
            && characterFavorability.CheckCanMarry() 
            //是否已经向其他人求婚或者已经结婚
            && (familyData.timeForMarry == null || familyData.timeForMarry.year == 0))
        {
            listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99205)));
        }
        //离开选项
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.manager.GetTextById(99103)));

        //继续查询该人物的所有对话
        GetTextForTalkByMinFavorability(npcInfo.id, characterFavorability.favorabilityLevel, action);
        action?.Invoke(listTextData);
    }

    /// <summary>
    /// 根据markID获取对话
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="favorabilityLevel"></param>
    public void GetTextForTalkByMarkId(long markId,Action<List<TextInfoBean>> action)
    {
        textInfoController.GetTextForTalkByMarkId(markId, action);
    }

    /// <summary>
    /// 根据好感获取对话
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="favorabilityLevel"></param>
    public void GetTextForTalkByMinFavorability(long userId, int favorabilityLevel, Action<List<TextInfoBean>> action)
    {
        textInfoController.GetTextForTalkByMinFavorability(userId, favorabilityLevel, action);
    }

    /// <summary>
    /// 获取捣乱对话
    /// </summary>
    /// <param name="userId"></param>
    public void GetTextForTalkType(long userId, TextTalkTypeEnum textTalkType, Action<List<TextInfoBean>> action)
    {
        textInfoController.GetTextForTalkByType(userId, textTalkType, action);
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

    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        listTextData = listData;
        action?.Invoke(listData);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        listTextData = listData;
        action?.Invoke(listData);
    }

    public void GetTextInfoForTalkByFirstMeetSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        listTextData = listData;
        action?.Invoke(listData);
    }

    public void GetTextInfoForTalkByMarkIdSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        listTextData = listData;
        action?.Invoke(listData);
    }

    public void GetTextInfoForTalkByUserIdSuccess(List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        mapTalkNormalData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkGiftData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkRecruitData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkRascalData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkExchangeData = new Dictionary<long, List<TextInfoBean>>();
        for (int i=0;i< listData.Count;i++)
        {
            TextInfoBean itemTalkInfo = listData[i];
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
        //action?.Invoke(listData);
    }

    public void GetTextInfoForTalkByTypeSuccess(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData, Action<List<TextInfoBean>> action)
    {
        listTextData = listData;
        action?.Invoke(listData);
    }
    #endregion
}
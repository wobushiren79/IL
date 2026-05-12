using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TextInfoManager : BaseManager
{
    //查询的对话内容
    public List<TextInfoBean> listTextData;

    public Dictionary<long, List<TextInfoBean>> mapTalkNormalData;
    public Dictionary<long, List<TextInfoBean>> mapTalkGiftData;
    public Dictionary<long, List<TextInfoBean>> mapTalkRecruitData;
    public Dictionary<long, List<TextInfoBean>> mapTalkRascalData;
    public Dictionary<long, List<TextInfoBean>> mapTalkExchangeData;

    private void Awake()
    {
    }

    private static TextInfoBean ConvertTalkBean(TextTalkBean src)
    {
        if (src == null) return null;
        TextInfoBean bean = new TextInfoBean();
        bean.id = src.id;
        bean.valid = src.valid;
        bean.mark_id = src.mark_id;
        bean.type = src.type;
        bean.text_order = src.text_order;
        bean.next_order = src.next_order;
        bean.talk_type = src.talk_type;
        bean.user_id = src.user_id;
        bean.condition_min_favorability = src.condition_min_favorability;
        bean.condition_max_favorability = src.condition_max_favorability;
        bean.select_type = src.select_type;
        bean.add_favorability = src.add_favorability;
        bean.pre_data_minigame = src.pre_data_minigame;
        bean.reward_data = src.reward_data;
        bean.wait_time = src.wait_time;
        bean.is_stoptime = src.is_stoptime;
        bean.scene_expression = src.scene_expression;
        bean.pre_data = src.pre_data;
        bean.name = src.name_language;
        bean.content = src.content_language;
        return bean;
    }

    private static TextInfoBean ConvertStoryBean(TextStoryBean src)
    {
        if (src == null) return null;
        TextInfoBean bean = new TextInfoBean();
        bean.id = src.id;
        bean.valid = src.valid;
        bean.mark_id = src.mark_id;
        bean.type = src.type;
        bean.text_order = src.text_order;
        bean.next_order = src.next_order;
        bean.talk_type = src.talk_type;
        bean.user_id = src.user_id;
        bean.condition_min_favorability = src.condition_min_favorability;
        bean.condition_max_favorability = src.condition_max_favorability;
        bean.select_type = src.select_type;
        bean.add_favorability = src.add_favorability;
        bean.pre_data_minigame = src.pre_data_minigame;
        bean.reward_data = src.reward_data;
        bean.wait_time = src.wait_time;
        bean.is_stoptime = src.is_stoptime;
        bean.scene_expression = src.scene_expression;
        bean.pre_data = src.pre_data;
        bean.name = src.name_language;
        bean.content = src.content_language;
        return bean;
    }

    private List<TextInfoBean> QueryTalkBeans(Func<TextTalkBean, bool> predicate)
    {
        List<TextInfoBean> result = new List<TextInfoBean>();
        TextTalkBean[] array = TextTalkCfg.GetAllArrayData();
        if (array == null) return result;
        foreach (TextTalkBean item in array)
        {
            if (predicate(item))
                result.Add(ConvertTalkBean(item));
        }
        return result;
    }

    private List<TextInfoBean> QueryStoryBeans(Func<TextStoryBean, bool> predicate)
    {
        List<TextInfoBean> result = new List<TextInfoBean>();
        TextStoryBean[] array = TextStoryCfg.GetAllArrayData();
        if (array == null) return result;
        foreach (TextStoryBean item in array)
        {
            if (predicate(item))
                result.Add(ConvertStoryBean(item));
        }
        return result;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="id">当 textEnum为Look 或 Story时 为markId</param>
    public void GetTextById(TextEnum textEnum, long id, Action<List<TextInfoBean>> action)
    {
        List<TextInfoBean> listData = null;
        switch (textEnum)
        {
            case TextEnum.Look:
                listData = QueryTalkBeans(b => b.mark_id == id);
                break;
            case TextEnum.Talk:
                listData = QueryTalkBeans(b => b.mark_id == id);
                break;
            case TextEnum.Story:
                listData = QueryStoryBeans(b => b.mark_id == id);
                break;
        }
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取小镇居民第一次见面时的对话
    /// </summary>
    public void GetTextForTownFirstMeet(long userId, Action<List<TextInfoBean>> action)
    {
        List<TextInfoBean> listData = QueryTalkBeans(b => b.user_id == userId && b.talk_type == (int)TextTalkTypeEnum.First);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取交流对话选项
    /// </summary>
    public void GetTextForTalkOptions(GameDataBean gameData, NpcInfoBean npcInfo, Action<List<TextInfoBean>> action)
    {
        listTextData = new List<TextInfoBean>();

        List<NpcTalkTypeEnum> npcTalkTypes = npcInfo.GetTalkTypes();
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 0, TextHandler.Instance.GetTextById(99101)));
        foreach (NpcTalkTypeEnum itemType in npcTalkTypes)
        {
            switch (itemType)
            {
                case NpcTalkTypeEnum.Talk:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99102)));
                    break;
                case NpcTalkTypeEnum.OneTalk:
                    if (!GameCommonInfo.DailyLimitData.CheckIsTalkNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99102)));
                    }
                    break;
                case NpcTalkTypeEnum.Recruit:
                    if (!gameData.CheckHasWorker(npcInfo.id + ""))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99104)));
                    }
                    break;
                case NpcTalkTypeEnum.Gift:
                    if (!GameCommonInfo.DailyLimitData.CheckIsGiftNpc(npcInfo.id))
                    {
                        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99105)));
                    }
                    break;
                case NpcTalkTypeEnum.GuildCoinExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99201)));
                    break;
                case NpcTalkTypeEnum.TrophyExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99202)));
                    break;
                case NpcTalkTypeEnum.EquipExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99203)));
                    break;
                case NpcTalkTypeEnum.ItemsExchange:
                    listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99204)));
                    break;
            }
        }

        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(npcInfo.id);
        FamilyDataBean familyData = gameData.GetFamilyData();
        if (
            npcInfo.CheckCanMarry()
            && characterFavorability.CheckCanMarry()
            && (familyData.timeForMarry == null || familyData.timeForMarry.year == 0))
        {
            listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99205)));
        }
        listTextData.Add(new TextInfoBean(TextInfoTypeEnum.Talk, 1, TextHandler.Instance.GetTextById(99103)));

        GetTextForTalkByMinFavorability(npcInfo.id, characterFavorability.favorabilityLevel, action);
        action?.Invoke(listTextData);
    }

    /// <summary>
    /// 根据markID获取对话
    /// </summary>
    public void GetTextForTalkByMarkId(long markId, Action<List<TextInfoBean>> action)
    {
        List<TextInfoBean> listData = QueryTalkBeans(b => b.mark_id == markId);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 根据好感获取对话
    /// </summary>
    public void GetTextForTalkByMinFavorability(long userId, int favorabilityLevel, Action<List<TextInfoBean>> action)
    {
        List<TextInfoBean> listData = QueryTalkBeans(b =>
            b.user_id == userId &&
            b.condition_min_favorability <= favorabilityLevel);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取捣乱对话
    /// </summary>
    public void GetTextForTalkType(long userId, TextTalkTypeEnum textTalkType, Action<List<TextInfoBean>> action)
    {
        List<TextInfoBean> listData = QueryTalkBeans(b =>
            b.user_id == userId &&
            b.talk_type == (int)textTalkType);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 通过等级获取赠送对话
    /// </summary>
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

    public List<TextInfoBean> QueryDataForFirstOrderByFavorability(long characterId, int favorability)
    {
        return QueryTalkBeans(b =>
            b.user_id == characterId &&
            b.text_order == 1 &&
            b.condition_min_favorability <= favorability &&
            b.condition_max_favorability > favorability);
    }

    public List<TextInfoBean> QueryDataForFirstOrderByFirstMeet(long characterId)
    {
        return QueryTalkBeans(b =>
            b.user_id == characterId &&
            b.text_order == 1 &&
            b.talk_type == (int)TextTalkTypeEnum.First);
    }
}

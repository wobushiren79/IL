using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameText : BaseUIComponent, ITextInfoView
{
    [Header("控件")]
    public UIGameTextForBook uiForBook;
    public UIGameTextForBehind uiForBehind;
    public UIGameTextForTalk uiForTalk;

    [Header("数据")]
    public int textOrder = 1;

    public List<TextInfoBean> listTextData;
    public TextInfoBean currentTextData;

    public Dictionary<long, List<TextInfoBean>> mapTalkNormalData;
    public Dictionary<long, List<TextInfoBean>> mapTalkGiftData;
    public Dictionary<long, List<TextInfoBean>> mapTalkRecruitData;

    private TextInfoController mTextInfoController;

    private TextEnum mTextEnum;
    public ICallBack callBack;

    //备用文本替换数据
    public SortedList<string, string> listMarkData = new SortedList<string, string>();

    private void Awake()
    {
        mTextInfoController = new TextInfoController(this, this);
    }

    public void NextText()
    {
        NextText(textOrder + 1);
    }

    public void NextText(int order)
    {
        //如果顺序为0 则在原顺序上+1
        if (order == 0)
            order = this.textOrder + 1;
        if (currentTextData != null && currentTextData.next_order != 0)
        {
            //指定顺序的话就加载指定数序
            order = currentTextData.next_order;
        }
        this.textOrder = order;
        List<TextInfoBean> textListData = GetTextDataByOrder(textOrder);
        if (!CheckUtil.ListIsNull(textListData))
            ShowText(textListData);
        else
        {
            switch (mTextEnum)
            {
                case TextEnum.Look:
                case TextEnum.Talk:
                    uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
                    break;
                case TextEnum.Story:
                    uiManager.CloseAllUI();
                    break;
            }
            //如果是时停 需要回复时停
            if (currentTextData.is_stoptime == 1)
            {
                GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
                if (gameTimeHandler != null)
                    gameTimeHandler.SetTimeRestore();
            }
            //回调
            if (callBack != null)
                callBack.UITextEnd();
        }
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="id">当 textEnum为Look 或 Story时 为markId</param>
    public void SetData(TextEnum textEnum, long id)
    {
        mTextEnum = textEnum;
        textOrder = 1;
        switch (textEnum)
        {
            case TextEnum.Look:
                mTextInfoController.GetTextForLook(id);
                break;
            case TextEnum.Talk:
                mTextInfoController.GetTextForTalkByMarkId(id);
                break;
            case TextEnum.Story:
                mTextInfoController.GetTextForStory(id);
                break;
        }
    }

    public void SetDataForTalk(long userId, NPCTypeEnum npcType)
    {
        mTextEnum = TextEnum.Talk;
        textOrder = 1;
        listTextData = new List<TextInfoBean>();
        switch (npcType)
        {
            case NPCTypeEnum.RecruitTown:
                listTextData.Add(new TextInfoBean(0, GameCommonInfo.GetUITextById(99101)));
                listTextData.Add(new TextInfoBean(1, GameCommonInfo.GetUITextById(99102)));
                listTextData.Add(new TextInfoBean(1, GameCommonInfo.GetUITextById(99104)));
                listTextData.Add(new TextInfoBean(1, GameCommonInfo.GetUITextById(99103)));
                break;
        }
        mTextInfoController.GetTextForTalkByUserId(userId);
    }

    /// <summary>
    /// 设置备用数据
    /// </summary>
    /// <param name="listMarkData"></param>
    public void SetListMark(SortedList<string, string> listMarkData)
    {
        this.listMarkData = listMarkData;
    }

    /// <summary>
    /// 根据顺序获取文本数据
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<TextInfoBean> GetTextDataByOrder(int order)
    {
        List<TextInfoBean> listData = new List<TextInfoBean>();
        if (listTextData == null || order > listTextData.Count)
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

    /// <summary>
    /// 展示文本数据
    /// </summary>
    /// <param name="textData"></param>
    public void ShowText(List<TextInfoBean> textListData)
    {
        if (CheckUtil.ListIsNull(textListData))
        {
            LogUtil.LogError("没有查询到相应文本对话数据");
            return;
        }
        currentTextData = textListData[0];
        uiForTalk.Close();
        uiForBook.Close();
        uiForBehind.Close();

        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        //时停选择 特殊处理
        if (currentTextData.is_stoptime == 1)
            //设置时间彻底停止计时
            uiGameManager.gameTimeHandler.SetTimeStop();
        switch ((TextInfoTypeEnum)currentTextData.type)
        {
            //对话和选择对话
            case TextInfoTypeEnum.Normal:
            case TextInfoTypeEnum.Select:
                uiForTalk.Open();
                uiForTalk.SetData(currentTextData, textListData);
                break;
            case TextInfoTypeEnum.Book:
                //书本详情
                uiForBook.Open();
                uiForBook.SetData(currentTextData.name, currentTextData.content);
                break;
            case TextInfoTypeEnum.Behind:
                //黑幕
                uiForBehind.Open();
                uiForBehind.SetData(currentTextData.content, currentTextData.wait_time);
                break;
        }
    }

    /// <summary>
    /// 替换特殊字符
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public string SetContentDetails(string content)
    {
        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        string userName = "";
        int sex = 1;
        if (uiGameManager.gameDataManager.gameData.userCharacter != null
            && uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name != null)
            userName = uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name;
        if (uiGameManager.gameDataManager.gameData.userCharacter != null)
            sex = uiGameManager.gameDataManager.gameData.userCharacter.body.sex;
        //替换名字
        content = content.Replace("{name}", userName);
        //替换小名
        string otherName = "";
        if (userName.Length > 1)
        {
            otherName = userName.Substring(userName.Length - 1, 1);
            //男的叫哥哥  女的叫姐姐
            string otherStr = sex == 1 ? GameCommonInfo.GetUITextById(99001) : GameCommonInfo.GetUITextById(99002);
            otherName += otherStr;
        }
        content = content.Replace("{othername}", otherName);

        //替换备用数据
        if (listMarkData != null)
        {
            foreach (var item in listMarkData)
            {
                content = content.Replace(item.Key, item.Value);
            }
        }

        return content;
    }

    /// <summary>
    /// 文本选择
    /// </summary>
    /// <param name="textData"></param>
    public void SelectText(TextInfoBean textData)
    {
        switch (mTextEnum)
        {
            case TextEnum.Story:
                NextText(textData.next_order);
                break;
            case TextEnum.Talk:
                if (textData.content.Equals(GameCommonInfo.GetUITextById(99102)))
                {
                    //对话
                    listTextData = RandomUtil.GetRandomDataByDictionary(mapTalkNormalData);
                    NextText(1);
                }
                else if (textData.content.Equals(GameCommonInfo.GetUITextById(99103)))
                {
                    //退出
                    NextText();
                }
                else if (textData.content.Equals(GameCommonInfo.GetUITextById(99104)))
                {
                    //招募
                    listTextData = RandomUtil.GetRandomDataByDictionary(mapTalkRecruitData);
                    NextText(1);
                }
                else if (textData.content.Equals(GameCommonInfo.GetUITextById(99105)))
                {
                    //送礼

                }
                else
                {
                    NextText(textData.next_order);
                }
                break;
        }
    }

    #region 文本获取回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        ShowText(listTextData);
    }

    public void GetTextInfoForTalkByUserIdSuccess(List<TextInfoBean> listData)
    {
        mapTalkNormalData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkGiftData = new Dictionary<long, List<TextInfoBean>>();
        mapTalkRecruitData = new Dictionary<long, List<TextInfoBean>>();
        foreach (TextInfoBean itemTalkInfo in listData)
        {
            long markId = itemTalkInfo.mark_id;
            Dictionary<long, List<TextInfoBean>> addMap = new Dictionary<long, List<TextInfoBean>>();
            switch ((TextTalkTypeEnum)itemTalkInfo.talk_type)
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
        ShowText(listTextData);
    }

    public void GetTextInfoForTalkByMarkIdSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        ShowText(listTextData);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        ShowText(listTextData);
    }

    public void GetTextInfoFail()
    {

    }


    #endregion

    public interface ICallBack
    {
        /// <summary>
        /// 文本展示结束
        /// </summary>
        void UITextEnd();

        /// <summary>
        /// 文本增加好感
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="favorability"></param>
        void UITextAddFavorability(long characterId, int favorability);

        /// <summary>
        /// 文本场景表情互动
        /// </summary>
        /// <param name="mapData"></param>
        void UITextSceneExpression(Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum> mapData);

        /// <summary>
        /// 文本选择结果处理
        /// </summary>
        void UITextSelectResult(TextInfoBean textData,List<CharacterBean> listUserData);
    }
}
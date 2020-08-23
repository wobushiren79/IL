using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameText : UIGameComponent, TextInfoManager.ICallBack, DialogView.IDialogCallBack
{
    [Header("控件")]
    public UIGameTextForBook uiForBook;
    public UIGameTextForBehind uiForBehind;
    public UIGameTextForTalk uiForTalk;


    [Header("数据")]
    public int textOrder = 1;

    public TextInfoBean currentTextData;

    public ICallBack callBack;
    private TextEnum mTextEnum;
    //谈话对象
    private NpcInfoBean mTalkNpcInfo;
    //备用文本替换数据
    public SortedList<string, string> listMarkData = new SortedList<string, string>();

    public override void Awake()
    {
        base.Awake();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        //uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiGameManager.textInfoManager.SetCallBack(this);
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
        if (currentTextData != null&&currentTextData.type!= (int)TextInfoTypeEnum.Select && currentTextData.next_order != 0)
        {
            //指定顺序的话就加载指定数序
            order = currentTextData.next_order;
        }
        this.textOrder = order;
        List<TextInfoBean> textListData =uiGameManager.textInfoManager.GetTextDataByOrder(textOrder);
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
                if (uiGameManager.gameTimeHandler != null)
                    uiGameManager.gameTimeHandler.SetTimeRestore();
            }
            //回调
            if (callBack != null)
                callBack.UITextEnd();
        }
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.Correct);
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
        uiGameManager.textInfoManager.GetTextById(textEnum, id);
    }

    /// <summary>
    /// 设置数据-聊天
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="npcType"></param>
    public void SetDataForTalk(NpcInfoBean npcInfo)
    {
        this.mTalkNpcInfo = npcInfo;
        mTextEnum = TextEnum.Talk;
        textOrder = 1;
        CharacterFavorabilityBean characterFavorability = uiGameManager.gameDataManager.gameData.GetCharacterFavorability(mTalkNpcInfo.id);
        //如果是小镇居民的第一次对话
        if (mTalkNpcInfo.GetNpcType() == NpcTypeEnum.Town && characterFavorability.firstMeet)
        {
            characterFavorability.firstMeet = false;
            uiGameManager.textInfoManager.GetTextForTownFirstMeet(mTalkNpcInfo.id);
            return;
        }
        //获取对话选项
        uiGameManager.textInfoManager.GetTextForTalkOptions(uiGameManager.gameDataManager.gameData, mTalkNpcInfo);
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
    /// 展示文本数据
    /// </summary>
    /// <param name="textData"></param>
    public void ShowText(List<TextInfoBean> textListData)
    {
        if (CheckUtil.ListIsNull(textListData))
        {
            LogUtil.LogWarning("没有查询到相应文本对话数据");
            return;
        }
        currentTextData = textListData[0];
        uiForTalk.Close();
        uiForBook.Close();
        uiForBehind.Close();
        
        //时停选择 特殊处理
        if (currentTextData.is_stoptime == 1)
            //设置时间彻底停止计时
            uiGameManager.gameTimeHandler.SetTimeStop();
        switch (currentTextData.GetTextType())
        {
            //对话和选择对话
            case TextInfoTypeEnum.Normal:
            case TextInfoTypeEnum.Select:
            case TextInfoTypeEnum.Talk:
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
        string userName = "";
        int sex = 1;
        if (uiGameManager.gameDataManager.gameData.userCharacter != null
            && uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name != null)
            userName = uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name;
        if (uiGameManager.gameDataManager.gameData.userCharacter != null)
            sex = uiGameManager.gameDataManager.gameData.userCharacter.body.sex;
        //去除空格 防止自动换行
        content = content.Replace(" ", "");
        //替换客栈名字
        content = content.Replace(GameSubstitutionInfo.Inn_Name, uiGameManager.gameDataManager.gameData.GetInnAttributesData().innName);
        //替换名字
        content = content.Replace(GameSubstitutionInfo.User_Name, userName);
        //替换小名
        string otherName = "";
        if (userName.Length > 1)
        {
            otherName = userName.Substring(userName.Length - 1, 1);
            //男的叫哥哥  女的叫姐姐
            string otherStr = sex == 1 ? GameCommonInfo.GetUITextById(99001) : GameCommonInfo.GetUITextById(99002);
            otherName += otherStr;
        }
        content = content.Replace(GameSubstitutionInfo.User_Other_Name, otherName);

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
            case TextEnum.Look:
            case TextEnum.Story:
                NextText(textData.next_order);
                break;
            case TextEnum.Talk:
                //不同的对话选项
                //对话
                //如果是对话选项
                if(textData.GetTextType()== TextInfoTypeEnum.Talk)
                {
                    if (textData.content.Equals(GameCommonInfo.GetUITextById(99102)))
                    {
                        //对话
                        uiGameManager.textInfoManager.listTextData = RandomUtil.GetRandomDataByDictionary(uiGameManager.textInfoManager.mapTalkNormalData);
                        NextText(1);
                        //增加好感
                        if (GameCommonInfo.DailyLimitData.AddTalkNpc(mTalkNpcInfo.id))
                        {
                            uiGameManager.gameDataManager.gameData.GetCharacterFavorability(mTalkNpcInfo.id).AddFavorability(1);
                        }
                        //增加数据记录
                        CharacterFavorabilityBean characterFavorability = uiGameManager.gameDataManager.gameData.GetCharacterFavorability(mTalkNpcInfo.id);
                        characterFavorability.AddTalkNumber(1);
                    }
                    //退出
                    else if (textData.content.Equals(GameCommonInfo.GetUITextById(99103)))
                    {
                        NextText();
                    }
                    //招募
                    else if (textData.content.Equals(GameCommonInfo.GetUITextById(99104)))
                    {

                        if (uiGameManager.gameDataManager.gameData.CheckIsMaxWorker())
                        {
                            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1051));
                        }
                        else
                        {
                            uiGameManager.textInfoManager.listTextData = RandomUtil.GetRandomDataByDictionary(uiGameManager.textInfoManager.mapTalkRecruitData);
                            NextText(1);
                        }
                    }
                    //送礼
                    else if (textData.content.Equals(GameCommonInfo.GetUITextById(99105)))
                    {

                        DialogBean dialogData = new DialogBean();
                        PickForItemsDialogView pickForItemsDialog = (PickForItemsDialogView)uiGameManager.dialogManager.CreateDialog(DialogEnum.PickForItems, this, dialogData);
                        pickForItemsDialog.SetData(null, PopupItemsSelection.SelectionTypeEnum.Gift);
                    }
                    //换取公会勋章
                    else if (textData.content.Equals(GameCommonInfo.GetUITextById(99201)))
                    {
                        //对话
                        uiGameManager.textInfoManager.listTextData = RandomUtil.GetRandomDataByDictionary(uiGameManager.textInfoManager.mapTalkExchangeData);
                        NextText(1);
                    }    
                    //换取奖杯
                    else if (textData.content.Equals(GameCommonInfo.GetUITextById(99202)))
                    {
                        //对话
                        uiGameManager.textInfoManager.listTextData = RandomUtil.GetRandomDataByDictionary(uiGameManager.textInfoManager.mapTalkExchangeData);
                        NextText(1);
                    }
                }
                else
                {
                    NextText(textData.next_order);
                }
                break;
        }
    }

    #region 文本获取回调
    public void SetTextInfoForLook(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForStory(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForTalkByFirstMeet(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForTalkOptions(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForTalkByMarkId(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForTalkByUserId(List<TextInfoBean> listData)
    {
        if (callBack != null)
            callBack.UITextInitReady();
    }

    public void SetTextInfoForTalkByType(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData)
    {
        if (callBack != null)
            callBack.UITextInitReady();
    }
    #endregion

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        NpcInfoManager npcInfoManager= GetUIManager<UIGameManager>().npcInfoManager;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        //获取选择物品
        PickForItemsDialogView pickForItemsDialog = (PickForItemsDialogView)dialogView;
        pickForItemsDialog.GetSelectedItems(out ItemsInfoBean itemsInfo,out ItemBean itemData);
        //获取赠送人
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(mTalkNpcInfo.id);
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(mTalkNpcInfo.id);
        int addFavorability;
        int favorabilityForTalk;
        //根据物品稀有度增加好感
        switch (itemsInfo.GetItemRarity())
        {
            case RarityEnum.Normal:
                addFavorability = 1;
                favorabilityForTalk = 1;
                break;
            case RarityEnum.Rare:
                addFavorability = 3;
                favorabilityForTalk = 3;
                break;
            case RarityEnum.SuperRare:
                addFavorability = 6;
                favorabilityForTalk = 3;
                break;
            case RarityEnum.SuperiorSuperRare:
                addFavorability = 10;
                favorabilityForTalk = 3;
                break;
            case RarityEnum.UltraRare:
                addFavorability = 15;
                favorabilityForTalk = 3;
                break;
            default:
                addFavorability = 1;
                favorabilityForTalk = 1;
                break;
        }
        characterFavorability.AddGiftNumber(addFavorability);
        //删减物品
        gameDataManager.gameData.AddItemsNumber(itemData.itemId,-1);
        //增加每日限制
        GameCommonInfo.DailyLimitData.AddGiftNpc(mTalkNpcInfo.id);
        //通过增加好感查询对话
        uiGameManager.textInfoManager.listTextData = uiGameManager.textInfoManager.GetGiftTalkByFavorability(favorabilityForTalk);
        ShowText(uiGameManager.textInfoManager.listTextData);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }

    #endregion

    public interface ICallBack
    {
        /// <summary>
        /// 文本准备完成
        /// </summary>
        void UITextInitReady();

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
        void UITextSelectResult(TextInfoBean textData, List<CharacterBean> listPickCharacterData);
    }
}
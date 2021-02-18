using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameText : BaseUIComponent, DialogView.IDialogCallBack
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
        //AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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
        if (currentTextData != null && currentTextData.type != (int)TextInfoTypeEnum.Select && currentTextData.next_order != 0)
        {
            //指定顺序的话就加载指定数序
            order = currentTextData.next_order;
        }
        this.textOrder = order;
        List<TextInfoBean> textListData = TextInfoHandler.Instance.manager.GetTextDataByOrder(textOrder);
        if (!CheckUtil.ListIsNull(textListData))
            ShowText(textListData);
        else
        {
            switch (mTextEnum)
            {
                case TextEnum.Look:
                case TextEnum.Talk:
                    UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
                    break;
                case TextEnum.Story:
                    UIHandler.Instance.manager.CloseAllUI();
                    break;
            }
            //如果是时停 需要回复时停
            if (currentTextData.is_stoptime == 1)
            {

                GameTimeHandler.Instance.SetTimeRestore();
            }
            //回调
            if (callBack != null)
                callBack.UITextEnd();
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Correct);
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
        TextInfoHandler.Instance.manager.GetTextById(textEnum, id, SetTextInfoData);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(mTalkNpcInfo.id);
        //如果是小镇居民的第一次对话
        if (mTalkNpcInfo.GetNpcType() == NpcTypeEnum.Town && characterFavorability.firstMeet)
        {
            characterFavorability.firstMeet = false;
            TextInfoHandler.Instance.manager.GetTextForTownFirstMeet(mTalkNpcInfo.id, SetTextInfoData);
            return;
        }
        //获取对话选项
        TextInfoHandler.Instance.manager.GetTextForTalkOptions(gameData, mTalkNpcInfo, SetTextInfoData);
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
            GameTimeHandler.Instance.SetTimeStop();
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.userCharacter != null
            && gameData.userCharacter.baseInfo.name != null)
            userName = gameData.userCharacter.baseInfo.name;
        if (gameData.userCharacter != null)
            sex = gameData.userCharacter.body.sex;
        //去除空格 防止自动换行
        content = content.Replace(" ", "");
        //替换客栈名字
        content = content.Replace(GameSubstitutionInfo.Inn_Name, gameData.GetInnAttributesData().innName);
        //替换名字
        content = content.Replace(GameSubstitutionInfo.User_Name, userName);
        //替换小名
        string otherName = "";
        if (userName.Length > 1)
        {
            otherName = userName.Substring(userName.Length - 1, 1);
            //男的叫哥哥  女的叫姐姐
            string otherStr = sex == 1 ? TextHandler.Instance.manager.GetTextById(99001) : TextHandler.Instance.manager.GetTextById(99002);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
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
                if (textData.GetTextType() == TextInfoTypeEnum.Talk)
                {
                    if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99102)))
                    {
                        //对话
                        TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkNormalData);
                        NextText(1);
                        //增加好感
                        if (GameCommonInfo.DailyLimitData.AddTalkNpc(mTalkNpcInfo.id))
                        {
                            gameData.GetCharacterFavorability(mTalkNpcInfo.id).AddFavorability(1);
                        }
                        //增加数据记录
                        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(mTalkNpcInfo.id);
                        characterFavorability.AddTalkNumber(1);
                    }
                    //退出
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99103)))
                    {
                        NextText();
                    }
                    //招募
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99104)))
                    {

                        if (gameData.CheckIsMaxWorker())
                        {
                            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1051));
                        }
                        else
                        {
                            TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkRecruitData);
                            NextText(1);
                        }
                    }
                    //送礼
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99105)))
                    {
                        DialogBean dialogData = new DialogBean();
                        PickForItemsDialogView pickForItemsDialog = DialogHandler.Instance.CreateDialog<PickForItemsDialogView>(DialogEnum.PickForItems, this, dialogData);
                        pickForItemsDialog.SetData(null, ItemsSelectionDialogView.SelectionTypeEnum.Gift);
                    }
                    //换取公会勋章
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99201)))
                    {
                        //对话
                        TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkExchangeData);
                        NextText(1);
                    }
                    //换取奖杯
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99202)))
                    {
                        //对话
                        TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkExchangeData);
                        NextText(1);
                    }
                    //换取无尽之塔装备
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99203)))
                    {
                        //对话
                        TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkExchangeData);
                        NextText(1);
                    }
                    //换取无尽之塔道具
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99204)))
                    {
                        //对话
                        TextInfoHandler.Instance.manager.listTextData = RandomUtil.GetRandomDataByDictionary(TextInfoHandler.Instance.manager.mapTalkExchangeData);
                        NextText(1);
                    }
                    //求婚
                    else if (textData.content.Equals(TextHandler.Instance.manager.GetTextById(99205)))
                    {
                        gameData.CheckHasItems(99900021, out bool hasItems, out long number);
                        //判断是否有信物
                        if (hasItems)
                        {
                            DialogBean dialogData = new DialogBean();
                            dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3121), mTalkNpcInfo.name);
                            DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
                            NextText(textData.next_order);
                        }
                        else
                        {
                            ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(99900021);
                            ToastHandler.Instance.ToastHint(string.Format(TextHandler.Instance.manager.GetTextById(5023), itemsInfo.name, "1"));
                        }
                    }
                }
                else
                {
                    NextText(textData.next_order);
                }
                break;
        }
    }


    /// <summary>
    /// 设置文本数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetTextInfoData(List<TextInfoBean> listData)
    {
        ShowText(listData);
        if (callBack != null)
            callBack.UITextInitReady();
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //送礼
        if (dialogView is PickForItemsDialogView)
        {
            //获取选择物品
            PickForItemsDialogView pickForItemsDialog = dialogView as PickForItemsDialogView;
            pickForItemsDialog.GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemData);
            //获取赠送人
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(mTalkNpcInfo.id);
            CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(mTalkNpcInfo.id);
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
            //增加送礼次数
            characterFavorability.AddGiftNumber(1);
            //删减物品
            gameData.AddItemsNumber(itemData.itemId, -1);
            //增加每日限制
            GameCommonInfo.DailyLimitData.AddGiftNpc(mTalkNpcInfo.id);
            //通过增加好感查询对话
            TextInfoHandler.Instance.manager.listTextData = TextInfoHandler.Instance.manager.GetGiftTalkByFavorability(favorabilityForTalk);
            ShowText(TextInfoHandler.Instance.manager.listTextData);

            //文本里面会默认加好感
            //先减去文本加的好感 再添加实际的好感加成
            characterFavorability.AddFavorability(-favorabilityForTalk);
            characterFavorability.AddFavorability(addFavorability);
        }
        //求婚
        else
        {
            //减去信物
            gameData.AddItemsNumber(99900021,-1);
            //弹窗提示
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1341));
            //设置3天后结婚
            FamilyDataBean familyData = gameData.GetFamilyData();
            familyData.timeForMarry = GameTimeHandler.Instance.GetAfterDay(3);
            //设置妻子数据
            CharacterForFamilyBean characterData = new CharacterForFamilyBean(mTalkNpcInfo, familyData.timeForMarry);
            characterData.SetFamilyType(FamilyTypeEnum.Mate);
            familyData.wifeCharacter = characterData;
        }
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
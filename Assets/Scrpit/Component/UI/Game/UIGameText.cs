using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameText : BaseUIComponent, ITextInfoView
{
    [Header("控件")]
    public Text tvContent;
    public Text tvName;
    public Image ivFavorability;

    public Text tvBookName;
    public Text tvBookContent;
    public Button btBookBack;

    public Text tvBehind;

    public GameObject objNext;
    public CharacterUICpt characterUICpt;

    public GameObject objTypeNormal;
    public GameObject objTypeBook;
    public GameObject objTypeBehind;

    public GameObject objSelectContent;
    public GameObject objSelectModel;

    //文本容器
    public RectTransform rtfTextContent;

    [Header("数据")]
    public int textOrder = 1;
    public List<TextInfoBean> listTextData;
    public TextInfoBean currentTextData;

    private TextInfoController mTextInfoController;
    private Tweener tweenerText;
    private TextEnum mTextEnum;
    private ICallBack mCallBack;

    //备用文本替换数据
    public List<string> listMarkData = new List<string>();
    private void Awake()
    {
        mTextInfoController = new TextInfoController(this, this);
    }

    private void Start()
    {
        if (btBookBack != null)
            btBookBack.onClick.AddListener(NextText);
    }

    private void Update()
    {
        if (this.currentTextData != null && this.currentTextData.type == 4)
        {
            return;
        }
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (tweenerText != null && tweenerText.IsActive() && tweenerText.IsPlaying())
            {
                tweenerText.Complete();
                //刷新控件
                if (rtfTextContent != null)
                    GameUtil.RefreshRectViewHight(rtfTextContent, true);
            }
            else
            {
                //当时选择对话时 不能跳过
                if (this.currentTextData != null && this.currentTextData.type == 1)
                {

                }
                else
                {
                    NextText();
                }
            }
        }
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
            if (!currentTextData.is_stoptime)
            {
                GameTimeHandler gameTimeHandler = GetUIMananger<UIGameManager>().gameTimeHandler;
                if (gameTimeHandler != null)
                    gameTimeHandler.SetTimeRestore();
            }
            //回调
            if (mCallBack != null)
                mCallBack.UITextEnd();
        }
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textEnum"></param>
    /// <param name="markId"></param>
    public void SetData(TextEnum textEnum, long markId)
    {
        mTextEnum = textEnum;
        textOrder = 1;
        switch (textEnum)
        {
            case TextEnum.Look:
                mTextInfoController.GetTextForLook(markId);
                break;
            case TextEnum.Talk:
                mTextInfoController.GetTextForTalk(markId);
                break;
            case TextEnum.Story:
                mTextInfoController.GetTextForStory(markId);
                break;
        }
    }

    /// <summary>
    /// 设置备用数据
    /// </summary>
    /// <param name="listMarkData"></param>
    public void SetListMark(List<string> listMarkData)
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
    public void ShowText(List<TextInfoBean> listTextData)
    {
        //清空选项
        CptUtil.RemoveChildsByName(objSelectContent.transform, "SelectButton", true);
        List<TextInfoBean> textListData = GetTextDataByOrder(textOrder);
        if (CheckUtil.ListIsNull(textListData))
        {
            LogUtil.LogError("没有查询到相应文本对话数据");
            return;
        }
        currentTextData = textListData[0];
        objTypeNormal.SetActive(false);
        objTypeBehind.SetActive(false);
        objTypeBook.SetActive(false);

        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        //时停选择 特殊处理
        if (currentTextData.is_stoptime)
            //设置时间彻底停止计时
            uiGameManager.gameTimeHandler.SetTimeStop();
        switch (currentTextData.type)
        {
            //对话和选择对话
            case 0:
            case 1:
                objTypeNormal.SetActive(true);
                //选择对话 特殊处理 增加选择框
                if (currentTextData.type == 1)
                {
                    objNext.gameObject.SetActive(false);
                    foreach (TextInfoBean itemData in textListData)
                    {
                        //提示文本
                        if (itemData.select_type == 0)
                        {
                            this.currentTextData = itemData;
                        }
                        // 选项
                        else
                        {
                            GameObject objSelect = Instantiate(objSelectModel, objSelectContent.transform);
                            objSelect.SetActive(true);
                            ItemGameTextSelectCpt itemCpt = objSelect.GetComponent<ItemGameTextSelectCpt>();
                            itemCpt.SetData(itemData, this);
                        }
                    }
                }
                else
                {
                    objNext.gameObject.SetActive(true);
                }
                //正常文本处理
                //角色图标设置
                CharacterBean characterData;
                if (currentTextData.user_id == 0)
                    characterData = uiGameManager.gameDataManager.gameData.userCharacter;
                else
                    characterData = uiGameManager.npcInfoManager.GetCharacterDataById(currentTextData.user_id);
                if (characterData == null)
                {
                    LogUtil.LogError("文本展示没有找到该文本发起者");
                    return;
                }
                //名字设置
                if (tvName != null)
                {
                    if (CheckUtil.StringIsNull(currentTextData.name))
                    {
                        tvName.text = characterData.baseInfo.titleName + "-" + characterData.baseInfo.name;
                    }
                    else
                        tvName.text = currentTextData.name;
                }
                if (characterUICpt != null)
                    characterUICpt.SetCharacterData(characterData.body, characterData.equips);
                //设置正文内容
                if (tvContent != null)
                {
                    tvContent.text = "";
                    string contentDetails = SetContentDetails(currentTextData.content);
                    //如果时停了就不播放动画了
                    if (Time.timeScale == 0)
                    {
                        tvContent.text = contentDetails;
                        //刷新控件大小
                        if (rtfTextContent != null)
                            GameUtil.RefreshRectViewHight(rtfTextContent, true);
                    }
                    else
                        tweenerText = tvContent.DOText(contentDetails, currentTextData.content.Length / 8f).SetEase(Ease.OutBack);
                }
                break;
            case 4:
                //书本详情
                if (tvBookName != null)
                    tvBookName.text = currentTextData.name;
                if (tvBookContent != null)
                    tvBookContent.text = currentTextData.content;
                objTypeBook.SetActive(true);
                break;
            case 5:
                //黑幕
                objTypeBehind.SetActive(true);
                if (tvBehind != null)
                {
                    tvBehind.text = SetContentDetails(currentTextData.content);
                    tweenerText = tvBehind.DOFade(0, currentTextData.wait_time).From().OnComplete(delegate
                    {
                        NextText(textOrder + 1);
                    });
                }
                break;
        }
        //添加好感度
        if (currentTextData.add_favorability != 0)
        {
            AddFavorability(currentTextData.user_id, currentTextData.add_favorability);
            //回调
            if (mCallBack != null)
                mCallBack.UITextAddFavorability(currentTextData.user_id, currentTextData.add_favorability);
        }
        //场景人物表情展示
        if (!CheckUtil.StringIsNull(currentTextData.scene_expression))
        {
            if (mCallBack != null)
            {
                Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum> mapData = new Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum>();
                List<string> listData = StringUtil.SplitBySubstringForListStr(currentTextData.scene_expression, ',');
                for (int i = 0; i < listData.Count; i += 2)
                {
                    int mapKey = int.Parse(listData[i]);
                    CharacterExpressionCpt.CharacterExpressionEnum mapValue = (CharacterExpressionCpt.CharacterExpressionEnum)int.Parse(listData[i + 1]);
                    mapData.Add(mapKey, mapValue);
                }
                mCallBack.UITextSceneExpression(mapData);
            }
        }
    }

    /// <summary>
    /// 增加好感
    /// </summary>
    /// <param name="characterId"></param>
    /// <param name="favorablility"></param>
    public void AddFavorability(long characterId, int favorablility)
    {
        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        CharacterFavorabilityBean favorabilityData = uiGameManager.gameDataManager.gameData.GetFavorabilityDataById(characterId);
        favorabilityData.AddFavorability(favorablility);
        //好感动画
        if (ivFavorability != null)
        {
            ivFavorability.transform.localScale = new Vector3(1, 1, 1);
            ivFavorability.transform.DOComplete();
            ivFavorability.gameObject.SetActive(true);
            ivFavorability.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).OnComplete(delegate ()
            {
                ivFavorability.gameObject.SetActive(false);
            });
            ivFavorability.DOColor(new Color(1, 1, 1, 0), 1).From();
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
        if (!CheckUtil.ListIsNull(listMarkData))
            content = string.Format(content, listMarkData.ToArray());
        return content;
    }

    #region 文本获取回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        ShowText(listTextData);
    }

    public void GetTextInfoForTalkSuccess(List<TextInfoBean> listData)
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
    }
}
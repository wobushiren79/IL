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
    public Text tvBehind;
    public GameObject objNext;
    public CharacterUICpt characterUICpt;

    public GameObject objTypeNormal;
    public GameObject objTypeBehind;

    public GameObject objSelectContent;
    public GameObject objSelectModel;

    [Header("数据")]
    public int textOrder = 1;
    public List<TextInfoBean> listTextData;
    public TextInfoBean currentTextData;

    private TextInfoController mTextInfoController;
    private Tweener tweenerText;
    private TextEnum mTextEnum;
    private CallBack mCallBack;

    private void Awake()
    {
        mTextInfoController = new TextInfoController(this, this);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //如果是调查或者对话 文本显示结束后 事件结束
        if (mTextEnum == TextEnum.Look || mTextEnum == TextEnum.Talk)
        {
            EventHandler.Instance.ChangeEventStatus(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interactive_E") || Input.GetButtonDown("Confirm"))
        {
            if (tweenerText != null && tweenerText.IsActive() && tweenerText.IsPlaying())
            {
                tweenerText.Complete();
            }
            else
            {
                //当时选择对话时 不能跳过
                if (this.currentTextData != null && this.currentTextData.type == 1)
                {

                }
                else
                {
                    NextText(textOrder + 1);
                }
            }
        }
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
            //回调
            if (mCallBack != null)
                mCallBack.TextEnd();
        }
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(CallBack callBack)
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
            if (itemData.order == order)
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
        switch (currentTextData.type)
        {
            //对话和选择对话
            case 0:
            case 1:
                objTypeNormal.SetActive(true);
                objTypeBehind.SetActive(false);
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
                UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
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
                        tvName.text = characterData.baseInfo.name;
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
                    tweenerText = tvContent.DOText(contentDetails, currentTextData.content.Length / 8f).SetEase(Ease.OutBack);
                }
                break;
            case 5:
                //黑幕
                objTypeNormal.SetActive(false);
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
        if (currentTextData.add_favorability!=0) {
            AddFavorability(currentTextData.user_id, currentTextData.add_favorability);
        }
    }

    /// <summary>
    /// 增加好感
    /// </summary>
    /// <param name="characterId"></param>
    /// <param name="favorablility"></param>
    public void AddFavorability(long characterId,int favorablility)
    {
        UIGameManager uiGameManager= GetUIMananger<UIGameManager>();
        uiGameManager.gameDataManager.gameData.AddFavorability(characterId, favorablility);
        LogUtil.Log("add favorability");
    }

    /// <summary>
    /// 替换特殊字符
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private string SetContentDetails(string content)
    {
        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        string userName = "";
        if (uiGameManager.gameDataManager.gameData.userCharacter != null 
            && uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name!=null)
            userName = uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name;
        //替换名字
        content = content.Replace("{name}", userName);
        //替换小名
        string otherName = "";
        if (userName.Length > 1)
        {
            otherName = userName.Substring(userName.Length - 1, 1);
            otherName += otherName;
        }
        content = content.Replace("{othername}", otherName);
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

    public interface CallBack
    {
        void TextEnd();
    }
}
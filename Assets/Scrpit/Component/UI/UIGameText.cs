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
    public CharacterUICpt characterUICpt;

    public GameObject objTypeNormal;
    public GameObject objTypeBehind;

    [Header("数据")]
    public int textOrder = 1;
    public List<TextInfoBean> listTextData;

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
            if (tweenerText != null && tweenerText.IsPlaying())
            {
                tweenerText.Complete();
            }
            else
            {
                NextText();
            }
        }
    }

    public void NextText()
    {
        textOrder += 1;
        TextInfoBean textData = GetTextDataByOrder(textOrder);
        if (textData != null)
            ShowText(textData);
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
    public TextInfoBean GetTextDataByOrder(int order)
    {
        TextInfoBean data = null;
        if (listTextData == null || order > listTextData.Count)
            return data;
        foreach (TextInfoBean itemData in listTextData)
        {
            if (itemData.order == order)
            {
                return itemData;
            }
        }
        return data;
    }

    /// <summary>
    /// 展示文本数据
    /// </summary>
    /// <param name="textData"></param>
    public void ShowText(TextInfoBean textData)
    {
        switch (textData.type)
        {
            case 0:
                objTypeNormal.SetActive(true);
                objTypeBehind.SetActive(false);
                UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
                if (tvContent != null)
                {
                    tvContent.text = "";
                    string contentDetails = SetContentDetails(textData.content);
                    tweenerText = tvContent.DOText(contentDetails, textData.content.Length / 8f).SetEase(Ease.OutBack);
                }
                CharacterBean characterData;
                if (textData.user_id == 0)
                    characterData = uiGameManager.gameDataManager.gameData.userCharacter;
                else
                    characterData = uiGameManager.npcInfoManager.GetCharacterDataById(textData.user_id);
                if (characterData == null)
                {
                    LogUtil.LogError("文本展示没有找到该文本发起者");
                    return;
                }
                if (tvName != null)
                    tvName.text = characterData.baseInfo.name;
                if (characterUICpt != null)
                    characterUICpt.SetCharacterData(characterData.body, characterData.equips);
                break;
            case 1:
                objTypeNormal.SetActive(false);
                objTypeBehind.SetActive(true);
                if (tvBehind != null)
                {
                    tvBehind.text = SetContentDetails(textData.content);
                    tweenerText = tvBehind.DOFade(0, textData.wait_time).From().OnComplete(delegate {
                        NextText();
                    });
                }
                break;
        }
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
        if (uiGameManager.gameDataManager.gameData.userCharacter != null)
            userName = uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.name;
        //替换名字
        content = content.Replace("{name}", userName);
        return content;
    }

    #region 文本获取回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        TextInfoBean textData = GetTextDataByOrder(textOrder);
        ShowText(textData);
    }

    public void GetTextInfoForTalkSuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        TextInfoBean textData = GetTextDataByOrder(textOrder);
        ShowText(textData);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {
        listTextData = listData;
        TextInfoBean textData = GetTextDataByOrder(textOrder);
        ShowText(textData);
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
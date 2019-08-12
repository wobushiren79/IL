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
    public CharacterUICpt characterUICpt;

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

    public override void OpenUI()
    {
        base.OpenUI();
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        if (gameObject.activeSelf)
        {
            if (GetUIMananger<UIGameManager>().controlHandler != null)
                GetUIMananger<UIGameManager>().controlHandler.RestoreControl();
        }
        base.CloseUI();
        //如果是调查或者对话 文本显示结束后 事件结束
        if(mTextEnum==TextEnum.Look|| mTextEnum == TextEnum.Talk)
        {
            EventHandler.Instance.isEventing = false;
        } 
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interactive_E")|| Input.GetButtonDown("Confirm"))
        {
            if (tweenerText != null && tweenerText.IsPlaying())
            {
                tweenerText.Complete();
                tweenerText.Kill();
            }
            else
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
        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        if (tvContent != null)
        {
            tvContent.text = "";
            tweenerText =  tvContent.DOText(textData.content, textData.content.Length/8f).SetEase(Ease.OutBack);
        }
        
        CharacterBean characterData;
        if (textData.user_id == 0)
        {
            characterData = uiGameManager.gameDataManager.gameData.userCharacter;
        }
        else
        {
            characterData = uiGameManager.npcInfoManager.GetCharacterDataById(textData.user_id);

        }
        if (characterData == null)
        {
            LogUtil.LogError("文本展示没有找到该文本发起者");
            return;
        }
        if (tvName != null)
        {
            tvName.text = characterData.baseInfo.name;
        }
           
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);

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
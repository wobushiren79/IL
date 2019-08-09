using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameText : BaseUIComponent
{
    [Header("控件")]
    public Text tvContent;
    public Text tvName;
    public CharacterUICpt characterUICpt;

    [Header("数据")]
    public int textOrder = 1;
    public List<TextInfoBean> listTextData;

    private Tweener tweenerText;
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
        EventHandler.Instance.isEventing = false;
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
                    uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
                }
            }
        }
    }

    public void SetData(TextEnum textEnum, List<TextInfoBean> listData)
    {
        listTextData = listData;
        textOrder = 1;
        TextInfoBean textData = GetTextDataByOrder(textOrder);
        ShowText(textData);
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
}
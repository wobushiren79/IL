using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemGameDialogPickCharacterCpt : BaseMonoBehaviour
{
    public CharacterUICpt characterUI;
    public UIPopupAbilityButton popupAbility;
    public Text tvName;
    public Image ivBack;
    public Button btPick;

    public Sprite spBackPick;
    public Sprite spBackUnPick;

    public bool isPick = false;
    public CharacterBean characterData;
    private ICallBack mCallBack;

    private void Start()
    {
        if (btPick != null)
            btPick.onClick.AddListener(PickCharacter);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    public void PickCharacter()
    {
        ChangeStatus();
        if (mCallBack!=null)
        {
            mCallBack.PickCharacter(this,isPick, characterData);
        }
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    public void ChangeStatus()
    {
        isPick = (!isPick);
        if (isPick)
        {
            SetBack(spBackPick);
        }
        else
        {
            SetBack(spBackUnPick);
        }
    }

    public void SetData(CharacterBean characterData)
    {
        this.characterData = characterData;
        SetCharacterUI(characterData);
        SetName(characterData.baseInfo.name);
        SetPopup(characterData);
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI!=null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
        }
    }

    /// <summary>
    /// 设置角色姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置弹出框
    /// </summary>
    /// <param name="characterData"></param>
    public void SetPopup(CharacterBean characterData)
    {
        popupAbility.SetData(characterData);
    }

    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="spBack"></param>
    public void SetBack(Sprite spBack)
    {
        if (ivBack != null)
            ivBack.sprite = spBack;
    }

    public interface ICallBack
    {
        void PickCharacter(ItemGameDialogPickCharacterCpt itemView,bool isPick,CharacterBean characterData);
    }
}
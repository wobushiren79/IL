using UnityEngine;
using UnityEditor;
public class ItemGameStatisticsForCustomerCpt : ItemBaseTextCpt
{
    public CharacterUICpt characterUI;
    public PopupLoveMenusButton infoLoveMenusPopup;
    //是否解锁
    protected bool isUnLock;

    public void SetData(CharacterBean characterData, bool isUnLock, string name, long number)
    {
        SetData(characterData, isUnLock, name, number, "");
    }

    public void SetData(CharacterBean characterData, bool isUnLock, string name, long number, string id)
    {
        this.isUnLock = isUnLock;
        SetCharacterUI(characterData, isUnLock);
        SetNumber(isUnLock, number);
        if (isUnLock)
        {
            if (name.IsNull())
            {
                SetName(characterData.baseInfo.name);
            }
            else
            {
                SetName(name);
            }
            ivIcon.gameObject.SetActive(false);
        }
        else
        {
            SetName("???", Color.gray);
            ivIcon.gameObject.SetActive(true);
        }
        if (!id.IsNull())
        {
            infoLoveMenusPopup.SetDataForTeamCustomer(id);
        }
        else
        {
            infoLoveMenusPopup.ShowObj(false);
        }
    }

    public void SetNumber(bool isUnLock, long number)
    {
        if (isUnLock)
        {
            SetContent(number + "");
        }
        else
        {
            SetContent("???");
            tvContent.color =Color.gray;
        }
    }

    public void SetCharacterUI(CharacterBean characterData, bool isUnLock)
    {
        if (characterUI == null)
            return;
        if (isUnLock)
        {
            characterUI.gameObject.SetActive(true);
        }
        else
        {
            characterUI.gameObject.SetActive(false);
        }
        characterUI.SetCharacterData(characterData.body, characterData.equips);
    }
}
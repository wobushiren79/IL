using UnityEngine;
using UnityEditor;
public class ItemGameStatisticsForCustomerCpt : ItemBaseTextCpt
{
    public CharacterUICpt characterUI;
    public InfoLoveMenusPopupButton infoLoveMenusPopup;
    //是否解锁
    protected bool isUnLock;
    public void SetData(CharacterBean characterData, bool isUnLock, string name)
    {
        SetData(characterData, isUnLock, name, 0);
    }

    public void SetData(CharacterBean characterData, bool isUnLock, string name,long teamId)
    {
        this.isUnLock = isUnLock;
        SetCharacterUI(characterData, isUnLock);

        if (isUnLock)
        {
            if (CheckUtil.StringIsNull(name))
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
            SetName("???",Color.gray);
            ivIcon.gameObject.SetActive(true);
        }
        if (teamId !=0 )
        {
            infoLoveMenusPopup.SetDataForTeamCustomer(teamId);
        }
        else
        {
            infoLoveMenusPopup.SetActive(false);
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
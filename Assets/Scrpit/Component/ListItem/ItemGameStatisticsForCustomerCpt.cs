using UnityEngine;
using UnityEditor;

public class ItemGameStatisticsForCustomerCpt : ItemBaseTextCpt
{
    public CharacterUICpt characterUI;

    //是否解锁
    protected bool isUnLock;
    public void SetData(CharacterBean characterData, bool isUnLock,string name)
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
            SetName("???");
            ivIcon.gameObject.SetActive(true);
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
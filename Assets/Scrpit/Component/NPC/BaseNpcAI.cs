using UnityEngine;
using UnityEditor;

public class BaseNpcAI : BaseMonoBehaviour
{
    public CharacterBean characterData;
    public CharacterDressManager characterDressManager;
    //角色移动控制
    public CharacterMoveCpt characterMoveCpt;

    private void Awake()
    {
        characterMoveCpt = GetComponent<CharacterMoveCpt>();
    }

    public void SetCharacterData(CharacterBean characterBean)
    {
        if (characterData == null)
            return;
        this.characterData = characterBean;
        //设置身体数据
        CharacterBodyCpt characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(gameObject, "Body");
        if (characterBody != null)
            characterBody.SetCharacterBody(characterData.body);
        //设置服装数据
        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(gameObject, "Body");
        if (characterDress != null)
        {
            EquipInfoBean hatEquip= characterDressManager.GetEquipById(characterBean.equips.hatId);
            characterDress.SetHat(hatEquip);

            EquipInfoBean clothesEquip = characterDressManager.GetEquipById(characterBean.equips.clothesId);
            characterDress.SetClothes(clothesEquip);

            EquipInfoBean shoesEquip = characterDressManager.GetEquipById(characterBean.equips.shoesId);
            characterDress.SetShoes(shoesEquip);
        }
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        characterMoveCpt.StopAutoMove();
        characterMoveCpt.Stop();
    }
}
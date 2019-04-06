using UnityEngine;
using UnityEditor;

public class BaseNpcAI : BaseMonoBehaviour
{
    public int intentType;//意图 1路过 2吃饭

    public CharacterBean characterData;
    public CharacterDressManager characterDressManager;

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
            characterDress.SetShoes(clothesEquip);
        }
    }
}
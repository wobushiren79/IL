using UnityEngine;
using UnityEditor;

public class CharacterDressCpt : BaseMonoBehaviour
{
    //帽子
    public SpriteRenderer sprHat;
    //衣服
    public SpriteRenderer sprClothes;
    //鞋子
    public SpriteRenderer sprShoesLeft;
    public SpriteRenderer sprShoesRight;

    //角色属性
    public CharacterEquipBean characterEquipData;
    //服装管理
    public CharacterDressManager characterDressManager;

    public CharacterEquipBean GetCharacterEquipData()
    {
        if(characterEquipData==null)
            characterEquipData = new CharacterEquipBean();
        return characterEquipData;
    }



    public void SetHat(EquipInfoBean equipInfo)
    {
        if (sprHat == null)
            return;
        Sprite hatSP;
        if (equipInfo == null)
            hatSP = null;
        else
        {
            hatSP = characterDressManager.GetHatSpriteByName(equipInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.hatId = equipInfo.equip_id;
        }
        sprHat.sprite = hatSP;
    }

    public void SetClothes(EquipInfoBean equipInfo)
    {
        if (sprClothes == null)
            return;
        Sprite clothesSP;
        if (equipInfo == null)
            clothesSP = null;
        else
        {
            clothesSP = characterDressManager.GetClothesSpriteByName(equipInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.clothesId = equipInfo.equip_id;
        }    
        sprClothes.sprite = clothesSP;
    }

    public void SetShoes(EquipInfoBean equipInfo)
    {
        if (sprShoesLeft == null|| sprShoesRight==null)
            return;
        Sprite shoesSP;
        if (equipInfo == null)
            shoesSP = null;
        else
        {
            shoesSP = characterDressManager.GetShoesSpriteByName(equipInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.shoesId = equipInfo.equip_id;
        }
        sprShoesLeft.sprite = shoesSP;
        sprShoesRight.sprite = shoesSP;
    }
}
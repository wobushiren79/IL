using UnityEngine;
using UnityEditor;

public class CharacterDressCpt : BaseMonoBehaviour
{
    //帽子
    public SpriteRenderer sprHat;
    public SpriteRenderer sprHair;

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
        if (characterEquipData == null)
            characterEquipData = new CharacterEquipBean();
        return characterEquipData;
    }

    public void SetHat(ItemsInfoBean itemsInfo)
    {
        if (sprHat == null)
            return;
        Sprite hatSP;
        if (itemsInfo == null||itemsInfo.icon_key==null)
        {
            sprHair.color = new Color(sprHair.color.r, sprHair.color.g, sprHair.color.b, 1);
            hatSP = null;
        }
        else
        {
            sprHair.color = new Color(sprHair.color.r, sprHair.color.g, sprHair.color.b, 0);
            hatSP = characterDressManager.GetHatSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.hatId = itemsInfo.id;
        }
        sprHat.sprite = hatSP;
    }

    public void SetClothes(ItemsInfoBean itemsInfo)
    {
        if (sprClothes == null)
            return;
        Sprite clothesSP = null;
        if (itemsInfo == null)
            clothesSP = null;
        else
        {
            if (itemsInfo.id != 0)
                clothesSP = characterDressManager.GetClothesSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.clothesId = itemsInfo.id;
        }
        sprClothes.sprite = clothesSP;
    }

    public void SetShoes(ItemsInfoBean itemsInfo)
    {
        if (sprShoesLeft == null || sprShoesRight == null)
            return;
        Sprite shoesSP;
        if (itemsInfo == null)
            shoesSP = null;
        else
        {
            shoesSP = characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.shoesId = itemsInfo.id;
        }
        sprShoesLeft.sprite = shoesSP;
        sprShoesRight.sprite = shoesSP;
    }
}
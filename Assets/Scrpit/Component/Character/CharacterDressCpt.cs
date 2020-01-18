using UnityEngine;
using UnityEditor;

public class CharacterDressCpt : BaseMonoBehaviour
{
    //面具
    public SpriteRenderer sprMask;
    //帽子
    public SpriteRenderer sprHat;
    public SpriteRenderer sprHair;

    //衣服
    public SpriteRenderer sprClothes;
    //鞋子
    public SpriteRenderer sprShoesLeft;
    public SpriteRenderer sprShoesRight;
    //手持
    public SpriteRenderer sprHand;

    //角色属性
    public CharacterEquipBean characterEquipData;
    //服装管理
    protected CharacterDressManager characterDressManager;
    //道具管理
    protected GameItemsManager gameItemsManager;

    public void Awake()
    {
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    public CharacterEquipBean GetCharacterEquipData()
    {
        if (characterEquipData == null)
            characterEquipData = new CharacterEquipBean();
        return characterEquipData;
    }

    /// <summary>
    /// 设置帽子
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetHat(ItemsInfoBean itemsInfo)
    {
        if (sprHat == null)
            return;
        Sprite hatSP;
        if (itemsInfo == null || itemsInfo.icon_key == null)
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

    /// <summary>
    /// 设置面具
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetMask(ItemsInfoBean itemsInfo)
    {
        if (sprMask == null)
            return;
        Sprite maskSP = null;
        if (itemsInfo == null)
            maskSP = null;
        else
        {
            if (itemsInfo.id != 0)
                maskSP = characterDressManager.GetMaskSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.maskId = itemsInfo.id;
        }
        sprMask.sprite = maskSP;
    }

    /// <summary>
    /// 设置衣服
    /// </summary>
    /// <param name="itemsInfo"></param>
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

    /// <summary>
    /// 设置鞋子
    /// </summary>
    /// <param name="itemsInfo"></param>
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

    /// <summary>
    /// 设置手持
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetHand(ItemsInfoBean itemsInfo)
    {
        if (sprHand == null)
            return;
        Sprite handSP;
        if (itemsInfo == null)
            handSP = null;
        else
        {
            handSP = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.handId = itemsInfo.id;
        }
        sprHand.sprite = handSP;
        //设置旋转角度
        if (itemsInfo != null && itemsInfo.rotation_angle != 0)
        {
            sprHand.transform.localEulerAngles = new Vector3(0, 0, itemsInfo.rotation_angle);
        }
        else
        {
            sprHand.transform.localEulerAngles = new Vector3(0, 0, 45);
        }
    }
}
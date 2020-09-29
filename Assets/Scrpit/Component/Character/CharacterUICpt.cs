﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUICpt : BaseMonoBehaviour
{
    public Image ivHead;
    public Image ivHair;
    public Image ivEye;
    public Image ivMouth;
    public Image ivBody;
    public Image ivFootLeft;
    public Image ivFootRight;

    public Image ivMask;
    public Image ivHand;
    public Image ivHat;
    public Image ivClothes;
    public Image ivShoes;

    //角色身体数据
    public CharacterBodyBean characterBodyData;
    public CharacterEquipBean characterEquipData;

    public CharacterBodyManager characterBodyManager;
    public CharacterDressManager characterDressManager;
    public GameItemsManager gameItemsManager;

    public void Awake()
    {
        characterBodyManager = Find< CharacterBodyManager>( ImportantTypeEnum.CharacterManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    public void SetCharacterData(BodyTypeEnum bodyType, string bodyData, Color color)
    {
        switch (bodyType)
        {
            case BodyTypeEnum.Hair:
                SetHair(bodyData, color);
                break;
            case BodyTypeEnum.Eye:
                SetEye(bodyData, color);
                break;
            case BodyTypeEnum.Mouth:
                SetMouth(bodyData, color);
                break;
            case BodyTypeEnum.Skin:
                SetSkin(color);
                int sex = 1;
                if (characterBodyData != null)
                    sex = characterBodyData.sex;
                SetSex(sex, bodyData);
                break;
        }
    }

    public void SetCharacterData(CharacterBodyBean characterBodyData, CharacterEquipBean characterEquipData)
    {
        if (characterBodyData != null)
            this.characterBodyData = characterBodyData;
        if (characterEquipData != null)
            this.characterEquipData = characterEquipData;

        SetHair(characterBodyData.hair, characterBodyData.hairColor.GetColor());
        SetEye(characterBodyData.eye, characterBodyData.eyeColor.GetColor());
        SetMouth(characterBodyData.mouth, characterBodyData.mouthColor.GetColor());

        SetSkin(characterBodyData.skinColor.GetColor());
        SetSex(characterBodyData.sex, characterBodyData.skin);

        if (characterEquipData.maskTFId != 0)
        {
            //幻化处理
            SetMask(characterEquipData.maskTFId);
        }
        else
        {
            SetMask(characterEquipData.maskId);
        }

        if (characterEquipData.handTFId != 0)
        {
            //幻化处理
            SetHand(characterEquipData.handTFId);
        }
        else
        {
            SetHand(characterEquipData.handId);
        }

        if (characterEquipData.clothesTFId != 0)
        {
            //幻化处理
            SetClothes(characterEquipData.clothesTFId);
        }
        else
        {
            SetClothes(characterEquipData.clothesId);
        }

        if (characterEquipData.shoesTFId != 0)
        {
            //幻化处理
            SetShoes(characterEquipData.shoesTFId);
        }
        else
        {
            SetShoes(characterEquipData.shoesId);
        }

        if (characterEquipData.hatTFId != 0)
        {
            //幻化处理
            SetHat(characterEquipData.hatTFId, characterBodyData.hairColor.GetColor());
        }
        else
        {
            SetHat(characterEquipData.hatId, characterBodyData.hairColor.GetColor());
        }
    }


    /// <summary>
    /// 设置头发
    /// </summary>
    /// <param name="hairName"></param>
    /// <param name="hairColor"></param>
    public void SetHair(string hairName, Color hairColor)
    {
        if (ivHair == null)
            return;
        Sprite spHair = characterBodyManager.GetHairSpriteByName(hairName);
        ivHair.sprite = spHair;
        if (spHair == null)
        {
            ivHair.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivHair.color = hairColor;
        }
    }

    /// <summary>
    /// 设置眼睛
    /// </summary>
    /// <param name="eyeName"></param>
    /// <param name="eyeColor"></param>
    public void SetEye(string eyeName, Color eyeColor)
    {
        if (ivEye == null)
            return;
        Sprite spEye = characterBodyManager.GetEyeSpriteByName(eyeName);
        ivEye.sprite = spEye;
        if (spEye == null)
        {
            ivEye.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivEye.color = eyeColor;
        }
    }

    /// <summary>
    /// 设置嘴巴
    /// </summary>
    /// <param name="mouthName"></param>
    /// <param name="mouthColor"></param>
    public void SetMouth(string mouthName, Color mouthColor)
    {
        if (ivMouth == null)
            return;
        Sprite spMouth = characterBodyManager.GetMouthSpriteByName(mouthName);
        ivMouth.sprite = spMouth;
        if (spMouth == null)
        {
            ivMouth.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivMouth.color = mouthColor;
        }
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex">0未知 1男 2女 3中性</param>
    public void SetSex(int sex, string otherSkin)
    {
        if (characterBodyManager == null)
            return;
        Sprite spTrunk = null;
        if (CheckUtil.StringIsNull(otherSkin)|| otherSkin.Equals("Def"))
        {
            switch (sex)
            {
                case 0:
                    spTrunk = characterBodyManager.GetTrunkSpriteByName("character_body_man");
                    break;
                case 1:
                    spTrunk = characterBodyManager.GetTrunkSpriteByName("character_body_man");
                    break;
                case 2:
                    spTrunk = characterBodyManager.GetTrunkSpriteByName("character_body_woman");
                    break;
                case 3:
                    spTrunk = characterBodyManager.GetTrunkSpriteByName("character_body_man");
                    break;
            }
            if (ivBody != null && spTrunk != null)
                ivBody.sprite = spTrunk;
            if (ivHead != null)
                ivHead.sprite = characterBodyManager.GetTrunkSpriteByName("character_head");
            if (ivFootLeft != null)
                ivFootLeft.sprite = characterBodyManager.GetTrunkSpriteByName("character_body_left_foot");
            if (ivFootRight != null)
                ivFootRight.sprite = characterBodyManager.GetTrunkSpriteByName("character_body_right_foot");
        }
        else
        {

            if (ivHead != null)
                ivHead.sprite = characterBodyManager.GetTrunkSpriteByName(otherSkin + "_0");
            if (ivBody != null)
                ivBody.sprite = characterBodyManager.GetTrunkSpriteByName(otherSkin + "_1");
            if (ivFootLeft != null)
                ivFootLeft.sprite = characterBodyManager.GetTrunkSpriteByName(otherSkin + "_2");
            if (ivFootRight != null)
                ivFootRight.sprite = characterBodyManager.GetTrunkSpriteByName(otherSkin + "_3");
        }
    }

    /// <summary>
    /// 设置皮肤颜色
    /// </summary>
    /// <param name="skinColor"></param>
    public void SetSkin(Color skinColor)
    {
        if (ivHead)
            ivHead.color = skinColor;
        if (ivBody)
            ivBody.color = skinColor;
        if (ivFootLeft)
            ivFootLeft.color = skinColor;
        if (ivFootRight)
            ivFootRight.color = skinColor;
    }


    /// <summary>
    /// 设置服装
    /// </summary>
    /// <param name="clothesId"></param>
    public void SetMask(long maskId)
    {
        if (ivMask == null)
            return;
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(maskId);
        if (itemsInfo == null)
        {
            ivMask.color = new Color(1, 1, 1, 0);
            return;
        }
        Sprite spMask = characterDressManager.GetMaskSpriteByName(itemsInfo.icon_key);
        if (spMask == null)
        {
            ivMask.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivMask.color = new Color(1, 1, 1, 1);
        }
        ivMask.sprite = spMask;
    }

    /// <summary>
    /// 设置手持
    /// </summary>
    /// <param name="handId"></param>
    public void SetHand(long handId)
    {
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(handId);
        Sprite spHand = null;
        if (itemsInfo != null)
        {
            spHand = gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
        }
        if (spHand == null)
        {
            ivHand.color = new Color(1, 1, 1, 0);
        }
        else
        {
            int scaleY = (int)spHand.rect.height / 32;
            float scale = (0.5f * scaleY);
            ivHand.transform.localScale = new Vector3(scale, scale, scale);
            ivHand.color = new Color(1, 1, 1, 1);
        }
        ivHand.sprite = spHand;

        //设置旋转角度
        if (itemsInfo != null && itemsInfo.rotation_angle != 0)
        {
            ivHand.transform.localEulerAngles = new Vector3(0, 0, itemsInfo.rotation_angle);
        }
        else
        {
            ivHand.transform.localEulerAngles = new Vector3(0, 0, 45);
        }
    }

    /// <summary>
    /// 设置服装
    /// </summary>
    /// <param name="clothesId"></param>
    public void SetClothes(long clothesId)
    {
        //皇帝的新衣
        if (clothesId == 219999)
        {
            clothesId = 0;
        }
        if (ivClothes == null)
            return;
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(clothesId);
        if (itemsInfo == null)
        {
            ivClothes.color = new Color(1, 1, 1, 0);
            return;
        }
        Sprite spClothes = characterDressManager.GetClothesSpriteByName(itemsInfo.icon_key);
        if (spClothes == null)
        {
            ivClothes.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivClothes.color = new Color(1, 1, 1, 1);
        }
        ivClothes.sprite = spClothes;
    }

    /// <summary>
    /// 设置鞋子
    /// </summary>
    /// <param name="shoesId"></param>
    public void SetShoes(long shoesId)
    {
        //皇帝的新衣
        if (shoesId == 319999)
        {
            shoesId = 0;
        }
        if (ivShoes == null)
            return;
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(shoesId);
        if (itemsInfo == null)
        {
            ivShoes.color = new Color(1, 1, 1, 0);
            return;
        }
        Sprite spShoes = characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
        if (spShoes == null)
        {
            ivShoes.color = new Color(1, 1, 1, 0);
        }
        else
        {
            ivShoes.color = new Color(1, 1, 1, 1);
        }
        ivShoes.sprite = spShoes;
    }

    /// <summary>
    ///  设置帽子ID
    /// </summary>
    /// <param name="hatId"></param>
    public void SetHat(long hatId, Color hairColor)
    {
        //皇帝的新衣
        if (hatId == 119999)
        {
            hatId = 0;
        }
        if (ivHat == null)
            return;
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(hatId);
        if (itemsInfo == null)
        {
            ivHat.color = new Color(1, 1, 1, 0);
            return;
        }

        Sprite spHat = characterDressManager.GetHatSpriteByName(itemsInfo.icon_key);
        if (spHat == null)
        {
            ivHat.color = new Color(1, 1, 1, 0);
            if (ivHair.color.a != 0)
            {
                ivHair.color = hairColor;
            }
        }
        else
        {
            ivHat.color = new Color(1, 1, 1, 1);
            ivHair.color = new Color(1, 1, 1, 0);
        }
        ivHat.sprite = spHat;
    }
}

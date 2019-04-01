using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterBodyCpt : BaseMonoBehaviour
{
    //头
    public SpriteRenderer sprHead;
    //躯干
    public SpriteRenderer sprTrunk;
    //脚
    public SpriteRenderer sprFootLeft;
    public SpriteRenderer sprFootRight;
    //头发
    public SpriteRenderer sprHair;
    //眼睛
    public SpriteRenderer sprEye;
    //嘴巴
    public SpriteRenderer sprMouth;

    //角色属性
    public CharacterBodyBean characterBodyData;
    //角色身体资源管理
    public CharacterBodyManager characterBodyManager;

    /// <summary>
    /// 获取身体属性
    /// </summary>
    /// <returns></returns>
    public CharacterBodyBean GetBodyData()
    {
        return characterBodyData;
    }

    /// <summary>
    /// 设置角色身体属性
    /// </summary>
    /// <param name="characterAttributesBean"></param>
    public void SetCharacterBody(CharacterBodyBean characterBodyData)
    {
        if (characterBodyData == null)
            return;
        this.characterBodyData = characterBodyData;
        SetSex(this.characterBodyData.sex);
        SetSkin(this.characterBodyData.skinColor.GetColor());
        SetHair(this.characterBodyData.hair, this.characterBodyData.hairColor.GetColor());
        SetEye(this.characterBodyData.eye, this.characterBodyData.eyeColor.GetColor());
        SetMouth(this.characterBodyData.mouth, this.characterBodyData.mouthColor.GetColor());
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex">0未知 1男 2女 3中性</param>
    public void SetSex(int sex)
    {
        if (characterBodyManager == null)
            return;
        Sprite spTrunk = null;
        switch (sex)
        {
            case 0:
                spTrunk = characterBodyManager.GetTrunkSpriteByName("man");
                break;
            case 1:
                spTrunk = characterBodyManager.GetTrunkSpriteByName("man");
                break;
            case 2:
                spTrunk = characterBodyManager.GetTrunkSpriteByName("woman");
                break;
            case 3:
                spTrunk = characterBodyManager.GetTrunkSpriteByName("man");
                break;
        }
        if (sprTrunk != null && spTrunk != null)
            sprTrunk.sprite = spTrunk;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.sex = sex;
    }

    /// <summary>
    /// 设置头发
    /// </summary>
    /// <param name="hair"></param>
    /// <param name="hairColor"></param>
    public void SetHair(string hair, Color hairColor)
    {
        if (characterBodyManager == null || sprHair == null)
            return;
        Sprite spHair = characterBodyManager.GetHairSpriteByName(hair);
        if (hair != null)
            sprHair.sprite = spHair;
        sprHair.color = hairColor;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.hair = hair;
        characterBodyData.hairColor = TypeConversionUtil.ColorToColorBean(hairColor);
    }
    public void SetHair(string hair)
    {
        if (sprHair == null)
            return;
        SetHair(hair, sprHair.color);
    }
    public void SetHair(Color hairColor)
    {
        if (sprHair == null)
            return;
        SetHair(null, hairColor);
    }

    /// <summary>
    ///  设置眼睛
    /// </summary>
    /// <param name="mouth"></param>
    /// <param name="mouthColor"></param>
    public void SetEye(string eye, Color eyeColor)
    {
        if (characterBodyManager == null || sprEye == null)
            return;
        Sprite spEye = characterBodyManager.GetEyeSpriteByName(eye);
        if (eye != null)
            sprEye.sprite = spEye;
        sprEye.color = eyeColor;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.eye = eye;
        characterBodyData.eyeColor = TypeConversionUtil.ColorToColorBean(eyeColor);
    }
    public void SetEye(string eye)
    {
        if (characterBodyManager == null || sprEye == null)
            return;
        SetEye(eye, sprEye.color);
    }
    public void SetEye(Color eyeColor)
    {
        if (sprEye == null)
            return;
        SetEye(null,eyeColor);
    }

    /// <summary>
    ///  设置嘴巴
    /// </summary>
    /// <param name="mouth"></param>
    /// <param name="mouthColor"></param>
    public void SetMouth(string mouth, Color mouthColor)
    {
        if (characterBodyManager == null || sprMouth == null)
            return;
        Sprite spMouth = characterBodyManager.GetMouthSpriteByName(mouth);
        if (mouth != null)
            sprMouth.sprite = spMouth;
        sprMouth.color = mouthColor;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.mouth = mouth;
        characterBodyData.mouthColor = TypeConversionUtil.ColorToColorBean(mouthColor);
    }
    public void SetMouth(string mouth)
    {
        if (characterBodyManager == null || sprMouth == null)
            return;
        SetMouth( mouth, sprMouth.color);
    }
    public void SetMouth(Color mouthColor)
    {
        if (sprMouth == null)
            return;
        SetMouth(null, mouthColor);
    }

    /// <summary>
    /// 设置皮肤
    /// </summary>
    /// <param name="skinColor">皮肤颜色</param>
    public void SetSkin(Color skinColor)
    {
        if (sprHead == null
            || sprTrunk == null
            || sprFootLeft == null
            || sprFootRight == null)
            return;
        sprHead.color = skinColor;
        sprTrunk.color = skinColor;
        sprFootLeft.color = skinColor;
        sprFootRight.color = skinColor;
    }
}

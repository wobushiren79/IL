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
    public CharacterBean characterData;
    //角色身体资源管理
    public CharacterBodyManager characterBodyManager;

    /// <summary>
    /// 设置角色身体属性
    /// </summary>
    /// <param name="characterAttributesBean"></param>
    public void SetCharacterBody(CharacterBean characterAttributesBean)
    {
        if (characterAttributesBean == null)
            return;
        this.characterData = characterAttributesBean;
        SetSex(this.characterData.body.sex);
        SetSkin(this.characterData.body.skinColor.GetColor());
        SetHair(this.characterData.body.hair, this.characterData.body.hairColor.GetColor());
        SetEye(this.characterData.body.eye, this.characterData.body.eyeColor.GetColor());
        SetMouth(this.characterData.body.mouth, this.characterData.body.mouthColor.GetColor());
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
    }

    /// <summary>
    /// 设置头发
    /// </summary>
    /// <param name="hair"></param>
    /// <param name="hairColor"></param>
    public void SetHair(string hair,Color hairColor)
    {
        if (characterBodyManager == null|| sprHair == null)
            return;
        Sprite spHair = characterBodyManager.GetHairSpriteByName(hair);
        sprHair.sprite = spHair;
        sprHair.color = hairColor;
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
        sprEye.sprite = spEye;
        sprEye.color = eyeColor;
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
        sprMouth.sprite = spMouth;
        sprMouth.color = mouthColor;
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

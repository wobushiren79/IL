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


    /// <summary>
    /// 获取身体属性
    /// </summary>
    /// <returns></returns>
    public CharacterBodyBean GetCharacterBodyData()
    {
        if (characterBodyData == null)
        {
            characterBodyData = new CharacterBodyBean();
        }
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
        SetSex(this.characterBodyData.sex, this.characterBodyData.skin);
        SetSkin(this.characterBodyData.skinColor.GetColor());
        SetHair(this.characterBodyData.hair, this.characterBodyData.hairColor.GetColor());
        SetEye(this.characterBodyData.eye, this.characterBodyData.eyeColor.GetColor());
        SetMouth(this.characterBodyData.mouth, this.characterBodyData.mouthColor.GetColor());
        SetFace(this.characterBodyData.face);
    }

    /// <summary>
    /// 设置朝向
    /// </summary>
    /// <param name="face"></param>
    public void SetFace(int face)
    {
        Vector3 bodyScale = transform.localScale;
        switch (face)
        {
            case 1:
                bodyScale.x = -1;
                break;

            case 2:
                bodyScale.x = 1;
                break;
        }
        transform.localScale = bodyScale;
    }

    public int GetFace()
    {
        if (transform.localScale.x < 0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }



    /// <summary>
    /// 设置头发
    /// </summary>
    /// <param name="hair"></param>
    /// <param name="hairColor"></param>
    public void SetHair(string hair, Color hairColor)
    {
        if (sprHair == null)
            return;
        Sprite spHair = CharacterBodyHandler.Instance.manager.GetHairSpriteByName(hair);
        if (hair != null)
            sprHair.sprite = spHair;
        sprHair.color = hairColor;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        if (hair != null)
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
    public void SetEye(string eye, Color eyeColor, bool isSave)
    {
        if (sprEye == null)
            return;
        Sprite spEye = CharacterBodyHandler.Instance.manager.GetEyeSpriteByName(eye);
        if (eye != null)
            sprEye.sprite = spEye;
        sprEye.color = eyeColor;
        if (!isSave)
            return;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        if (eye != null)
            characterBodyData.eye = eye;
        characterBodyData.eyeColor = TypeConversionUtil.ColorToColorBean(eyeColor);
    }
    public void SetEye(string eye)
    {
        if (sprEye == null)
            return;
        SetEye(eye, sprEye.color, true);
    }
    public void SetEye(Color eyeColor)
    {
        if (sprEye == null)
            return;
        SetEye(null, eyeColor, true);
    }
    public void SetEye(string eye, Color eyeColor)
    {
        SetEye(eye, eyeColor, true);
    }


    /// <summary>
    ///  设置嘴巴
    /// </summary>
    /// <param name="mouth"></param>
    /// <param name="mouthColor"></param>
    public void SetMouth(string mouth, Color mouthColor)
    {
        if (sprMouth == null)
            return;
        Sprite spMouth = CharacterBodyHandler.Instance.manager.GetMouthSpriteByName(mouth);
        if (mouth != null)
            sprMouth.sprite = spMouth;
        sprMouth.color = mouthColor;
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        if (mouth != null)
            characterBodyData.mouth = mouth;
        characterBodyData.mouthColor = TypeConversionUtil.ColorToColorBean(mouthColor);
    }
    public void SetMouth(string mouth)
    {
        if (sprMouth == null)
            return;
        SetMouth(mouth, sprMouth.color);
    }
    public void SetMouth(Color mouthColor)
    {
        if (sprMouth == null)
            return;
        SetMouth(null, mouthColor);
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex">0未知 1男 2女 3中性</param>
    public void SetSex(int sex, string otherSkin)
    {
        Sprite spTrunk = null;
        if (CheckUtil.StringIsNull(otherSkin)||otherSkin.Equals("Def"))
        {
            switch (sex)
            {
                case 0:
                    spTrunk = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_man");
                    break;
                case 1:
                    spTrunk = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_man");
                    break;
                case 2:
                    spTrunk = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_woman");
                    break;
                case 3:
                    spTrunk = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_man");
                    break;
            }
            if (sprTrunk != null && spTrunk != null)
                sprTrunk.sprite = spTrunk;
            if (sprHead != null)
                sprHead.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_head");
            if (sprFootLeft != null)
                sprFootLeft.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_left_foot");
            if (sprFootRight != null)
                sprFootRight.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_right_foot");
        }
        else
        {
            if (sprHead != null)
                sprHead.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName(otherSkin + "_0");
            if (sprTrunk != null)
                sprTrunk.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName(otherSkin + "_1");
            if (sprFootLeft != null)
                sprFootLeft.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName(otherSkin + "_2");
            if (sprFootRight != null)
                sprFootRight.sprite = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName(otherSkin + "_3");
        }

        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.sex = sex;
        characterBodyData.skin = otherSkin;
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
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.skinColor = TypeConversionUtil.ColorToColorBean(skinColor);
    }
}

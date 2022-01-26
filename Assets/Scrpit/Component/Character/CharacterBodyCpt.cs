using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterBodyCpt : BaseMonoBehaviour
{
    //身体
    public SpriteRenderer srBody;

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
        if (srBody == null)
            return;
        Texture texHair = CharacterBodyHandler.Instance.manager.GetHairTexByName(hair);
        if (hair != null)
            srBody.material.SetTexture("_Hair", texHair);
        srBody.material.SetColor("_ColorHair", hairColor);
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        if (hair != null)
            characterBodyData.hair = hair;
        characterBodyData.hairColor = TypeConversionUtil.ColorToColorBean(hairColor);
    }
    public void SetHair(string hair)
    {
        if (srBody == null)
            return;
        SetHair(hair, srBody.material.GetColor("_ColorHair"));
    }
    public void SetHair(Color hairColor)
    {
        if (srBody == null)
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
        if (srBody == null)
            return;
        Texture texEye = CharacterBodyHandler.Instance.manager.GetEyeTexByName(eye);
        if (eye != null)
            srBody.sharedMaterial.SetTexture("_Eye", texEye);
        srBody.material.SetColor("_ColorEye", eyeColor);
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
        if (srBody == null)
            return;
        SetEye(eye, srBody.material.GetColor("_ColorEye"), true);
    }
    public void SetEye(Color eyeColor)
    {
        if (srBody == null)
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
        if (srBody == null)
            return;
        Texture texMouth = CharacterBodyHandler.Instance.manager.GetMouthTexByName(mouth);
        if (mouth != null)
            srBody.material.SetTexture("_Mouth", texMouth);
        srBody.material.SetColor("_ColorMouth", mouthColor);
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        if (mouth != null)
            characterBodyData.mouth = mouth;
        characterBodyData.mouthColor = TypeConversionUtil.ColorToColorBean(mouthColor);
    }
    public void SetMouth(string mouth)
    {
        if (srBody == null)
            return;
        SetMouth(mouth, srBody.material.GetColor("_ColorMouth"));
    }
    public void SetMouth(Color mouthColor)
    {
        if (srBody == null)
            return;
        SetMouth(null, mouthColor);
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex">0未知 1男 2女 3中性</param>
    public void SetSex(int sex, string otherSkin)
    {
        Texture texTrunk = null;
        if (CheckUtil.StringIsNull(otherSkin)||otherSkin.Equals("Def"))
        {
            switch (sex)
            {
                case 0:
                    texTrunk = CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_man");
                    break;
                case 1:
                    texTrunk = CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_man");
                    break;
                case 2:
                    texTrunk = CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_woman");
                    break;
                case 3:
                    texTrunk = CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_man");
                    break;
            }
            if (srBody == null)
                return;
            if (texTrunk != null)
                srBody.material.SetTexture("_Trunk", texTrunk);
            srBody.material.SetTexture("_Head", CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_head"));
            srBody.material.SetTexture("_FootLeft", CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_left_foot"));
            srBody.material.SetTexture("_FootRight", CharacterBodyHandler.Instance.manager.GetTrunkTexByName("character_body_right_foot"));
        }
        else
        {
            if (srBody == null)
                return;
            srBody.material.SetTexture("_Head", CharacterBodyHandler.Instance.manager.GetTrunkTexByName(otherSkin + "_0"));
            srBody.material.SetTexture("_Trunk", CharacterBodyHandler.Instance.manager.GetTrunkTexByName(otherSkin + "_1"));
            srBody.material.SetTexture("_FootLeft", CharacterBodyHandler.Instance.manager.GetTrunkTexByName(otherSkin + "_2"));
            srBody.material.SetTexture("_FootRight", CharacterBodyHandler.Instance.manager.GetTrunkTexByName(otherSkin + "_3"));
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
        if (srBody == null)
            return;
        srBody.material.SetColor("_ColorHead", skinColor);
        srBody.material.SetColor("_ColorTrunk", skinColor);
        srBody.material.SetColor("_ColorFootLeft", skinColor);
        srBody.material.SetColor("_ColorFootRight", skinColor);
        //数据保存
        if (characterBodyData == null)
            characterBodyData = new CharacterBodyBean();
        characterBodyData.skinColor = TypeConversionUtil.ColorToColorBean(skinColor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHeadUICpt : BaseMonoBehaviour
{
    public Image ivHead;
    public Image ivHair;
    public Image ivEye;
    public Image ivMouth;


    //角色身体数据
    public CharacterBodyBean characterBodyData;
    public CharacterBodyManager characterBodyManager;

    public void SetCharacterData(CharacterBodyBean characterBodyData)
    {
        if (characterBodyData != null)
            this.characterBodyData = characterBodyData;
        SetSkin(characterBodyData.skinColor.GetColor());
        SetHair(characterBodyData.hair,characterBodyData.hairColor.GetColor());
        SetEye(characterBodyData.eye, characterBodyData.eyeColor.GetColor());
        SetMouth(characterBodyData.mouth, characterBodyData.mouthColor.GetColor());
    }

    /// <summary>
    /// 设置皮肤颜色
    /// </summary>
    /// <param name="skinColor"></param>
    public void SetSkin(Color skinColor)
    {
        if (ivHead == null)
            return;
        ivHead.color = skinColor;
    }

    /// <summary>
    /// 设置头发
    /// </summary>
    /// <param name="hairName"></param>
    /// <param name="hairColor"></param>
    public void SetHair(string hairName,Color hairColor)
    {
        if (ivHair == null)
            return;
        Sprite spHair=  characterBodyManager.GetHairSpriteByName(hairName);
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

    //设置眼睛
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
}

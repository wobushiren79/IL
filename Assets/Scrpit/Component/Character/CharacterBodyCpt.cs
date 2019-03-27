using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterBodyCpt : BaseMonoBehaviour
{
    //躯干
    public SpriteRenderer sprTrunk;
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

    private void Start()
    {
        SetSex(2);
        SetHair("hair_3",new Color(0,0,0,1));
    }

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
}

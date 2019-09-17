﻿using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class BaseNpcAI : BaseMonoBehaviour
{
    //角色数据
    public CharacterBean characterData;
    //装备控制管理
    public GameItemsManager gameItemsManager;
    //角色移动控制
    public CharacterMoveCpt characterMoveCpt;
    //角色表情
    public CharacterExpressionCpt characterExpression;

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterBean"></param>
    public virtual void SetCharacterData(CharacterBean characterBean)
    {
        if (characterBean == null)
            return;
        this.characterData = characterBean;
        //设置身体数据
        CharacterBodyCpt characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(gameObject, "Body");
        if (characterBody != null)
            characterBody.SetCharacterBody(characterData.body);
        //设置服装数据
        CharacterDressCpt characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(gameObject, "Body");
        if (characterDress != null)
        {
            ItemsInfoBean hatEquip = gameItemsManager.GetItemsById(characterBean.equips.hatId);
            characterDress.SetHat(hatEquip);

            ItemsInfoBean clothesEquip = gameItemsManager.GetItemsById(characterBean.equips.clothesId);
            characterDress.SetClothes(clothesEquip);

            ItemsInfoBean shoesEquip = gameItemsManager.GetItemsById(characterBean.equips.shoesId);
            characterDress.SetShoes(shoesEquip);
        }
        //设置属性数据
        //获取属性数据
        characterData.GetAttributes(gameItemsManager,
             out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        //设置速度
        if (characterMoveCpt != null)
        {
            float speed = totalAttributes.speed * 0.05f + 1;
            //速度修正
            if (speed <= 0.1f)
            {
                //最低不小于0.1
                speed = 0.1f;
            }
            characterMoveCpt.moveSpeed = speed;
        }

    }

    /// <summary>
    /// 设置表情
    /// </summary>
    /// <param name="expressionEnum"></param>
    public void SetExpression(CharacterExpressionCpt.CharacterExpressionEnum expressionEnum)
    {
        if (characterExpression != null)
            characterExpression.SetExpression(expressionEnum);
    }
    public void SetExpression(int expression)
    {
        if (characterExpression != null)
            characterExpression.SetExpression(expression);
    }

    /// <summary>
    /// 设置角色朝向
    /// </summary>
    /// <param name="face"></param>
    public void SetCharacterFace(int face)
    {
        //设置身体数据
        CharacterBodyCpt characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(gameObject, "Body");
        if (characterBody != null)
            characterBody.SetFace(face);
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        characterMoveCpt.StopAutoMove();
        characterMoveCpt.StopAnim();
    }

}
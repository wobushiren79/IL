﻿using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Rendering;

public class BaseNpcAI : BaseObservable<IBaseObserver>
{
    //角色数据
    public CharacterBean characterData;
    //角色好感度
    public CharacterFavorabilityBean characterFavorabilityData;

    //角色移动控制
    public CharacterMoveCpt characterMoveCpt;
    //角色吼叫控制
    public CharacterShoutCpt characterShoutCpt;
    //角色表情
    public CharacterExpressionCpt characterExpression;
    //角色图标控制
    public CharacterStatusIconCpt characterStatusIcon;

    //角色层级
    public SortingGroup characterForRenderer;

    //游戏数据管理

    protected CharacterBodyCpt characterBody;
    protected CharacterDressCpt characterDress;

    //拿取
    public GameObject objTake;

    public virtual void Awake()
    {
        characterBody = CptUtil.GetCptInChildrenByName<CharacterBodyCpt>(gameObject, "Body");
        characterDress = CptUtil.GetCptInChildrenByName<CharacterDressCpt>(gameObject, "Body");
    }

    public virtual void OnDestroy()
    {
        StopAllCoroutines();
    }


    /// <summary>
    ///  设置角色数据
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="characterBean"></param>
    public virtual void SetCharacterData(CharacterBean characterBean)
    {
        if (characterBean == null)
            return;
        this.characterData = characterBean;
        //设置身体数据
        if (characterBody != null)
            characterBody.SetCharacterBody(characterData.body);
        //设置服装数据
        if (characterDress != null)
        {
            //设置面具
            ItemsInfoBean maskEquip = null;
            if (characterBean.equips.maskTFId != 0)
            {
                maskEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.maskTFId);
            }
            else
            {
                maskEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.maskId);
            }
            characterDress.SetMask(maskEquip);

            //设置手持
            ItemsInfoBean handEquip = null;
            if (characterBean.equips.handTFId != 0)
            {
                handEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.handTFId);
            }
            else
            {
                handEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.handId);
            }
            characterDress.SetHand(handEquip);

            //设置头部
            ItemsInfoBean hatEquip = null;
            if (characterBean.equips.hatTFId != 0)
            {
                hatEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.hatTFId);
            }
            else
            {
                hatEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.hatId);
            }
            characterDress.SetHat(hatEquip);


            //设置衣服
            ItemsInfoBean clothesEquip = null;
            if (characterBean.equips.clothesTFId != 0)
            {
                clothesEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.clothesTFId);
            }
            else
            {
                clothesEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.clothesId);
            }
            characterDress.SetClothes(clothesEquip);

            //设置鞋子
            ItemsInfoBean shoesEquip = null;
            if (characterBean.equips.shoesTFId != 0)
            {
                shoesEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.shoesTFId);
            }
            else
            {
                shoesEquip = GameItemsHandler.Instance.manager.GetItemsById(characterBean.equips.shoesId);
            }
            characterDress.SetShoes(shoesEquip);
        }
        //设置属性数据
        //获取属性数据
        characterData.GetAttributes(
             out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        //设置速度
        if (characterMoveCpt != null)
        {
            float speed = totalAttributes.speed * 0.04f + 1f;
            //速度修正
            if (speed <= 0.1f)
            {
                //最低不小于0.1
                speed = 0.1f;
            }
            characterMoveCpt.SetMoveSpeed(speed);
        }
    }

    /// <summary>
    /// 设置角色移动
    /// </summary>
    /// <param name="movePosition"></param>
    public void SetCharacterMove(Vector3 movePosition)
    {
        characterMoveCpt.SetDestination(movePosition);
    }


    /// <summary>
    /// 检测角色是否到达目的地
    /// </summary>
    public bool CheckCharacterIsArrive()
    {
        return characterMoveCpt.IsAutoMoveStop();
    }

    /// <summary>
    /// 设置角色好感度
    /// </summary>
    /// <param name="characterFavorabilityData"></param>
    public virtual void SetFavorabilityData(CharacterFavorabilityBean characterFavorabilityData)
    {
        this.characterFavorabilityData = characterFavorabilityData;
    }

    /// <summary>
    /// 设置表情
    /// </summary>
    /// <param name="expressionEnum"></param>
    public void SetExpression(CharacterExpressionCpt.CharacterExpressionEnum expressionEnum, float desTime)
    {
        if (characterExpression != null)
            characterExpression.SetExpression(expressionEnum, desTime);
    }
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
    //取消表情
    public void CancelExpression()
    {
        if (characterExpression != null)
            characterExpression.CancelExpression();
    }

    /// <summary>
    /// 增加团队图标
    /// </summary>
    /// <param name="iconColor"></param>
    public void AddStatusIconForGuestTeam(Color iconColor)
    {
        CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
        Sprite iconGuestTeam = IconDataHandler.Instance.manager.GetIconSpriteByName("team_2");
        statusIconData.iconStatus = CharacterStatusIconEnum.NpcType;
        statusIconData.spColor = iconColor;
        statusIconData.spIcon = iconGuestTeam;
        characterStatusIcon.AddStatusIcon(statusIconData);
    }

    /// <summary>
    /// 增加恶棍图标
    /// </summary>
    public void AddStatusIconForRascal()
    {
        CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
        Sprite iconGuestTeam = IconDataHandler.Instance.manager.GetIconSpriteByName("devil_1");
        statusIconData.iconStatus = CharacterStatusIconEnum.NpcType;
        statusIconData.spIcon = iconGuestTeam;
        characterStatusIcon.AddStatusIcon(statusIconData);
    }

    /// <summary>
    /// 增加好友图标
    /// </summary>
    public void AddStatusIconForFriend()
    {
        CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
        Sprite iconGuestTeam = IconDataHandler.Instance.manager.GetIconSpriteByName("ui_features_favorability");
        statusIconData.iconStatus = CharacterStatusIconEnum.NpcType;
        statusIconData.spColor = Color.red;
        statusIconData.spIcon = iconGuestTeam;
        characterStatusIcon.AddStatusIcon(statusIconData);
    }

    /// <summary>
    /// 增加状态效果图标
    /// </summary>
    /// <param name="iconName"></param>
    /// <param name="spColor"></param>
    public void AddStatusIconForEffect(Sprite spIcon, Color spColor, string markId)
    {
        if (spIcon == null)
            return;
        CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
        statusIconData.iconStatus = CharacterStatusIconEnum.Effect;
        statusIconData.spColor = Color.white;
        statusIconData.spIcon = spIcon;
        statusIconData.markId = markId;
        characterStatusIcon.AddStatusIcon(statusIconData);
    }

    /// <summary>
    /// 添加进度图标
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="markId"></param>
    public void AddStatusIconForPro(Sprite spIcon, RuntimeAnimatorController runtimeAnimator, string markId)
    {
        if (spIcon == null)
            return;
        CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
        statusIconData.iconStatus = CharacterStatusIconEnum.Pro;
        statusIconData.spColor = Color.white;
        statusIconData.spIcon = spIcon;
        statusIconData.markId = markId;
        statusIconData.iconAnimatorController = runtimeAnimator;
        characterStatusIcon.AddStatusIcon(statusIconData);
    }

    /// <summary>
    /// 通过标记ID 删除图标
    /// </summary>
    /// <param name="markId"></param>
    public void RemoveStatusIconByMarkId(string markId)
    {
        characterStatusIcon.RemoveStatusIconByMarkId(markId);
    }

    /// <summary>
    /// 通过类型删除图标
    /// </summary>
    /// <param name="type"></param>
    public void RemoveStatusIconByType(CharacterStatusIconEnum type)
    {
        try
        {
            characterStatusIcon.RemoveStatusIconByType(type);
        }
        catch
        {
  
        }        
    }

    /// <summary>
    /// 设置角色朝向 1左 2 右
    /// </summary>
    /// <param name="face"></param>
    public void SetCharacterFace(int face)
    {
        //设置身体数据
        if (characterBody != null)
            characterBody.SetFace(face);
    }

    /// <summary>
    /// 获取角色朝向
    /// </summary>
    /// <returns></returns>
    public int GetCharacterFace()
    {
        //设置身体数据
        return characterBody.GetFace();
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        characterMoveCpt.StopAutoMove();
        characterMoveCpt.SetAnimStatus(0);
    }

    /// <summary>
    /// 设置角色死亡
    /// </summary>
    public virtual void SetCharacterDead()
    {
        //设置角色死亡
        characterMoveCpt.SetAnimStatus(10);
        if (characterBody != null)
        {
            characterBody.SetEye("character_eye_special_dead", new Color(0, 0, 0), false);
            characterBody.transform.DOLocalRotate(new Vector3(0, 0, -90), 0.1f).SetEase(Ease.OutBack);
            characterBody.transform.localPosition = new Vector3(0, 0.3f, 0);
        }
    }

    /// <summary>
    /// 设置角色睡觉
    /// </summary>
    /// <param name="direction"></param>
    public virtual void SetCharacterSleep(Direction2DEnum direction)
    {
        switch (direction)
        {
            case Direction2DEnum.Left:
                SetCharacterFace(1);
                characterBody.transform.DOLocalRotate(new Vector3(0, 0, -90), 0.1f).SetEase(Ease.OutBack);
                characterBody.transform.localPosition = new Vector3(0, 0.2f, 0);
                break;
            case Direction2DEnum.UP:
                characterBody.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.1f).SetEase(Ease.OutBack);
                break;
            case Direction2DEnum.Right:
                SetCharacterFace(2);
                characterBody.transform.DOLocalRotate(new Vector3(0, 0, 90), 0.1f).SetEase(Ease.OutBack);
                characterBody.transform.localPosition = new Vector3(0, 0.2f, 0);
                break;
            case Direction2DEnum.Down:
                characterBody.transform.localPosition = new Vector3(0, 0f, 0);
                break;
        }
        if (characterDress != null)
        {
            characterDress.SetClothes(null);
            characterDress.SetHat(null);
            characterDress.SetShoes(null);
            characterDress.SetMask(null);
            characterDress.SetHand(null);
        }
        if (characterForRenderer != null)
            characterForRenderer.sortingOrder = -2;
    }

    /// <summary>
    /// 设置角色复活
    /// </summary>
    public virtual void SetCharacterLive()
    {
        characterBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.OutBack);
        characterBody.transform.localPosition = new Vector3(0, 0.5f, 0);
        if(characterForRenderer!=null)
            characterForRenderer.sortingOrder = 0;
    }

    /// <summary>
    /// 设置喊叫
    /// </summary>
    /// <param name="content"></param>
    public void SetShout(string content)
    {
        characterShoutCpt.Shout(content);
    }

    /// <summary>
    /// 设置拿取
    /// </summary>
    /// <param name="objThings"></param>
    public void SetTake(GameObject objThings)
    {
        if (objTake != null)
            objThings.transform.SetParent(objTake.transform);
    }

    /// <summary>
    /// 展示手拿物品
    /// </summary>
    public void ShowHand()
    {

    }
}
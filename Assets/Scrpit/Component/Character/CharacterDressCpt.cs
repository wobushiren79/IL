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
    public SpriteRenderer sprShoes;
    //手持
    public SpriteRenderer sprHand;

    //角色属性
    public CharacterEquipBean characterEquipData;

    //动画
    public Animator animForClothes;
    protected AnimatorOverrideController aocForClothes;
    public Animator animForMask;
    protected AnimatorOverrideController aocForMask;
    public Animator animForHat;
    protected AnimatorOverrideController aocForHat;
    public Animator animForShoes;
    protected AnimatorOverrideController aocForShoes;
    public Animator animForHand;
    protected AnimatorOverrideController aocForHand;

    public AnimationClip animForOriginalClip;

    public void Awake()
    {

        aocForMask = new AnimatorOverrideController(animForMask.runtimeAnimatorController);
        animForMask.runtimeAnimatorController = aocForMask;

        aocForHat = new AnimatorOverrideController(animForHat.runtimeAnimatorController);
        animForHat.runtimeAnimatorController = aocForHat;

        aocForClothes = new AnimatorOverrideController(animForClothes.runtimeAnimatorController);
        animForClothes.runtimeAnimatorController = aocForClothes;

        aocForShoes = new AnimatorOverrideController(animForShoes.runtimeAnimatorController);
        animForShoes.runtimeAnimatorController = aocForShoes;

        aocForHand = new AnimatorOverrideController(animForHand.runtimeAnimatorController);
        animForHand.runtimeAnimatorController = aocForHand;
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
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 119999)
        {
            itemsInfo = null;
        }
        if (sprHat == null)
            return;
        Sprite hatSP;
        if (itemsInfo == null || itemsInfo.icon_key == null)
        {
            sprHair.color = new Color(sprHair.color.r, sprHair.color.g, sprHair.color.b, 1);
            hatSP = null;
            characterEquipData.hatId = 0;
        }
        else
        {
            sprHair.color = new Color(sprHair.color.r, sprHair.color.g, sprHair.color.b, 0);
            hatSP = CharacterDressHandler.Instance.manager.GetHatSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.hatId = itemsInfo.id;
        }
        sprHat.sprite = hatSP;
        //设置动画
        SetAnimForEquip(animForHat, aocForHat, itemsInfo);
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
                maskSP = CharacterDressHandler.Instance.manager.GetMaskSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.maskId = itemsInfo.id;
        }
        sprMask.sprite = maskSP;
        //设置动画
        SetAnimForEquip(animForMask, aocForMask, itemsInfo);
    }

    /// <summary>
    /// 设置衣服
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetClothes(ItemsInfoBean itemsInfo)
    {
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 219999)
        {
            itemsInfo = null;
        }
        if (sprClothes == null)
            return;
        Sprite clothesSP = null;
        if (itemsInfo == null)
        {
            clothesSP = null;
        }
        else
        {
            if (itemsInfo.id != 0)
            {
                clothesSP = CharacterDressHandler.Instance.manager.GetClothesSpriteByName(itemsInfo.icon_key);
            }
               
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.clothesId = itemsInfo.id;

        }
        sprClothes.sprite = clothesSP;
        //设置动画
        SetAnimForEquip(animForClothes, aocForClothes, itemsInfo);
    }


    /// <summary>
    /// 设置鞋子
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetShoes(ItemsInfoBean itemsInfo)
    {
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 319999)
        {
            itemsInfo = null;
        }
        if (sprShoes == null)
            return;
        Sprite shoesSP;
        if (itemsInfo == null)
            shoesSP = null;
        else
        {
            shoesSP = CharacterDressHandler.Instance.manager.GetShoesSpriteByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.shoesId = itemsInfo.id;
        }
        sprShoes.sprite = shoesSP;
        //设置动画
        //需要考虑左右脚
        SetAnimForEquip(animForShoes, aocForShoes, itemsInfo);
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
            handSP = GameItemsHandler.Instance.manager.GetItemsSpriteByName(itemsInfo.icon_key);
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
        //设置动画
        SetAnimForEquip(animForHand, aocForHand, itemsInfo);
    }

    /// <summary>
    /// 设置动画
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetAnimForEquip(Animator animator, AnimatorOverrideController animatorForTarget,ItemsInfoBean itemsInfo)
    {
        if(animatorForTarget == null || itemsInfo == null || itemsInfo.id == 0)
        {
            animatorForTarget["Original"] = animForOriginalClip;
            animator.enabled = false;
            return;
        }       
        //设置动画
        if (!CheckUtil.StringIsNull(itemsInfo.anim_key))
        {
            GeneralEnum itemType = itemsInfo.GetItemsType();
            AnimationClip animationClip = null;
            switch (itemType)
            {
                case GeneralEnum.Mask:
                case GeneralEnum.Hat:
                case GeneralEnum.Clothes:
                case GeneralEnum.Shoes:
                    animationClip = CharacterDressHandler.Instance.manager.GetAnimByName(itemType, itemsInfo.anim_key);
                    break;
                case GeneralEnum.Chef:
                case GeneralEnum.Waiter:
                case GeneralEnum.Accoutant:
                case GeneralEnum.Accost:
                case GeneralEnum.Beater:
                    animationClip = GameItemsHandler.Instance.manager.GetItemsAnimClipByName(itemsInfo.anim_key);
                    break;
            }
   
            if (animationClip != null)
            {
                animator.enabled = true;
                animatorForTarget["Original"] = animationClip;
            }
            else
            {
                animatorForTarget["Original"] = animForOriginalClip;
                animator.enabled = false;
            }
        }
        else
        {
            animatorForTarget["Original"] = animForOriginalClip;
            animator.enabled = false;
        }
    }
}
using UnityEngine;
using UnityEditor;

public class CharacterDressCpt : BaseMonoBehaviour
{
    public SpriteRenderer srBody;

    //角色属性
    public CharacterEquipBean characterEquipData;

    //动画
    //public Animator animForClothes;
    //protected AnimatorOverrideController aocForClothes;
    //public Animator animForMask;
    //protected AnimatorOverrideController aocForMask;
    //public Animator animForHat;
    //protected AnimatorOverrideController aocForHat;
    //public Animator animForShoes;
    //protected AnimatorOverrideController aocForShoes;
    //public Animator animForHand;
    //protected AnimatorOverrideController aocForHand;

    public AnimationClip animForOriginalClip;

    protected CharacterBodyCpt characterBodyCpt;

    public void Awake()
    {

        //aocForMask = new AnimatorOverrideController(animForMask.runtimeAnimatorController);
        //animForMask.runtimeAnimatorController = aocForMask;

        //aocForHat = new AnimatorOverrideController(animForHat.runtimeAnimatorController);
        //animForHat.runtimeAnimatorController = aocForHat;

        //aocForClothes = new AnimatorOverrideController(animForClothes.runtimeAnimatorController);
        //animForClothes.runtimeAnimatorController = aocForClothes;

        //aocForShoes = new AnimatorOverrideController(animForShoes.runtimeAnimatorController);
        //animForShoes.runtimeAnimatorController = aocForShoes;

        //aocForHand = new AnimatorOverrideController(animForHand.runtimeAnimatorController);
        //animForHand.runtimeAnimatorController = aocForHand;

        characterBodyCpt = GetComponent<CharacterBodyCpt>();
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
        Texture2DArray hatTEX = SetAnimForEquip("Hat", itemsInfo);
        Color colorHair = srBody.material.GetColor("_ColorHair");
        if (itemsInfo == null || itemsInfo.icon_key == null)
        {
            if (characterBodyCpt != null)
            {
                string hairData =  characterBodyCpt.characterBodyData.hair;
                //如果有头发才显示头发
                if (!CheckUtil.StringIsNull(hairData))
                {
                    colorHair = new Color(colorHair.r, colorHair.g, colorHair.b, 1);
                    srBody.material.SetColor("_ColorHair", colorHair);
                }
            }
            hatTEX = null;
            characterEquipData.hatId = 0;
        }
        else
        {       
            colorHair = new Color(colorHair.r, colorHair.g, colorHair.b, 0);
            srBody.material.SetColor("_ColorHair", colorHair);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.hatId = itemsInfo.id;
        }
        if (hatTEX == null)
        {
            srBody.material.SetColor("_ColorHat", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorHat", new Color(1, 1, 1, 1));
        }

        //设置动画
        //SetAnimForEquip(animForHat, aocForHat, itemsInfo);
    }

    /// <summary>
    /// 设置面具
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetMask(ItemsInfoBean itemsInfo)
    {
        if (srBody == null)
            return;
        Texture2DArray maskTex = SetAnimForEquip("Mask", itemsInfo);
        if (itemsInfo == null)
        {

        }
        else
        {
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.maskId = itemsInfo.id;
        }
        if (maskTex == null)
        {
            srBody.material.SetColor("_ColorMask", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorMask", new Color(1, 1, 1, 1));
        }

        //设置动画
        //SetAnimForEquip(animForMask, aocForMask, itemsInfo);
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
        if (srBody == null)
            return;
        Texture2DArray clothesTex = SetAnimForEquip("Clothes", itemsInfo);
        if (itemsInfo == null)
        {

        }
        else
        {
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.clothesId = itemsInfo.id;

        }
        if (clothesTex == null)
        {
            srBody.material.SetColor("_ColorClothes", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorClothes", new Color(1, 1, 1, 1));
        }

        //设置动画
        //SetAnimForEquip(animForClothes, aocForClothes, itemsInfo);
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
        if (srBody == null)
            return;
        Texture2DArray shoesTex = SetAnimForEquip("Shoes", itemsInfo);
        if (itemsInfo == null)
        {

        }
        else
        {
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.shoesId = itemsInfo.id;
        }
        if (shoesTex == null)
        {
            srBody.material.SetColor("_ColorShoes", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorShoes", new Color(1, 1, 1, 1));
        }

        //设置动画
        //需要考虑左右脚
        //SetAnimForEquip(animForShoes, aocForShoes, itemsInfo);
    }

    /// <summary>
    /// 设置手持
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetHand(ItemsInfoBean itemsInfo)
    {
        if (srBody == null)
            return;
        if (itemsInfo == null)
        {

        }
        else
        {
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.handId = itemsInfo.id;
        }
        Texture2DArray handTex = SetAnimForEquip("Hand", itemsInfo);
        if (handTex == null)
        {
            srBody.material.SetColor("_ColorHand", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorHand", new Color(1, 1, 1, 1));
        }
        //设置旋转角度
        if (itemsInfo != null && itemsInfo.rotation_angle != 0)
        {
            srBody.material.SetFloat("_HandRotate", itemsInfo.rotation_angle);
        }
        else
        {
            srBody.material.SetFloat("_HandRotate", 45);
        }
        //设置动画
        //SetAnimForEquip(animForHand, aocForHand, itemsInfo);
    }

    public Texture2DArray SetAnimForEquip(string animPro, ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null || itemsInfo.id == 0)
            return null;

        Texture2DArray texture2DArray = null;
        int animLength;
        string animKey = "";
        if (itemsInfo.anim_length != 0)
        {
            animKey = itemsInfo.anim_key;
            animLength = itemsInfo.anim_length;
        }
        else
        {
            animKey = itemsInfo.icon_key;
            animLength = 1;
        }

        GeneralEnum itemType = itemsInfo.GetItemsType();
        switch (itemType)
        {
            case GeneralEnum.Mask:
                texture2DArray = CharacterDressHandler.Instance.manager.GetMaskTextureByName($"{animKey}", animLength);
                break;
            case GeneralEnum.Hat:
                texture2DArray = CharacterDressHandler.Instance.manager.GetHatTextureByName($"{animKey}", animLength);
                break;
            case GeneralEnum.Clothes:
                texture2DArray = CharacterDressHandler.Instance.manager.GetClothesTextureByName($"{animKey}", animLength);
                break;
            case GeneralEnum.Shoes:
                texture2DArray = CharacterDressHandler.Instance.manager.GetShoesTextureByName($"{animKey}", animLength);
                break;
            case GeneralEnum.Chef:
            case GeneralEnum.Waiter:
            case GeneralEnum.Accoutant:
            case GeneralEnum.Accost:
            case GeneralEnum.Beater:
                texture2DArray = GameItemsHandler.Instance.manager.GetItemsTextureByName($"{animKey}", animLength);
                break;
        }
        srBody.material.SetFloat($"_{animPro}Length", animLength);
        srBody.material.SetTexture($"_{animPro}Array", texture2DArray);
        return texture2DArray;
    }

    /// <summary>
    /// 设置动画
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetAnimForEquip(Animator animator, AnimatorOverrideController animatorForTarget, ItemsInfoBean itemsInfo)
    {
        if (animatorForTarget == null || itemsInfo == null || itemsInfo.id == 0)
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
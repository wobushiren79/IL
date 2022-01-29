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
        this.itemsInfoHat = itemsInfo;
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 119999)
        {
            itemsInfo = null;
        }
        if (srBody == null)
            return;
        Texture2D hatTEX;
        Color colorHair= srBody.material.GetColor("_ColorHair");
        if (itemsInfo == null || itemsInfo.icon_key == null)
        {
            colorHair = new Color(colorHair.r, colorHair.g, colorHair.b, 1);
            srBody.material.SetColor("_ColorHair", colorHair);
            hatTEX = null;
            characterEquipData.hatId = 0;
        }
        else
        {
            colorHair = new Color(colorHair.r, colorHair.g, colorHair.b, 0);
            srBody.material.SetColor("_ColorHair", colorHair);
            hatTEX = CharacterDressHandler.Instance.manager.GetHatTextureByName(itemsInfo.icon_key);
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
            srBody.material.SetTexture("_Hat", hatTEX);
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
        this.itemsInfoMask = itemsInfo;
        if (srBody == null)
            return;
        Texture2D maskTex = null;
        if (itemsInfo == null)
            maskTex = null;
        else
        {
            if (itemsInfo.id != 0)
                maskTex = CharacterDressHandler.Instance.manager.GetMaskTextureByName(itemsInfo.icon_key);
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
            srBody.material.SetTexture("_Mask", maskTex);
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
        this.itemsInfoClothes = itemsInfo;
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 219999)
        {
            itemsInfo = null;
        }
        if (srBody == null)
            return;
        Texture2D clothesTex = null;
        if (itemsInfo == null)
        {
            clothesTex = null;
        }
        else
        {
            if (itemsInfo.id != 0)
            {
                clothesTex = CharacterDressHandler.Instance.manager.GetClothesTextureByName(itemsInfo.icon_key);
            }
               
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.clothesId = itemsInfo.id;

        }
        if (clothesTex == null)
        {
            srBody.material.SetColor("_ColorClothes", new Color(0,0,0,0));
        }
        else
        {
            srBody.material.SetColor("_ColorClothes", new Color(1, 1, 1, 1));
            srBody.material.SetTexture("_Clothes", clothesTex);
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
        this.itemsInfoShoes = itemsInfo;
        //皇帝的新衣
        if (itemsInfo != null && itemsInfo.id == 319999)
        {
            itemsInfo = null;
        }
        if (srBody == null)
            return;
        Texture2D shoesTex;
        if (itemsInfo == null)
            shoesTex = null;
        else
        {
            shoesTex = CharacterDressHandler.Instance.manager.GetShoesTextureByName(itemsInfo.icon_key);
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
            srBody.material.SetTexture("_Shoes", shoesTex);
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
        this.itemsInfoHand = itemsInfo;
        if (srBody == null)
            return;
        Texture2D handTex;
        if (itemsInfo == null)
            handTex = null;
        else
        {
            handTex = GameItemsHandler.Instance.manager.GetItemsTextureByName(itemsInfo.icon_key);
            //设置装备数据
            if (characterEquipData == null)
                characterEquipData = new CharacterEquipBean();
            characterEquipData.handId = itemsInfo.id;
        }
        if (handTex == null)
        {
            srBody.material.SetColor("_ColorHand", new Color(0, 0, 0, 0));
        }
        else
        {
            srBody.material.SetColor("_ColorHand", new Color(1, 1, 1, 1));
            srBody.material.SetTexture("_Hand", handTex);

            if (itemsInfo.anim_length != 0)
            {
                srBody.material.SetFloat("_HandLength", itemsInfo.anim_length);
                Texture2DArray texture2DArray = new Texture2DArray(handTex.width,
                handTex.height, itemsInfo.anim_length, handTex.format, true, false);
                texture2DArray.filterMode = FilterMode.Point;
                texture2DArray.wrapMode = TextureWrapMode.Repeat;

                for (int i = 0; i < itemsInfo.anim_length; i++)
                {
                    Texture2D itemTex = GameItemsHandler.Instance.manager.GetItemsTextureByName($"{itemsInfo.icon_key}_{i}");
                    texture2DArray.SetPixels(itemTex.GetPixels(), i, 0);
                }
                texture2DArray.Apply(false);

                srBody.material.SetTexture("_HandArray", texture2DArray);
            }
             
        }
        //设置旋转角度
        if (itemsInfo != null && itemsInfo.rotation_angle != 0)
        {
            srBody.material.SetFloat ("_HandRotate", itemsInfo.rotation_angle);
        }
        else
        {
            srBody.material.SetFloat("_HandRotate", 45);
        }
        //设置动画
        //SetAnimForEquip(animForHand, aocForHand, itemsInfo);
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

    /// <summary>
    /// 设置装备动画
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetAnimForEquip(ItemsInfoBean itemsInfo)
    {
        if ( itemsInfo == null || itemsInfo.id == 0)
            return;
        //设置动画
        if (itemsInfo.anim_length <= 0)
            return;
        GeneralEnum itemType = itemsInfo.GetItemsType();
        switch (itemType)
        {
            case GeneralEnum.Mask:
            case GeneralEnum.Hat:
            case GeneralEnum.Clothes:
            case GeneralEnum.Shoes:
                break;
            case GeneralEnum.Chef:
            case GeneralEnum.Waiter:
            case GeneralEnum.Accoutant:
            case GeneralEnum.Accost:
            case GeneralEnum.Beater:
                break;
        }
    }

    protected ItemsInfoBean itemsInfoMask;
    protected ItemsInfoBean itemsInfoHat;
    protected ItemsInfoBean itemsInfoClothes;
    protected ItemsInfoBean itemsInfoShoes;
    protected ItemsInfoBean itemsInfoHand;


    protected int animMaskIndex = -1;
    protected int animHatIndex = -1;
    protected int animClothesIndex = -1;
    protected int animShoesIndex = -1;
    protected int animHandIndex = -1;

    protected float timeChange = 0;
    /// <summary>
    /// 处理装备动画
    /// </summary>
    //protected void HandleForEquipAnim()
    //{
    //    timeChange += Time.deltaTime;
    //    if (timeChange < 0.5f)
    //        return;
    //    if (animHandIndex >= 0 && itemsInfoHand != null)
    //    {
    //        Texture2D texAnim = GameItemsHandler.Instance.manager.GetItemsTextureByName(itemsInfoHand.icon_key);
    //        srBody.material.SetTexture("_Hand", texAnim);
    //    }
    //    timeChange = 0;
    //}

    //protected void HandleForEquipItemAnim(int index, ItemsInfoBean itemsInfo)
    //{
    //    if (index >= 0 && itemsInfo != null)
    //    {
    //        GeneralEnum itemType = itemsInfo.GetItemsType();
    //        Texture2D texAnim = null;
    //        switch (itemType)
    //        {
    //            case GeneralEnum.Mask:
    //                texAnim = CharacterDressHandler.Instance.manager.GetMaskTextureByName(itemsInfo.icon_key);
    //                break;
    //            case GeneralEnum.Hat:
    //                texAnim = CharacterDressHandler.Instance.manager.GetHatTextureByName(itemsInfo.icon_key);
    //                break;
    //            case GeneralEnum.Clothes:
    //                texAnim = CharacterDressHandler.Instance.manager.GetClothesTextureByName(itemsInfo.icon_key);
    //                break;
    //            case GeneralEnum.Shoes:
    //                texAnim = CharacterDressHandler.Instance.manager.GetShoesTextureByName(itemsInfo.icon_key);
    //                break;
    //            case GeneralEnum.Chef:
    //            case GeneralEnum.Waiter:
    //            case GeneralEnum.Accoutant:
    //            case GeneralEnum.Accost:
    //            case GeneralEnum.Beater:
    //                texAnim = GameItemsHandler.Instance.manager.GetItemsTextureByName(itemsInfo.icon_key);
    //                break;
    //        }
    //        srBody.material.SetTexture("_Hand", texAnim);
    //    }
    //}
}
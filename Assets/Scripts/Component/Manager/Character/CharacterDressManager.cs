using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;

public class CharacterDressManager : BaseManager
{
    //鞋子动画列表
    public Dictionary<string, AnimationClip> dicShoesAnim = new Dictionary<string, AnimationClip>();
    //衣服动画列表
    public Dictionary<string, AnimationClip> dicClothesAnim = new Dictionary<string, AnimationClip>();
    //帽子动画列表
    public Dictionary<string, AnimationClip> dicHatAnim = new Dictionary<string, AnimationClip>();
    //面具动画列表
    public Dictionary<string, AnimationClip> dicMaskAnim = new Dictionary<string, AnimationClip>();

    public AnimationClip GetAnimByName(GeneralEnum generalEnum, string name)
    {
        switch (generalEnum)
        {
            case GeneralEnum.Mask:
                return GetMaskAnimClipByName(name);
            case GeneralEnum.Hat:
                return GetHatAnimClipByName(name);
            case GeneralEnum.Clothes:
                return GetClothesAnimClipByName(name);
            case GeneralEnum.Shoes:
                return GetShoesAnimClipByName(name);
        }
        return null;
    }

    public Sprite GetSpriteByName(GeneralEnum generalEnum, string name)
    {
        switch (generalEnum)
        {
            case GeneralEnum.Mask:
                return GetMaskSpriteByName(name);
            case GeneralEnum.Hat:
                return GetHatSpriteByName(name);
            case GeneralEnum.Clothes:
                return GetClothesSpriteByName(name);
            case GeneralEnum.Shoes:
                return GetShoesSpriteByName(name);
        }
        return null;
    }

    /// <summary>
    /// 根据名字获取面具
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMaskSpriteByName(string name)
    {
        return IconHandler.Instance.GetDressSpriteDataByName(1, name);
    }

    public Texture2DArray GetMaskTextureByName(string name, int animLength)
    {
        return IconHandler.Instance.GetDressTexture(name, animLength, 1);
    }

    /// <summary>
    /// 根据名字获取面具动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetMaskAnimClipByName(string name)
    {
        return GetAnimClipByName(1, name);
    }

    /// <summary>
    /// 根据名字获取帽子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetHatSpriteByName(string name)
    {
        return IconHandler.Instance.GetDressSpriteDataByName(2, name);
    }

    public Texture2DArray GetHatTextureByName(string name, int animLength)
    {
        return IconHandler.Instance.GetDressTexture(name, animLength, 2);
    }

    /// <summary>
    /// 根据名字获取帽子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetHatAnimClipByName(string name)
    {
        return GetAnimClipByName(2, name);
    }

    /// <summary>
    /// 根据名字获取鞋子动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetShoesAnimClipByName(string name)
    {
        return GetAnimClipByName(3, name);
    }

    /// <summary>
    /// 根据名字获取衣服图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetClothesSpriteByName(string name)
    {
        return IconHandler.Instance.GetDressSpriteDataByName(3, name);
    }


    public Texture2DArray GetClothesTextureByName(string name, int animLength)
    {
        return IconHandler.Instance.GetDressTexture(name, animLength, 3);
    }

    /// <summary>
    /// 根据名字获取衣服动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetClothesAnimClipByName(string name)
    {
        return GetAnimClipByName(3, name);
    }

    /// <summary>
    /// 根据名字获取鞋子
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetShoesSpriteByName(string name)
    {
        return IconHandler.Instance.GetDressSpriteDataByName(4, name);
    }

    public Texture2DArray GetShoesTextureByName(string name, int animLength)
    {
        return IconHandler.Instance.GetDressTexture(name, animLength, 4);
    }

    protected AnimationClip GetAnimClipByName(int type, string name)
    {
        if (name == null)
            return null;
        Dictionary<string, AnimationClip> dicData = null;
        string typeName = "";
        switch (type)
        {
            case 1:
                dicData = dicMaskAnim;
                typeName = "Mask";
                break;
            case 2:
                dicData = dicHatAnim;
                typeName = "Hat";
                break;
            case 3:
                dicData = dicClothesAnim;
                typeName = "Clothes";
                break;
            case 4:
                dicData = dicShoesAnim;
                typeName = "Shoes";
                break;
        }

        return GetModelForAddressablesSync(dicData, $"Assets/Anim/Animation/Equip/{typeName}/{name}.anim");
    }
}
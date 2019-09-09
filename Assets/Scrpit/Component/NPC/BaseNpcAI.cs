using UnityEngine;
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
            ItemsInfoBean hatEquip= gameItemsManager.GetItemsById(characterBean.equips.hatId);
            characterDress.SetHat(hatEquip);

            ItemsInfoBean clothesEquip = gameItemsManager.GetItemsById(characterBean.equips.clothesId);
            characterDress.SetClothes(clothesEquip);

            ItemsInfoBean shoesEquip = gameItemsManager.GetItemsById(characterBean.equips.shoesId);
            characterDress.SetShoes(shoesEquip);
        }
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
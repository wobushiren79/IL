using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameBackpackCpt : BaseMonoBehaviour
{
    public Text tvName;
    public Image ivIcon;

    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;

    public void SetData(ItemsInfoBean infoBean)
    {
        SetIcon(infoBean.icon_key, infoBean.items_type);
        SetName(infoBean.name);
    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="itemType"></param>
    public void SetIcon(string iconKey,int itemType)
    {
        Sprite spIcon = null;
        switch (itemType)
        {
            case (int)GeneralEnum.Hat:
                spIcon= characterDressManager.GetHatSpriteByName(iconKey);
                break;
            case (int)GeneralEnum.Clothes:
                spIcon = characterDressManager.GetClothesSpriteByName(iconKey);
                break;
            case (int)GeneralEnum.Shoes:
                spIcon = characterDressManager.GetShoesSpriteByName(iconKey);
                break;
            case (int)GeneralEnum.Book:
            case (int)GeneralEnum.Cook:
                spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
                break;
        }
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }
}
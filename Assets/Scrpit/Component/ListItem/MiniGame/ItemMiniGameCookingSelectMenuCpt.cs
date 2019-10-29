using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameCookingSelectMenuCpt : ItemGameBaseCpt
{
    public InfoFoodPopupButton infoFoodPopup;

    public Text tvName;
    public Image ivIcon;

    private void Start()
    {
        InfoFoodPopupShow infoFoodPopupShow = GetUIManager<UIGameManager>().infoFoodPopup;
        infoFoodPopup.SetPopupShowView(infoFoodPopupShow);
    }

    public void SetData(MenuOwnBean menuOwn, MenuInfoBean menuInfo)
    {
        infoFoodPopup.SetData(menuOwn, menuInfo);
        SetName(menuInfo.name);
        SetIcon(menuInfo.icon_key);
    }

    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    public void SetIcon(string iconKey)
    {
        InnFoodManager innFoodManager = GetUIManager<UIGameManager>().innFoodManager;
        Sprite spFood = innFoodManager.GetFoodSpriteByName(iconKey);
        if (ivIcon != null)
        {
            ivIcon.sprite = spFood;
        }
    }
}
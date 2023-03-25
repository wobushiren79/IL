using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameStatisticsForMenuCpt : ItemBaseTextCpt
{
    public Text tvNumber;
    public Image ivLevel;
    public Text tvRate;
    public Image ivIconBackground;
    public Image ivNameBackground;
    public Image ivNumberBackground;
    public PopupFoodButton popupButton;

    public MenuOwnBean menuOwn;
    public MenuInfoBean menuInfo;

    public Sprite spBackLock;
    public Sprite spBackUnlock;

    public void SetData(MenuOwnBean menuOwn,MenuInfoBean menuInfo)
    {
        this.menuOwn = menuOwn;
        this.menuInfo = menuInfo;
        SetLevel();
        SetRate();
        SetBackground();
        if (menuOwn != null)
        {
            popupButton.SetData(menuOwn, menuInfo);
            SetName(menuInfo.name);
            SetIcon(InnFoodHandler.Instance.manager.GetFoodSpriteByName(menuInfo.icon_key),Color.white);
            SetNumber(menuOwn.sellNumber + "");
        }
        else
        {
            popupButton.ShowObj(false);
            ivNumberBackground.gameObject. SetActive(false);
            ivNameBackground.gameObject.SetActive(false);
            SetName("???");
            SetNumber("???");
            SetIcon(IconDataHandler.Instance.manager.GetIconSpriteByName("questionmark_1"), Color.white);
        }
    }

    public void SetBackground()
    {
        if (menuOwn == null)
        {
            ivBackground.sprite = spBackUnlock;
            ivIconBackground.sprite = spBackUnlock;
        }
        else
        {
            ivBackground.sprite = spBackLock;
            ivIconBackground.sprite = spBackLock;
        }
    }

    public void SetLevel()
    {
        if (menuOwn == null)
        {
            ivLevel.gameObject.SetActive(false);
        }
        else
        {
            ivLevel.gameObject.SetActive(true);
            Sprite spLevel= menuOwn.GetMenuLevelIcon();
            if (spLevel != null)
            {
                ivLevel.gameObject.SetActive(true);
                ivLevel.sprite = spLevel;
            }
            else
            {
                ivLevel.gameObject.SetActive(false);
            }
        }
    }

    public void SetRate()
    {
        if (menuOwn == null)
        {
            tvRate.gameObject.SetActive(false);
            tvRate.text = "";
        }
        else
        {
            tvRate.gameObject.SetActive(true);
            RarityEnumTools.GetRarityDetails(menuInfo.GetRarity(),out string rarityName,out Color rarityColor);
            tvRate.text= rarityName;
            tvRate.color = rarityColor;
        }
    }

    public void SetNumber(string number)
    {
        if (tvNumber != null)
            tvNumber.text = number;
    }
}
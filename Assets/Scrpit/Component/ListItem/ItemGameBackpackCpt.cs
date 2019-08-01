using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class ItemGameBackpackCpt : ItemGameBaseCpt, IPointerClickHandler, ItemsSelectionBox.ICallBack, DialogView.IDialogCallBack
{
    public Text tvName;
    public RectTransform rtIcon;
    public Image ivIcon;
    public InfoItemsPopupButton infoItemsPopup;

    public ItemsInfoBean itemsInfoBean;
    public ItemBean itemBean;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    public void Start()
    {
        rightClick.AddListener(new UnityAction(ButtonRightClick));
        if (infoItemsPopup != null)
            infoItemsPopup.SetPopupShowView(GetUIManager<UIGameManager>().infoItemsPopup);
    }

    public void SetData(ItemsInfoBean infoBean, ItemBean itemBean)
    {
        this.itemsInfoBean = infoBean;
        this.itemBean = itemBean;
        SetIcon(infoBean.icon_key, infoBean.items_type);
        SetName(infoBean.name);
        infoItemsPopup.SetData(infoBean, ivIcon.sprite);
    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="itemType"></param>
    public void SetIcon(string iconKey, int itemType)
    {
        CharacterDressManager characterDressManager= GetUIManager<UIGameManager>().characterDressManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;

        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        Sprite spIcon = null;
        switch (itemType)
        {
            case (int)GeneralEnum.Hat:
                spIcon = characterDressManager.GetHatSpriteByName(iconKey);
                offsetMin = new Vector2(-50, -100);
                offsetMax = new Vector2(50, 0);
                break;
            case (int)GeneralEnum.Clothes:
                spIcon = characterDressManager.GetClothesSpriteByName(iconKey);
                offsetMin = new Vector2(-50, -25);
                offsetMax = new Vector2(50, 75);
                break;
            case (int)GeneralEnum.Shoes:
                spIcon = characterDressManager.GetShoesSpriteByName(iconKey);
                offsetMin = new Vector2(-50, 0);
                offsetMax = new Vector2(50, 100);
                break;
            case (int)GeneralEnum.Book:
            case (int)GeneralEnum.Menu:
                spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
                break;
        }
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
        if (rtIcon != null)
        {
            rtIcon.offsetMin = offsetMin;
            rtIcon.offsetMax = offsetMax;
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public virtual void ButtonRightClick()
    {
        if (itemsInfoBean == null)
            return;
        ItemsSelectionBox selectionBox = GetUIManager<UIGameManager>().itemsSelectionBox;
        if (selectionBox != null)
            selectionBox.SetCallBack(this);
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Menu:
                selectionBox.Open(1);
                break;
            default:
                selectionBox.Open(0);
                break;
        }
    }

    #region 选择回调
    public virtual void SelectionUse(ItemsSelectionBox view)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;

        if (itemsInfoBean == null || itemBean == null || gameDataManager == null)
            return;
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Menu:
                //添加菜谱
                if (gameDataManager.gameData.AddFoodMenu(itemsInfoBean.add_id))
                {
                    RemoveItems();
                    toastView.ToastHint(ivIcon.sprite, GameCommonInfo.GetUITextById(1006));
                }
                else
                {
                    toastView.ToastHint(GameCommonInfo.GetUITextById(1007));
                };
                break;
            default:
                break;
        }
    }

    public virtual void SelectionDiscard(ItemsSelectionBox view)
    {
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        if (dialogManager == null || itemsInfoBean == null)
            return;
        DialogBean dialogBean = new DialogBean
        {
            content = string.Format(GameCommonInfo.GetUITextById(3001), itemsInfoBean.name)
        };
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    public virtual void SelectionEquip(ItemsSelectionBox view)
    {

    }

    public virtual void SelectionUnload(ItemsSelectionBox view)
    {

    }
    #endregion

    #region 删除确认回调
    public void Submit(DialogView dialogView)
    {
        RemoveItems();
    }

    public void Cancel(DialogView dialogView)
    {

    }
    #endregion

    /// <summary>
    /// 删除物品
    /// </summary>
    public void RemoveItems()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        gameObject.transform.DOLocalMove(new Vector3(0, 0), 0.2f).SetEase(Ease.InCirc).OnComplete(delegate
        {
            gameDataManager.gameData.itemsList.Remove(itemBean);
            Destroy(gameObject);
        });
    }


}
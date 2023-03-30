using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownStoreForGoodsCpt : ItemTownStoreCpt, DialogView.IDialogCallBack
{
    public GameObject objCook;
    public Text tvCook;
    public GameObject objSpeed;
    public Text tvSpeed;
    public GameObject objAccount;
    public Text tvAccount;
    public GameObject objCharm;
    public Text tvCharm;
    public GameObject objForce;
    public Text tvForce;
    public GameObject objLucky;
    public Text tvLucky;
    public GameObject objCookBook;
    public Text tvCookBook;
    public PopupItemsButton infoItemsPopup;

    public ItemsInfoBean itemsInfo;

    public override void RefreshUI()
    {
        if (itemsInfo.GetItemsType()== GeneralEnum.Menu)
        {
            //如果是菜单则不需要实时刷新
        }
        else
        {
            base.RefreshUI();
        }

    }
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="storeInfo"></param>
    public void SetData(StoreInfoBean storeInfo)
    {
        this.storeInfo = storeInfo;
        this.itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(storeInfo.mark_id);
        if (itemsInfo == null || storeInfo == null)
            return;
        //如果图标key没有则替换成itemInfo的图标
        string iconKey = storeInfo.icon_key;
        if (iconKey.IsNull())
            iconKey = itemsInfo.icon_key;

        SetIcon(itemsInfo, storeInfo.mark, storeInfo.mark_id);
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s,
            storeInfo.guild_coin,
            storeInfo.trophy_elementary, storeInfo.trophy_intermediate, storeInfo.trophy_advanced, storeInfo.trophy_legendary);
        SetName(itemsInfo.name);
        SetContent(itemsInfo.content);
        SetOwn();
        SetGetNumber(storeInfo.get_number);
        int cookBookNumber = itemsInfo.items_type == (int)GeneralEnum.Menu ? 1 : 0;
        SetAttribute(
            cookBookNumber,
            itemsInfo.add_cook,
            itemsInfo.add_speed,
            itemsInfo.add_account,
            itemsInfo.add_charm,
            itemsInfo.add_force,
            itemsInfo.add_lucky);
        SetPopupData( itemsInfo);
    }

    /// <summary>
    /// 设置弹出框内容
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetPopupData(ItemsInfoBean itemsInfo)
    {
         if (infoItemsPopup != null)
            infoItemsPopup.SetData(itemsInfo, ivIcon.sprite);
    }


    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(ItemsInfoBean itemsInfo, string mark, long markId)
    {
        Sprite spIcon = null;
        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);

        if (!mark.IsNull())
        {
            switch ((GeneralEnum)int.Parse(mark))
            {
                case GeneralEnum.Hat:
                    offsetMin = new Vector2(-50, -75);
                    offsetMax = new Vector2(50, 25);
                    break;
                case GeneralEnum.Clothes:
                    offsetMin = new Vector2(-50, -25);
                    offsetMax = new Vector2(50, 75);
                    break;
                case GeneralEnum.Shoes:
                    offsetMin = new Vector2(-50, 0);
                    offsetMax = new Vector2(50, 100);
                    break;
                default:
                    break;
            }
        }

        spIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, false);
        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
        if (rtIcon != null)
        {
            rtIcon.offsetMin = offsetMin;
            rtIcon.offsetMax = offsetMax;
        }
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="add_cook"></param>
    /// <param name="add_speed"></param>
    /// <param name="add_account"></param>
    /// <param name="add_charm"></param>
    /// <param name="add_force"></param>
    /// <param name="add_lucky"></param>
    public void SetAttribute(int add_book, int add_cook, int add_speed, int add_account, int add_charm, int add_force, int add_lucky)
    {
        if (objCook != null && add_cook == 0)
            objCook.SetActive(false);
        if (objSpeed != null && add_speed == 0)
            objSpeed.SetActive(false);
        if (objAccount != null && add_account == 0)
            objAccount.SetActive(false);
        if (objCharm != null && add_charm == 0)
            objCharm.SetActive(false);
        if (objForce != null && add_force == 0)
            objForce.SetActive(false);
        if (objLucky != null && add_lucky == 0)
            objLucky.SetActive(false);
        if (objCookBook != null && add_book == 0)
            objCookBook.SetActive(false);
        if (tvCook != null)
            tvCook.text = TextHandler.Instance.manager.GetTextById(1) + "+" + add_cook;
        if (tvSpeed != null)
            tvSpeed.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Speed) + "+" + add_speed;
        if (tvAccount != null)
            tvAccount.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Account) + "+" + add_account;
        if (tvCharm != null)
            tvCharm.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm) + "+" + add_charm;
        if (tvForce != null)
            tvForce.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Force) + "+" + add_force;
        if (tvLucky != null)
            tvLucky.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Lucky) + "+" + add_lucky;
        if (tvCookBook != null)
            tvCookBook.text = TextHandler.Instance.manager.GetTextById(7) + "+" + add_book;
    }

    public override void SetOwn()
    {
        if(itemsInfo.GetItemsType()== GeneralEnum.Menu)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            bool isHas= gameData.CheckHasMenu(itemsInfo.add_id);
            if (tvOwn != null)
                if (isHas)
                {
                    tvOwn.text = (TextHandler.Instance.manager.GetTextById(195));
                }
                else
                {
                    tvOwn.text = (TextHandler.Instance.manager.GetTextById(196));
                    tvOwn.color = Color.gray;
                }
        }
        else
        {
            base.SetOwn();
        }
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public override void OnClickSubmitBuy()
    {
        base.OnClickSubmitBuy();

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        if (storeInfo == null)
            return;

        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogType = DialogEnum.PickForNumber;
        dialogBean.callBack = this;
        PickForNumberDialogView dialogView = UIHandler.Instance.ShowDialog<PickForNumberDialogView>(dialogBean);
        dialogView.SetData(ivIcon.sprite, 999);
    }

    #region 提交回调
    public virtual void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        if (storeInfo == null)
            return;
        if (dialogView as PickForNumberDialogView)
        {
            PickForNumberDialogView pickForNumberDialog = dialogView as PickForNumberDialogView;
            long number = pickForNumberDialog.GetPickNumber();
            if (!gameData.HasEnoughMoney(storeInfo.price_l* number, storeInfo.price_m* number, storeInfo.price_s* number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
                return;
            }
            if (!gameData.HasEnoughGuildCoin(storeInfo.guild_coin * number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1012));
                return;
            }
            if (!gameData.HasEnoughTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1021));
                return;
            }

            gameData.PayMoney(storeInfo.price_l * number, storeInfo.price_m * number, storeInfo.price_s * number);
            gameData.PayGuildCoin(storeInfo.guild_coin * number);
            gameData.PayTrophy(storeInfo.trophy_elementary * number, storeInfo.trophy_intermediate * number, storeInfo.trophy_advanced * number, storeInfo.trophy_legendary * number);

            //加上获取数量
            int getNumber = 1;
            if (storeInfo.get_number != 0)
            {
                getNumber = storeInfo.get_number;
            }
            gameData.AddItemsNumber(storeInfo.mark_id, number * getNumber);

            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, string.Format(TextHandler.Instance.manager.GetTextById(1010), itemsInfo.name + "x" + (number * getNumber)));
            RefreshUI();
        }
    }

    public virtual void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}
﻿using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ItemGameMenuFoodCpt : ItemGameBaseCpt, IRadioButtonCallBack, DialogView.IDialogCallBack
{
    [Header("控件")]
    public Text tvName;
    public InfoFoodPopupButton pbFood;
    public Image ivFood;
    public GameObject objPriceS;
    public Text tvPriceS;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceL;
    public Text tvPriceL;
    public RadioButtonView rbShow;
    public Text tvShow;
    public InfoPromptPopupButton pbReputation;
    public Image ivReputation;
    public Text tvMenuRarity;

    public GameObject objResearch;
    public Button btResearch;
    public InfoPromptPopupButton popupForResearch;

    public GameObject objResearchCancel;
    public Button btResearchCancel;

    public Image ivBackground;

    public Color colorForSell;
    public Color colorForNoSell;
    public Color colorForNoIng;

    public Sprite spBackgroundInit;
    public Sprite spBackgroundStar;
    public Sprite spBackgroundMoon;
    public Sprite spBackgroundSun;

    public string stringForNoIng;

    [Header("数据")]
    public MenuOwnBean menuOwnData;
    public MenuInfoBean foodData;

    private void Awake()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (popupForResearch != null)
            popupForResearch.SetPopupShowView(uiGameManager.infoPromptPopup);

    }

    private void Start()
    {
        if (rbShow != null)
            rbShow.SetCallBack(this);
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (pbFood != null)
        {
            pbFood.SetPopupShowView(uiGameManager.infoFoodPopup);
        }
        if (pbReputation != null)
        {
            pbReputation.SetPopupShowView(uiGameManager.infoPromptPopup);
            pbReputation.SetContent(GameCommonInfo.GetUITextById(100));
        }
        if (btResearch != null)
            btResearch.onClick.AddListener(OnClickResearch);
        if (btResearchCancel != null)
            btResearchCancel.onClick.AddListener(OnClickResearchCancel);
        stringForNoIng = GameCommonInfo.GetUITextById(13001);
    }


    private void Update()
    {
        if (menuOwnData == null || foodData == null)
            return;

        //设置材料是否足够
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameDataManager.gameData.CheckCookFood(foodData))
        {
            if (menuOwnData.isSell)
            {
                tvShow.color = colorForSell;
            }
            else
            {
                tvShow.color = colorForNoSell;
            }
        }
        else
        {
            tvShow.color = colorForNoIng;
            tvShow.text = stringForNoIng;
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="menuOwn"></param>
    /// <param name="data"></param>
    public void SetData(MenuOwnBean menuOwn, MenuInfoBean data)
    {
        foodData = data;
        menuOwnData = menuOwn;
        //设置详细信息弹窗
        if (pbFood != null)
            pbFood.SetData(menuOwnData, foodData);
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();

        SetLevel(menuOwn.GetMenuLevel(), menuOwn.GetMenuLevelIcon(uiGameManager.iconDataManager));
        SetFoodIcon(foodData.icon_key);
        SetName(data.name);
        SetSellStatus(menuOwnData);
        menuOwn.GetPrice(data, out long priceL, out long priceM, out long priceS);
        SetPrice(priceL, priceM, priceS);
        SetResearch(menuOwn.GetMenuStatus());
        SetResearchPopup(menuOwn, data);
        SetMenuClass(data.GetRarity());
    }

    /// <summary>
    /// 设置菜单等级
    /// </summary>
    /// <param name="menuClass"></param>
    public void SetMenuClass(RarityEnum menuRarity)
    {
        RarityEnumTools.GetRarityDetails(menuRarity, out string rarityName, out Color rarityColor);
        tvMenuRarity.text = rarityName;
        tvMenuRarity.color = rarityColor;
    }

    /// <summary>
    /// 设置研究弹出框内容
    /// </summary>
    /// <param name="menuOwn"></param>
    /// <param name="data"></param>
    public void SetResearchPopup(MenuOwnBean menuOwn, MenuInfoBean data)
    {
        if (popupForResearch == null)
            return;
        string content = GameCommonInfo.GetUITextById(285);

        SortedList<IngredientsEnum, long> listIng = menuOwn.GetResearchIngredients(data);
        if (listIng != null && listIng.Count != 0)
        {
            content += (" " + GameCommonInfo.GetUITextById(286) + "\n");
        }
        foreach (var item in listIng)
        {
            content += (IngredientsEnumTools.GetIngredientName(item.Key) + "x" + item.Value + " ");
        }
        popupForResearch.SetContent(content);
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    /// <param name="level"></param>
    /// <param name="spIcon"></param>
    public void SetLevel(LevelTypeEnum level, Sprite spIcon)
    {
        pbReputation.gameObject.SetActive(true);
        ivReputation.sprite = spIcon;
        if (level == LevelTypeEnum.Init)
        {
            tvName.color = new Color(0, 0, 0, 1f);
            ivBackground.sprite = spBackgroundInit;
            pbReputation.gameObject.SetActive(false);
        }
        else if (level == LevelTypeEnum.Star)
        {
            tvName.color = new Color(0, 0.75f, 0, 1f);
            ivBackground.sprite = spBackgroundStar;
        }
        else if (level == LevelTypeEnum.Moon)
        {
            tvName.color = new Color(0.1f, 0.75f, 0.95f, 1f);
            ivBackground.sprite = spBackgroundMoon;
        }
        else if (level == LevelTypeEnum.Sun)
        {
            tvName.color = new Color(1, 0.85f, 0.2f, 1f);
            ivBackground.sprite = spBackgroundSun;
        }
        else
        {
            tvName.color = new Color(0, 0, 0, 1f);
            ivBackground.sprite = spBackgroundInit;
        }
    }

    /// <summary>
    /// 设置研究
    /// </summary>
    /// <param name="menuStatus"></param>
    public void SetResearch(ResearchStatusEnum menuStatus)
    {
        objResearch.SetActive(false);
        objResearchCancel.SetActive(false);
        switch (menuStatus)
        {
            case ResearchStatusEnum.Normal:
                break;
            case ResearchStatusEnum.WaitForResearch:
                objResearch.SetActive(true);
                break;
            case ResearchStatusEnum.Researching:
                objResearchCancel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 设置食物图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetFoodIcon(string iconKey)
    {
        InnFoodManager innFoodManager = GetUIManager<UIGameManager>().innFoodManager;
        Sprite spFood = innFoodManager.GetFoodSpriteByName(iconKey);
        //食物图标设置
        if (ivFood != null)
        {
            ivFood.sprite = spFood;
        }
    }

    /// <summary>
    /// 设置售卖状态
    /// </summary>
    /// <param name="menuOwn"></param>
    public void SetSellStatus(MenuOwnBean menuOwn)
    {
        //菜单是否买卖设置
        if (menuOwn.isSell)
        {
            if (rbShow != null)
                rbShow.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            if (tvShow != null)
                tvShow.text = GameCommonInfo.GetUITextById(2021);
        }
        else
        {
            if (rbShow != null)
                rbShow.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            if (tvShow != null)
                tvShow.text = GameCommonInfo.GetUITextById(2020);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        //名字设置
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="data"></param>
    public void SetPrice(long price_l, long price_m, long price_s)
    {
        //价格设置
        if (price_l == 0)
        {
            if (objPriceL != null)
                objPriceL.SetActive(false);
        }
        else
        {
            if (objPriceL != null)
                objPriceL.SetActive(true);
            if (tvPriceL != null)
                tvPriceL.text = price_l + "";
        }
        if (price_m == 0)
        {
            if (objPriceM != null)
                objPriceM.SetActive(false);
        }
        else
        {
            if (objPriceM != null)
                objPriceM.SetActive(true);
            if (tvPriceM != null)
                tvPriceM.text = price_m + "";
        }
        if (price_s == 0)
        {
            if (objPriceS != null)
                objPriceS.SetActive(false);
        }
        else
        {
            if (objPriceS != null)
                objPriceS.SetActive(true);
            if (tvPriceS != null)
                tvPriceS.text = price_s + "";
        }
    }

    /// <summary>
    /// 点击研究
    /// </summary>
    public void OnClickResearch()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //首先判断客栈等级是否足够
        if (!menuOwnData.CheckCanResearch(uiGameManager.gameDataManager.gameData, out string failStr))
        {
            ToastHandler.Instance.ToastHint(failStr);
            return;
        }

        DialogBean dialogData = new DialogBean
        {
            title = GameCommonInfo.GetUITextById(3071)
        };
        PickForCharacterDialogView pickForCharacterDialog = DialogHandler.Instance.CreateDialog<PickForCharacterDialogView>(DialogEnum.PickForCharacter, this, dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);
        //设置排出人员 （老板和没有在休息的员工）
        List<CharacterBean> listCharacter = uiGameManager.gameDataManager.gameData.listWorkerCharacter;
        List<string> listExpelIds = new List<string>();
        listExpelIds.Add(uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.characterId);
        foreach (CharacterBean itemData in listCharacter)
        {
            //休息日 排出不是工作或者休息的
            if (uiGameManager.gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
            {
                if (itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work)
                {
                    listExpelIds.Add(itemData.baseInfo.characterId);
                }
            }
            //工作日 排出除休息中的所有员工
            else if (uiGameManager.gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Work)
            {
                if (itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest)
                {
                    listExpelIds.Add(itemData.baseInfo.characterId);
                }
            }
        }
        pickForCharacterDialog.SetExpelCharacter(listExpelIds);
    }

    /// <summary>
    /// 点击研究取消
    /// </summary>
    public void OnClickResearchCancel()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean
        {
            content = GameCommonInfo.GetUITextById(3072)
        };
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    #region 售卖状态回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStatus)
    {
        if (view == rbShow && tvShow != null)
        {
            UIGameManager uiGameManager = GetUIManager<UIGameManager>();
            AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
            switch (buttonStatus)
            {
                case RadioButtonView.RadioButtonStatus.Selected:
                    tvShow.text = GameCommonInfo.GetUITextById(2021);
                    menuOwnData.isSell = true;
                    break;
                case RadioButtonView.RadioButtonStatus.Unselected:
                    tvShow.text = GameCommonInfo.GetUITextById(2020);
                    menuOwnData.isSell = false;
                    break;
            }
        }
    }
    #endregion

    #region 弹出框回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (dialogView as PickForCharacterDialogView)
        {
            //角色选择
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            List<CharacterBean> listPickCharacter = pickForCharacterDialog.GetPickCharacter();
            if (!CheckUtil.ListIsNull(listPickCharacter))
            {

                SortedList<IngredientsEnum, long> listIng = menuOwnData.GetResearchIngredients(foodData);
                bool hasEnoughIng = true;
                foreach (var itemIng in listIng)
                {
                    if (!uiGameManager.gameDataManager.gameData.HasEnoughIng(itemIng.Key, itemIng.Value))
                    {
                        hasEnoughIng = false;
                        break;
                    }
                }
                if (!hasEnoughIng)
                {
                    ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1045));
                    return;
                }
                foreach (var itemIng in listIng)
                {
                    uiGameManager.gameDataManager.gameData.DeductIng(itemIng.Key, itemIng.Value);
                }
                //开始研究
                menuOwnData.StartResearch(listPickCharacter);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1201), listPickCharacter[0].baseInfo.name, foodData.name);
                ToastHandler.Instance.ToastHint(ivFood.sprite, toastStr);
            }
        }
        else
        {
            //如果是研究中则询问
            if (menuOwnData.GetMenuStatus() == ResearchStatusEnum.Researching)
            {
                //普通弹窗（取消研究）
                menuOwnData.CancelResearch(uiGameManager.gameDataManager.gameData);
            }
            else
            {

            }
        }
        //重新设置数据
        SetData(menuOwnData, foodData);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion 
}
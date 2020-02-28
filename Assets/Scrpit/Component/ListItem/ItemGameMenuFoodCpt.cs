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

    public GameObject objResearch;
    public Button btResearch;
    public GameObject objResearchCancel;
    public Button btResearchCancel;

    [Header("数据")]
    public Sprite spReputation1;
    public Sprite spReputation2;
    public Sprite spReputation3;

    public MenuOwnBean menuOwnData;
    public MenuInfoBean foodData;

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
    }

    private void Update()
    {
        if (menuOwnData == null || foodData == null)
            return;

        //设置材料是否足够
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameDataManager.gameData.CheckCookFood(foodData))
        {
            tvName.color = Color.black;
            tvShow.color = Color.black;
        }
        else
        {
            tvName.color = Color.red;
            tvShow.color = Color.red;
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
        SetLevel(menuOwn.menuLevel, menuOwn.GetMenuLevelIcon(uiGameManager.iconDataManager));
        SetFoodIcon(foodData.icon_key);
        SetName(data.name);
        SetSellStatus(menuOwnData);
        SetPrice(foodData.price_l, foodData.price_m, foodData.price_s);
        SetResearch(menuOwn.GetMenuStatus());
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    /// <param name="level"></param>
    /// <param name="spIcon"></param>
    public void SetLevel(int level,Sprite spIcon)
    { 
        if (level==0)
        {
            pbReputation.gameObject.SetActive(false);
        }
        else
        {
            pbReputation.gameObject.SetActive(true);
            ivReputation.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置研究
    /// </summary>
    /// <param name="menuStatus"></param>
    public void SetResearch(MenuStatusEnum menuStatus)
    {
        objResearch.SetActive(false);
        objResearchCancel.SetActive(false);
        switch (menuStatus)
        {
            case MenuStatusEnum.Normal:
                break;
            case MenuStatusEnum.WaitForResearch:
                objResearch.SetActive(true);
                break;
            case MenuStatusEnum.Researching:
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
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean
        {
            title = GameCommonInfo.GetUITextById(3071)
        };
        PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)uiGameManager.dialogManager.CreateDialog(DialogEnum.PickForCharacter, this, dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);
        //设置排出人员 （老板和没有在休息的员工）
        List<CharacterBean> listCharacter = uiGameManager.gameDataManager.gameData.listWorkerCharacter;
        pickForCharacterDialog.SetExpelCharacter(uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.characterId);
    }

    /// <summary>
    /// 点击研究取消
    /// </summary>
    public void OnClickResearchCancel()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        uiGameManager.audioHandler.PlaySound( AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean
        {
            content = GameCommonInfo.GetUITextById(3072)
        };
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
    }

    #region 售卖状态回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStatus)
    {
        if (view == rbShow && tvShow != null)
        {
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
        if (dialogView as PickForCharacterDialogView)
        {
            //角色选择
            PickForCharacterDialogView pickForCharacterDialog = (PickForCharacterDialogView)dialogView;
            List<CharacterBean> listPickCharacter = pickForCharacterDialog.GetPickCharacter();
            if (!CheckUtil.ListIsNull(listPickCharacter))
            {
                //开始研究
                menuOwnData.StartResearch(listPickCharacter);
            }
        }
        else
        {
            //普通弹窗（取消研究）
            UIGameManager uiGameManager = GetUIManager<UIGameManager>();
            menuOwnData.CancelResearch(uiGameManager.gameDataManager.gameData);
        }
        //重新设置数据
        SetData(menuOwnData, foodData);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion 
}
﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITownBeautySalon : UIBaseOne, IRadioGroupCallBack, ItemTownBeautySalonCpt.ICallBack, ColorView.ICallBack
{
    public RadioGroupView rgType;
    public CharacterUICpt characterUI;
    public PriceShowView priceShowView;
    public Text tvName;
    public Image ivSex;

    public ColorView cvHair;
    public ColorView cvMouth;
    public ColorView cvEye;
    public ColorView cvSkin;

    public Button btSubmit;
    public Button btSelectCharacter;
    public Button btClear;

    public ScrollGridVertical gridVertical;

    public Sprite spMan;
    public Sprite spWoman;

    public string selectHair;
    public string selectMouth;
    public string selectEye;
    public string selectSkin;

    protected long priceL;
    protected long priceM;
    protected long priceS;

    protected CharacterBean characterData;
    protected List<string> listSelectData = new List<string>();
    protected BodyTypeEnum bodyType = BodyTypeEnum.Hair;


    public override void Awake()
    {
        base.Awake();
        if (btSubmit)
            btSubmit.onClick.AddListener(OnClickForSubmit);
        if (btSelectCharacter)
            btSelectCharacter.onClick.AddListener(OnClickForSelectCharacter);
        if (btClear)
            btClear.onClick.AddListener(OnClickForClear);
        if (rgType)
            rgType.SetCallBack(this);
        if (gridVertical)
            gridVertical.AddCellListener(OnCellForItem);

        if (cvHair)
            cvHair.SetCallBack(this);
        if (cvMouth)
            cvMouth.SetCallBack(this);
        if (cvEye)
            cvEye.SetCallBack(this);
        if (cvSkin)
            cvSkin.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        characterData = gameData.userCharacter;
        ClearData();
        rgType.SetPosition(0, true);
    }


    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        SetCharacterData();
        SetPrice();
        SetName(characterData.baseInfo.name);
        SetSex(characterData.body.sex);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        tvName.text = name;
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
    public void SetSex(int sex)
    {
        if (sex == 1)
        {
            ivSex.sprite = spMan;
        }
        else
        {
            ivSex.sprite = spWoman;
        }
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    public void SetPrice()
    {
        priceL = 0;
        priceM = 0;
        priceS = 0;

        if (!selectHair.IsNull())
        {
            priceM += 1;
        }
        if (!selectMouth.IsNull())
        {
            priceM += 10;
        }
        if (!selectEye.IsNull())
        {
            priceL += 10;
        }

        if (!selectSkin.IsNull())
        {
            priceL += 100;
        }
        priceShowView.SetPrice(1, priceL, priceM, priceS);
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    public void SetCharacterData()
    {
        if (characterUI)
        {
            characterUI.SetCharacterData(characterData.body, new CharacterEquipBean());
            if (selectHair.IsNull())
            {
                cvHair.gameObject.SetActive(false);
            }
            else
            {
                cvHair.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Hair, selectHair, cvHair.GetColor());
            }

            if (selectMouth.IsNull())
            {
                cvMouth.gameObject.SetActive(false);
            }
            else
            {
                cvMouth.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Mouth, selectMouth, cvMouth.GetColor());
            }

            if (selectEye.IsNull())
            {
                cvEye.gameObject.SetActive(false);
            }
            else
            {
                cvEye.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Eye, selectEye, cvEye.GetColor());
            }

            if (selectSkin.IsNull())
            {
                cvSkin.gameObject.SetActive(false);
            }
            else
            {
                cvSkin.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Skin, selectSkin, cvSkin.GetColor());
            }
        }
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {
        cvHair.SetData(1, 1, 1);
        cvEye.SetData(1, 1, 1);
        cvMouth.SetData(1, 1, 1);
        cvSkin.SetData(1, 1, 1);
        selectHair = "";
        selectMouth = "";
        selectEye = "";
        selectSkin = "";
        RefreshUI();
    }

    public void OnClickForSelectCharacter()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.PickForCharacter;
        dialogData.callBack = this;
        PickForCharacterDialogView pickForCharacterDialog = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);

    }

    public void OnClickForClear()
    {
        ClearData();
    }

    public void OnClickForSubmit()
    {
        if (selectHair.IsNull()
            && selectEye.IsNull()
            && selectMouth.IsNull()
            && selectSkin.IsNull())
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(7005));
            return;
        }
        DialogBean dialogData = new DialogBean();
        string price = "";
        if (priceL != 0)
        {
            price += priceL + TextHandler.Instance.manager.GetTextById(16);
        }
        if (priceM != 0)
        {
            price += priceM + TextHandler.Instance.manager.GetTextById(17);
        }
        if (priceS != 0)
        {
            price += priceS + TextHandler.Instance.manager.GetTextById(18);
        }
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3104), price, characterData.baseInfo.name);
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    /// <summary>
    /// 列表回调
    /// </summary>
    /// <param name="cell"></param>
    public void OnCellForItem(ScrollGridCell cell)
    {
        ItemTownBeautySalonCpt itemTownBeautySalon = cell.GetComponent<ItemTownBeautySalonCpt>();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        bool isLock = false;
        //获取美颜庄老板的好感
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(130011);
        int level = characterFavorability.GetFavorabilityLevel();
        float unlockNumber = (listSelectData.Count / 6f) * (level + 1);
        if (cell.index < unlockNumber)
        {
            isLock = false;
        }
        else
        {
            isLock = true;
        }

        itemTownBeautySalon.SetData(bodyType, listSelectData[cell.index], isLock);
        itemTownBeautySalon.SetCallBack(this);
    }


    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        switch (rbview.name)
        {
            case "Hair":
                this.bodyType = BodyTypeEnum.Hair;
                listSelectData = CharacterBodyHandler.Instance.manager.GetAllHair();
                break;
            case "Eye":
                this.bodyType = BodyTypeEnum.Eye;
                listSelectData = CharacterBodyHandler.Instance.manager.GetAllEye();
                break;
            case "Mouth":
                this.bodyType = BodyTypeEnum.Mouth;
                listSelectData = CharacterBodyHandler.Instance.manager.GetAllMouth();
                break;
            case "Skin":
                this.bodyType = BodyTypeEnum.Skin;
                listSelectData.Clear();
                listSelectData.Add("Def");
                listSelectData.AddRange(CharacterBodyHandler.Instance.manager.GetAllTrunk());
                break;
        }
        gridVertical.SetCellCount(listSelectData.Count);
        gridVertical.RefreshAllCells();
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion


    #region  弹窗回调
    public override void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        base.Submit(dialogView, dialogBean);
        if (dialogView as PickForCharacterDialogView)
        {
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            List<CharacterBean> listPickCharacter = pickForCharacterDialog.GetPickCharacter();
            if (!listPickCharacter.IsNull())
            {
                characterData = listPickCharacter[0];
                ClearData();
            }
        }
        else
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (gameData.HasEnoughMoney(priceL, priceM, priceS))
            {
                gameData.PayMoney(priceL, priceM, priceS);
                if (!selectSkin.IsNull())
                {
                    characterData.body.skin = selectSkin;
                    characterData.body.skinColor = cvSkin.GetColorBean();
                }
                if (!selectEye.IsNull())
                {
                    characterData.body.eye = selectEye;
                    characterData.body.eyeColor = cvEye.GetColorBean();
                }
                if (!selectMouth.IsNull())
                {
                    characterData.body.mouth = selectMouth;
                    characterData.body.mouthColor = cvMouth.GetColorBean();
                }
                if (!selectHair.IsNull())
                {
                    characterData.body.hair = selectHair;
                    characterData.body.hairColor = cvHair.GetColorBean();
                }
                ClearData();
            }
            else
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
            }

        }
    }

    public override void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        base.Cancel(dialogView, dialogBean);
    }
    #endregion

    #region 物品选择回调
    public void SelectItem(BodyTypeEnum type, string data, long priceL, long priceM, long priceS)
    {
        switch (type)
        {
            case BodyTypeEnum.Hair:
                selectHair = data;
                break;
            case BodyTypeEnum.Eye:
                selectEye = data;
                break;
            case BodyTypeEnum.Mouth:
                selectMouth = data;
                break;
            case BodyTypeEnum.Skin:
                selectSkin = data;
                break;
        }
        RefreshUI();
    }
    #endregion

    #region 颜色选择回调
    public void ColorChange(ColorView colorView, float r, float g, float b)
    {
        if (colorView == cvHair)
        {

        }
        else if (colorView == cvEye)
        {

        }
        else if (colorView == cvMouth)
        {

        }
        else if (colorView == cvSkin)
        {

        }
        SetCharacterData();
    }
    #endregion
}
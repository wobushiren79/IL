using UnityEngine;
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
        characterData = uiGameManager.gameDataManager.gameData.userCharacter;
        ClearData();
        rgType.SetPosition(0, true);
    }


    public override void RefreshUI()
    {
        base.RefreshUI();
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

        if (!CheckUtil.StringIsNull(selectHair))
        {
            priceM += 1;
        }
        if (!CheckUtil.StringIsNull(selectMouth))
        {
            priceM += 10;
        }
        if (!CheckUtil.StringIsNull(selectEye))
        {
            priceL += 10;
        }

        if (!CheckUtil.StringIsNull(selectSkin))
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
            if (CheckUtil.StringIsNull(selectHair))
            {
                cvHair.gameObject.SetActive(false);
            }
            else
            {
                cvHair.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Hair, selectHair, cvHair.GetColor());
            }

            if (CheckUtil.StringIsNull(selectMouth))
            {
                cvMouth.gameObject.SetActive(false);
            }
            else
            {
                cvMouth.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Mouth, selectMouth, cvMouth.GetColor());
            }

            if (CheckUtil.StringIsNull(selectEye))
            {
                cvEye.gameObject.SetActive(false);
            }
            else
            {
                cvEye.gameObject.SetActive(true);
                characterUI.SetCharacterData(BodyTypeEnum.Eye, selectEye, cvEye.GetColor());
            }

            if (CheckUtil.StringIsNull(selectSkin))
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
        PickForCharacterDialogView pickForCharacterDialog = DialogHandler.Instance.CreateDialog<PickForCharacterDialogView>(DialogEnum.PickForCharacter, this, dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);

    }

    public void OnClickForClear()
    {
        ClearData();
    }

    public void OnClickForSubmit()
    {
        if (CheckUtil.StringIsNull(selectHair)
            && CheckUtil.StringIsNull(selectEye)
            && CheckUtil.StringIsNull(selectMouth)
            && CheckUtil.StringIsNull(selectSkin))
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(7005));
            return;
        }
        DialogBean dialogData = new DialogBean();
        string price = "";
        if (priceL != 0)
        {
            price += priceL + GameCommonInfo.GetUITextById(16);
        }
        if (priceM != 0)
        {
            price += priceM + GameCommonInfo.GetUITextById(17);
        }
        if (priceS != 0)
        {
            price += priceS + GameCommonInfo.GetUITextById(18);
        }
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3104), price, characterData.baseInfo.name);
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    /// <summary>
    /// 列表回调
    /// </summary>
    /// <param name="cell"></param>
    public void OnCellForItem(ScrollGridCell cell)
    {
        ItemTownBeautySalonCpt itemTownBeautySalon = cell.GetComponent<ItemTownBeautySalonCpt>();
        bool isLock = false;
        //获取美颜庄老板的好感
        CharacterFavorabilityBean characterFavorability = uiGameManager.gameDataManager.gameData.GetCharacterFavorability(130011);
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
            if (!CheckUtil.ListIsNull(listPickCharacter))
            {
                characterData = listPickCharacter[0];
                ClearData();
            }
        }
        else
        {
            GameDataBean gameData = uiGameManager.gameDataManager.gameData;
            if (gameData.HasEnoughMoney(priceL, priceM, priceS))
            {
                gameData.PayMoney(priceL, priceM, priceS);
                if (!CheckUtil.StringIsNull(selectSkin))
                {
                    characterData.body.skin = selectSkin;
                    characterData.body.skinColor = cvSkin.GetColorBean();
                }
                if (!CheckUtil.StringIsNull(selectEye))
                {
                    characterData.body.eye = selectEye;
                    characterData.body.eyeColor = cvEye.GetColorBean();
                }
                if (!CheckUtil.StringIsNull(selectMouth))
                {
                    characterData.body.mouth = selectMouth;
                    characterData.body.mouthColor = cvMouth.GetColorBean();
                }
                if (!CheckUtil.StringIsNull(selectHair))
                {
                    characterData.body.hair = selectHair;
                    characterData.body.hairColor = cvHair.GetColorBean();
                }
                ClearData();
            }
            else
            {
                ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1005));
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
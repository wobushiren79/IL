using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public partial class UIMainCreate : BaseUIComponent,
    IRadioGroupCallBack,
    ColorView.ICallBack,
    SelectView.ICallBack,
    DialogView.IDialogCallBack

{
    public ColorView ui_ColorHair;
    public ColorView ui_ColorEye;
    public ColorView ui_ColorMouth;

    public List<IconBean> listSelectHair;
    public List<IconBean> listSelectEye;
    public List<IconBean> listSelectMouth;

    public List<ItemsInfoBean> listSelectHat;
    public List<ItemsInfoBean> listSelectClothes;
    public List<ItemsInfoBean> listSelectShoes;

    public override void Awake()
    {
        base.Awake();
        ui_ColorHair = ui_ItemMainCreateColorAndSelectView_Hair.GetComponent<ColorView>();
        ui_ColorEye = ui_ItemMainCreateColorAndSelectView_Eye.GetComponent<ColorView>();
        ui_ColorMouth = ui_ItemMainCreateColorAndSelectView_Mouth.GetComponent<ColorView>();
    }

    private void Start()
    {
        InitData();
        if (ui_Sex != null)
            ui_Sex.SetCallBack(this);
        if (ui_Skin != null)
            ui_Skin.SetCallBack(this);
        if (ui_ColorHair != null)
            ui_ColorHair.SetCallBack(this);
        if (ui_ItemMainCreateColorAndSelectView_Hair != null)
        {
            ui_ItemMainCreateColorAndSelectView_Hair.SetListData(listSelectHair.Count);
            ui_ItemMainCreateColorAndSelectView_Hair.SetCallBack(this);
        }
        if (ui_ColorEye != null)
            ui_ColorEye.SetCallBack(this);
        if (ui_ItemMainCreateColorAndSelectView_Eye != null)
        {
            ui_ItemMainCreateColorAndSelectView_Eye.SetListData(listSelectEye.Count);
            ui_ItemMainCreateColorAndSelectView_Eye.SetCallBack(this);
        }
        if (ui_ColorMouth != null)
            ui_ColorMouth.SetCallBack(this);
        if (ui_ItemMainCreateColorAndSelectView_Mouth != null)
        {
            ui_ItemMainCreateColorAndSelectView_Mouth.SetListData(listSelectMouth.Count);
            ui_ItemMainCreateColorAndSelectView_Mouth.SetCallBack(this);
        }

        if (ui_ItemMainCreateEquipView_Hat != null)
        {
            ui_ItemMainCreateEquipView_Hat.SetListData(listSelectHat.Count);
            ui_ItemMainCreateEquipView_Hat.SetCallBack(this);
        }
        if (ui_ItemMainCreateEquipView_Clothes != null)
        {
            ui_ItemMainCreateEquipView_Clothes.SetListData(listSelectClothes.Count);
            ui_ItemMainCreateEquipView_Clothes.SetCallBack(this);
        }
        if (ui_ItemMainCreateEquipView_Shoes != null)
        {
            ui_ItemMainCreateEquipView_Shoes.SetListData(listSelectShoes.Count);
            ui_ItemMainCreateEquipView_Shoes.SetCallBack(this);
        }

        ui_ItemMainCreateColorAndSelectView_Hair.SetPosition(0);
        ui_ItemMainCreateColorAndSelectView_Eye.SetPosition(0);
        ui_ItemMainCreateColorAndSelectView_Mouth.SetPosition(0);
        ui_ItemMainCreateEquipView_Hat.SetPosition(0);
        ui_ItemMainCreateEquipView_Clothes.SetPosition(0);
        ui_ItemMainCreateEquipView_Shoes.SetPosition(0);
        ui_Sex.SetPosition(0,true);
        ui_Skin.SetData(1,1,1);
        ui_ColorHair.SetData(1, 1, 1);
        ui_ColorEye.SetData(1,1,1);
        ui_ColorMouth.SetData(1, 1, 1);
    }


    public override void OnClickForButton(Button viewButton)
    {
        if (viewButton == ui_BTBack)
        {
            OpenStartUI();
        }
        else if (viewButton == ui_BTCreate)
        {
            CreateNewGame();
        }
        else if (viewButton == ui_RandomCharacter)
        {
            OnClickRandomCharacter();
        }
    }
    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit();
    }

    public void InitData()
    {
        //初始化可选择头型数据
        List<Sprite> listHair = CharacterBodyHandler.Instance.manager.GetCreateCharacterHair();
        listSelectHair = new List<IconBean>();
        foreach (Sprite itemSprite in listHair)
        {
            IconBean iconBean = new IconBean();
            iconBean.key = itemSprite.name;
            iconBean.key = iconBean.key.Replace("(Clone)", "");
            iconBean.value = itemSprite;
            listSelectHair.Add(iconBean);
        }
        //初始化可选择眼睛
        List<Sprite> listEye = CharacterBodyHandler.Instance.manager.GetCreateCharacterEye();
        listSelectEye = new List<IconBean>();
        foreach (Sprite itemSprite in listEye)
        {
            IconBean iconBean = new IconBean();
            iconBean.key = itemSprite.name;
            iconBean.key = iconBean.key.Replace("(Clone)", "");
            iconBean.value = itemSprite;
            listSelectEye.Add(iconBean);
        }
        //初始化可选择嘴巴
        List<Sprite> listMouth = CharacterBodyHandler.Instance.manager.GetCreateCharacterMouth();
        listSelectMouth = new List<IconBean>();
        foreach (Sprite itemSprite in listMouth)
        {
            IconBean iconBean = new IconBean();
            iconBean.key = itemSprite.name;
            iconBean.key = iconBean.key.Replace("(Clone)", "");
            iconBean.value = itemSprite;
            listSelectMouth.Add(iconBean);
        }
        //初始化帽子
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.HatForLevel0, out string hatListStr);
        long[] listHat = hatListStr.SplitForArrayLong(',');
        listSelectHat = GameItemsHandler.Instance.manager.GetItemsById(listHat);
        listSelectHat.Insert(0, new ItemsInfoBean());
        //初始化衣服
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ClothesForLevel0, out string clothesListStr);
        long[] listClothes= clothesListStr.SplitForArrayLong(',');
        listSelectClothes = GameItemsHandler.Instance.manager.GetItemsById(listClothes);
        listSelectClothes.Insert(0, new ItemsInfoBean());
        //初始化鞋子
        GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ShoesForLevel0, out string shoesListStr);
        long[] listShoes = shoesListStr.SplitForArrayLong(',');
        listSelectShoes = GameItemsHandler.Instance.manager.GetItemsById(listShoes);
        listSelectShoes.Insert(0, new ItemsInfoBean());
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        if (ui_CreateContent != null)
            ui_CreateContent.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutExpo);
        if (ui_BTCreate != null)
            ui_BTCreate.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack);
        if (ui_BTBack != null)
            ui_BTBack.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.1f);
    }

    /// <summary>
    /// 创建新游戏
    /// </summary>
    public void CreateNewGame()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

        if (ui_ETInnName.text.IsNull())
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1000));
            return;
        }
        if (ui_ETUserName.text.IsNull())
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1001));
            return;
        }
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.manager.GetTextById(3012);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    /// <summary>
    /// 返回开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
    }

    /// <summary>
    /// 随机按钮
    /// </summary>
    public void OnClickRandomCharacter ()
    {
        ui_Skin.RandomData();
        ui_ColorHair.RandomData();
        ui_ItemMainCreateColorAndSelectView_Hair.RandomSelect();


        ui_ColorEye.RandomData();
        ui_ItemMainCreateColorAndSelectView_Eye.RandomSelect();

        ui_ColorMouth.RandomData();
        ui_ItemMainCreateColorAndSelectView_Mouth.RandomSelect();

        ui_ItemMainCreateEquipView_Clothes.RandomSelect();
        ui_ItemMainCreateEquipView_Shoes.RandomSelect();

        ui_AttributesChange.RandomData();
    }


    #region 性别回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

        if (position == 0)
        {
            ui_CharacterUI.characterBodyData.SetSex(SexEnum.Man);
            ui_CharacterUI.SetSex(SexEnum.Man, null);
        }
        else
        {
            ui_CharacterUI.characterBodyData.SetSex(SexEnum.Woman);
            ui_CharacterUI.SetSex(SexEnum.Woman, null);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion

    #region 颜色回调
    public void ColorChange(ColorView colorView, float r, float g, float b)
    {
        if (colorView == ui_Skin)
        {
            Color colorSkin = ui_Skin.GetColor();
            ui_CharacterUI.characterBodyData.skinColor = new ColorBean(colorSkin);
            ui_CharacterUI.SetSkin(colorSkin);
        }
        else if (colorView == ui_ColorHair)
        {
            Color colorHair = ui_ColorHair.GetColor();
            ui_CharacterUI.characterBodyData.hairColor = new ColorBean(colorHair);
            ui_CharacterUI.SetHairColor(colorHair);
        }
        else if (colorView == ui_ColorEye)
        {
            Color colorEye = ui_ColorEye.GetColor();
            ui_CharacterUI.characterBodyData.eyeColor = new ColorBean(colorEye);
            ui_CharacterUI.SetEyeColor(colorEye);
        }
        else if (colorView == ui_ColorMouth)
        {
            Color colorMouth = ui_ColorMouth.GetColor();
            ui_CharacterUI.characterBodyData.mouthColor = new ColorBean(colorMouth);
            ui_CharacterUI.SetMouthColor(colorMouth);
        }

    }
    #endregion

    #region 选择回调
    public void ChangeSelectPosition(SelectView selectView, int position)
    {
        if (selectView == ui_ItemMainCreateColorAndSelectView_Hair)
        {
            string hairName = listSelectHair[position].key;
            ui_CharacterUI.characterBodyData.hair = hairName;
            ui_CharacterUI.SetHair(hairName, ui_ColorHair.GetColor());
        }
        else if (selectView == ui_ItemMainCreateColorAndSelectView_Eye)
        {
            string eyeName = listSelectEye[position].key;
            ui_CharacterUI.characterBodyData.eye = eyeName;
            ui_CharacterUI.SetEye(eyeName, ui_ColorEye.GetColor());
        }
        else if (selectView == ui_ItemMainCreateColorAndSelectView_Mouth)
        {
            string mouthName = listSelectMouth[position].key;
            ui_CharacterUI.characterBodyData.mouth = mouthName;
            ui_CharacterUI.SetMouth(mouthName, ui_ColorMouth.GetColor());
        }
        else if (selectView == ui_ItemMainCreateEquipView_Hat)
        {
            long hatId = listSelectHat[position].id;
            ui_CharacterUI.characterEquipData.hatId = hatId;
            ui_CharacterUI.SetHat(hatId, ui_ColorHair.GetColor());
        }
        else if (selectView == ui_ItemMainCreateEquipView_Clothes)
        {
            long clothesId = listSelectClothes[position].id;
            ui_CharacterUI.characterEquipData.clothesId = clothesId;
            ui_CharacterUI.SetClothes(clothesId);
        }
        else if (selectView == ui_ItemMainCreateEquipView_Shoes)
        {
            long shoesId = listSelectShoes[position].id;
            ui_CharacterUI.characterEquipData.shoesId = shoesId;
            ui_CharacterUI.SetShoes(shoesId);
        }
    }
    #endregion

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = new GameDataBean();
        gameData.innAttributes.innName = ui_ETInnName.text;

        gameData.userCharacter = new CharacterBean();
        gameData.userCharacter.baseInfo.name = ui_ETUserName.text;
        gameData.userCharacter.body = ui_CharacterUI.characterBodyData;
        gameData.userCharacter.equips = ui_CharacterUI.characterEquipData;
        ui_AttributesChange.GetAttributesPoints(out int cook, out int speed, out int account, out int charm, out int force, out int lucky);
        gameData.userCharacter.attributes.cook = cook;
        gameData.userCharacter.attributes.speed = speed;
        gameData.userCharacter.attributes.account = account;
        gameData.userCharacter.attributes.charm = charm;
        gameData.userCharacter.attributes.force = force;
        gameData.userCharacter.attributes.lucky = lucky;
        gameData.userCharacter.attributes.life = 50;

        GameDataHandler.Instance.manager.CreateGameData(gameData);
        GameDataHandler.Instance.manager.GetGameDataByUserId(gameData.userId);
        UIHandler.Instance.CloseAllUI();
        GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameInnScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
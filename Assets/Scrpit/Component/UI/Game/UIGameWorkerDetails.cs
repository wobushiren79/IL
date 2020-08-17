using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class UIGameWorkerDetails : UIGameComponent, IRadioGroupCallBack
{
    [Header("控件")]
    public CharacterUICpt characterUICpt;

    public ItemGameBackpackEquipCpt equipHand;
    public ItemGameBackpackEquipCpt equipHat;
    public ItemGameBackpackEquipCpt equipClothes;
    public ItemGameBackpackEquipCpt equipShoes;

    //幻化
    public ItemGameBackpackEquipCpt equipTFHand;
    public ItemGameBackpackEquipCpt equipTFHat;
    public ItemGameBackpackEquipCpt equipTFClothes;
    public ItemGameBackpackEquipCpt equipTFShoes;

    public Text tvLoyal;
    public CharacterAttributeView characterAttributeView;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;
    public Image ivSex;

    public ItemGameWorkerDetailsWorkerCpt detailsForChef;
    public ItemGameWorkerDetailsWorkerCpt detailsForWaiter;
    public ItemGameWorkerDetailsWorkerCpt detailsForAccounting;
    public ItemGameWorkerDetailsWorkerCpt detailsForAccost;
    public ItemGameWorkerDetailsWorkerCpt detailsForBeater;

    public RadioGroupView rgWorkerTitle;
    public UIGameWorkerDetailsGeneralInfo generalInfo;
    public UIGameWorkerDetailsChefInfo workerChefInfo;
    public UIGameWorkerDetailsWaiterInfo workerWaiterInfo;
    public UIGameWorkerDetailsAccountantInfo workerAccountantInfo;
    public UIGameWorkerDetailsAccostInfo workerAccostInfo;
    public UIGameWorkerDetailsBeaterInfo workerBeaterInfo;
    public UIGameWorkerDetailsSkillInfo workerSkillInfo;
    public UIGameWorkerDetailsBookInfo workerBookInfo;

    public Button btBack;

    [Header("数据")]
    public CharacterBean characterData;
    public Sprite spSexMan;
    public Sprite spSexWoman;

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);

        if (rgWorkerTitle != null)
        {
            rgWorkerTitle.SetCallBack(this);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="workerType"></param>
    public void InitDataByWorker(string name)
    {
        if (characterData == null)
            return;

        workerChefInfo.Close();
        workerWaiterInfo.Close();
        workerAccountantInfo.Close();
        workerAccostInfo.Close();
        workerBeaterInfo.Close();
        workerSkillInfo.Close();
        workerBookInfo.Close();
        generalInfo.Close();
        if (name.Contains("General"))
        {
            generalInfo.Open();
            generalInfo.SetData(characterData);
        }
       else if (name.Contains("Skill"))
        {
            workerSkillInfo.Open();
            workerSkillInfo.SetData(characterData.attributes.listSkills);
        }
        else if (name.Contains("Book"))
        {
            workerBookInfo.Open();
            workerBookInfo.SetData(characterData.attributes.listLearnBook);
        }
        else if (name.Contains("Chef"))
        {
            InnFoodManager innFoodManager = GetUIManager<UIGameManager>().innFoodManager;
            workerChefInfo.Open();
            workerChefInfo.SetData(innFoodManager, characterData.baseInfo.chefInfo);
        }
        else if (name.Contains("Waiter"))
        {
            workerWaiterInfo.Open();
            workerWaiterInfo.SetData(characterData.baseInfo.waiterInfo);
        }
        else if (name.Contains("Accountant"))
        {
            workerAccountantInfo.Open();
            workerAccountantInfo.SetData(characterData.baseInfo.accountantInfo);
        }
        else if (name.Contains("Accost"))
        {
            workerAccostInfo.Open();
            workerAccostInfo.SetData(characterData.baseInfo.accostInfo);
        }
        else if (name.Contains("Beater"))
        {
            workerBeaterInfo.Open();
            workerBeaterInfo.SetData(characterData.baseInfo.beaterInfo);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (characterData == null)
            return;
        SetLoyal(characterData.attributes.loyal);
        SetSex(characterData.body.sex);
        SetAttributes(characterData);
        SetEquip(characterData.equips);
        SetWorkerInfo(characterData.baseInfo);
        characterUICpt.SetCharacterData(characterData.body, characterData.equips);
        rgWorkerTitle.SetPosition(0, false);
        InitDataByWorker("General");
    }

    public void OpenWorkUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    /// <summary>
    /// 设置忠诚
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(int loyal)
    {
        if (tvLoyal != null)
        {
            tvLoyal.text = loyal + "";
        }
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
    public void SetSex(int sex)
    {
        if (ivSex == null)
            return;
        if (sex == 1)
        {
            ivSex.sprite = spSexMan;
        }
        else if (sex == 2)
        {
            ivSex.sprite = spSexWoman;
        }
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="characterEquip"></param>
    public void SetEquip(CharacterEquipBean equips)
    {

        //装备物品刷新
        equipHand.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.handId), null);
        equipHat.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.hatId), null);
        equipClothes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.clothesId), null);
        equipShoes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.shoesId), null);

        equipTFHand.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.handTFId), null);
        equipTFHat.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.hatTFId), null);
        equipTFClothes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.clothesTFId), null);
        equipTFShoes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(equips.shoesTFId), null);

    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    /// <param name="characterEquip"></param>
    public void SetAttributes(CharacterBean characterData)
    {
        characterData.GetAttributes(GetUIManager<UIGameManager>().gameItemsManager,
            out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "：" + selfAttributes.cook + (equipAttributes.cook == 0 ? "" : "+" + equipAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "：" + selfAttributes.speed + (equipAttributes.speed == 0 ? "" : "+" + equipAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "：" + selfAttributes.account + (equipAttributes.account == 0 ? "" : "+" + equipAttributes.account);
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "：" + selfAttributes.charm + (equipAttributes.charm == 0 ? "" : "+" + equipAttributes.charm);
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "：" + selfAttributes.force + (equipAttributes.force == 0 ? "" : "+" + equipAttributes.force);
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "：" + selfAttributes.lucky + (equipAttributes.lucky == 0 ? "" : "+" + equipAttributes.lucky);
        if (characterAttributeView != null)
            characterAttributeView.SetData(totalAttributes.cook, totalAttributes.speed, totalAttributes.account, totalAttributes.charm, totalAttributes.force, totalAttributes.lucky);
    }

    /// <summary>
    /// 设置职业数据
    /// </summary>
    public void SetWorkerInfo(CharacterBaseBean characterBase)
    {
        if (characterBase == null)
            return;
        if (detailsForChef != null)
            detailsForChef.SetData(WorkerEnum.Chef, characterBase.chefInfo);
        if (detailsForWaiter != null)
            detailsForWaiter.SetData(WorkerEnum.Waiter, characterBase.waiterInfo);
        if (detailsForAccounting != null)
            detailsForAccounting.SetData(WorkerEnum.Accountant, characterBase.accountantInfo);
        if (detailsForAccost != null)
            detailsForAccost.SetData(WorkerEnum.Accost, characterBase.accostInfo);
        if (detailsForBeater != null)
            detailsForBeater.SetData(WorkerEnum.Beater, characterBase.beaterInfo);
    }

    /// <summary>
    /// 设置装备图标
    /// </summary>
    /// <param name="iv"></param>
    /// <param name="spIcon"></param>
    private void SetEquipSprite(Image iv, Sprite spIcon)
    {
        if (spIcon != null)
        {
            iv.sprite = spIcon;
            iv.color = new Color(1, 1, 1, 1);
        }
        else
            iv.color = new Color(1, 1, 1, 0);
    }


    #region 数据类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        InitDataByWorker(rbview.name);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}
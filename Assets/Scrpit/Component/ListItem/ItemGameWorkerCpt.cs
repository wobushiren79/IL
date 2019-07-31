using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameWorkerCpt : ItemGameBaseCpt, IRadioButtonCallBack
{
    [Header("控件")]
    public Text tvName;
    public InfoPromptPopupButton pbName;

    public Text tvPrice;
    public InfoPromptPopupButton pbPrice;

    public Text tvLoyal;
    public InfoPromptPopupButton pbLoyal;

    public Text tvSpeed;
    public InfoPromptPopupButton pbSpeed;
    public Text tvAccount;
    public InfoPromptPopupButton pbAccount;
    public Text tvCharm;
    public InfoPromptPopupButton pbCharm;
    public Text tvCook;
    public InfoPromptPopupButton pbCook;
    public Text tvForce;
    public InfoPromptPopupButton pbForce;
    public Text tvLucky;
    public InfoPromptPopupButton pbLucky;

    public Button btEquip;
    public Button btFire;

    public RadioButtonView rbAccounting;
    public RadioButtonView rbChef;
    public RadioButtonView rbWaiter;
    public RadioButtonView rbAccost;
    public RadioButtonView rbBeater;

    [Header("数据")]
    public CharacterUICpt characterUICpt;
    public CharacterBean characterData;

    private void Start()
    {
        UIGameManager uIGameManager = GetUIManager<UIGameManager>();
        if (pbName != null)
        {
            pbName.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbName.SetContent(GameCommonInfo.GetUITextById(11001));
        }
        if (pbPrice != null)
        {
            pbPrice.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbPrice.SetContent(GameCommonInfo.GetUITextById(11002));
        }   
        if (pbLoyal != null)
        {
            pbLoyal.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbLoyal.SetContent(GameCommonInfo.GetUITextById(11003));
        }
        if (pbCook != null)
        {
            pbCook.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbCook.SetContent(GameCommonInfo.GetUITextById(1));
        }  
        if (pbSpeed != null)
        {
            pbSpeed.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbSpeed.SetContent(GameCommonInfo.GetUITextById(2));
        }   
        if (pbAccount != null)
        {
            pbAccount.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbAccount.SetContent(GameCommonInfo.GetUITextById(3));
        }   
        if (pbCharm != null)
        {
            pbCharm.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbCharm.SetContent(GameCommonInfo.GetUITextById(4));
        }
        if (pbForce != null)
        {
            pbForce.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbForce.SetContent(GameCommonInfo.GetUITextById(5));
        }
        if (pbLucky != null)
        {
            pbLucky.SetPopupShowView(uIGameManager.InfoPromptPopup);
            pbLucky.SetContent(GameCommonInfo.GetUITextById(6));
        }


        if (rbAccounting != null)
            rbAccounting.SetCallBack(this);
        if (rbChef != null)
            rbChef.SetCallBack(this);
        if (rbWaiter != null)
            rbWaiter.SetCallBack(this);
        if (rbAccost != null)
            rbAccost.SetCallBack(this);
        if (rbBeater != null)
            rbBeater.SetCallBack(this);

        if (btEquip != null)
            btEquip.onClick.AddListener(OpenEquipUI);
        if (btFire != null)
            btFire.onClick.AddListener(FireWorker);
    }

    public void SetData(CharacterBean data)
    {
        if (data == null)
            return;
        characterData = data;
        if (characterData.baseInfo != null)
        {
            CharacterBaseBean characterBase = characterData.baseInfo;
            SetName(characterBase.name);
            SetPrice(characterBase.priceS, characterBase.priceM, characterBase.priceL);
            SetWork(characterBase.isChef, characterBase.isWaiter, characterBase.isAccounting, characterBase.isBeater, characterBase.isAccost);
        }
        if (characterData.attributes != null)
        {
            CharacterAttributesBean characterAttributes = characterData.attributes;
            SetLoyal(characterAttributes.loyal);
            SetAttributes(characterData.attributes, characterData.equips);
        }
        if (data.body != null && data.equips != null)
            characterUICpt.SetCharacterData(data.body, data.equips);
    }

    /// <summary>
    /// 打开装备UI
    /// </summary>
    public void OpenEquipUI()
    {
        if (uiComponent != null)
        {
            UIGameEquip uiequip = (UIGameEquip)GetUIManager().GetUIByName(EnumUtil.GetEnumName(UIEnum.GameEquip));
            uiequip.SetCharacterData(characterData);
            uiComponent.uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameEquip));  
        }
    }

    /// <summary>
    /// 开除该员工
    /// </summary>
    public void FireWorker()
    {

    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    public void SetAttributes(CharacterAttributesBean characterAttributes, CharacterEquipBean characterEquip)
    {
        CharacterAttributesBean extraAttributes = new CharacterAttributesBean();
        if (uiComponent!=null&&GetUIManager<UIGameManager>().gameItemsManager != null && characterEquip != null)
        {
            GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
            extraAttributes = characterEquip.GetEquipAttributes(gameItemsManager);
        }
        if (tvCook != null)
            tvCook.text = extraAttributes.cook + characterAttributes.cook + "";
        if (tvSpeed != null)
            tvSpeed.text = extraAttributes.speed + characterAttributes.speed + "";
        if (tvAccount != null)
            tvAccount.text = extraAttributes.account+ characterAttributes.account + "";
        if (tvCharm != null)
            tvCharm.text = extraAttributes.charm+characterAttributes.charm + "";
        if (tvForce != null)
            tvForce.text = extraAttributes.force+characterAttributes.force + "";
        if (tvLucky != null)
            tvLucky.text = extraAttributes.lucky+ characterAttributes.lucky + "";
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName == null)
            return;
        tvName.text = name;
    }

    /// <summary>
    /// 设置工资
    /// </summary>
    /// <param name="priceS"></param>
    /// <param name="priceM"></param>
    /// <param name="priceL"></param>
    public void SetPrice(long priceS, long priceM, long priceL)
    {
        if (tvPrice == null)
            return;
        tvPrice.text = priceS + " / 天";
    }

    /// <summary>
    /// 设置忠诚度
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(float loyal)
    {
        if (tvLoyal == null)
            return;
        tvLoyal.text = loyal + "";
    }

    /// <summary>
    ///  设置工作
    /// </summary>
    /// <param name="isChef"></param>
    /// <param name="isWaiter"></param>
    /// <param name="isAccounting"></param>
    /// <param name="isBeater"></param>
    /// <param name="isAccost"></param>
    public void SetWork(bool isChef, bool isWaiter, bool isAccounting, bool isBeater, bool isAccost)
    {
        if (rbAccounting != null)
        {
            if (isAccounting)
                rbAccounting.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            else
                rbAccounting.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
        if (rbChef != null)
        {
            if (isChef)
                rbChef.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            else
                rbChef.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
        if (rbWaiter != null)
        {
            if (isWaiter)
                rbWaiter.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            else
                rbWaiter.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
        if (rbAccost != null)
        {
            if (isAccost)
                rbAccost.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            else
                rbAccost.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
        if (rbBeater != null)
        {
            if (isBeater)
                rbBeater.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            else
                rbBeater.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
    }

    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStates buttonStates)
    {
        if (characterData == null || characterData.baseInfo == null)
            return;
        CharacterBaseBean characterBase = characterData.baseInfo;
        if (view == rbAccounting)
        {
            characterBase.isAccounting = (buttonStates == RadioButtonView.RadioButtonStates.Selected) ? true : false;
        }
        else if (view == rbWaiter)
        {
            characterBase.isWaiter = (buttonStates == RadioButtonView.RadioButtonStates.Selected) ? true : false;
        }
        else if (view == rbChef)
        {
            characterBase.isChef = (buttonStates == RadioButtonView.RadioButtonStates.Selected) ? true : false;
        }
        else if (view == rbAccost)
        {
            characterBase.isAccost = (buttonStates == RadioButtonView.RadioButtonStates.Selected) ? true : false;
        }
        else if (view == rbBeater)
        {
            characterBase.isBeater = (buttonStates == RadioButtonView.RadioButtonStates.Selected) ? true : false;
        }
        if (GetUIManager<UIGameManager>().innHandler != null)
            GetUIManager<UIGameManager>().innHandler.InitWorker();
    }
}
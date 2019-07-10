using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameWorkerCpt : BaseMonoBehaviour, IRadioButtonCallBack
{
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

    public CharacterUICpt characterUICpt;
    public CharacterBean characterData;
    public InnHandler innHandler;
    public GameItemsManager gameItemsManager;
    public BaseUIManager baseUIManager;
    private void Start()
    {
        if (pbName != null)
            pbName.SetContent(GameCommonInfo.GetUITextById(11001));
        if (pbPrice != null)
            pbPrice.SetContent(GameCommonInfo.GetUITextById(11002));
        if (pbLoyal != null)
            pbLoyal.SetContent(GameCommonInfo.GetUITextById(11003));

        if (pbSpeed != null)
            pbSpeed.SetContent(GameCommonInfo.GetUITextById(11004));
        if (pbAccount != null)
            pbAccount.SetContent(GameCommonInfo.GetUITextById(11005));
        if (pbCharm != null)
            pbCharm.SetContent(GameCommonInfo.GetUITextById(11006));
        if (pbCook != null)
            pbCook.SetContent(GameCommonInfo.GetUITextById(11007));
        if (pbForce != null)
            pbForce.SetContent(GameCommonInfo.GetUITextById(11008));
        if (pbLucky != null)
            pbLucky.SetContent(GameCommonInfo.GetUITextById(11009));

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
        if (baseUIManager != null)
        {
            baseUIManager.OpenUIAndCloseOtherByName("Equip");
            UIGameEquip uiComponent = (UIGameEquip)baseUIManager.GetUIByName("Equip");
            uiComponent.SetCharacterData(characterData);
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
        if (gameItemsManager != null && characterEquip != null)
        {
            extraAttributes = characterEquip.GetEquipAttributes(gameItemsManager);
        }
        if (tvCook != null)
            tvCook.text = characterAttributes.cook + ""+ (extraAttributes.cook == 0 ? "" : "+" + extraAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = characterAttributes.speed + (extraAttributes.speed == 0 ? "" : "+" + extraAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = characterAttributes.account + (extraAttributes.account == 0 ? "" : "+" + extraAttributes.account);
        if (tvCharm != null)
            tvCharm.text = characterAttributes.charm + (extraAttributes.charm == 0 ? "" : "+" + extraAttributes.charm);
        if (tvForce != null)
            tvForce.text = characterAttributes.force + (extraAttributes.force == 0 ? "" : "+" + extraAttributes.force);
        if (tvLucky != null)
            tvLucky.text = characterAttributes.lucky + (extraAttributes.lucky == 0 ? "" : "+" + extraAttributes.lucky);
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
        if (innHandler != null)
            innHandler.InitWorker();
    }
}
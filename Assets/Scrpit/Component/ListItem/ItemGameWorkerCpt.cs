using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemGameWorkerCpt : ItemGameBaseCpt, IRadioButtonCallBack, DialogView.IDialogCallBack
{
    [Header("控件")]
    public Text tvName;
    public InfoPromptPopupButton pbName;

    public Text tvPrice;
    public InfoPromptPopupButton pbPrice;

    public Text tvLoyal;
    public InfoPromptPopupButton pbLoyal;

    public Image ivSex;

    public Text tvStatus;

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
    public Button btDetails;
    public Button btGift;
    public Button btFire;


    public RadioButtonView rbChef;
    public InputField etPriorityChef;
    public RadioButtonView rbWaiter;
    public InputField etPriorityWaiter;
    public RadioButtonView rbAccounting;
    public InputField etPriorityAccounting;
    public RadioButtonView rbAccost;
    public InputField etPriorityAccost;
    public RadioButtonView rbBeater;
    public InputField etPriorityBeater;

    public InfoCharacterPopupButton infoCharacterPopup;

    [Header("数据")]
    public CharacterUICpt characterUICpt;
    public CharacterBean characterData;

    protected InfoPromptPopupShow infoPromptPopup;
    protected AudioHandler audioHandler;
    protected DialogManager dialogManager;
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected GameTimeHandler gameTimeHandler;
    protected ToastManager toastManager;
    private void Awake()
    {
        gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        infoPromptPopup = GetUIManager<UIGameManager>().infoPromptPopup;
        audioHandler = GetUIManager<UIGameManager>().audioHandler;
        dialogManager = GetUIManager<UIGameManager>().dialogManager;
        gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        toastManager = GetUIManager<UIGameManager>().toastManager;
    }

    private void Start()
    {
        if (pbName != null)
        {
            pbName.SetPopupShowView(infoPromptPopup);
            pbName.SetContent(GameCommonInfo.GetUITextById(61));
        }
        if (pbPrice != null)
        {
            pbPrice.SetPopupShowView(infoPromptPopup);
            pbPrice.SetContent(GameCommonInfo.GetUITextById(11002));
        }
        if (pbLoyal != null)
        {
            pbLoyal.SetPopupShowView(infoPromptPopup);
            pbLoyal.SetContent(GameCommonInfo.GetUITextById(11003));
        }
        if (pbCook != null)
        {
            pbCook.SetPopupShowView(infoPromptPopup);
            pbCook.SetContent(GameCommonInfo.GetUITextById(1));
        }
        if (pbSpeed != null)
        {
            pbSpeed.SetPopupShowView(infoPromptPopup);
            pbSpeed.SetContent(GameCommonInfo.GetUITextById(2));
        }
        if (pbAccount != null)
        {
            pbAccount.SetPopupShowView(infoPromptPopup);
            pbAccount.SetContent(GameCommonInfo.GetUITextById(3));
        }
        if (pbCharm != null)
        {
            pbCharm.SetPopupShowView(infoPromptPopup);
            pbCharm.SetContent(GameCommonInfo.GetUITextById(4));
        }
        if (pbForce != null)
        {
            pbForce.SetPopupShowView(infoPromptPopup);
            pbForce.SetContent(GameCommonInfo.GetUITextById(5));
        }
        if (pbLucky != null)
        {
            pbLucky.SetPopupShowView(infoPromptPopup);
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
        if (etPriorityChef != null)
            etPriorityChef.onValueChanged.AddListener(PriorityChangeForChef);
        if (etPriorityWaiter != null)
            etPriorityWaiter.onValueChanged.AddListener(PriorityChangeForWaiter);
        if (etPriorityAccounting != null)
            etPriorityAccounting.onValueChanged.AddListener(PriorityChangeForAccounting);
        if (etPriorityAccost != null)
            etPriorityAccost.onValueChanged.AddListener(PriorityChangeForAccost);
        if (etPriorityBeater != null)
            etPriorityBeater.onValueChanged.AddListener(PriorityChangeForBeater);

        if (btEquip != null)
            btEquip.onClick.AddListener(OpenEquipUI);
        if (btDetails != null)
            btDetails.onClick.AddListener(OpenDeitalsUI);
        if (btFire != null)
            btFire.onClick.AddListener(FireWorker);
        if (btGift != null)
            btGift.onClick.AddListener(SendGift);

        if (infoCharacterPopup != null)
        {
            infoCharacterPopup.SetData(characterData);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetData(CharacterBean data)
    {
        if (data == null)
            return;
        characterData = data;
        if (characterData.baseInfo != null)
        {
            CharacterBaseBean characterBase = characterData.baseInfo;
            SetName(characterBase.name);

            SetPrice(characterBase.priceS, characterBase.priceM, characterBase.priceL);
            SetWork(
                characterBase.chefInfo.isWorking,
                characterBase.waiterInfo.isWorking,
                characterBase.accountantInfo.isWorking, 
                characterBase.accostInfo.isWorking,
                characterBase.beaterInfo.isWorking );
            SetPriority(
                characterBase.chefInfo.priority,
                characterBase.waiterInfo.priority,
                characterBase.accountantInfo.priority,
                characterBase.accostInfo.priority,
                characterBase.beaterInfo.priority);

            WorkerStatusEnum workerStatus = characterBase.GetWorkerStatus( out string workerStatusStr);
            SetStatus(workerStatus, workerStatusStr);
        }
        if (characterData.attributes != null)
        {
            CharacterAttributesBean characterAttributes = characterData.attributes;
            SetLoyal(characterAttributes.loyal);
            SetAttributes(characterData.attributes, characterData.equips);
        }
        if (data.body != null && data.equips != null)
        {
            characterUICpt.SetCharacterData(data.body, data.equips);
            SetSex(data.body.sex);
        }
        //如果是用户，则不能解雇 也不能送礼
        if (data == gameDataManager.gameData.userCharacter)
        {
            if (btFire != null)
                btFire.gameObject.SetActive(false);
            if (btGift != null)
                btGift.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 打开装备UI
    /// </summary>
    public void OpenEquipUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (uiComponent != null)
        {
            UIGameEquip uiequip = (UIGameEquip)GetUIManager().GetUIByName(EnumUtil.GetEnumName(UIEnum.GameEquip));
            uiequip.SetCharacterData(characterData);
            uiComponent.uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameEquip));
        }
    }

    /// <summary>
    /// 打开详情页
    /// </summary>
    public void OpenDeitalsUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (uiComponent != null)
        {
            UIGameWorkerDetails uiWorkerDetails = (UIGameWorkerDetails)GetUIManager().GetUIByName(EnumUtil.GetEnumName(UIEnum.GameWorkerDetails));
            uiWorkerDetails.SetCharacterData(characterData);
            uiComponent.uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorkerDetails));
        }
    }

    /// <summary>
    /// 开除该员工
    /// </summary>
    public void FireWorker()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1082));
        }
        else
        {
            DialogBean dialogData = new DialogBean();
            dialogData.content = string.Format(GameCommonInfo.GetUITextById(3063), characterData.baseInfo.name);
            dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
        }
    }

    /// <summary>
    ///  送礼
    /// </summary>
    public void SendGift()
    {
        DialogBean dialogData = new DialogBean();
        // dialogData.content = string.Format(GameCommonInfo.GetUITextById(3063), characterData.baseInfo.name);
        PickForItemsDialogView dialogView = (PickForItemsDialogView)dialogManager.CreateDialog(DialogEnum.PickForItems, this, dialogData);
        dialogView.SetData(null, PopupItemsSelection.SelectionTypeEnum.Gift);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    public void SetAttributes(CharacterAttributesBean characterAttributes, CharacterEquipBean characterEquip)
    {

        characterData.GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);

        if (tvCook != null)
            tvCook.text = totalAttributes.cook + "";
        if (tvSpeed != null)
            tvSpeed.text = totalAttributes.speed + "";
        if (tvAccount != null)
            tvAccount.text = totalAttributes.account + "";
        if (tvCharm != null)
            tvCharm.text = totalAttributes.charm + "";
        if (tvForce != null)
            tvForce.text = totalAttributes.force + "";
        if (tvLucky != null)
            tvLucky.text = totalAttributes.lucky + "";
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
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
    public void SetSex(int sex)
    {
        IconDataManager iconDataManager = ((UIGameManager)uiComponent.uiManager).iconDataManager;
        if (ivSex != null)
        {
            if (sex == 1)
            {
                ivSex.sprite = iconDataManager.GetIconSpriteByName("sex_man");
            }
            else if (sex == 2)
            {
                ivSex.sprite = iconDataManager.GetIconSpriteByName("sex_woman");
            }
        }
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
        tvPrice.text = priceS + "/天";
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
    /// <param name="isAccountant"></param>
    /// <param name="isBeater"></param>
    /// <param name="isAccost"></param>
    public void SetWork(bool isChef, bool isWaiter, bool isAccountant, bool isAccost, bool isBeater)
    {
        if (rbAccounting != null)
        {
            if (isAccountant)
                rbAccounting.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbAccounting.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
        if (rbChef != null)
        {
            if (isChef)
                rbChef.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbChef.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
        if (rbWaiter != null)
        {
            if (isWaiter)
                rbWaiter.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbWaiter.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
        if (rbAccost != null)
        {
            if (isAccost)
                rbAccost.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbAccost.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
        if (rbBeater != null)
        {
            if (isBeater)
                rbBeater.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbBeater.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
    }

    /// <summary>
    /// 设置优先级
    /// </summary>
    /// <param name="priorityChef"></param>
    /// <param name="priorityWaiter"></param>
    /// <param name="priorityAccountant"></param>
    /// <param name="priorityBeater"></param>
    /// <param name="priorityAccost"></param>
    public void SetPriority(int priorityChef, int priorityWaiter, int priorityAccountant, int priorityAccost, int priorityBeater)
    {
        if (etPriorityChef != null)
            etPriorityChef.text = priorityChef + "";
        if (etPriorityWaiter != null)
            etPriorityWaiter.text = priorityWaiter + "";
        if (etPriorityAccounting != null)
            etPriorityAccounting.text = priorityAccountant + "";
        if (etPriorityAccost != null)
            etPriorityAccost.text = priorityAccost + "";
        if (etPriorityBeater != null)
            etPriorityBeater.text = priorityBeater + "";
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="workStatus"></param>
    public void SetStatus(WorkerStatusEnum workerStatus, string workerStatusStr)
    {
        if (tvStatus != null)
        {
            switch (workerStatus)
            {
                case WorkerStatusEnum.Work:
                    if (gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Rest)
                    {
                        tvStatus.color = Color.green;
                        workerStatusStr = GameCommonInfo.GetUITextById(282);
                    }
                    else if (gameTimeHandler.dayStauts == GameTimeHandler.DayEnum.Work)
                    {
                        tvStatus.color = Color.red;
                    }
                    break;
                case WorkerStatusEnum.Research:
                    tvStatus.color = Color.red;
                    break;
                case WorkerStatusEnum.Rest:
                    tvStatus.color = Color.green;
                    break;
                case WorkerStatusEnum.Vacation:
                    tvStatus.color = Color.blue;
                    break;
                default:
                    tvStatus.color = Color.red;
                    break;
            }
            tvStatus.text = workerStatusStr;
        }
    }

    /// <summary>
    ///  更改优先级
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="content"></param>
    public void PriorityChange(WorkerEnum worker, string content)
    {
        if (int.TryParse(content, out int priority))
        {
            CharacterWorkerBaseBean characterWorker= characterData.baseInfo.GetWorkerInfoByType(worker);
            characterWorker.SetPriority(priority);
            if (GetUIManager<UIGameManager>().innHandler != null)
                GetUIManager<UIGameManager>().innHandler.InitWorker();
        }
    }
    public void PriorityChangeForChef(string content)
    {
        PriorityChange(WorkerEnum.Chef, content);
    }
    public void PriorityChangeForWaiter(string content)
    {
        PriorityChange(WorkerEnum.Waiter, content);
    }
    public void PriorityChangeForAccounting(string content)
    {
        PriorityChange(WorkerEnum.Accountant, content);
    }
    public void PriorityChangeForAccost(string content)
    {
        PriorityChange(WorkerEnum.Accost, content);
    }
    public void PriorityChangeForBeater(string content)
    {
        PriorityChange(WorkerEnum.Beater, content);
    }

    public virtual void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates)
    {
        if (characterData == null || characterData.baseInfo == null)
            return;
        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        CharacterBaseBean characterBase = characterData.baseInfo;
        if (view == rbChef)
        {
            characterBase.chefInfo.isWorking  = (buttonStates == RadioButtonView.RadioButtonStatus.Selected) ? true : false;
        }
        else if (view == rbWaiter)
        {
            characterBase.waiterInfo.isWorking = (buttonStates == RadioButtonView.RadioButtonStatus.Selected) ? true : false;
        }
        else if (view == rbAccounting)
        {
            characterBase.accountantInfo.isWorking = (buttonStates == RadioButtonView.RadioButtonStatus.Selected) ? true : false;
        }
        else if (view == rbAccost)
        {
            characterBase.accostInfo.isWorking = (buttonStates == RadioButtonView.RadioButtonStatus.Selected) ? true : false;
        }
        else if (view == rbBeater)
        {
            characterBase.beaterInfo.isWorking = (buttonStates == RadioButtonView.RadioButtonStatus.Selected) ? true : false;
        }
        if (GetUIManager<UIGameManager>().innHandler != null)
            GetUIManager<UIGameManager>().innHandler.InitWorker();
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as PickForItemsDialogView)
        {
            // 如果送礼
            PickForItemsDialogView pickForItems = (PickForItemsDialogView)dialogView;
            pickForItems.GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemData);
            //减去礼物
            gameDataManager.gameData.AddItemsNumber(itemsInfo.id, -1);
            //添加忠诚
            characterData.attributes.AddLoyal(1);
            //刷新UI
            SetData(characterData);
        }
        else
        {
            //如果是确认
            toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1081), characterData.baseInfo.name));
            gameDataManager.gameData.RemoveWorker(characterData);
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate
            {
                Destroy(gameObject);
            });
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
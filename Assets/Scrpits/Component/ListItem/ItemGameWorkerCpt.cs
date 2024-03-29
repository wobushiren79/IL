﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class ItemGameWorkerCpt : ItemGameBaseCpt, DialogView.IDialogCallBack, WorkerPriorityView.ICallBack
{
    [Header("控件")]
    public Text tvName;
    public UIPopupPromptButton pbName;

    public Text tvPrice;
    public UIPopupPromptButton pbPrice;

    public Image ivLoyal;
    public Text tvLoyal;
    public UIPopupPromptButton pbLoyal;

    public Text tvLevelUp;

    public Image ivSex;

    public Text tvStatus;

    public TextMeshProUGUI tvCook;
    public UIPopupPromptButton pbCook;
    public TextMeshProUGUI tvSpeed;
    public UIPopupPromptButton pbSpeed;
    public TextMeshProUGUI tvAccount;
    public UIPopupPromptButton pbAccount;
    public TextMeshProUGUI tvCharm;
    public UIPopupPromptButton pbCharm;
    public TextMeshProUGUI tvForce;
    public UIPopupPromptButton pbForce;
    public TextMeshProUGUI tvLucky;
    public UIPopupPromptButton pbLucky;

    public Button btEquip;
    public Button btDetails;
    public Button btGift;
    public Button btFire;

    public WorkerPriorityView wvChef;
    public WorkerPriorityView wvWaiterForSend;
    public WorkerPriorityView wvWaiterForCleanTable;
    public WorkerPriorityView wvWaiterForCleanBed;
    public WorkerPriorityView wvAccounting;
    public WorkerPriorityView wvAccostForSolicit;
    public WorkerPriorityView wvAccostForGuide;
    public WorkerPriorityView wvBeater;

    public UIPopupCharacterButton infoCharacterPopup;

    [Header("数据")]
    public CharacterUICpt characterUICpt;
    public CharacterBean characterData;

    protected UIPopupPromptShow infoPromptPopup;

    private void Start()
    {
        if (pbName != null)
        {
            pbName.SetContent(TextHandler.Instance.manager.GetTextById(61));
        }
        if (pbPrice != null)
        {
            pbPrice.SetContent(TextHandler.Instance.manager.GetTextById(11002));
        }
        if (pbLoyal != null)
        {
            pbLoyal.SetContent(TextHandler.Instance.manager.GetTextById(11003));
        }
        if (pbCook != null)
        {
            pbCook.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Cook));
        }
        if (pbSpeed != null)
        {
            pbSpeed.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Speed));
        }
        if (pbAccount != null)
        {
            pbAccount.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Account));
        }
        if (pbCharm != null)
        {
            pbCharm.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm));
        }
        if (pbForce != null)
        {
            pbForce.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Force));
        }
        if (pbLucky != null)
        {
            pbLucky.SetContent(AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Lucky));
        }

        if (wvChef != null)
            wvChef.SetCallBack(this);
        if (wvWaiterForSend != null)
            wvWaiterForSend.SetCallBack(this);
        if (wvWaiterForCleanTable != null)
            wvWaiterForCleanTable.SetCallBack(this);
        if (wvWaiterForCleanBed != null)
            wvWaiterForCleanBed.SetCallBack(this);
        if (wvAccounting != null)
            wvAccounting.SetCallBack(this);
        if (wvAccostForSolicit != null)
            wvAccostForSolicit.SetCallBack(this);
        if (wvAccostForGuide != null)
            wvAccostForGuide.SetCallBack(this);
        if (wvBeater != null)
            wvBeater.SetCallBack(this);

        if (btEquip != null)
            btEquip.onClick.AddListener(OpenEquipUI);
        if (btDetails != null)
            btDetails.onClick.AddListener(OpenDeitalsUI);
        if (btFire != null)
            btFire.onClick.AddListener(FireWorker);
        if (btGift != null)
            btGift.onClick.AddListener(SendGift);

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
                characterBase.waiterInfo.isWorkingForSend,characterBase.waiterInfo.isWorkingForCleanTable,characterBase.waiterInfo.isWorkingCleanBed,
                characterBase.accountantInfo.isWorking,
                characterBase.accostInfo.isWorkingForSolicit, characterBase.accostInfo.isWorkingForGuide,
                characterBase.beaterInfo.isWorking);
            SetPriority(
                characterBase.chefInfo.priority,
                characterBase.waiterInfo.priorityForSend, characterBase.waiterInfo.priorityForCleanTable, characterBase.waiterInfo.priorityForCleanBed,
                characterBase.accountantInfo.priority,
                characterBase.accostInfo.priorityForSolicit, characterBase.accostInfo.priorityForGuide,
                characterBase.beaterInfo.priority);

            WorkerStatusEnum workerStatus = characterBase.GetWorkerStatus(out string workerStatusStr);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (data == gameData.userCharacter || data is CharacterForFamilyBean)
        {
            if (btFire != null)
                btFire.gameObject.SetActive(false);
            if (btGift != null)
                btGift.gameObject.SetActive(false);
        }
        else
        {
            if (btFire != null)
                btFire.gameObject.SetActive(true);
            if (btGift != null)
                btGift.gameObject.SetActive(true);
        }
        SetLevelUp(characterData);


        if (infoCharacterPopup != null)
        {
            infoCharacterPopup.SetData(characterData);
        }
    }

    /// <summary>
    /// 设置升级
    /// </summary>
    /// <param name="isCanLevelUp"></param>
    public void SetLevelUp(CharacterBean characterData)
    {
        List<CharacterWorkerBaseBean> listWorker = characterData.baseInfo.GetAllWorkerInfo();
        bool isCanLevelUp = false;
        foreach (CharacterWorkerBaseBean characterWorker in listWorker)
        {
            characterWorker.GetWorkerExp(out long nextLevelExp, out long currentExp, out float levelProportion);
            if (currentExp >= nextLevelExp)
            {
                isCanLevelUp = true;
                break;
            }
        }
        if (isCanLevelUp)
        {
            tvLevelUp.gameObject.SetActive(true);
        }
        else
        {
            tvLevelUp.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 打开装备UI
    /// </summary>
    public void OpenEquipUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (uiComponent != null)
        {
            UIGameEquip uiequip = UIHandler.Instance.OpenUIAndCloseOther<UIGameEquip>();
            uiequip.SetCharacterData(((UIGameWorker)uiComponent).listCharacterData, characterData);
            UIHandler.Instance.OpenUIAndCloseOther<UIGameEquip>();
        }
    }

    /// <summary>
    /// 打开详情页
    /// </summary>
    public void OpenDeitalsUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (uiComponent != null)
        {
            UIGameWorkerDetails uiWorkerDetails = UIHandler.Instance.OpenUIAndCloseOther<UIGameWorkerDetails>();
            UIGameWorker uiWorker = (UIGameWorker)uiComponent;
            uiWorkerDetails.SetCharacterData(uiWorker.listCharacterData, characterData);
        }
    }

    /// <summary>
    /// 开除该员工
    /// </summary>
    public void FireWorker()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1082));
        }
        else
        {
            if (characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work || characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Rest)
            {
                DialogBean dialogData = new DialogBean();
                dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3063), characterData.baseInfo.name);
                dialogData.dialogType = DialogEnum.Normal;
                dialogData.callBack = this;
                UIHandler.Instance.ShowDialog<DialogView>(dialogData);
            }
            else
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1083));
            }
        }
    }

    /// <summary>
    ///  送礼
    /// </summary>
    public void SendGift()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        // dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3063), characterData.baseInfo.name);
        dialogData.dialogType = DialogEnum.PickForItems;
        dialogData.callBack = this;
        PickForItemsDialogView dialogView = UIHandler.Instance.ShowDialog<PickForItemsDialogView>(dialogData);
        dialogView.SetData(null, ItemsSelectionDialogView.SelectionTypeEnum.Gift);
        dialogView.SetSubmitDestroy(false);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    public void SetAttributes(CharacterAttributesBean characterAttributes, CharacterEquipBean characterEquip)
    {

        characterData.GetAttributes(
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        SetAttributesItem(tvCook, totalAttributes.cook);
        SetAttributesItem(tvSpeed, totalAttributes.speed);
        SetAttributesItem(tvAccount, totalAttributes.account);
        SetAttributesItem(tvCharm, totalAttributes.charm);
        SetAttributesItem(tvForce, totalAttributes.force);
        SetAttributesItem(tvLucky, totalAttributes.lucky);
    }

    public void SetAttributesItem(TextMeshProUGUI tvAttributes, int attributes)
    {
        if (tvAttributes == null)
            return;
        tvAttributes.text = attributes + "";
        if (attributes < 20)
        {
            tvAttributes.color = new Color(1, 1, 1, 1);
        }
        else if (attributes >= 20 && attributes < 40)
        {
            tvAttributes.color = new Color(0, 1, 0, 1);
        }
        else if (attributes >= 40 && attributes < 60)
        {
            tvAttributes.color = new Color(0, 1, 1, 1);
        }
        else if (attributes >= 60 && attributes < 80)
        {
            tvAttributes.color = new Color(1, 0, 1, 1);
        }
        else if (attributes >= 80)
        {
            tvAttributes.color = new Color(1, 1, 0, 1);
        }
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
        if (ivSex != null)
        {
            if (sex == 1)
            {
                ivSex.sprite = IconHandler.Instance.GetIconSpriteByName("sex_man");
            }
            else if (sex == 2)
            {
                ivSex.sprite = IconHandler.Instance.GetIconSpriteByName("sex_woman");
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
    public void SetWork(
        bool isChef, 
        bool isWaiterForSend, bool isWaiterForCleanTable, bool isWaiterForCleanBed,
        bool isAccountant, 
        bool isAccostForSolicit, bool isAccostForGuide,
        bool isBeater)
    {

        if(wvChef!=null)
            wvChef.SetWork(isChef);
        if (wvWaiterForSend != null)
            wvWaiterForSend.SetWork(isWaiterForSend);
        if (wvWaiterForCleanTable != null)
            wvWaiterForCleanTable.SetWork(isWaiterForCleanTable);
        if (wvWaiterForCleanBed != null)
            wvWaiterForCleanBed.SetWork(isWaiterForCleanBed);
        if (wvAccounting != null)
            wvAccounting.SetWork(isAccountant);
        if (wvAccostForSolicit != null)
            wvAccostForSolicit.SetWork(isAccostForSolicit);
        if (wvAccostForGuide != null)
            wvAccostForGuide.SetWork(isAccostForGuide);
        if (wvBeater != null)
            wvBeater.SetWork(isBeater);
    }

    /// <summary>
    /// 设置优先级
    /// </summary>
    /// <param name="priorityChef"></param>
    /// <param name="priorityWaiter"></param>
    /// <param name="priorityAccountant"></param>
    /// <param name="priorityBeater"></param>
    /// <param name="priorityAccost"></param>
    public void SetPriority(
        int priorityChef, 
        int priorityWaiterForSend, int priorityWaiterForCleanTable, int priorityWaiterForCleanBed,
        int priorityAccountant, 
        int priorityAccostForSolicit, int priorityAccostForGuide,
        int priorityBeater)
    {
        if (wvChef)
            wvChef.SetPriority(priorityChef);
        if (wvWaiterForSend)
            wvWaiterForSend.SetPriority(priorityWaiterForSend);
        if (wvWaiterForCleanTable)
            wvWaiterForCleanTable.SetPriority(priorityWaiterForCleanTable);
        if (wvWaiterForCleanBed)
            wvWaiterForCleanBed.SetPriority(priorityWaiterForCleanBed);
        if (wvAccounting)
            wvAccounting.SetPriority(priorityAccountant);
        if (wvAccostForSolicit)
            wvAccostForSolicit.SetPriority(priorityAccostForSolicit);
        if (wvAccostForGuide)
            wvAccostForGuide.SetPriority(priorityAccostForGuide);
        if (wvBeater)
            wvBeater.SetPriority(priorityBeater);
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
                    if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
                    {
                        tvStatus.color = Color.green;
                        workerStatusStr = TextHandler.Instance.manager.GetTextById(282);
                    }
                    else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
                    {
                        tvStatus.color = Color.red;
                    }
                    break;
                case WorkerStatusEnum.Research:
                case WorkerStatusEnum.CourtyardManager:
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

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (dialogView as PickForItemsDialogView)
        {
            // 如果送礼
            PickForItemsDialogView pickForItems = dialogView as PickForItemsDialogView;
            pickForItems.GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemData);
            //减去礼物
            gameData.AddItemsNumber(itemsInfo.id, -1);
            //添加忠诚
            int addLoyal = ((int)itemsInfo.GetItemRarity() + 1) * 5;
            characterData.attributes.AddLoyal(addLoyal);
            //刷新UI
            SetData(characterData);
            //提示
            UIHandler.Instance.ToastHint<ToastView>(ivLoyal.sprite, string.Format(TextHandler.Instance.manager.GetTextById(1132), characterData.baseInfo.name, addLoyal + ""));
            pickForItems.RefreshUI();
        }
        else
        {
            //如果是确认 开除员工
            //返还装备
            if (characterData.equips.maskId != 0)
                gameData.AddItemsNumber(characterData.equips.maskId, 1);
             if (characterData.equips.maskTFId != 0)
                gameData.AddItemsNumber(characterData.equips.maskTFId, 1);

            if (characterData.equips.handId != 0)
                gameData.AddItemsNumber(characterData.equips.handId, 1);
            if (characterData.equips.handTFId != 0)
                gameData.AddItemsNumber(characterData.equips.handTFId, 1);

            if (characterData.equips.hatId != 0)
                gameData.AddItemsNumber(characterData.equips.hatId, 1);
            if (characterData.equips.hatTFId != 0)
                gameData.AddItemsNumber(characterData.equips.hatTFId, 1);

            if (characterData.equips.clothesId != 0)
                gameData.AddItemsNumber(characterData.equips.clothesId, 1);
            if (characterData.equips.clothesTFId != 0)
                gameData.AddItemsNumber(characterData.equips.clothesTFId, 1);

            if (characterData.equips.shoesId != 0)
                gameData.AddItemsNumber(characterData.equips.shoesId, 1);
            if (characterData.equips.shoesTFId != 0)
                gameData.AddItemsNumber(characterData.equips.shoesTFId, 1);

            UIHandler.Instance.ToastHint<ToastView>(string.Format(TextHandler.Instance.manager.GetTextById(1081), characterData.baseInfo.name));
            gameData.RemoveWorker(characterData);

            transform.DOScale(Vector3.zero, 0.3f).OnComplete(delegate
            {
                if (gameObject != null)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    characterData.baseInfo.characterId = null;
                }
                uiComponent.RefreshUI();
            });
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }


    #endregion


    #region 优先级和状态修改回调
    public void ChangePriority(WorkerPriorityView view, int priority)
    {
        //厨师优先级
        if (view == wvChef)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Chef);
            characterWorker.SetPriority(priority);
        }
        //伙计优先级
        else if (view == wvWaiterForSend)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetPriorityForSend(priority);
        }
        else if (view == wvWaiterForCleanTable)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetPriorityForCleanTable(priority);
        }
        else if (view == wvWaiterForCleanBed)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetPriorityForCleanBed(priority);
        }
        //账房优先级
        else if (view == wvAccounting)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
            characterWorker.SetPriority(priority);
        }
        //接待优先级
        else if (view == wvAccostForSolicit)
        {
            CharacterWorkerForAccostBean characterWorker = (CharacterWorkerForAccostBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
            characterWorker.SetPriorityForSolicit(priority);
        }
        else if (view == wvAccostForGuide)
        {
            CharacterWorkerForAccostBean characterWorker = (CharacterWorkerForAccostBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
            characterWorker.SetPriorityForGuide(priority);
        }
        //打手优先级
        else if (view == wvBeater)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Beater);
            characterWorker.SetPriority(priority);
        }

        InnHandler.Instance.InitWorker();
    }

    public void ChangeStatus(WorkerPriorityView view, bool isWork)
    {
        if (characterData == null || characterData.baseInfo == null)
            return;
        //厨师状态
        if (view == wvChef)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Chef);
            characterWorker.SetWorkStatus(isWork);
        }
        //伙计状态
        else if (view == wvWaiterForSend)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetWorkStatusForSend(isWork);
        }
        else if (view == wvWaiterForCleanTable)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetWorkStatusForCleanTable(isWork);
        }
        else if (view == wvWaiterForCleanBed)
        {
            CharacterWorkerForWaiterBean characterWorker = (CharacterWorkerForWaiterBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
            characterWorker.SetWorkStatusForCleanBed(isWork);
        }
        //账房状态
        else if (view == wvAccounting)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
            characterWorker.SetWorkStatus(isWork);
        }
        //接待状态
        else if (view == wvAccostForSolicit)
        {
            CharacterWorkerForAccostBean characterWorker = (CharacterWorkerForAccostBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
            characterWorker.SetWorkStatusForSolicit(isWork);
        }
        else if (view == wvAccostForGuide)
        {
            CharacterWorkerForAccostBean characterWorker = (CharacterWorkerForAccostBean)characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
            characterWorker.SetWorkStatusForGuide(isWork);
        }
        //打手状态
        else if (view == wvBeater)
        {
            CharacterWorkerBaseBean characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Beater);
            characterWorker.SetWorkStatus(isWork);
        }

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

        InnHandler.Instance.InitWorker();
    }
    #endregion
}
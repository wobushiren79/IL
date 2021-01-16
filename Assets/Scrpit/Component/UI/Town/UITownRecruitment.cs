using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UITownRecruitment : UIBaseOne, DialogView.IDialogCallBack
{
    //人员数量
    public Text tvNumber;
    //重金寻聘
    public Button btFindWorker;
    public InfoPromptPopupButton infoPromptPopupButton;
    public Text tvNull;

    public GameObject objCandidateContent;
    public GameObject objCandidateModel;

    public override void Awake()
    {
        base.Awake();
        if (GameCommonInfo.DailyLimitData.listRecruitmentCharacter == null)
        {
            CreateCandidateData();
        }
        else
        {
            CreateRecruitmentList(GameCommonInfo.DailyLimitData.listRecruitmentCharacter);
        }
    }

    public override void Start()
    {
        base.Start();
        if (btFindWorker != null)
            btFindWorker.onClick.AddListener(FindWorkerByMoney);
        if (infoPromptPopupButton != null)
        {
            infoPromptPopupButton.SetPopupShowView(uiGameManager.infoPromptPopup);
            infoPromptPopupButton.SetContent(GameCommonInfo.GetUITextById(271));
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        CreateRecruitmentList(GameCommonInfo.DailyLimitData.listRecruitmentCharacter);
    }

    private new void Update()
    {
        base.Update();
        if (uiGameManager.gameDataManager != null && tvNumber != null)
        {
            tvNumber.text = uiGameManager.gameDataManager.gameData.listWorkerCharacter.Count + "/" + uiGameManager.gameDataManager.gameData.workerNumberLimit;
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    /// <summary>
    /// 创建应聘者数据
    /// </summary>
    public void CreateCandidateData()
    {
        GameCommonInfo.InitRandomSeed();
        for (int i = 0; i < Random.Range(1, 15); i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomWorkerData(uiGameManager.characterBodyManager);
            GameCommonInfo.DailyLimitData.AddRecruitmentCharacter(characterData);
        }
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listData"></param>
    public void CreateRecruitmentList(List<CharacterBean> listData)
    {
        CptUtil.RemoveChildsByActive(objCandidateContent.transform);
        if (CheckUtil.ListIsNull(listData))
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            GameObject objCandidate = Instantiate(objCandidateContent, objCandidateModel);
            ItemTownCandidateCpt itemCpt = objCandidate.GetComponent<ItemTownCandidateCpt>();
            itemCpt.SetData(itemData);
            objCandidate.transform
                .DOScale(new Vector3(0, 0, 0), 0.5f)
                .From()
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.05f);
        }
    }

    /// <summary>
    /// 移除数据
    /// </summary>
    /// <param name="characterData"></param>
    public void RemoveCandidate(CharacterBean characterData)
    {
        GameCommonInfo.DailyLimitData.RemoveRecruitmentCharacter(characterData);
        RefreshUI();
    }

    /// <summary>
    /// 重金寻聘
    /// </summary>
    public void FindWorkerByMoney()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        //检测是否超过人员上限
        if (uiGameManager.gameDataManager.gameData.listWorkerCharacter.Count >= uiGameManager.gameDataManager.gameData.workerNumberLimit)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1051));
            return;
        }
        pickMoneyL = 0;
        pickMoneyM = 0;
        pickMoneyS = 0;
        DialogBean dialogData = new DialogBean();
        dialogData.title = GameCommonInfo.GetUITextById(3062);
        PickForMoneyDialogView pickForMoneyDialog = uiGameManager.dialogManager.CreateDialog<PickForMoneyDialogView>(DialogEnum.PickForMoney, this, dialogData);
        pickForMoneyDialog.SetData(1, 1, 100);
        pickForMoneyDialog.SetMaxMoney(99999,99999,99999);
    }

    protected int pickMoneyL = 0;
    protected int pickMoneyM = 0;
    protected int pickMoneyS = 0;
    #region 弹窗回调
    public override void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        base.Submit(dialogView, dialogBean);
        if (dialogView as PickForMoneyDialogView)
        {
            //如果是金钱选择回调
            PickForMoneyDialogView pickForMoneyDialog = dialogView as PickForMoneyDialogView;
            pickForMoneyDialog.GetPickMoney(out pickMoneyL, out pickMoneyM, out pickMoneyS);
            ShowPickCharacter();
        }
        else if (dialogView as FindCharacterDialogView)
        {
            if (!CheckUtil.StringIsNull(dialogBean.remark)&& dialogBean.remark.Equals("Continue"))
            {
                ShowPickCharacter();
            }
            else
            {
                //如果是招募回调
                GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
                ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
                FindCharacterDialogView findCharacterDialog = (FindCharacterDialogView)dialogView;
                gameDataManager.gameData.listWorkerCharacter.Add(findCharacterDialog.characterData);
                toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1053), findCharacterDialog.characterData.baseInfo.name));
            }
        }
    }


    public override void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        base.Cancel(dialogView, dialogBean);
    }
    #endregion

    protected void ShowPickCharacter()
    {
        if (!uiGameManager.gameDataManager.gameData.HasEnoughMoney(pickMoneyL, pickMoneyM, pickMoneyS))
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        uiGameManager.gameDataManager.gameData.PayMoney(pickMoneyL, pickMoneyM, pickMoneyS);

        DialogManager dialogManager = uiGameManager.dialogManager;
        CharacterBodyManager characterBodyManager = uiGameManager.characterBodyManager;

        DialogBean dialogData = new DialogBean();
        //根据金额获取角色
        CharacterBean characterData = CharacterBean.CreateRandomWorkerDataByPrice(characterBodyManager, pickMoneyL, pickMoneyM, pickMoneyS);
        FindCharacterDialogView findCharacterDialog = dialogManager.CreateDialog<FindCharacterDialogView>(DialogEnum.FindCharacter, this, dialogData);
        findCharacterDialog.SetData(characterData);
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.Reward);
    }
}
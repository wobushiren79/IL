using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
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

    private void Awake()
    {
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
        InfoPromptPopupShow infoPromptPopupShow = GetUIManager<UIGameManager>().infoPromptPopup;
        if (btFindWorker != null)
            btFindWorker.onClick.AddListener(FindWorkerByMoney);
        if (infoPromptPopupButton != null)
        {
            infoPromptPopupButton.SetPopupShowView(infoPromptPopupShow);
            infoPromptPopupButton.SetContent("根据寻聘金额随机获取角色，金额越高角色属性越强，获得稀有角色概率越高");
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
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (gameDataManager != null && tvNumber != null)
        {
            tvNumber.text = gameDataManager.gameData.listWorkerCharacter.Count + "/" + gameDataManager.gameData.workerNumberLimit;
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
        CharacterBodyManager characterBodyManager = GetUIManager<UIGameManager>().characterBodyManager;
        for (int i = 0; i < Random.Range(1,15); i++)
        {
            CharacterBean characterData = CharacterBean.CreateRandomWorkerData(characterBodyManager);
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
        //检测是否超过人员上限
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        if (gameDataManager.gameData.listWorkerCharacter.Count >= gameDataManager.gameData.workerNumberLimit)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1051));
            return;
        }
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        DialogBean dialogData = new DialogBean();
        dialogData.title = GameCommonInfo.GetUITextById(3062);
        PickForMoneyDialogView pickForMoneyDialog = (PickForMoneyDialogView)dialogManager.CreateDialog(DialogEnum.PickForMoney, this, dialogData);
        pickForMoneyDialog.SetData(1, 1, 100);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView is PickForMoneyDialogView)
        {
            //如果是金钱选择回调
            DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
            CharacterBodyManager characterBodyManager = GetUIManager<UIGameManager>().characterBodyManager;
            PickForMoneyDialogView pickForMoneyDialog = (PickForMoneyDialogView)dialogView;
            DialogBean dialogData = new DialogBean();
            //根据金额获取角色
            CharacterBean characterData = CharacterBean.CreateRandomWorkerDataByPrice(characterBodyManager, pickForMoneyDialog.moneyL, pickForMoneyDialog.moneyM, pickForMoneyDialog.moneyS);
            FindCharacterDialogView findCharacterDialog = (FindCharacterDialogView)dialogManager.CreateDialog(DialogEnum.FindCharacter, this, dialogData);
            findCharacterDialog.SetData(characterData);
        }
        else if (dialogView is FindCharacterDialogView)
        {
            //如果是招募回调
            GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
            FindCharacterDialogView findCharacterDialog = (FindCharacterDialogView)dialogView;
            gameDataManager.gameData.listWorkerCharacter.Add(findCharacterDialog.characterData);
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
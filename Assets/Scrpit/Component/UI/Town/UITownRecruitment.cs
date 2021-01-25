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
    public PopupPromptButton popupPromptButton;
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
        if (popupPromptButton != null)
        {
            popupPromptButton.SetContent(GameCommonInfo.GetUITextById(271));
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
        if (tvNumber != null)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            tvNumber.text = gameData.listWorkerCharacter.Count + "/" + gameData.workerNumberLimit;
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
            CharacterBean characterData = CharacterBean.CreateRandomWorkerData();
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //检测是否超过人员上限
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.listWorkerCharacter.Count >= gameData.workerNumberLimit)
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1051));
            return;
        }
        pickMoneyL = 0;
        pickMoneyM = 0;
        pickMoneyS = 0;
        DialogBean dialogData = new DialogBean();
        dialogData.title = GameCommonInfo.GetUITextById(3062);
        PickForMoneyDialogView pickForMoneyDialog = DialogHandler.Instance.CreateDialog<PickForMoneyDialogView>(DialogEnum.PickForMoney, this, dialogData);
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
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                FindCharacterDialogView findCharacterDialog = (FindCharacterDialogView)dialogView;
                gameData.listWorkerCharacter.Add(findCharacterDialog.characterData);
                ToastHandler.Instance.ToastHint(string.Format(GameCommonInfo.GetUITextById(1053), findCharacterDialog.characterData.baseInfo.name));
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (!gameData.HasEnoughMoney(pickMoneyL, pickMoneyM, pickMoneyS))
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameData.PayMoney(pickMoneyL, pickMoneyM, pickMoneyS);

        

        DialogBean dialogData = new DialogBean();
        //根据金额获取角色
        CharacterBean characterData = CharacterBean.CreateRandomWorkerDataByPrice(pickMoneyL, pickMoneyM, pickMoneyS);
        FindCharacterDialogView findCharacterDialog = DialogHandler.Instance.CreateDialog<FindCharacterDialogView>(DialogEnum.FindCharacter, this, dialogData);
        findCharacterDialog.SetData(characterData);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
    }
}
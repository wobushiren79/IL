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

    protected List<CharacterBean> listData = new List<CharacterBean>();

    public ScrollGridVertical gridVertical;

    public override void Awake()
    {
        base.Awake();
        if (gridVertical)
            gridVertical.AddCellListener(OnCellForItem);
    }

    public override void Start()
    {
        base.Start();
        if (btFindWorker != null)
            btFindWorker.onClick.AddListener(FindWorkerByMoney);
        if (popupPromptButton != null)
        {
            popupPromptButton.SetContent(TextHandler.Instance.manager.GetTextById(271));
        }
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (GameCommonInfo.DailyLimitData.listRecruitmentCharacter == null)
        {
            CreateCandidateData();
        }
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

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        ItemTownCandidateCpt itemCpt = itemCell.GetComponent<ItemTownCandidateCpt>();
        CharacterBean itemData = listData[itemCell.index];
        itemCpt.SetData(itemData);
    }

    /// <summary>
    /// 创建应聘者数据
    /// </summary>
    public void CreateCandidateData()
    {
        GameCommonInfo.InitRandomSeed();
        int randomNumber = Random.Range(1, 15);
        for (int i = 0; i < randomNumber; i++)
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
        this.listData = listData;
        if (listData.IsNull())
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
        gridVertical.SetCellCount(listData.Count);
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
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1051));
            return;
        }
        pickMoneyL = 0;
        pickMoneyM = 0;
        pickMoneyS = 0;
        DialogBean dialogData = new DialogBean();
        dialogData.title = TextHandler.Instance.manager.GetTextById(3062);
        dialogData.dialogType = DialogEnum.PickForMoney;
        PickForMoneyDialogView pickForMoneyDialog = UIHandler.Instance.ShowDialog<PickForMoneyDialogView>(dialogData);
        pickForMoneyDialog.SetData(1, 1, 100);
        pickForMoneyDialog.SetMaxMoney(99999, 99999, 99999);
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
            if (!dialogBean.remark.IsNull() && dialogBean.remark.Equals("Continue"))
            {
                ShowPickCharacter();
            }
            else
            {
                //如果是招募回调
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                FindCharacterDialogView findCharacterDialog = dialogView as FindCharacterDialogView;
                gameData.listWorkerCharacter.Add(findCharacterDialog.characterData);
                UIHandler.Instance.ToastHint<ToastView>(string.Format(TextHandler.Instance.manager.GetTextById(1053), findCharacterDialog.characterData.baseInfo.name));
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
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
            return;
        }
        gameData.PayMoney(pickMoneyL, pickMoneyM, pickMoneyS);



        DialogBean dialogData = new DialogBean();
        //根据金额获取角色
        CharacterBean characterData = CharacterBean.CreateRandomWorkerDataByPrice(pickMoneyL, pickMoneyM, pickMoneyS);
        dialogData.dialogType = DialogEnum.FindCharacter;
        FindCharacterDialogView findCharacterDialog = UIHandler.Instance.ShowDialog<FindCharacterDialogView>(dialogData);
        findCharacterDialog.SetData(characterData);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
    }
}
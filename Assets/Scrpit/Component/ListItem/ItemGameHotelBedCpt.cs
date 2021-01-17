using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemGameHotelBedCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public InfoBedPopupButton infoBedPopup;
    public BedShowView bedShow;
    public Image ivLevel;
    public Image ivBgLevel;
    public Text tvName;
    public Text tvPrice;


    public InfoPromptPopupButton popupForResearch;
    public GameObject objResearch;
    public Button btResearch;

    public GameObject objResearchCancel;
    public Button btResearchCancel;

    public Button btRemove;
    public BuildBedBean buildBedData;

    public Sprite spBgLevel0;
    public Sprite spBgLevel1;
    public Sprite spBgLevel2;
    public Sprite spBgLevel3;

    private void Awake()
    {
        if (btRemove != null)
            btRemove.onClick.AddListener(OnClickForRemove);
        if (btResearch != null)
            btResearch.onClick.AddListener(OnClickResearch);
        if (btResearchCancel != null)
            btResearchCancel.onClick.AddListener(OnClickResearchCancel);
    }

    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        infoBedPopup.SetData(buildBedData);
        SetBed(buildBedData);
        SetName(buildBedData.bedName);
        buildBedData.GetPrice(out long outPriceL, out long outPriceM, out long outPriceS);
        SetPrice(outPriceS);
        SetLevel(buildBedData);
        SetResearch(buildBedData.GetBedStatus());
        SetResearchPopup(buildBedData);
        SetRemove( buildBedData);
    }

    /// <summary>
    /// 设置移除按钮
    /// </summary>
    /// <param name="buildBedData"></param>
    public void SetRemove(BuildBedBean buildBedData)
    {
        if (buildBedData.isSet)
        {
            btRemove.gameObject.SetActive(false);
        }
        else
        {
            btRemove.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置床
    /// </summary>
    /// <param name="buildBedData"></param>
    public void SetBed(BuildBedBean buildBedData)
    {
        if (bedShow != null)
        {
            bedShow.SetData(buildBedData);
        }
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    /// <param name="buildBedData"></param>
    public void SetLevel(BuildBedBean buildBedData)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();

        //设置等级图标
        Sprite spLevel = buildBedData.GetBedLevelIcon(uiGameManager.iconDataManager);
        if (spLevel == null)
        {
            ivLevel.gameObject.SetActive(false);
        }
        else
        {
            ivLevel.gameObject.SetActive(true);
            ivLevel.sprite = spLevel;
        }

        //设置不同的等级框
        LevelTypeEnum levelType = buildBedData.GetBedLevel();
        switch (levelType)
        {
            case LevelTypeEnum.Init:
                ivBgLevel.sprite = spBgLevel0;
                break;
            case LevelTypeEnum.Star:
                ivBgLevel.sprite = spBgLevel1;
                break;
            case LevelTypeEnum.Moon:
                ivBgLevel.sprite = spBgLevel2;
                break;
            case LevelTypeEnum.Sun:
                ivBgLevel.sprite = spBgLevel3;
                break;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="priceS"></param>
    public void SetPrice(long priceS)
    {
        if (tvPrice != null)
        {
            tvPrice.text = priceS + "/" + GameCommonInfo.GetUITextById(37);
        }
    }

    /// <summary>
    /// 设置研究
    /// </summary>
    /// <param name="menuStatus"></param>
    public void SetResearch(ResearchStatusEnum menuStatus)
    {
        objResearch.SetActive(false);
        objResearchCancel.SetActive(false);
        switch (menuStatus)
        {
            case ResearchStatusEnum.Normal:
                break;
            case ResearchStatusEnum.WaitForResearch:
                objResearch.SetActive(true);
                break;
            case ResearchStatusEnum.Researching:
                objResearchCancel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 设置研究弹出框内容
    /// </summary>
    /// <param name="menuOwn"></param>
    /// <param name="data"></param>
    public void SetResearchPopup(BuildBedBean buildBedData)
    {
        if (popupForResearch == null)
            return;
        buildBedData.GetResearchPrice(out long researchPriceL, out long researchPriceM, out long researchPriceS);
        string content = GameCommonInfo.GetUITextById(285);
        content += (" " + GameCommonInfo.GetUITextById(286) + "\n");
        if (researchPriceL!=0)
        {
            content +=( researchPriceL+ GameCommonInfo.GetUITextById(16));
        }
        if (researchPriceM != 0)
        {
            content += (researchPriceM+ GameCommonInfo.GetUITextById(17));
        }
        if (researchPriceS != 0)
        {
            content += (researchPriceS + GameCommonInfo.GetUITextById(18));
        }
        popupForResearch.SetContent(content);
    }

    /// <summary>
    /// 点击-移除
    /// </summary>
    public void OnClickForRemove()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (buildBedData.GetBedStatus()== ResearchStatusEnum.Researching)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1313));
            return;
        }
        if (buildBedData.isSet)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1314));
            return;
        }
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 1;
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3001), buildBedData.bedName);

        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    /// <summary>
    /// 点击研究
    /// </summary>
    public void OnClickResearch()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //首先判断客栈等级是否足够
        if (!buildBedData.CheckCanResearch(uiGameManager.gameDataManager.gameData, out string failStr))
        {
            uiGameManager.toastManager.ToastHint(failStr);
            return;
        }
        DialogBean dialogData = new DialogBean
        {
            title = GameCommonInfo.GetUITextById(3071)
        };
        PickForCharacterDialogView pickForCharacterDialog = DialogHandler.Instance.CreateDialog<PickForCharacterDialogView>(DialogEnum.PickForCharacter, this, dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);
        //设置排出人员 （老板和没有在休息的员工）
        List<CharacterBean> listCharacter = uiGameManager.gameDataManager.gameData.listWorkerCharacter;
        List<string> listExpelIds = new List<string>();
        listExpelIds.Add(uiGameManager.gameDataManager.gameData.userCharacter.baseInfo.characterId);
        foreach (CharacterBean itemData in listCharacter)
        {
            //休息日 排出不是工作或者休息的
            if (uiGameManager.gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
            {
                if (itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work)
                {
                    listExpelIds.Add(itemData.baseInfo.characterId);
                }
            }
            //工作日 排出除休息中的所有员工
            else if (uiGameManager.gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Work)
            {
                if (itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest)
                {
                    listExpelIds.Add(itemData.baseInfo.characterId);
                }
            }
        }
        pickForCharacterDialog.SetExpelCharacter(listExpelIds);
    }

    /// <summary>
    /// 点击研究取消
    /// </summary>
    public void OnClickResearchCancel()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean
        {
            dialogPosition = 2,
            content = GameCommonInfo.GetUITextById(3072)
        };
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }


    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (dialogView as PickForCharacterDialogView)
        {
            buildBedData.GetResearchPrice(out long researchPriceL, out long researchPriceM, out long researchPriceS);
            //先判断一下是否有钱支付
            if (!uiGameManager.gameDataManager.gameData.HasEnoughMoney(researchPriceL, researchPriceM, researchPriceS))
            {
                uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
                return;
            }
            //扣除金钱
            uiGameManager.gameDataManager.gameData.PayMoney(researchPriceL,researchPriceM,researchPriceS);
            //角色选择
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            List<CharacterBean> listPickCharacter = pickForCharacterDialog.GetPickCharacter();
            if (!CheckUtil.ListIsNull(listPickCharacter))
            {
                //开始研究
                buildBedData.StartResearch(listPickCharacter);
                string toastStr = string.Format(GameCommonInfo.GetUITextById(1201), listPickCharacter[0].baseInfo.name, buildBedData.bedName);
                uiGameManager.toastManager.ToastHint(toastStr);
            }
        }
        else
        {
            if (dialogBean.dialogPosition == 1)
            {
                if (buildBedData.GetBedStatus() == ResearchStatusEnum.Researching)
                {
                    uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1313));
                    return;
                }
                if (buildBedData.isSet)
                {
                    uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1314));
                    return;
                }
                //丢弃确认
                uiGameManager.gameDataManager.gameData.RemoveBed(buildBedData);
                uiComponent.RefreshUI();
            }
            else if (dialogBean.dialogPosition == 2)
            {
                //普通弹窗（取消研究）
                buildBedData.CancelResearch(uiGameManager.gameDataManager.gameData);
            }
        }
        //重新设置数据
        if (gameObject)
            SetData(buildBedData);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
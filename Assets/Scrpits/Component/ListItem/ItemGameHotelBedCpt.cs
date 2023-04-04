using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemGameHotelBedCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public UIPopupBedButton infoBedPopup;
    public BedShowView bedShow;
    public Image ivLevel;
    public Image ivBgLevel;
    public Text tvName;
    public Text tvPrice;
    public Button btEtName;


    public UIPopupPromptButton popupForResearch;
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
        if (btEtName != null)
            btEtName.onClick.AddListener(OnClickForEdName);
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
        //设置等级图标
        Sprite spLevel = buildBedData.GetBedLevelIcon();
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
            tvPrice.text = priceS + "/" + TextHandler.Instance.manager.GetTextById(37);
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
        string content = TextHandler.Instance.manager.GetTextById(285);
        content += (" " + TextHandler.Instance.manager.GetTextById(286) + "\n");
        if (researchPriceL!=0)
        {
            content +=( researchPriceL+ TextHandler.Instance.manager.GetTextById(16));
        }
        if (researchPriceM != 0)
        {
            content += (researchPriceM+ TextHandler.Instance.manager.GetTextById(17));
        }
        if (researchPriceS != 0)
        {
            content += (researchPriceS + TextHandler.Instance.manager.GetTextById(18));
        }
        popupForResearch.SetContent(content);
    }

    /// <summary>
    /// 点击-移除
    /// </summary>
    public void OnClickForRemove()
    {
        
        if (buildBedData.GetBedStatus()== ResearchStatusEnum.Researching)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1313));
            return;
        }
        if (buildBedData.isSet)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1314));
            return;
        }
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 1;
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3001), buildBedData.bedName);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    /// <summary>
    /// 点击研究
    /// </summary>
    public void OnClickResearch()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //首先判断客栈等级是否足够
        if (!buildBedData.CheckCanResearch(gameData, out string failStr))
        {
            UIHandler.Instance.ToastHint<ToastView>(failStr);
            return;
        }
        DialogBean dialogData = new DialogBean
        {
            dialogType= DialogEnum.PickForCharacter,
            title = TextHandler.Instance.manager.GetTextById(3071),
            callBack = this
        };
        PickForCharacterDialogView pickForCharacterDialog = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
        pickForCharacterDialog.SetPickCharacterMax(1);
        //设置排出人员 （老板和没有在休息的员工）
        List<CharacterBean> listCharacter = gameData.listWorkerCharacter;
        List<string> listExpelIds = new List<string>();
        listExpelIds.AddRange(gameData.GetAllFamilyCharacterIds());
        foreach (CharacterBean itemData in listCharacter)
        {
            //休息日 排出不是工作或者休息的
            if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
            {
                if (itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemData.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work)
                {
                    listExpelIds.Add(itemData.baseInfo.characterId);
                }
            }
            //工作日 排出除休息中的所有员工
            else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean
        {
            dialogType = DialogEnum.Normal,
            dialogPosition = 2,
            content = TextHandler.Instance.manager.GetTextById(3072),
            callBack = this
        };
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    public void OnClickForEdName()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        //dialogData.content = buildBedData.bedName;
        dialogData.title = TextHandler.Instance.manager.GetTextById(8001);
        dialogData.dialogType = DialogEnum.InputText;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<InputTextDialogView>(dialogData);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (dialogView is PickForCharacterDialogView)
        {
            buildBedData.GetResearchPrice(out long researchPriceL, out long researchPriceM, out long researchPriceS);
            //先判断一下是否有钱支付
            if (!gameData.HasEnoughMoney(researchPriceL, researchPriceM, researchPriceS))
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
                return;
            }
            //扣除金钱
            gameData.PayMoney(researchPriceL,researchPriceM,researchPriceS);
            //角色选择
            PickForCharacterDialogView pickForCharacterDialog = dialogView as PickForCharacterDialogView;
            List<CharacterBean> listPickCharacter = pickForCharacterDialog.GetPickCharacter();
            if (!listPickCharacter.IsNull())
            {
                //开始研究
                buildBedData.StartResearch(listPickCharacter);
                string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1201), listPickCharacter[0].baseInfo.name, buildBedData.bedName);
                UIHandler.Instance.ToastHint<ToastView>(toastStr);
            }
        }
        else if (dialogView is InputTextDialogView)
        {
            InputTextDialogView inputTextDialog= dialogView as InputTextDialogView;    
            string bedName = inputTextDialog.GetText();
            if(!bedName.IsNull())
                buildBedData.bedName = bedName;
        }
        else
        {
            if (dialogBean.dialogPosition == 1)
            {
                if (buildBedData.GetBedStatus() == ResearchStatusEnum.Researching)
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1313));
                    return;
                }
                if (buildBedData.isSet)
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1314));
                    return;
                }
                //丢弃确认
                gameData.RemoveBed(buildBedData);
                uiComponent.RefreshUI();
            }
            else if (dialogBean.dialogPosition == 2)
            {
                //普通弹窗（取消研究）
                buildBedData.CancelResearch(gameData);
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
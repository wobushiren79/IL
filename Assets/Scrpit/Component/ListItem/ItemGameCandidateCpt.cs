using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameCandidateCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Text tvName;
    public Text tvPrice;
    public Button btSubmit;
    public Image ivSex;
    public InfoAbilityPopupButton infoAbilityPopupButton;
    public CharacterUICpt characterUICpt;

    public Sprite spMan;
    public Sprite spWoman;

    public CharacterBean characterData;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(EmploymentCandidate);
        if (infoAbilityPopupButton != null)
            infoAbilityPopupButton.SetPopupShowView(GetUIManager<UIGameManager>().infoAbilityPopup);
    }

    /// <summary>
    /// 雇佣
    /// </summary>
    public void EmploymentCandidate()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        //检测是否超过人员上限
        if (gameDataManager == null)
            return;
        if (gameDataManager.gameData.workCharacterList.Count >= gameDataManager.gameData.workerNumberLimit)
        {
            toastView.ToastHint("招聘的员工数量已达最大！");
            return;
        };
        //确认
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = "初次聘用该员工需先支付一次日薪，是否支付 " + characterData.baseInfo.priceS + "文 聘用 " + characterData.baseInfo.name + " ？";
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    public void SetData(CharacterBean characterData)
    {
        if (characterData == null)
            return;
        this.characterData = characterData;
        SetName(characterData.baseInfo.name);
        SetPrice(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS);
        SetAbility(characterData);
        SetSex(characterData.body.sex);
        SetIcon(characterData);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="characterData"></param>
    public void SetIcon(CharacterBean characterData)
    {
        characterUICpt.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = "姓名：" + name;
    }

    /// <summary>
    /// 设置日薪
    /// </summary>
    /// <param name="price_l"></param>
    /// <param name="price_m"></param>
    /// <param name="price_s"></param>
    public void SetPrice(long price_l, long price_m, long price_s)
    {
        if (tvPrice != null)
        {
            string priceStr = "日薪：";
            if (price_l > 0)
                priceStr += price_l + "金";
            if (price_m > 0)
                priceStr += price_m + "银";
            if (price_s > 0)
                priceStr += price_s + "文";
            tvPrice.text = priceStr;
        }
    }

    public void SetSex(int sex)
    {
        if (ivSex != null)
        {
            switch (sex)
            {
                case 1:
                    ivSex.sprite = spMan;
                    break;
                case 2:
                    ivSex.sprite = spWoman;
                    break;
            }
        }
    }

    /// <summary>
    /// 设置能力
    /// </summary>
    /// <param name="characterData"></param>
    public void SetAbility(CharacterBean characterData)
    {
        infoAbilityPopupButton.SetData(characterData);
    }

    #region  dialog 回调
    public void Submit(DialogView dialogView)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;

        if (!gameDataManager.gameData.HasEnoughMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS);
        gameDataManager.gameData.workCharacterList.Add(characterData);
        GetUIComponent<UITownRecruitment>().RemoveCandidate(characterData);
    }

    public void Cancel(DialogView dialogView)
    {
    }
    #endregion
}
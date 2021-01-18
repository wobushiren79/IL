using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownCandidateCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Text tvName;
    public Text tvPrice;
    public Button btSubmit;
    public Image ivSex;
    public PopupAbilityButton popupAbilityButton;
    public CharacterUICpt characterUICpt;

    public Sprite spMan;
    public Sprite spWoman;

    public CharacterBean characterData;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(EmploymentCandidate);
    }

    /// <summary>
    /// 雇佣
    /// </summary>
    public void EmploymentCandidate()
    {
        UIGameManager uiGameManager= GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //检测是否超过人员上限
        if (gameDataManager == null)
            return;
        if (gameDataManager.gameData.listWorkerCharacter.Count >= gameDataManager.gameData.workerNumberLimit)
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1051));
            return;
        };
        //确认
        DialogBean dialogBean = new DialogBean();
        dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3061), characterData.baseInfo.priceS + "", characterData.baseInfo.name + "");
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogBean);
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
            tvName.text = GameCommonInfo.GetUITextById(61) + "：" + name;
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
            string priceStr = GameCommonInfo.GetUITextById(62) + "：";
            if (price_l > 0)
                priceStr += price_l + GameCommonInfo.GetUITextById(16);
            if (price_m > 0)
                priceStr += price_m + GameCommonInfo.GetUITextById(17);
            if (price_s > 0)
                priceStr += price_s + GameCommonInfo.GetUITextById(18);
            tvPrice.text = priceStr;
        }
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
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
        popupAbilityButton.SetData(characterData);
    }

    #region  dialog 回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;

        if (!gameDataManager.gameData.HasEnoughMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS))
        {
            ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS);
        gameDataManager.gameData.listWorkerCharacter.Add(characterData);
        GetUIComponent<UITownRecruitment>().RemoveCandidate(characterData);

        ToastHandler.Instance.ToastHint(string.Format(GameCommonInfo.GetUITextById(1053), characterData.baseInfo.name));
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {
    }
    #endregion
}
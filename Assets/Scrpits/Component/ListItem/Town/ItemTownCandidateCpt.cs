using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownCandidateCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Text tvName;
    public Text tvPrice;
    public Button btSubmit;
    public Image ivSex;
    public UIPopupAbilityButton popupAbilityButton;
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //检测是否超过人员上限
        if (gameData.listWorkerCharacter.Count >= gameData.workerNumberLimit)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1051));
            return;
        };
        //确认
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3061), characterData.baseInfo.priceS + "", characterData.baseInfo.name + "");
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
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
            tvName.text = TextHandler.Instance.manager.GetTextById(61) + "：" + name;
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
            string priceStr = TextHandler.Instance.manager.GetTextById(62) + "：";
            if (price_l > 0)
                priceStr += price_l + TextHandler.Instance.manager.GetTextById(16);
            if (price_m > 0)
                priceStr += price_m + TextHandler.Instance.manager.GetTextById(17);
            if (price_s > 0)
                priceStr += price_s + TextHandler.Instance.manager.GetTextById(18);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        if (!gameData.HasEnoughMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS))
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
            return;
        }
        gameData.PayMoney(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS);
        gameData.listWorkerCharacter.Add(characterData);
        GetUIComponent<UITownRecruitment>().RemoveCandidate(characterData);

        UIHandler.Instance.ToastHint<ToastView>(string.Format(TextHandler.Instance.manager.GetTextById(1053), characterData.baseInfo.name));
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {
    }
    #endregion
}
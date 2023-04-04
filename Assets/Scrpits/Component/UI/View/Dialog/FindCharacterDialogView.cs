using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
public class FindCharacterDialogView : DialogView
{
    public GameObject objContent;

    public CharacterUICpt characterUI;
    public Text tvName;
    public Image ivSex;
    public Text tvPrice;
    public Image ivCardBG;
    public Image ivHaloBG;
    public Sprite spSexMan;
    public Sprite spSexWoman;
    public UIPopupAbilityButton popupAbilityButton;
    public Button btContinue;


    public CharacterBean characterData;

    public Color colorForNormal;
    public Color colorForRare;
    public Sprite spBGNormal;
    public Sprite spBGRare;


    public override void Start()
    {
        base.Start();
        StartAnim();
        btContinue.onClick.AddListener(OnClickForContinue);
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    public void StartAnim()
    {
        ui_Submit.gameObject.SetActive(false);
        ui_Cancel.gameObject.SetActive(false);
        btContinue.gameObject.SetActive(false);
        if (objContent != null)
        {
            objContent.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack).OnComplete(delegate ()
            {
                ui_Submit.gameObject.SetActive(true);
                ui_Cancel.gameObject.SetActive(true);
                btContinue.gameObject.SetActive(true);
                ui_Submit.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetEase(Ease.OutBack);
                ui_Cancel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetEase(Ease.OutBack);
                btContinue.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetEase(Ease.OutBack);
                objContent.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 5).SetLoops(-1, LoopType.Yoyo);
            });
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetData(CharacterBean characterData)
    {
        this.characterData = characterData;
        SetName(characterData.baseInfo.name);
        SetSex(characterData.body.sex);
        SetPrice(characterData.baseInfo.priceL, characterData.baseInfo.priceM, characterData.baseInfo.priceS);
        SetCharacterUI(characterData);
        SetPopupInfo(characterData);
        SetBackground((NpcTypeEnum)characterData.baseInfo.characterType);
    }

    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="npcType"></param>
    public void SetBackground(NpcTypeEnum npcType)
    {
        if (ivCardBG == null || ivHaloBG == null )
            return;
        if (npcType == NpcTypeEnum.RecruitRare)
        {
            ivCardBG.sprite = spBGRare;
            ivHaloBG.color = colorForRare;
            tvName.color = colorForRare;
        }
        else
        {
            ivCardBG.sprite = spBGNormal;
            ivHaloBG.color = colorForNormal;
            tvName.color = colorForNormal;
        }
    }

    /// <summary>
    /// 设置弹窗框数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetPopupInfo(CharacterBean characterData)
    {
        if (popupAbilityButton != null)
        {
            popupAbilityButton.SetData(characterData);
        }
    }

    /// <summary>
    /// 设置角色UI
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI != null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
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
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
    public void SetSex(int sex)
    {
        if (ivSex != null)
        {
            if (sex == 1)
            {
                ivSex.sprite = spSexMan;
            }
            else if (sex == 2)
            {
                ivSex.sprite = spSexWoman;
            }
        }
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
    /// 继续点击
    /// </summary>
    public void OnClickForContinue()
    {
        dialogData.remark = "Continue";
        base.SubmitOnClick();
    }
}
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameFavorabilityCpt : ItemGameBaseCpt
{
    public CharacterUICpt characterUI;
    public Image ivSex;
    public Text tvName;
    public Text tvTitleName;
    public Text tvFavorability;

    public GameObject objFavorabilityContainer;
    public Image ivFavorabilityModel;

    public Image ivGift;
    public Image ivTalk;

    public Sprite spMan;
    public Sprite spWoman;
    public Sprite spLove;
    public Sprite spUnLove;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="characterFavorability"></param>
    /// <param name="characterData"></param>
    public void SetData(CharacterFavorabilityBean characterFavorability, CharacterBean characterData )
    {
        SetCharacterUI(characterData);
        SetName(characterData.baseInfo.titleName, characterData.baseInfo.name);
        SetSex(characterData.body.sex);
        SetFavorabilityLevel(characterData.npcInfoData.marry_status,characterFavorability.favorabilityLevel);
        characterFavorability.GetFavorability(out int favorability, out int favorabilityMax);
        SetFavorability(favorability, favorabilityMax);
        bool isTalk=  GameCommonInfo.DailyLimitData.CheckIsTalkNpc(characterFavorability.characterId);
        bool isGift = GameCommonInfo.DailyLimitData.CheckIsGiftNpc(characterFavorability.characterId);
        SetInteractive(isTalk, isGift);
    }

    /// <summary>
    /// 设置角色形象
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
    /// <param name="titleName"></param>
    /// <param name="name"></param>
    public void SetName(string titleName, string name)
    {
        if (tvTitleName != null)
        {
            if (CheckUtil.StringIsNull(titleName))
            {
                tvTitleName.gameObject.SetActive(false);
            }
            else
            {
                tvTitleName.text = titleName;
            }
         
        }
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
        if (ivSex == null)
            return;
        if (sex==1)
        {
            ivSex.sprite = spMan;
        }
        else if (sex == 2)
        {
            ivSex.sprite = spWoman;
        }
    }

    /// <summary>
    /// 设置好感度
    /// </summary>
    /// <param name="favorabilityLevel"></param>
    public void SetFavorabilityLevel(int marryStatus, int favorabilityLevel)
    {
        CptUtil.RemoveChildsByActive(objFavorabilityContainer);
        int loveMax = 5;
        if (marryStatus==1)
        {
            loveMax = 6;
        }
        for (int i = 0; i < loveMax; i++)
        {
            GameObject objItem= Instantiate(objFavorabilityContainer,ivFavorabilityModel.gameObject);
            Image ivItem= objItem.GetComponent<Image>();
            if(i < favorabilityLevel)
            {
                ivItem.sprite = spLove;
            }
            else
            {
                ivItem.sprite = spUnLove;
            }
        }
    }

    /// <summary>
    /// 设置好感
    /// </summary>
    /// <param name="favorability"></param>
    /// <param name="favorabilityMax"></param>
    public void SetFavorability(int favorability,int favorabilityMax)
    {
        if (tvFavorability != null)
        {
            tvFavorability.text = "(" + favorability + "/" + favorabilityMax + ")";
        }
    }

    /// <summary>
    /// 设置互动
    /// </summary>
    /// <param name="isTalk"></param>
    /// <param name="isGift"></param>
    public void SetInteractive(bool isTalk,bool isGift)
    {
        if (ivTalk != null)
        {
            if (isTalk)
                ivTalk.color = new Color(1f,1f,1f,1f);
            else
                ivTalk.color = new Color(1f, 1f, 1f, 0.1f);
        }
        if (ivGift != null)
        {
            if (isGift)
                ivGift.color = new Color(1f, 1f, 1f, 1f);
            else
                ivGift.color = new Color(1f, 1f, 1f, 0.1f);
        }
    }
}
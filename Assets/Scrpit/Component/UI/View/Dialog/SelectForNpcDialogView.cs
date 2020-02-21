using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SelectForNpcDialogView : DialogView
{
    public CharacterUICpt characterUI;

    public GameObject objNpcName;
    public Text tvNpcName;
    public GameObject objNpcType;
    public Text tvNpcType;

    public GameObject objMood;
    public Image ivMood;
    public Text tvMood;
    public GameObject objTeam;
    public GameObject objFriend;


    private NpcAICustomerCpt npcAICustomer;

    private void Update()
    {
        HandleForMood();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="baseNpc"></param>
    public void SetData(BaseNpcAI baseNpc)
    {
        CharacterBean characterData = baseNpc.characterData;
        if (characterData == null)
            return;
        SetName(characterData.baseInfo.name);
        SetCharacterUI(characterData);
        if (baseNpc as NpcAICustomerCpt)
        {
            SetDataForCustomer((NpcAICustomerCpt)baseNpc);
        }
        else if (baseNpc as NpcAIWorkerCpt)
        {
            SetDataForWork((NpcAIWorkerCpt)baseNpc);
        }
        else if (baseNpc as NpcAIRascalCpt)
        {
            SetDataForRascal((NpcAIRascalCpt)baseNpc);
        }
    }

    /// <summary>
    /// 设置角色UI
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI != null)
            characterUI.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (CheckUtil.StringIsNull(name))
        {
            objNpcName.SetActive(false);
        }
        else
        {
            objNpcName.SetActive(true);
            tvNpcName.text = GameCommonInfo.GetUITextById(61) + ":" + name;
        }
    }

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <param name="type"></param>
    public void SetType(string type)
    {
        tvNpcType.text = type;
    }

    /// <summary>
    /// 设置表情
    /// </summary>
    /// <param name="moodStr"></param>
    /// <param name="spMood"></param>
    public void SetMood(string moodStr, Sprite spMood)
    {
        if (ivMood != null)
            ivMood.sprite = spMood;
        if (tvMood != null)
            tvMood.text = moodStr;
    }

    /// <summary>
    /// 设置顾客数据
    /// </summary>
    /// <param name="npcAICustomer"></param>
    public void SetDataForCustomer(NpcAICustomerCpt npcAICustomer)
    {
        this.npcAICustomer = npcAICustomer;
        SetType(GameCommonInfo.GetUITextById(60));
        if (npcAICustomer as NpcAICostomerForFriendCpt)
        {
            objFriend.SetActive(true);
        }
        if (npcAICustomer as NpcAICustomerForGuestTeamCpt)
        {
            NpcAICustomerForGuestTeamCpt npcTeam = (NpcAICustomerForGuestTeamCpt)npcAICustomer;
            if (!CheckUtil.StringIsNull(npcTeam.teamCode))
            {
                objTeam.SetActive(true);
            }
        }
        objMood.SetActive(true);
    }

    /// <summary>
    /// 设置工作者数据
    /// </summary>
    /// <param name="npcAIWorker"></param>
    public void SetDataForWork(NpcAIWorkerCpt npcAIWorker)
    {
        SetType(GameCommonInfo.GetUITextById(63));
    }

    /// <summary>
    /// 设置捣乱者数据
    /// </summary>
    /// <param name="npcAIRascal"></param>
    public void SetDataForRascal(NpcAIRascalCpt npcAIRascal)
    {
        SetType(GameCommonInfo.GetUITextById(59));
    }

    /// <summary>
    /// 表情处理
    /// </summary>
    public void HandleForMood()
    {
        if (npcAICustomer != null && npcAICustomer.orderForCustomer != null)
        {
            PraiseTypeEnum praiseType = npcAICustomer.orderForCustomer.innEvaluation.GetPraise();
            string praiseTypeStr = npcAICustomer.orderForCustomer.innEvaluation.GetPraiseDetails();
            SetMood(praiseTypeStr, npcAICustomer.characterMoodCpt.GetCurrentMoodSprite());
        }
    }


}
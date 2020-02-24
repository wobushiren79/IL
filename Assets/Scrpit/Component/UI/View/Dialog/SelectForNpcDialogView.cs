using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SelectForNpcDialogView : DialogView, IBaseObserver
{
    public CharacterUICpt characterUI;

    //姓名
    public GameObject objNpcName;
    public Text tvNpcName;
    //类型
    public GameObject objNpcType;
    public Text tvNpcType;
    //状态
    public GameObject objNpcStatus;
    public Text tvStatus;
    //心情
    public GameObject objMood;
    public Image ivMood;
    public Text tvMood;

    //团队
    public GameObject objTeam;
    public Image ivTeam;
    public Text tvTeamName;
    //好友
    public GameObject objFriend;

    //功能
    public GameObject objFunction;
    public Button btExpel;

    protected BaseNpcAI targetNpcAI;
    protected NpcAICustomerCpt targetNpcAIForCustomer;
    protected NpcAIWorkerCpt targetNpcAIForWorker;

    public override void Start()
    {
        base.Start();
        if (btExpel != null)
            btExpel.onClick.AddListener(OnClickExpel);
    }

    private void Update()
    {
        HandleForMood();
    }

    private void OnDestroy()
    {
        //移除通知
        if (targetNpcAI != null)
            targetNpcAI.RemoveObserver(this);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="baseNpc"></param>
    public void SetData(BaseNpcAI baseNpc)
    {
        this.targetNpcAI = baseNpc;
        //添加通知
        baseNpc.AddObserver(this);

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
    /// 设置状态
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(string status)
    {
        if (tvStatus != null)
            tvStatus.text = GameCommonInfo.GetUITextById(74) + ":" + status;
    }

    /// <summary>
    /// 设置顾客数据
    /// </summary>
    /// <param name="npcAICustomer"></param>
    public void SetDataForCustomer(NpcAICustomerCpt npcAICustomer)
    {
        this.targetNpcAIForCustomer = npcAICustomer;
        //设置类型
        if (npcAICustomer.orderForCustomer == null)
        {
            SetType(GameCommonInfo.GetUITextById(70));
        }
        else
        {
            SetType(GameCommonInfo.GetUITextById(60));
            //设置状态
            objNpcStatus.SetActive(true);
            npcAICustomer.GetCustomerStatus(out string customerStatus);
            SetStatus(customerStatus);
        }
        //设置好友
        if (npcAICustomer as NpcAICostomerForFriendCpt)
        {
            objFriend.SetActive(true);
        }
        //设置团队
        if (npcAICustomer as NpcAICustomerForGuestTeamCpt)
        {
            NpcAICustomerForGuestTeamCpt npcTeam = (NpcAICustomerForGuestTeamCpt)npcAICustomer;
            if (!CheckUtil.StringIsNull(npcTeam.teamCode))
            {
                objTeam.SetActive(true);
                ivTeam.color = npcTeam.teamColor;
                tvTeamName.text =GameCommonInfo.GetUITextById(49)+":"+ npcTeam.teamData.name;
            }
        }
        ShowCustomerData();
    }

    /// <summary>
    /// 设置工作者数据
    /// </summary>
    /// <param name="npcAIWorker"></param>
    public void SetDataForWork(NpcAIWorkerCpt npcAIWorker)
    {
        this.targetNpcAIForWorker = npcAIWorker;
        SetType(GameCommonInfo.GetUITextById(63));
        //设置状态
        objNpcStatus.SetActive(true);
        npcAIWorker.GetWorkerStatus(out string workerStatus);
        SetStatus(workerStatus);
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
        if (targetNpcAIForCustomer != null)
        {
            if (targetNpcAIForCustomer.orderForCustomer != null)
            {
                PraiseTypeEnum praiseType = targetNpcAIForCustomer.orderForCustomer.innEvaluation.GetPraise();
                string praiseTypeStr = targetNpcAIForCustomer.orderForCustomer.innEvaluation.GetPraiseDetails();
                SetMood(praiseTypeStr, targetNpcAIForCustomer.characterMoodCpt.GetCurrentMoodSprite());
            }
        }
    }

    /// <summary>
    /// 点击-驱除
    /// </summary>
    public void OnClickExpel()
    {
        if (targetNpcAIForCustomer != null)
        {
            targetNpcAIForCustomer.ChangeMood(-99999);
        }
    }

    /// <summary>
    /// 展示客户数据
    /// </summary>
    public void ShowCustomerData()
    {
        if (targetNpcAIForCustomer != null && targetNpcAIForCustomer.orderForCustomer != null)
        {
            objMood.SetActive(true);
            if (targetNpcAIForCustomer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Leave)
            {
                objFunction.SetActive(false);
                btExpel.gameObject.SetActive(false);
            }
            else
            {
                objFunction.SetActive(true);
                btExpel.gameObject.SetActive(true);
            }
        }

    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable == targetNpcAI)
        {
            if (targetNpcAI as NpcAICustomerCpt)
            {
                if (type == (int)NpcAICustomerCpt.CustomerNotifyEnum.StatusChange)
                {
                    SetDataForCustomer((NpcAICustomerCpt)targetNpcAI);
                }
            }
            else if (targetNpcAI as NpcAIWorkerCpt)
            {
                if (type == (int)NpcAIWorkerCpt.WorkerNotifyEnum.StatusChange)
                {
                    SetDataForWork((NpcAIWorkerCpt)targetNpcAI);
                }
            }
        }
    }
    #endregion
}
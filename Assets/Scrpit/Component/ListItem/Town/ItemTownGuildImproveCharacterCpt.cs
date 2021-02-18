using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using static GameControlHandler;

public class ItemTownGuildImproveCharacterCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    [Header("控件")]
    public CharacterUICpt characterUICpt;
    public Image ivWorker;
    public Text tvName;

    public Text tvLowLevelName;
    public Image ivLowLevelIcon;
    public Text tvHighLevelName;
    public Image ivHighLevelIcon;

    public Image ivMoney;
    public Text tvMoney;
    public Text tvTime;

    public Button btSubmit;
    public Button btSend;

    [Header("数据")]
    public Sprite spWorkerChef;
    public Sprite spWorkerWaiter;
    public Sprite spWorkerAccountant;
    public Sprite spWorkerAccost;
    public Sprite spWorkerBeater;

    public Sprite spWorkerLevel_1;
    public Sprite spWorkerLevel_2;
    public Sprite spWorkerLevel_3;
    public Sprite spWorkerLevel_4;
    public Sprite spWorkerLevel_5;
    public Sprite spWorkerLevel_6;

    public Sprite spMoneyL;
    public Sprite spMoneyM;
    public Sprite spMoneyS;

    public StoreInfoBean levelData;
    public CharacterBean characterData;
    public WorkerEnum workerType;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickForImprove);
        if (btSend != null)
            btSend.onClick.AddListener(OnClickForSend);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="characterData"></param>
    /// <param name="workerData"></param>
    public void SetData(WorkerEnum workerType, CharacterBean characterData, CharacterWorkerBaseBean workerData, StoreInfoBean levelData)
    {
        this.workerType = workerType;
        this.levelData = levelData;
        this.characterData = characterData;
        SetWorkerIcon(workerType);
        SetName(characterData.baseInfo.name);
        SetCharacter(characterData);
        SetLowLevelData(workerType, workerData.workerLevel);
        SetHighLevelData(workerType, workerData.workerLevel + 1);
        SetMoney(levelData.price_l, levelData.price_m, levelData.price_s);
        SetTime(int.Parse(levelData.mark));
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacter(CharacterBean characterData)
    {
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置职业图标
    /// </summary>
    /// <param name="spWorker"></param>
    public void SetWorkerIcon(WorkerEnum workerType)
    {
        Sprite spWorker = null;
        switch (workerType)
        {
            case WorkerEnum.Chef:
                spWorker = spWorkerChef;
                break;
            case WorkerEnum.Waiter:
                spWorker = spWorkerWaiter;
                break;
            case WorkerEnum.Accountant:
                spWorker = spWorkerAccountant;
                break;
            case WorkerEnum.Accost:
                spWorker = spWorkerAccost;
                break;
            case WorkerEnum.Beater:
                spWorker = spWorkerBeater;
                break;
        }
        if (ivWorker != null)
            ivWorker.sprite = spWorker;
    }

    /// <summary>
    /// 设置低等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="spLevel"></param>
    public void SetLowLevelData(WorkerEnum workerType, int level)
    {
        GetLevelData(workerType, level, out string name, out Sprite spLevel);
        if (tvLowLevelName != null)
            tvLowLevelName.text = name;
        if (ivLowLevelIcon != null)
        {
            if (spLevel == null)
                ivLowLevelIcon.color = new Color(0, 0, 0, 0);
            else
                ivLowLevelIcon.sprite = spLevel;
        }
    }

    /// <summary>
    /// 设置高等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="spLevel"></param>
    public void SetHighLevelData(WorkerEnum workerType, int level)
    {
        GetLevelData(workerType, level, out string name, out Sprite spLevel);
        if (tvHighLevelName != null)
            tvHighLevelName.text = name;
        if (ivHighLevelIcon != null)
            if (spLevel == null)
                ivHighLevelIcon.color = new Color(0, 0, 0, 0);
            else
                ivHighLevelIcon.sprite = spLevel;
    }

    /// <summary>
    /// 获取等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="name"></param>
    /// <param name="spLevel"></param>
    private void GetLevelData(WorkerEnum workerType, int level, out string name, out Sprite spLevel)
    {
        string workName = CharacterWorkerBaseBean.GetWorkerName(workerType);
        string workLevelName = CharacterWorkerBaseBean.GetWorkerLevelName(level);
        name = workLevelName + workName;
        spLevel = null;
        switch (level)
        {
            case 0:
                break;
            case 1:
                spLevel = spWorkerLevel_1;
                break;
            case 2:
                spLevel = spWorkerLevel_2;
                break;
            case 3:
                spLevel = spWorkerLevel_3;
                break;
            case 4:
                spLevel = spWorkerLevel_4;
                break;
            case 5:
                spLevel = spWorkerLevel_5;
                break;
            case 6:
                spLevel = spWorkerLevel_6;
                break;
        }
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (moneyL != 0)
        {
            ivMoney.sprite = spMoneyL;
            tvMoney.text = moneyL + "";
        }
        else if (moneyM != 0)
        {
            ivMoney.sprite = spMoneyM;
            tvMoney.text = moneyM + "";
        }
        else if (moneyS != 0)
        {
            ivMoney.sprite = spMoneyS;
            tvMoney.text = moneyS + "";
        }
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(int time)
    {
        if (tvTime != null)
            tvTime.text = time + TextHandler.Instance.manager.GetTextById(37);
    }

    /// <summary>
    /// 晋升确认
    /// </summary>
    public void  OnClickForImprove()
    {
        ImproveCheck(0);
    }

    /// <summary>
    /// 派遣晋升
    /// </summary>
    public void OnClickForSend()
    {
        ImproveCheck(1);
    }

    /// <summary>
    /// 晋升确认
    /// </summary>
    protected void ImproveCheck(int type)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //判断是否有足够的金钱       
        GameDataBean gameData= GameDataHandler.Instance.manager.GetGameData();
        if (!gameData.HasEnoughMoney(levelData.price_l, levelData.price_m, levelData.price_s))
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1005));
            return;
        }
        //判断时间是否过晚
        GameTimeHandler.Instance.GetTime(out float hour, out float min);
        if (hour >= 18 || hour < 6)
        {
            ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1031));
            return;
        }

        DialogBean dialogData = new DialogBean();
        string contentStr = "???";
        if (type == 0)
        {
             contentStr = string.Format(TextHandler.Instance.manager.GetTextById(3008), tvTime.text, tvName.text, tvLowLevelName.text, tvHighLevelName.text);
        }
        else if (type == 1)
        {
             contentStr = string.Format(TextHandler.Instance.manager.GetTextById(3015), tvTime.text, tvName.text, tvLowLevelName.text, tvHighLevelName.text);
        }
        dialogData.content = contentStr;
        dialogData.dialogPosition = type;
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //支付金钱
        gameData.PayMoney(levelData.price_l, levelData.price_m, levelData.price_s);
        //扣除时间
        int preGameTime = int.Parse(levelData.mark);
        //扣除时间
        GameTimeHandler.Instance.AddHour(preGameTime);
        //如果有研究菜谱 菜谱增加经验
        GameDataHandler.Instance.AddTimeProcess(preGameTime * 60);
        //判断玩哪个游戏
        MiniGameBaseBean miniGameData = null;
        switch (workerType)
        {
            case WorkerEnum.Chef:
                miniGameData = InitChefGame();
                break;
            case WorkerEnum.Waiter:
                //设置弹幕游戏数据
                miniGameData = InitWaiterGame();
                break;
            case WorkerEnum.Accountant:
                //设置算账游戏
                miniGameData = InitAccountantGame();
                break;
            case WorkerEnum.Accost:
                //设置辩论游戏
                miniGameData = InitAccostGame();
                break;
            case WorkerEnum.Beater:
                miniGameData = InitBeaterGame();
                break;
            default:
                break;
        }
        miniGameData.preGameTime = preGameTime;
        miniGameData.gameReason = MiniGameReasonEnum.Improve;
        if (dialogBean.dialogPosition == 0)
        {
            //设置竞技场数据
            GameCommonInfo.SetArenaPrepareData(miniGameData);
            //保存之前的位置
            GameCommonInfo.ScenesChangeData.beforeUserPosition = GameControlHandler.Instance.manager.GetControl<BaseControl>(ControlEnum.Normal).transform.position;
            //跳转到竞技场
            GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameArenaScene);
        }
        else
        {


            bool isWin = characterData.CalculationGuildSendWin(miniGameData.gameType);
            if (isWin)
            {
                ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(7021));
                AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
                //完成奖励
                RewardTypeEnumTools.CompleteReward(miniGameData.GetListUserCharacterData(), miniGameData.listReward);

                //数据添加
                Sprite attributeIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("keyboard_button_up_1");
                string attributeRewardContent = "";
                foreach (MiniGameCharacterBean miniGameCharacterData in miniGameData.listUserGameData)
                {
                    switch (miniGameData.gameType)
                    {
                        case MiniGameEnum.Cooking:
                            attributeRewardContent = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Cook) + " +5";
                            miniGameCharacterData.characterData.baseInfo.chefInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Barrage:
                            attributeRewardContent = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Speed) + " +5";
                            miniGameCharacterData.characterData.baseInfo.waiterInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Account:
                            attributeRewardContent = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Account) + " +5";
                            miniGameCharacterData.characterData.baseInfo.accountantInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Debate:
                            attributeRewardContent = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm) + " +5";
                            miniGameCharacterData.characterData.baseInfo.accostInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                        case MiniGameEnum.Combat:
                            attributeRewardContent = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Force) + " +5";
                            miniGameCharacterData.characterData.baseInfo.beaterInfo.LevelUp(miniGameCharacterData.characterData.attributes);
                            break;
                    }
                }
                ToastHandler.Instance.ToastHint(attributeIcon, attributeRewardContent);
                //刷新UI
                ((UITownGuildImprove)uiComponent).RefreshUI();
            }
            else
            {
                ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(7022));
                AudioHandler.Instance.PlaySound(AudioSoundEnum.Passive);
            }
        }
    }

    /// <summary>
    /// 初始化跑堂游戏
    /// </summary>
    private MiniGameBaseBean InitWaiterGame()
    {
        MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Barrage);
        miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, levelData.pre_data_minigame, characterData);
        return miniGameData;
    }

    /// <summary>
    /// 初始化计算考试
    /// </summary>
    private MiniGameBaseBean InitAccountantGame()
    {
        MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Account);
        miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, levelData.pre_data_minigame, characterData);
        return miniGameData;
    }

    /// <summary>
    /// 初始化吆喝考试
    /// </summary>
    private MiniGameBaseBean InitAccostGame()
    {
        MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Debate);
        miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, levelData.pre_data_minigame, characterData);
        return miniGameData;
    }

    /// <summary>
    /// 初始化打手考试
    /// </summary>
    private MiniGameBaseBean InitBeaterGame()
    {
        MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Combat);
        miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, levelData.pre_data_minigame, characterData);
        return miniGameData;
    }

    /// <summary>
    /// 初始化厨师考试
    /// </summary>
    private MiniGameBaseBean InitChefGame()
    {
        MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Cooking);
        miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, levelData.pre_data_minigame);
        //先清除数据中的指定敌人
        miniGameData.listEnemyGameData.Clear();
        //随机生成敌人
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        CharacterWorkerBaseBean characterWorkerData= characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Chef);
        int equipLevel = (characterWorkerData.GetLevel() + 1) / 2;
        for (int i = 0; i < UnityEngine.Random.Range(1, 16); i++)
        {
            CharacterBean randomEnemy = CharacterBean.CreateRandomEnemyData(100, 10 , equipLevel);
            listEnemyData.Add(randomEnemy);
        }
        miniGameData.InitData(characterData, listEnemyData);
        return miniGameData;
    }


    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
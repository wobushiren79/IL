using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

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

    [Header("数据")]
    public Sprite spWorkerChef;
    public Sprite spWorkerWaiter;
    public Sprite spWorkerAccounting;
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
            btSubmit.onClick.AddListener(ImproveCheck);
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
            case WorkerEnum.Accounting:
                spWorker = spWorkerAccounting;
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
            tvTime.text = time + GameCommonInfo.GetUITextById(37);
    }

    /// <summary>
    /// 晋升确认
    /// </summary>
    public void ImproveCheck()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        //判断是否有足够的金钱
        if (!gameDataManager.gameData.HasEnoughMoney(levelData.price_l, levelData.price_m, levelData.price_s))
        {
            ToastView toastView = GetUIManager<UIGameManager>().toastView;
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        //判断时间是否过晚
        gameTimeHandler.GetTime(out float hour, out float min);
        if (hour >= 18 && hour < 6)
        {
            ToastView toastView = GetUIManager<UIGameManager>().toastView;
            toastView.ToastHint(GameCommonInfo.GetUITextById(1031));
            return;
        }

        DialogBean dialogData = new DialogBean();
        string contentStr = string.Format(GameCommonInfo.GetUITextById(3008), tvTime.text, tvName.text, tvLowLevelName.text, tvHighLevelName.text);
        dialogData.content = contentStr;
        dialogManager.CreateDialog(0, this, dialogData);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        ControlHandler controlHandler = GetUIManager<UIGameManager>().controlHandler;
        //支付金钱
        gameDataManager.gameData.PayMoney(levelData.price_l, levelData.price_m, levelData.price_s);

        //设置竞技场数据
        GameCommonInfo.ArenaPrepareData = new ArenaPrepareBean();
        //判断玩哪个游戏
        MiniGameEnum gameType;
        switch (workerType)
        {
            case WorkerEnum.Chef:
                gameType = MiniGameEnum.Cooking;
                break;
            case WorkerEnum.Waiter:
                //设置弹幕游戏数据
                gameType = MiniGameEnum.Barrage;
                GameCommonInfo.ArenaPrepareData.gameBarrageData = new MiniGameBarrageBean
                {
                    gameReason = MiniGameReasonEnum.Improve,
                    gameLevel = levelData.mark_type,
                    winLife = levelData.minigame_win_life,
                    winSurvivalTime = levelData.minigame_win_survivaltime,
                    launchInterval = levelData.barrage_launch_interval,
                    launchSpeed = levelData.barrage_launch_speed,
                    launchTypes = StringUtil.SplitBySubstringForArrayEnum<BarrageEjectorCpt.LaunchTypeEnum>(levelData.barrage_launch_types, ',')
                };
                GameCommonInfo.ArenaPrepareData.gameBarrageData.InitData(gameItemsManager, characterData);
                break;
            case WorkerEnum.Accounting:
                gameType = MiniGameEnum.Barrage;
                break;
            case WorkerEnum.Accost:
                gameType = MiniGameEnum.Barrage;
                break;
            case WorkerEnum.Beater:
                gameType = MiniGameEnum.Barrage;
                break;
            default:
                gameType = MiniGameEnum.Barrage;
                break;
        }
        GameCommonInfo.ArenaPrepareData.gameType = gameType;
        //保存之前的位置
        GameCommonInfo.ScenesChangeData.beforeUserPosition = controlHandler.GetControl(ControlHandler.ControlEnum.Normal).transform.position;
        //跳转到竞技场
        SceneUtil.SceneChange(ScenesEnum.GameArenaScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
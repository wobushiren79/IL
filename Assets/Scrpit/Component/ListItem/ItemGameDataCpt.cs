using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameDataCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Text tvInnName;
    public Text tvUserName;
    public Text tvGameTime;
    public Text tvPlayTime;
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;
    public Text tvGuildCoin;

    public Button btContinue;
    public Button btDelete;

    public CharacterUICpt characterUI;
    public GameDataSimpleBean gameData;

    protected AudioHandler audioHandler;
    protected DialogManager dialogManager;

    private void Awake()
    {
        audioHandler = GetUIManager<UIGameManager>().audioHandler;
        dialogManager = GetUIManager<UIGameManager>().dialogManager;
    }

    private void Start()
    {
        if (btContinue != null)
            btContinue.onClick.AddListener(GameContinue);
        if (btDelete != null)
            btDelete.onClick.AddListener(GameDataDelete);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="gameData"></param>
    public void SetData(GameDataSimpleBean gameData)
    {
        this.gameData = gameData;
        SetCharacterUI(gameData.userCharacter);
        SetName(gameData.innName, gameData.userCharacter.baseInfo.name);
        SetMoney(gameData.moneyL, gameData.moneyM, gameData.moneyS);
        SetGameTime(gameData.gameTime.year, gameData.gameTime.month, gameData.gameTime.day);
        SetGuildCoin(gameData.guildCoin);
        SetPlayTime(gameData.playTime.hour, gameData.playTime.minute);
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (gameData.userCharacter != null && characterData != null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="innName"></param>
    /// <param name="userName"></param>
    public void SetName(string innName, string userName)
    {
        if (tvInnName != null)
            tvInnName.text = innName;
        if (tvUserName != null)
            tvUserName.text = GameCommonInfo.GetUITextById(58) + ":" + userName;
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (tvMoneyL != null)
            tvMoneyL.text = "" + moneyL;
        if (tvMoneyM != null)
            tvMoneyM.text = "" + moneyM;
        if (tvMoneyS != null)
            tvMoneyS.text = "" + moneyS;
    }

    /// <summary>
    /// 设置公会英贝利
    /// </summary>
    /// <param name="number"></param>
    public void SetGuildCoin(long number)
    {
        if (tvGuildCoin != null)
            tvGuildCoin.text = "" + number;
    }

    /// <summary>
    /// 设置游戏时间
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public void SetGameTime(int year, int month, int day)
    {
        if (tvGameTime != null)
        {
            tvGameTime.text =
                year + GameCommonInfo.GetUITextById(29) +
                month + GameCommonInfo.GetUITextById(30) +
                day + GameCommonInfo.GetUITextById(31);
        }
    }

    /// <summary>
    /// 设置游玩时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    public void SetPlayTime(int hour, int min)
    {
        if (tvPlayTime != null)
        {
            tvPlayTime.text = GameCommonInfo.GetUITextById(48) + " " + hour + ":" + min;
        }
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void GameContinue()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForNormal);

        GameCommonInfo.GameUserId = gameData.userId;
        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void GameDataDelete()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForBack);

        DialogBean dialogData = new DialogBean();
        dialogData.content = GameCommonInfo.GetUITextById(3011);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
    }

    #region 弹窗确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        gameDataManager.DeleteGameDataByUserId(gameData.userId);
        Destroy(gameObject);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}
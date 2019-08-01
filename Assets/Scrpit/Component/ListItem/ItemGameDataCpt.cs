using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameDataCpt : ItemGameBaseCpt
{
    public Text tvInnName;
    public Text tvUserName;
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    public Button btContinue;
    public Button btDelete;

    public CharacterUICpt characterUI;
    public GameDataSimpleBean gameData;

    private void Start()
    {
        if (btContinue != null)
            btContinue.onClick.AddListener(GameContinue);
        if (btDelete != null)
            btDelete.onClick.AddListener(GameDataDelete);
    }

    public void SetData(GameDataSimpleBean gameData)
    {
        this.gameData = gameData;
        if (gameData.userCharacter != null && gameData.userCharacter.body != null)
        {
            characterUI.SetCharacterData(gameData.userCharacter.body, gameData.userCharacter.equips);
        }
        if (tvInnName != null)
            tvInnName.text = gameData.innName;
        if (tvUserName != null && gameData.userCharacter != null && gameData.userCharacter.baseInfo != null)
            tvUserName.text = gameData.userCharacter.baseInfo.name;
        long lMoney = gameData.moneyL;
        long mMoney = gameData.moneyM;
        long sMoney = gameData.moneyS;
        if (tvMoneyL != null)
            tvMoneyL.text = "" + lMoney;
        if (tvMoneyM != null)
            tvMoneyM.text = "" + mMoney;
        if (tvMoneyS != null)
            tvMoneyS.text = "" + sMoney;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void GameContinue()
    {
        GameCommonInfo.gameUserId = gameData.userId;
        SceneUtil.SceneChange(EnumUtil.GetEnumName(ScenesEnum.GameInnScene));
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void GameDataDelete()
    {
        GameDataManager gameDataManager=  GetUIManager<UIGameManager>().gameDataManager;
        gameDataManager.DeleteGameDataByUserId(gameData.userId);
        Destroy(gameObject);
    }
}
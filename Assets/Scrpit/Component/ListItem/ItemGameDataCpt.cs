using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameDataCpt : BaseMonoBehaviour
{

    public Text tvInnName;
    public Text tvUserName;
    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    public Button btContinue;

    public CharacterHeadUICpt characterHeadUI;
    public GameDataSimpleBean gameData;

    private void Start()
    {
        if (btContinue != null)
            btContinue.onClick.AddListener(GameContinue);
    }

    public void SetData(GameDataSimpleBean gameData)
    {
        this.gameData = gameData;
        if (gameData.userCharacter != null && gameData.userCharacter.body != null)
        {
            characterHeadUI.SetCharacterData(gameData.userCharacter.body);
        }
        if (tvInnName != null)
            tvInnName.text = gameData.innName;
        if (tvUserName != null && gameData.userCharacter != null && gameData.userCharacter.baseInfo != null)
            tvUserName.text = gameData.userCharacter.baseInfo.name;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void GameContinue()
    {
        GameCommonInfo.gameUserId = gameData.userId;
    }


}
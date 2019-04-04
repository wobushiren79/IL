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
    public Button btDelete;

    public CharacterHeadUICpt characterHeadUI;
    public GameDataSimpleBean gameData;
    public MainScenesManager scenesManager;

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
            characterHeadUI.SetCharacterData(gameData.userCharacter.body);
        }
        if (tvInnName != null)
            tvInnName.text = gameData.innName;
        if (tvUserName != null && gameData.userCharacter != null && gameData.userCharacter.baseInfo != null)
            tvUserName.text = gameData.userCharacter.baseInfo.name;
        long lMoney;
        long mMoney;
        long sMoney;
        GameDataBean.GetMoneyDetails(gameData.money, out lMoney, out mMoney, out sMoney);
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
        SceneUtil.SceneChange("GameInnScene");
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void GameDataDelete()
    {
        scenesManager.DeleteGameDataByUserId(gameData.userId);
        Destroy(gameObject);
    }
}
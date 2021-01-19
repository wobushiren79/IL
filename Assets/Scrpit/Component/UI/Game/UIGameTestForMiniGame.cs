using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIGameTestForMiniGame : BaseUIComponent
{
    public GameObject objMiniGameCombat;
    public Button btCombat;

    public InputField etCombatPlayerIds;
    public InputField etCombatEnemyIds;
    public Button btCombatBuild;



    protected MiniGameCombatHandler miniGameCombatHandler;

    public void InitUI()
    {
        objMiniGameCombat.gameObject.SetActive(false);
    }

    private void Start()
    {
        miniGameCombatHandler = Find<MiniGameCombatHandler>( ImportantTypeEnum.MiniGameHandler);
        btCombat.onClick.AddListener(OnClickShowCombatUI);
        btCombatBuild.onClick.AddListener(OnClickCombatBuild);
    }

    /// <summary>
    /// 展示战斗设置UI
    /// </summary>
    public void OnClickShowCombatUI()
    {
        InitUI();
        objMiniGameCombat.gameObject.SetActive(true);    
    }

    /// <summary>
    /// 生成战斗
    /// </summary>
    public void OnClickCombatBuild()
    {
        MiniGameCombatBean miniGameCombat = (MiniGameCombatBean)MiniGameEnumTools.GetMiniGameData(MiniGameEnum.Combat);
        if (CheckUtil.StringIsNull(etCombatPlayerIds.text))
        {
            LogUtil.LogError("没有玩家ID");
            return;
        }
        if (CheckUtil.StringIsNull(etCombatPlayerIds.text))
        {
            LogUtil.LogError("没有敌人ID");
            return;
        }
        List<long> playerIds = GetIds(etCombatPlayerIds.text);
        List<long> EnemyIds = GetIds(etCombatEnemyIds.text);
        miniGameCombat.winBringDownNumber = EnemyIds.Count;
        miniGameCombat.winSurvivalNumber = 1;
        List<CharacterBean> listOurData = new List<CharacterBean>();
        foreach (long id in playerIds)
        {
            listOurData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(id));
        }
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        foreach (long id in EnemyIds)
        {
            listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(id));
        }
        miniGameCombat.InitData(listOurData, listEnemyData);
        //找到竞技场战斗的地点
        miniGameCombat.miniGamePosition = new Vector3(0, 10, 0);
        //初始化游戏
        miniGameCombatHandler.InitGame(miniGameCombat);
    }




    protected List<long> GetIds( string idsStr)
    {
        long[] ids= StringUtil.SplitBySubstringForArrayLong(idsStr,',');
        return ids.ToList();
    }

}
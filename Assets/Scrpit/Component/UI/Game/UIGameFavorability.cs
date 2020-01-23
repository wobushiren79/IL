using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameFavorability : BaseUIComponent
{
    public Button btBack;

    public GameObject objFavorabilityContainer;
    public GameObject objFavorabilityModel;

    private void Start()
    {
        if (btBack!=null)
        {
            btBack.onClick.AddListener(OpenMainUI);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameDataManager gameDataManager =  GetUIManager<UIGameManager>().gameDataManager;
        NpcInfoManager npcInfoManager = GetUIManager<UIGameManager>().npcInfoManager;

        List<CharacterFavorabilityBean> listData =  gameDataManager.gameData.listCharacterFavorability;
        if (listData == null)
            return;
        CptUtil.RemoveChildsByActive(objFavorabilityContainer);
        foreach (CharacterFavorabilityBean itemData in listData)
        {
            CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemData.characterId);
            //只显示小镇居民数据
            if (characterData.npcInfoData.npc_type != (int)NPCTypeEnum.Town)
                continue;
            //只显示好感1以上的
            if (itemData.favorability<=0)
                continue;
            GameObject objFavorability= Instantiate(objFavorabilityContainer, objFavorabilityModel);
            ItemGameFavorabilityCpt itemFavorability = objFavorability.GetComponent<ItemGameFavorabilityCpt>();
            itemFavorability.SetData(itemData, characterData);
        }
    }

    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}
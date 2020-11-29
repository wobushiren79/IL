using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class UIGameFavorability : UIGameComponent
{
    public Button btBack;

    public ScrollGridVertical gridVertical;
    public List<CharacterFavorabilityBean> listFavorabilityData = new List<CharacterFavorabilityBean>();

    private void Start()
    {
        if (btBack != null)
        {
            btBack.onClick.AddListener(OpenMainUI);
        }
        if (gridVertical != null)
        {
            gridVertical.AddCellListener(OnCellForItem);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        CharacterFavorabilityBean characterFavorability = listFavorabilityData[itemCell.index];
        CharacterBean characterData = uiGameManager.npcInfoManager.GetCharacterDataById(characterFavorability.characterId);
        ItemGameFavorabilityCpt itemFavorability = itemCell.GetComponent<ItemGameFavorabilityCpt>();
        itemFavorability.SetData(characterFavorability, characterData);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        List<CharacterFavorabilityBean> listData = uiGameManager.gameDataManager.gameData.listCharacterFavorability;
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterFavorabilityBean characterFavorability = listData[i];
            CharacterBean characterData = uiGameManager.npcInfoManager.GetCharacterDataById(characterFavorability.characterId);
     
            //只显示小镇居民数据
            if (characterData.npcInfoData.npc_type != (int)NpcTypeEnum.Town)
                continue;
            //只显示好感1以上的
            if (characterFavorability.favorability <= 0)
                continue;
            listFavorabilityData.Add(characterFavorability);
        }
        gridVertical.SetCellCount(listFavorabilityData.Count);
    }


    public void OpenMainUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}
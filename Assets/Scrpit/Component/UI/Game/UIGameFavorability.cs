using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class UIGameFavorability : BaseUIComponent
{
    public Button btBack;

    public ScrollGridVertical gridVertical;
    public List<CharacterFavorabilityBean> listFavorabilityData = new List<CharacterFavorabilityBean>();

    public Button ui_ItemSort_Favorability;
    public Button ui_ItemSort_Gift;
    public Button ui_ItemSort_Talk;

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
        if (ui_ItemSort_Favorability)
            ui_ItemSort_Favorability.onClick.AddListener(OnClickForSortFavorability);
        if (ui_ItemSort_Gift)
            ui_ItemSort_Gift.onClick.AddListener(OnClickForSortGift);
        if (ui_ItemSort_Talk)
            ui_ItemSort_Talk.onClick.AddListener(OnClickForSortTalk);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        CharacterFavorabilityBean characterFavorability = listFavorabilityData[itemCell.index];
        CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(characterFavorability.characterId);
        ItemGameFavorabilityCpt itemFavorability = itemCell.GetComponent<ItemGameFavorabilityCpt>();
        itemFavorability.SetData(characterFavorability, characterData);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        listFavorabilityData.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterFavorabilityBean> listData = gameData.listCharacterFavorability;
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterFavorabilityBean characterFavorability = listData[i];
            CharacterBean characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(characterFavorability.characterId);

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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }


    #region 排序
    public void OnClickForSortFavorability()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        listFavorabilityData = listFavorabilityData.OrderByDescending(data =>
        {
            return data.favorability;
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    public void OnClickForSortGift()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        listFavorabilityData = listFavorabilityData.OrderByDescending(data =>
        {
            return GameCommonInfo.DailyLimitData.CheckIsGiftNpc(data.characterId);
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    public void OnClickForSortTalk()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        listFavorabilityData = listFavorabilityData.OrderByDescending(data =>
        {
            return GameCommonInfo.DailyLimitData.CheckIsTalkNpc(data.characterId);
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    #endregion
}
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class UIGameFavorability : UIGameComponent
{
    public Button btBack;

    public GameObject objFavorabilityContainer;
    public GameObject objFavorabilityModel;

    private void Start()
    {
        if (btBack != null)
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

        List<CharacterFavorabilityBean> listData = uiGameManager.gameDataManager.gameData.listCharacterFavorability;
        if (listData == null)
            return;
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objFavorabilityContainer);
        StartCoroutine(CoroutineForCreateList(listData));
    }

    public IEnumerator CoroutineForCreateList(List<CharacterFavorabilityBean> listData)
    {
        foreach (CharacterFavorabilityBean itemData in listData)
        {
            CharacterBean characterData = uiGameManager.npcInfoManager.GetCharacterDataById(itemData.characterId);
            //只显示小镇居民数据
            if (characterData.npcInfoData.npc_type != (int)NpcTypeEnum.Town)
                continue;
            //只显示好感1以上的
            if (itemData.favorability <= 0)
                continue;
            GameObject objFavorability = Instantiate(objFavorabilityContainer, objFavorabilityModel);
            ItemGameFavorabilityCpt itemFavorability = objFavorability.GetComponent<ItemGameFavorabilityCpt>();
            itemFavorability.SetData(itemData, characterData);
            yield return new WaitForEndOfFrame();
        }
    }

    public void OpenMainUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }
}
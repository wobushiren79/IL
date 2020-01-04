using UnityEngine;
using UnityEditor;

public class ItemTownGuildStoreCpt : ItemTownStoreForGoodsCpt
{


    public override void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataManager gameDataManager=   GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughGuildCoin(storeInfo.guild_coin))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1012));
            return;
        }
        gameDataManager.gameData.PayGuildCoin(storeInfo.guild_coin);
        toastManager.ToastHint(ivIcon.sprite, string.Format(GameCommonInfo.GetUITextById(1010), itemsInfo.name));
        gameDataManager.gameData.AddNewItems(storeInfo.mark_id, 1);
        RefreshUI();
    }


}
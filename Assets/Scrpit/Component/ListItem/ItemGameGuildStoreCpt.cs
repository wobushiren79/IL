using UnityEngine;
using UnityEditor;

public class ItemGameGuildStoreCpt : ItemGameGroceryCpt
{


    public override void Submit(DialogView dialogView)
    {
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughGuildCoin(storeInfo.guild_coin))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1012));
            return;
        }
        gameDataManager.gameData.PayGuildCoin(storeInfo.guild_coin);
        toastView.ToastHint(ivIcon.sprite, string.Format(GameCommonInfo.GetUITextById(1010), itemsInfo.name));
        gameDataManager.gameData.AddNewItems(storeInfo.mark_id, 1);
        RefreshUI();
    }


}
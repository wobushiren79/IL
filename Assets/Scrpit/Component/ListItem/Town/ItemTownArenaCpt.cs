using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownArenaCpt : ItemGameBaseCpt
{
    public Text tvTitle;

    public void SetData(MiniGameBaseBean miniGameData)
    {
        InitDataForType(miniGameData.gameType, miniGameData);
    }

    public void InitDataForType(MiniGameEnum gameType, MiniGameBaseBean miniGameData)
    {
        SetTitle(miniGameData.GetGameName());
        switch (gameType)
        {
            case MiniGameEnum.Cooking:
        
                break;
            case MiniGameEnum.Barrage:
                break;
            case MiniGameEnum.Account:
                break;
            case MiniGameEnum.Debate:
                break;
            case MiniGameEnum.Combat:
                break;
        }
    }

    public void InitDataForCooking(MiniGameBaseBean miniGameData)
    {
        MiniGameCookingBean miniGameCookingData = (MiniGameCookingBean)miniGameData;
    }

    public void SetTitle(string title)
    {
        if (tvTitle!=null)
        {
            tvTitle.text = title;
        }
    }
}
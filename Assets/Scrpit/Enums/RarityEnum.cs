using UnityEngine;
using UnityEditor;

public enum RarityEnum
{
    Normal = 0,//人
    Rare = 1,//地
    SuperRare = 2,//天
    SuperiorSuperRare = 3,//神
    UltraRare = 4,//荒
}

public class RarityEnumTools
{
    public static void GetRarityDetails(RarityEnum rarity, out string rarityName, out Color rarityColor)
    {
        rarityName = "";
        rarityColor = Color.black;
        switch (rarity)
        {
            case RarityEnum.Normal:
                rarityName = TextHandler.Instance.manager.GetTextById(105);
                rarityColor = new Color(0.81f, 0.27f, 0.1f, 0.3f);
                break;
            case RarityEnum.Rare:
                rarityName = TextHandler.Instance.manager.GetTextById(106);
                rarityColor = new Color(0f, 0.35f, 0.83f, 0.3f);
                break;
            case RarityEnum.SuperRare:
                rarityName = TextHandler.Instance.manager.GetTextById(107);
                rarityColor = new Color(0.8f, 0f, 0.7f, 0.3f);
                break;
            case RarityEnum.SuperiorSuperRare:
                rarityName = TextHandler.Instance.manager.GetTextById(108);
                rarityColor = new Color(0f, 1f, 0f, 0.3f);
                break;
            case RarityEnum.UltraRare:
                rarityName = TextHandler.Instance.manager.GetTextById(109);
                break;
        }
    }
}


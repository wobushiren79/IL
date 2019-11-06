using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemMiniGameCookingSettlementCpt : ItemGameBaseCpt
{
    public CharacterUICpt characterUI;
    public Text tvName;
    public Text tvThemeScore;
    public Text tvColorScore;
    public Text tvSweetScore;
    public Text tvTasteScore;
    public Text tvTotalScore;
    public Text tvRank;
    
    public void SetData(NpcAIMiniGameCookingCpt itemNpc,int rank)
    {
        MiniGameCharacterForCookingBean characterGameData =  itemNpc.characterMiniGameData;
        SetCharacter(characterGameData.characterData);
        SetName(characterGameData.characterData.baseInfo.name);
        SetScore(characterGameData.scoreForTheme,
            characterGameData.scoreForColor,
            characterGameData.scoreForSweet,
            characterGameData.scoreForTaste,
            characterGameData.scoreForTotal);
        SetRank(rank);
    }

    public void SetCharacter(CharacterBean characterData)
    {
        if (characterUI != null)
            characterUI.SetCharacterData(characterData.body, characterData.equips);
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetScore(int themeScore,int colorScore,int sweetScore,int tasteScore,int totalScore)
    {
        if (tvThemeScore != null)
            tvThemeScore.text = themeScore + "";
        if (tvColorScore != null)
            tvColorScore.text = colorScore + "";
        if (tvSweetScore != null)
            tvSweetScore.text = sweetScore + "";
        if (tvTasteScore != null)
            tvTasteScore.text = tasteScore + "";
        if (tvTotalScore != null)
            tvTotalScore.text = totalScore + "";
    }

    public void SetRank(int rank)
    {
        if (tvRank != null)
            tvRank.text = rank+"";
    }
}
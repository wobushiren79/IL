using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InfoFoodPopupShow : PopupShowView
{
    public Text tvTime;

    public GameObject objOilsalt;
    public Text tvIngOilsalt;

    public GameObject objMeat;
    public Text tvIngMeat;

    public GameObject objRiverfresh;
    public Text tvIngRiverfresh;

    public GameObject objSeafood;
    public Text tvIngSeafood;

    public GameObject objVegetables;
    public Text tvIngVegetables;

    public GameObject objMelonfruit;
    public Text tvIngMelonfruit;

    public GameObject objWaterwine;
    public Text tvIngWaterwine;

    public GameObject objFlour;
    public Text tvIngFlour;

    public GameDataManager gameDataManager;

    public MenuOwnBean ownData;
    public MenuInfoBean foodData;



    public override void Update()
    {
        base.Update();
        if (gameDataManager == null)
            return;

        if (foodData.ing_oilsalt <= gameDataManager.gameData.ingOilsalt)
            tvIngOilsalt.color = Color.black;
        else
            tvIngOilsalt.color = Color.red;

        if (foodData.ing_meat <= gameDataManager.gameData.ingMeat)
            tvIngMeat.color = Color.black;
        else
            tvIngMeat.color = Color.red;

        if (foodData.ing_riverfresh <= gameDataManager.gameData.ingRiverfresh)
            tvIngRiverfresh.color = Color.black;
        else
            tvIngRiverfresh.color = Color.red;

        if (foodData.ing_seafood <= gameDataManager.gameData.ingSeafood)
            tvIngSeafood.color = Color.black;
        else
            tvIngSeafood.color = Color.red;

        if (foodData.ing_vegetables <= gameDataManager.gameData.ingVegetables)
            tvIngVegetables.color = Color.black;
        else
            tvIngVegetables.color = Color.red;

        if (foodData.ing_melonfruit <= gameDataManager.gameData.ingMelonfruit)
            tvIngMelonfruit.color = Color.black;
        else
            tvIngMelonfruit.color = Color.red;

        if (foodData.ing_waterwine <= gameDataManager.gameData.ingWaterwine)
            tvIngWaterwine.color = Color.black;
        else
            tvIngWaterwine.color = Color.red;

        if (foodData.ing_flour <= gameDataManager.gameData.ingFlour)
            tvIngFlour.color = Color.black;
        else
            tvIngFlour.color = Color.red;
    }

    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData)
    {
        this.ownData = ownData;
        this.foodData = foodData;

        if (ownData != null && foodData != null)
        {
            if (tvTime != null)
                tvTime.text = foodData.cook_time+"分钟";
            //油烟类
            if (foodData.ing_oilsalt == 0)
                objOilsalt.SetActive(false);
            else
            {
                objOilsalt.SetActive(true);
                if (tvIngOilsalt != null)
                    tvIngOilsalt.text = foodData.ing_oilsalt + "";
            }
            //肉类
            if (foodData.ing_meat == 0)
                objMeat.SetActive(false);
            else
            {
                objMeat.SetActive(true);
                if (tvIngMeat != null)
                    tvIngMeat.text = foodData.ing_meat + "";
            }
            //河鲜
            if (foodData.ing_riverfresh == 0)
                objRiverfresh.SetActive(false);
            else
            {
                objRiverfresh.SetActive(true);
                if (tvIngRiverfresh != null)
                    tvIngRiverfresh.text = foodData.ing_riverfresh + "";
            }
            //海鲜
            if (foodData.ing_seafood == 0)
                objSeafood.SetActive(false);
            else
            {
                objSeafood.SetActive(true);
                if (tvIngSeafood != null)
                    tvIngSeafood.text = foodData.ing_seafood + "";
            }
            //蔬菜
            if (foodData.ing_vegetables == 0)
                objVegetables.SetActive(false);
            else
            {
                objVegetables.SetActive(true);
                if (tvIngVegetables != null)
                    tvIngVegetables.text = foodData.ing_vegetables + "";
            }
            //瓜果
            if (foodData.ing_melonfruit == 0)
                objMelonfruit.SetActive(false);
            else
            {
                objMelonfruit.SetActive(true);
                if (tvIngMelonfruit != null)
                    tvIngMelonfruit.text = foodData.ing_melonfruit + "";
            }
            //酒水
            if (foodData.ing_waterwine == 0)
                objWaterwine.SetActive(false);
            else
            {
                objWaterwine.SetActive(true);
                if (tvIngWaterwine != null)
                    tvIngWaterwine.text = foodData.ing_waterwine + "";
            }
            //面粉
            if (foodData.ing_flour == 0)
                objFlour.SetActive(false);
            else
            {
                objFlour.SetActive(true);
                if (tvIngFlour != null)
                    tvIngFlour.text = foodData.ing_flour + "";
            }
        }
    }
}
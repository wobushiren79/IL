using UnityEngine;
using UnityEditor;

public class CookingThemeBean : BaseBean
{
    public long theme_id;
    public string name;

    public int ing_oilsalt;
    public int ing_meat;
    public int ing_riverfresh;
    public int ing_seafood;

    public int ing_vegetables;
    public int ing_melonfruit;
    public int ing_waterwine;
    public int ing_flour;

    /// <summary>
    /// 获取菜单和主题的相似度
    /// </summary>
    /// <param name="menuInfo"></param>
    public float GetSimilarity(MenuInfoBean menuInfo)
    {
        float similarity = 0;
        int similarityNumber = 0;
        if (ing_oilsalt != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_oilsalt, ing_oilsalt);
            similarityNumber++;
        }
        if (ing_meat != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_meat, ing_meat);
            similarityNumber++;
        }
        if (ing_riverfresh != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_riverfresh, ing_riverfresh);
            similarityNumber++;
        }
        if (ing_seafood != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_seafood, ing_seafood);
            similarityNumber++;
        }
        if (ing_vegetables != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_vegetables, ing_vegetables);
            similarityNumber++;
        }
        if (ing_melonfruit != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_melonfruit, ing_melonfruit);
            similarityNumber++;
        }
        if (ing_waterwine != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_waterwine, ing_waterwine);
            similarityNumber++;
        }
        if (ing_flour != 0)
        {
            similarity += GetIngSimilarity(menuInfo.ing_flour, ing_flour);
            similarityNumber++;
        }
        if (similarityNumber == 0)
            return 1;
        return similarity/ similarityNumber;
    }

    private float GetIngSimilarity(int ingMenu, int ingTheme)
    {
        if (ingMenu >= ingTheme)
        {
            return 1f;
        }
        else
        {
            return (float)ingMenu / (float)ingTheme;
        }
    }
}

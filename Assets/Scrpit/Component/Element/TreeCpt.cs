using UnityEngine;
using UnityEditor;

public class TreeCpt : BaseMonoBehaviour
{
    public SpriteRenderer srTree;

    public Sprite spSpring;
    public Sprite spSummer;
    public Sprite spAutumn;
    public Sprite spWinter;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="seasons"></param>
    public void SetData(SeasonsEnum seasons)
    {
        Sprite spSeasons = null;
        switch (seasons)
        {
            case SeasonsEnum.Spring:
                spSeasons = spSpring;
                break;
            case SeasonsEnum.Summer:
                spSeasons = spSummer;
                break;
            case SeasonsEnum.Autumn:
                spSeasons = spAutumn;
                break;
            case SeasonsEnum.Winter:
                spSeasons = spWinter;
                break;
        }
        srTree.sprite = spSeasons;
    }
}
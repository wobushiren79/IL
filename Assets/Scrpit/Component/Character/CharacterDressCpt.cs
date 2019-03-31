using UnityEngine;
using UnityEditor;

public class CharacterDressCpt : BaseMonoBehaviour
{
    //头
    public SpriteRenderer sprHat;
    //躯干
    public SpriteRenderer sprClothes;
    //脚
    public SpriteRenderer sprShoesLeft;
    public SpriteRenderer sprShoesRight;

    //服装管理
    public CharacterDressManager characterDressManager;


    public void SetHat(long clothesId)
    {

    }
}
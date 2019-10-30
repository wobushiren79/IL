using UnityEngine;
using UnityEditor;

public class MiniGameCharacterBean 
{
    //角色类型 1操作的角色 0敌人
    public int characterType;
    //角色最高血量
    public int characterMaxLife;
    //角色当前血量
    public int characterCurrentLife;

    //角色数据
    public CharacterBean characterData;
    /// <summary>
    /// 增加生命
    /// </summary>
    /// <param name="life"></param>
    public void AddLife(int life)
    {
        characterCurrentLife += life;
        if (characterCurrentLife < 0)
        {
            characterCurrentLife = 0;
        }
    }
  

}
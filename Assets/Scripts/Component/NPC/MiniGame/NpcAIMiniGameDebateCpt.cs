using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class NpcAIMiniGameDebateCpt : BaseNpcAI
{
    public MiniGameCharacterForDebateBean characterMiniGameData;
    //角色血量
    public CharacterLifeCpt characterLifeCpt;
    //伤害特效
    public ParticleSystem psDamage;
    //伤害数字
    public GameObject objDamageModel;

    public void SetData(MiniGameCharacterForDebateBean miniGameCharacterData)
    {
        this.characterMiniGameData = miniGameCharacterData;
        SetCharacterData(miniGameCharacterData.characterData);
        //更新血量显示
        characterLifeCpt.SetData(characterMiniGameData.characterCurrentLife, characterMiniGameData.characterMaxLife);
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(int damage)
    {
        characterMiniGameData.AddLife(-damage);

        ShowTextInfo("-" + damage);
        //流血特效
        if (damage != 0)
        {
            transform.DOKill();
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOShakeScale(1, 0.5f);
        }
        //伤害特效
        psDamage.Play();
        //更新血量显示
        characterLifeCpt.SetData(characterMiniGameData.characterCurrentLife, characterMiniGameData.characterMaxLife);
    }

    /// <summary>
    /// 展示文字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public GameObject ShowTextInfo(string text)
    {
        GameObject objItemDamage = Instantiate(gameObject, objDamageModel);
        JumpTextCpt jumpText = objItemDamage.GetComponent<JumpTextCpt>();
        jumpText.SetData(text, Color.red);
        return objItemDamage;
    }
}
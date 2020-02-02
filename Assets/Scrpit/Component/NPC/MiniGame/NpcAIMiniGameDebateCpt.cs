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
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(int damage)
    {
        characterMiniGameData.ChangeLife(-damage);

        TextMesh tvItem = ShowTextInfo("-" + damage);
        tvItem.characterSize = 0.15f;
        tvItem.color = Color.red;
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
    public TextMesh ShowTextInfo(string text)
    {
        GameObject objItemDamage = Instantiate(gameObject, objDamageModel);
        TextMesh tvItem = objItemDamage.GetComponentInChildren<TextMesh>();
        tvItem.text = text + "";
        //数字特效
        objItemDamage.transform.DOScale(new Vector3(0, 0, 0), 1f).From().SetEase(Ease.OutElastic);
        objItemDamage.transform.DOLocalMoveY(1.5f, 2.5f).OnComplete(delegate ()
        {
            Destroy(objItemDamage);
        });
        DOTween.To(() => 1f, alpha => tvItem.color = new Color(tvItem.color.r, tvItem.color.g, tvItem.color.b, alpha), 0f, 1).SetDelay(4);
        return tvItem;
    }
}
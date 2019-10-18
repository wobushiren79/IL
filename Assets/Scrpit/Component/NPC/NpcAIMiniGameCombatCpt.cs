using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class NpcAIMiniGameCombatCpt : BaseNpcAI
{
    //战斗游戏处理
    public MiniGameCombatHandler gameCombatHandler;
    //角色数据
    public MiniGameCharacterBean characterMiniGameData;
    //角色血量
    public CharacterLifeCpt characterLifeCpt;
    //角色出血特效
    public ParticleSystem psBlood;
    //伤害特效
    public ParticleSystem psDamage;
    //选中特效
    public ParticleSystem psSelected;
    //战斗状态
    public SpriteRenderer srCombatStatus;
    //伤害数字
    public GameObject objDamageModel;

    //防御图标
    public Sprite spStatusDefend;
    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            psSelected.gameObject.SetActive(true);
            psSelected.Play();
        }
        else
            psSelected.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置战斗状态
    /// </summary>
    /// <param name="status">0,没有  1防御</param>
    public void SetCombatStatus(int status)
    {
        srCombatStatus.color = new Color(1, 1, 1, 1);
        switch (status)
        {
            case 0:
                srCombatStatus.color = new Color(1, 1, 1, 0);
                break;
            case 1:
                srCombatStatus.sprite = spStatusDefend;
                srCombatStatus.transform.localScale = new Vector3(1, 1, 1);
                srCombatStatus.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
                break;
            case 2:
                break;
        }

    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="powerLevel">伤害等级</param>
    /// <param name="damage"></param>
    public void UnderAttack(float powerLevel, int damage)
    {
        characterMiniGameData.AddLife(-damage);

        TextMesh tvItem = ShowTextInfo("-" + damage);
        if (powerLevel > 0.8f)
        {
            tvItem.characterSize = 0.15f;
            tvItem.color = Color.red;
        }
        else
        {
            tvItem.characterSize = 0.1f;
            tvItem.color = Color.white;
        }
        //流血特效
        if (damage != 0)
        {
            psBlood.Play();
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
        objItemDamage.transform.DOLocalMoveY(3, 5).OnComplete(delegate ()
        {
            Destroy(objItemDamage);
        });
        DOTween.To(() => 1f, alpha => tvItem.color = new Color(tvItem.color.r, tvItem.color.g, tvItem.color.b, alpha), 0f, 1).SetDelay(4);
        return tvItem;
    }
}
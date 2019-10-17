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
    //选中特效
    public ParticleSystem psSelected;
    //战斗状态
    public SpriteRenderer srCombatStatus;

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
}
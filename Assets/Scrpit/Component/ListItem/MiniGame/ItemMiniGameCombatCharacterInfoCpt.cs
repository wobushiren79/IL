using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemMiniGameCombatCharacterInfoCpt : ItemGameBaseCpt
{
    public CharacterUICpt characterUI;
    public Text tvName;
    public Text tvLife;
    public Text tvForce;
    public Slider sliderLife;

    public GameObject objDead;
    public Image ivDead;
    public Text tvDead;

    public MiniGameCharacterForCombatBean gameCharacterData;
    public int oriForce;

    private void Update()
    {
        if (gameCharacterData != null)
        {
            if (gameCharacterData.characterCurrentLife > 0)
                SetLife(gameCharacterData.characterCurrentLife, gameCharacterData.characterMaxLife);
            else
                SetDead();
            int totalForce = this.gameCharacterData.GetEffectForceRate(oriForce);
            SetForce(totalForce);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void SetData(MiniGameCharacterBean gameCharacterData)
    {
        this.gameCharacterData = (MiniGameCharacterForCombatBean)gameCharacterData;

        SetCharacterUI(this.gameCharacterData.characterData);
        SetName(this.gameCharacterData.characterData.baseInfo.name);
        SetLife(this.gameCharacterData.characterCurrentLife, gameCharacterData.characterMaxLife);

        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        this.gameCharacterData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        oriForce = characterAttributes.force;
        int totalForce= this.gameCharacterData.GetEffectForceRate(oriForce);
        SetForce(totalForce);
    }

    public void SetDead()
    {
        if (!objDead.activeSelf)
        {
            objDead.SetActive(true);
            ivDead.DOFade(0,1).From();
            tvDead.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1).From();
            tvDead.transform.DOLocalRotate(new Vector3(0,0,-45),1).From();
        }
    }

    /// <summary>
    /// 设置角色图标
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI != null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
        }
    }

    /// <summary>
    /// 设置姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置生命值
    /// </summary>
    /// <param name="currentLife"></param>
    /// <param name="maxLife"></param>
    public void SetLife(int currentLife, int maxLife)
    {
        if (tvLife != null)
            tvLife.text = currentLife + "/" + maxLife;
        if (sliderLife != null)
            sliderLife.value = (float)currentLife / (float)maxLife;
    }

    /// <summary>
    /// 设置武力
    /// </summary>
    /// <param name="force"></param>
    public void SetForce(int force)
    {
        if (tvForce != null)
            tvForce.text = "" + force;
    }
}
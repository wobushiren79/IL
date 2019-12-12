using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class InfoAbilityPopupShow : PopupShowView
{
    public Text tvLoyal;
    public Text tvLife;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;

    private GameItemsManager mGameItemsManager;

    private void Awake()
    {
        mGameItemsManager = FindObjectOfType<GameItemsManager>();
    }

    public void SetData(CharacterBean characterData)
    {
        if (characterData == null)
            return;
        characterData.GetAttributes(mGameItemsManager, out CharacterAttributesBean totalAttributes);
        SetLoyal(totalAttributes.loyal);
        SetCook(totalAttributes.cook);
        SetSpeed(totalAttributes.speed);
        SetAccount(totalAttributes.account);
        SetCharm(totalAttributes.charm);
        SetForce(totalAttributes.force);
        SetLucky(totalAttributes.lucky);
        SetLife(totalAttributes.life);
    }

    /// <summary>
    /// 设置生命值
    /// </summary>
    /// <param name="life"></param>
    public void SetLife(int life)
    {
        if (tvLife != null)
            tvLife.text = GameCommonInfo.GetUITextById(9) + " " + life;
    }

    /// <summary>
    /// 设置忠诚
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(int loyal)
    {
        if (tvLoyal != null)
            tvLoyal.text = GameCommonInfo.GetUITextById(8)+" " + loyal;
    }

    /// <summary>
    /// 设置厨力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetCook(int cook)
    {
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + " " + cook;
    }

    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="loyal"></param>
    public void SetSpeed(int speed)
    {
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + " " + speed;
    }

    /// <summary>
    /// 设置计算
    /// </summary>
    /// <param name="loyal"></param>
    public void SetAccount(int account)
    {
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + " " + account;
    }

    /// <summary>
    /// 设置魅力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetCharm(int charm)
    {
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + " " + charm;
    }

    /// <summary>
    /// 设置武力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetForce(int force)
    {
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + " " + force;
    }

    /// <summary>
    /// 设置幸运
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLucky(int lucky)
    {
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + " " + lucky;
    }
}
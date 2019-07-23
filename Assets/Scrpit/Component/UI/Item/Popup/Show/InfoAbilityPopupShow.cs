using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class InfoAbilityPopupShow : PopupShowView
{
    public Text tvLoyal;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;

    public void SetData(CharacterBean characterData)
    {
        if (characterData == null)
            return;
        SetLoyal(characterData.attributes.loyal);
        SetCook(characterData.attributes.cook);
        SetSpeed(characterData.attributes.speed);
        SetAccount(characterData.attributes.account);
        SetCharm(characterData.attributes.charm);
        SetForce(characterData.attributes.force);
        SetLucky(characterData.attributes.lucky);
    }

    /// <summary>
    /// 设置忠诚
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(int loyal)
    {
        if (tvLoyal != null)
            tvLoyal.text = "忠诚 " + loyal;
    }

    /// <summary>
    /// 设置厨力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetCook(int cook)
    {
        if (tvCook != null)
            tvCook.text = "厨力 " + cook;
    }

    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="loyal"></param>
    public void SetSpeed(int speed)
    {
        if (tvSpeed != null)
            tvSpeed.text = "速度 " + speed;
    }

    /// <summary>
    /// 设置计算
    /// </summary>
    /// <param name="loyal"></param>
    public void SetAccount(int account)
    {
        if (tvAccount != null)
            tvAccount.text = "计算 " + account;
    }

    /// <summary>
    /// 设置魅力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetCharm(int charm)
    {
        if (tvCharm != null)
            tvCharm.text = "魅力 " + charm;
    }

    /// <summary>
    /// 设置武力
    /// </summary>
    /// <param name="loyal"></param>
    public void SetForce(int force)
    {
        if (tvForce != null)
            tvForce.text = "武力 " + force;
    }

    /// <summary>
    /// 设置幸运
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLucky(int lucky)
    {
        if (tvLucky != null)
            tvLucky.text = "幸运 " + lucky;
    }
}
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameWorkerCpt : BaseMonoBehaviour
{
    public Text tvName;
    public Text tvPrice;

    public Text tvLoyal;

    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvCook;
    public Text tvForce;
    public Text tvLucky;

    public Button btEquip;
    public Button btFire;

    public RadioButtonView rbAccounting;
    public RadioButtonView rbChef;
    public RadioButtonView rbWaiter;
    public RadioButtonView rbShout;
    public RadioButtonView rbBeater;

    public CharacterBean characterData;

    public void SetData(CharacterBean data)
    {
        characterData = data;
        if (characterData.baseInfo != null)
        {
            CharacterBaseBean characterBase = characterData.baseInfo;
            SetName(characterBase.name);
            SetPrice(characterBase.priceS, characterBase.priceM, characterBase.priceL);
           
        }
        if (characterData.attributes != null)
        {
            CharacterAttributesBean characterAttributes = characterData.attributes;
            SetLoyal(characterAttributes.loyal);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName == null)
            return;
        tvName.text = name;
    }

    /// <summary>
    /// 设置工资
    /// </summary>
    /// <param name="priceS"></param>
    /// <param name="priceM"></param>
    /// <param name="priceL"></param>
    public void SetPrice(long priceS, long priceM, long priceL)
    {
        if (tvPrice == null)
            return;
        tvPrice.text = priceS + " / 天";
    }

    /// <summary>
    /// 设置忠诚度
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(float loyal)
    {
        if (tvLoyal == null)
            return;
        tvLoyal.text = loyal+"";
    }
}
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameDressStoreCpt : ItemGameGroceryCpt
{
    public GameObject objCook;
    public Text tvCook;
    public GameObject objSpeed;
    public Text tvSpeed;
    public GameObject objAccount;
    public Text tvAccount;
    public GameObject objCharm;
    public Text tvCharm;
    public GameObject objForce;
    public Text tvForce;
    public GameObject objLucky;
    public Text tvLucky;

    public new void SetData(StoreInfoBean storeInfo)
    {
        base.SetData(storeInfo);
        SetAttribute(itemsInfo.add_cook,
            itemsInfo.add_speed,
            itemsInfo.add_account,
            itemsInfo.add_charm,
            itemsInfo.add_force,
            itemsInfo.add_lucky);
    }

    public void SetAttribute(int add_cook, int add_speed, int add_account, int add_charm, int add_force, int add_lucky)
    {
        if (objCook != null && add_cook == 0)
            objCook.SetActive(false);
        if (objSpeed != null && add_speed == 0)
            objSpeed.SetActive(false);
        if (objAccount != null && add_account == 0)
            objAccount.SetActive(false);
        if (objCharm != null && add_charm == 0)
            objCharm.SetActive(false);
        if (objForce != null && add_force == 0)
            objForce.SetActive(false);
        if (objLucky != null && add_lucky == 0)
            objLucky.SetActive(false);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "+" + add_cook;
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "+" + add_speed;
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "+" + add_account;
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "+" + add_charm;
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "+" + add_force;
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "+" + add_lucky;
    }
}
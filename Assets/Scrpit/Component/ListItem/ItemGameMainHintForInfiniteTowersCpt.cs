using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemGameMainHintForInfiniteTowersCpt : ItemGameBaseCpt
{
    public Image ivIcon;
    public Text tvName;
    public ProgressView progressView;

    public UserInfiniteTowersBean infiniteTowersData;
    protected IconDataManager iconDataManager;

    public void SetData(UserInfiniteTowersBean  infiniteTowersData)
    {
        this.infiniteTowersData = infiniteTowersData;
        RefreshData();
    }

    public void RefreshData()
    {
        if (infiniteTowersData != null)
        {
            SetName(infiniteTowersData.layer + GameCommonInfo.GetUITextById(83));
            SetProgress(infiniteTowersData.proForSend);
        }
    }

    public void SetIcon(Sprite spFoodIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spFoodIcon;
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetProgress(float pro)
    {
        progressView.SetData(pro);
    }

}
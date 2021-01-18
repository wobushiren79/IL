﻿using UnityEngine;
using UnityEditor;

public class RevenueCartogramBarForItem : CartogramBarForItem
{
    public PopupRecordButton popupButton;
    public InnRecordBean innRecordData;

    public void SetRecordData(InnRecordBean innRecordData)
    {
        this.innRecordData = innRecordData;
        popupButton.SetData(innRecordData);
    }

    public override void SetHData(string data)
    {
        data += GameCommonInfo.GetUITextById(31);
        base.SetHData(data);
    }

    public override void SetVData(string data)
    {
        if (cartogramData.value_4 == (int)GameTimeHandler.DayEnum.Rest)
        {
            data = GameCommonInfo.GetUITextById(20);
        }
        else
        {
            data = "";
            if (cartogramData.value_1 != 0)
            {
                data += (cartogramData.value_1 + GameCommonInfo.GetUITextById(16));
            }
            if (cartogramData.value_2 != 0)
            {
                data += (cartogramData.value_2 + GameCommonInfo.GetUITextById(17));
            }
            if (cartogramData.value_3 != 0)
            {
                data += (cartogramData.value_3 + GameCommonInfo.GetUITextById(18));
            }
            if (cartogramData.value_1 == 0 && cartogramData.value_2 == 0 && cartogramData.value_3 == 0)
            {
                data += GameCommonInfo.GetUITextById(47);
            }
        }
        base.SetVData(data);
    }
}
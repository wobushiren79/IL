using UnityEngine;
using UnityEditor;

public class RevenueCartogramBarForItem : CartogramBarForItem
{
    public UIPopupRecordButton popupButton;
    public InnRecordBean innRecordData;

    public void SetRecordData(InnRecordBean innRecordData)
    {
        this.innRecordData = innRecordData;
        popupButton.SetData(innRecordData);
    }

    public override void SetHData(string data)
    {
        data += TextHandler.Instance.manager.GetTextById(31);
        base.SetHData(data);
    }

    public override void SetVData(string data)
    {
        if (cartogramData.value_4 == (int)GameTimeHandler.DayEnum.Rest)
        {
            data = TextHandler.Instance.manager.GetTextById(20);
        }
        else
        {
            data = "";
            if (cartogramData.value_1 != 0)
            {
                data += (cartogramData.value_1 + TextHandler.Instance.manager.GetTextById(16));
            }
            if (cartogramData.value_2 != 0)
            {
                data += (cartogramData.value_2 + TextHandler.Instance.manager.GetTextById(17));
            }
            if (cartogramData.value_3 != 0)
            {
                data += (cartogramData.value_3 + TextHandler.Instance.manager.GetTextById(18));
            }
            if (cartogramData.value_1 == 0 && cartogramData.value_2 == 0 && cartogramData.value_3 == 0)
            {
                data += TextHandler.Instance.manager.GetTextById(47);
            }
        }
        base.SetVData(data);
    }
}
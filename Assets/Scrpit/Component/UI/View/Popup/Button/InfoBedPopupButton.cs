using UnityEngine;
using UnityEditor;

public class InfoBedPopupButton : PopupButtonView<InfoBedPopupShow>
{
    public BuildBedBean buildBedData;

    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        popupShow.SetData(buildBedData);
    }
}
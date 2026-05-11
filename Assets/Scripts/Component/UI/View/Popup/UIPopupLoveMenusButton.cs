using UnityEngine;
using UnityEditor;

public class UIPopupLoveMenusButton : PopupButtonView<UIPopupLoveMenusShow>
{
    public string idForTeamCustomer;

    public void SetDataForTeamCustomer(string id)
    {
        this.idForTeamCustomer = id;
    }

    public override void PopupShow()
    {
        popupShow.SetDataForTeamCustomer(long.Parse(idForTeamCustomer));
    }

    public override void PopupHide()
    {

    }
}
using UnityEngine;
using UnityEditor;

public class PopupLoveMenusButton : PopupButtonView<PopupLoveMenusShow>
{
    public string idForTeamCustomer;

    public void SetDataForTeamCustomer(string id)
    {
        this.idForTeamCustomer = id;
    }

    public override void ClosePopup()
    {
    }

    public override void OpenPopup()
    {
        popupShow.SetDataForTeamCustomer(long.Parse(idForTeamCustomer));
    }

}
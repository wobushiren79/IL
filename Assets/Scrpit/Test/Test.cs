using UnityEngine;
using UnityEngine.UI;



using Pathfinding;
using System;
using System.Threading.Tasks;

public class Test : BaseMonoBehaviour
{
    public CharacterBodyCpt characterBodyCpt;
    public CharacterDressCpt characterDressCpt;
    private void Start()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(2100002);
            //characterDressCpt.SetHat(itemsInfo);

            //itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(140065);
            //characterDressCpt.SetMask(itemsInfo);

            itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(250007);
            characterDressCpt.SetClothes(itemsInfo);

            itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(350007);
            characterDressCpt.SetShoes(itemsInfo);

            itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(600005);
            characterDressCpt.SetHand(itemsInfo);

            characterBodyCpt.SetHair(null,Color.white);
        }
    }
}

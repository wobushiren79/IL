using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIPopupLoveMenusShow : PopupShowView
{
    public GameObject objLoveMenuContainer;
    public GameObject objLoveMenuModel;

    public Sprite spUnLock;

    public void SetDataForTeamCustomer(long teamId)
    {
        CptUtil.RemoveChildsByActive(objLoveMenuContainer);
        //获取团队数据
        NpcTeamBean npcTeamData = NpcTeamHandler.Instance.manager.GetCustomerTeam(teamId);
        if (npcTeamData == null)
            return;
        //获取喜爱的菜单
        List<long> listLoveMenu = npcTeamData.GetLoveMenus(); 
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        foreach (long menuId in listLoveMenu)
        {
            MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(menuId);
            Sprite spFood = InnFoodHandler.Instance.manager.GetFoodSpriteByName(menuInfo.icon_key);
            GameObject objLoveMenu = Instantiate(objLoveMenuContainer, objLoveMenuModel);
            ItemBaseTextCpt itemLoveMenu = objLoveMenu.GetComponent<ItemBaseTextCpt>();

            if (userAchievement.CheckHasTeamCustomerLoveMenu(teamId, menuId))
            {
                itemLoveMenu.SetData(spFood, menuInfo.name, "");
            }
            else
            {
                itemLoveMenu.SetData(spUnLock, "???", Color.gray, "");
            }
        }
    }
}
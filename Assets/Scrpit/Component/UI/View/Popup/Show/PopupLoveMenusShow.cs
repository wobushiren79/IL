using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PopupLoveMenusShow : PopupShowView
{
    protected GameDataManager gameDataManager;
    protected NpcTeamManager npcTeamManager;
    protected InnFoodManager innFoodManager;

    public GameObject objLoveMenuContainer;
    public GameObject objLoveMenuModel;

    public Sprite spUnLock;
    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
    }

    public void SetDataForTeamCustomer(long teamId)
    {
        CptUtil.RemoveChildsByActive(objLoveMenuContainer);
        //获取团队数据
        NpcTeamBean npcTeamData = npcTeamManager.GetCustomerTeam(teamId);
        if (npcTeamData == null)
            return;
        //获取喜爱的菜单
        List<long> listLoveMenu = npcTeamData.GetLoveMenus();
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
        foreach (long menuId in listLoveMenu)
        {
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(menuId);
            Sprite spFood = innFoodManager.GetFoodSpriteByName(menuInfo.icon_key);
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
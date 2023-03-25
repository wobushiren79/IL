﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameTest : BaseUIComponent
{
    public InputField etStoryId;
    public Button btStoryCreate;

    public InputField etItemId;
    public InputField etItemNumber;
    public Button btItemCreate;

    public InputField etBuildItemId;
    public InputField etBuildItemNumber;
    public Button btBuildItemCreate;

    //团队顾客生成
    public InputField etNpcGuestTeamId;
    public Button btNpcGuestTeam;
    public Button btNpcFriend;
    public Button btNpcFriendTeam;
    public Button btNpcRascal;
    public Button btNpcSundry;
    public Button btNpcConvert;

    public Button btAddAll;
    private void Start()
    {
        if (btStoryCreate != null)
            btStoryCreate.onClick.AddListener(CreateStory);
        if (btItemCreate != null)
            btItemCreate.onClick.AddListener(AddItem);
        if (btBuildItemCreate != null)
            btBuildItemCreate.onClick.AddListener(AddBuildItem);
        if (btNpcGuestTeam != null)
            btNpcGuestTeam.onClick.AddListener(CreateGuestTeam);
        if (btNpcFriend != null)
            btNpcFriend.onClick.AddListener(CreateFriendForOne);
        if (btNpcFriendTeam != null)
            btNpcFriendTeam.onClick.AddListener(CreateFriendForTeam);
        if (btNpcRascal != null)
            btNpcRascal.onClick.AddListener(CreateRascal);
        if (btNpcSundry != null)
            btNpcSundry.onClick.AddListener(CreateSundry);
        if (btNpcConvert != null)
            btNpcConvert.onClick.AddListener(CreateConvert);
        if (btAddAll != null)
            btAddAll.onClick.AddListener(AddAll);
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    public void CreateStory()
    {
        if (long.TryParse(etStoryId.text, out long storyId)) 
        { 
            GameEventHandler.Instance.EventTriggerForStory(storyId);
        }
    }

    /// <summary>
    /// 增加所有
    /// </summary>
    public void AddAll()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        gameData.listItems.Clear();
        List<ItemsInfoBean> listItem = GameItemsHandler.Instance.manager.GetAllItems();
        foreach (ItemsInfoBean itemsInfo in listItem)
        {
            gameData.AddItemsNumber(itemsInfo.id, 1);
        }
        gameData.listBuild.Clear();
        Dictionary<long, BuildItemBean> mapbuild = InnBuildHandler.Instance.manager.listBuildData;
        foreach (var itemBuild in mapbuild)
        {
            gameData.AddBuildNumber(itemBuild.Key, 99);
        }
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddItem()
    {
        if (long.TryParse(etItemId.text, out long itemId))
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if (long.TryParse(etItemNumber.text, out long itemNumber))
            {
                gameData.AddItemsNumber(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("道具数量输入错误");
            }
        }
        else
        {
            LogUtil.LogError("道具ID输入错误");
        }
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddBuildItem()
    {
        if (long.TryParse(etBuildItemId.text, out long itemId))
        {

            if (long.TryParse(etBuildItemNumber.text, out long itemNumber))
            {
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                gameData.AddBuildNumber(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("建筑数量输入错误");
            }
        }
        else
        {
            LogUtil.LogError("建筑ID输入错误");
        }
    }

    /// <summary>
    /// 生成指定团队
    /// </summary>
    public void CreateGuestTeam()
    {
        NpcHandler.Instance.builderForCustomer.BuildGuestTeam(long.Parse(etNpcGuestTeamId.text));
    }

    /// <summary>
    /// 生成好友
    /// </summary>
    public void CreateFriendForOne()
    {
        NpcHandler.Instance.builderForEvent.BuildTownFriendsForOne(long.Parse(etNpcGuestTeamId.text));
    }

    /// <summary>
    /// 生成好友
    /// </summary>
    public void CreateFriendForTeam()
    {
        NpcHandler.Instance.builderForEvent.BuildTownFriendsForTeam(long.Parse(etNpcGuestTeamId.text));
    }

    /// <summary>
    /// 生成捣乱者
    /// </summary>
    public void CreateRascal()
    {
        NpcHandler.Instance.builderForEvent.BuildRascal(long.Parse(etNpcGuestTeamId.text));
    }

    /// <summary>
    /// 生成杂项
    /// </summary>
    public void CreateSundry()
    {
        NpcHandler.Instance.builderForEvent.BuildSundry(long.Parse(etNpcGuestTeamId.text));
    }

    public void CreateConvert()
    {
        NpcHandler.Instance.builderForEvent.BuildConvert(long.Parse(etNpcGuestTeamId.text));
    }
}
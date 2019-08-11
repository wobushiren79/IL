using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoryBuilder : BaseMonoBehaviour, StoryInfoManager.CallBack
{
    [Header("控件")]
    public GameObject objNpcModle;

    [Header("数据")]
    public StoryInfoManager storyInfoManager;
    public NpcInfoManager npcInfoManager;

    public StoryInfoBean storyInfo;
    public List<StoryInfoDetailsBean> listStoryDetails;

    public int storyOrder = 1;
    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        storyInfoManager.GetStoryDetailsById(storyInfo.id, this);
    }

    public void GetStoryDetailsSuccess(List<StoryInfoDetailsBean> listData)
    {
        listStoryDetails = listData;
        storyOrder = 1;
        ClearStoryScene();
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(storyOrder);
        CreateStoryScene(listOrderData);
    }

    /// <summary>
    /// 根据顺序获取故事
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<StoryInfoDetailsBean> GetStoryDetailsByOrder(int order)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        if (listStoryDetails == null)
            return listData;

        foreach (StoryInfoDetailsBean itemData in listStoryDetails)
        {
            if (itemData.order == order)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建故事场景
    /// </summary>
    public void CreateStoryScene(List<StoryInfoDetailsBean> listData)
    {
        if (storyInfo == null)
            return;
        //设置剧情发生坐标
        gameObject.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            switch (itemData.type)
            {
                case 1:
                    CreateNpc(itemData);
                    break;
            }
        }
    }

    /// <summary>
    /// 生成NPC
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateNpc(StoryInfoDetailsBean itemData)
    {
        GameObject objNpc = Instantiate(objNpcModle, transform);
        objNpc.SetActive(true);
        objNpc.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
        NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemData.npc_id);
        aiNpc.SetCharacterData(characterData);
    }

    /// <summary>
    /// 清理剧情场景
    /// </summary>
    public void ClearStoryScene()
    {
        CptUtil.RemoveChild(transform);
        gameObject.transform.position = Vector3.zero;
    }
}